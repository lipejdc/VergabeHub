using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Text.RegularExpressions;
using Vergabe.BW.Constants;
using VergabeHub.Models;

namespace Vergabe.BW;

internal class BWNoticeScraper
{
    internal static IWebDriver? ChromeDriver { get; set; }

    internal static List<Notice> ScrapeAllNoticesToList(By nextPageButtonSelector)
    {
        //There are 20 notices per page
        List<Notice> notices = new();

        //Here you need a wait because the page needs time to load and the element is not there yet
        //If not, you get a "NoSuchElementException"
        WebDriverWait wait = new WebDriverWait(ChromeDriver, TimeSpan.FromSeconds(3));
        int currentPage = 1;

        try
        {
            //This wait ensures that all notices are fully loaded before starting with scraping 
            //Without this you often get a "NoSuchElementException" cause they aren't loaded
            //Sometimes I was getting here an System.IndexOutOfRangeException and NoSuchElementException
            //I changed the waiter below from "ElementExists" to "VisibilityOfAllElementsLocatedBy"
            //Now it works properly
            var waitForBrowsePagesVisibility = wait
            .Until(ExpectedConditions
            .VisibilityOfAllElementsLocatedBy(By.XPath(BWApplicationConstants.XPathBrowsePagesText)));

            //Gets the string of total pages and converts it into an integer
            var totalPages = Convert.ToInt32(ChromeDriver!
                .FindElement(By.XPath(BWApplicationConstants.XPathBrowsePagesText)).Text.Split()[3]);
            //Gets the number of total notices found and converts it into an integer
            var totalNoticesFound = Convert.ToInt32(ChromeDriver!
                .FindElement(By.XPath(BWApplicationConstants.XPathBrowsePagesText)).Text.Split()[6]);


            while (notices.Count != totalNoticesFound)
            {
                //The nextPageButton is below the notices' table. This Wait ensures that the whole table is loaded
                //before we start scraping from it. Without it, you get different exceptions
                IWebElement nextPageButton = wait
                    .Until(ExpectedConditions
                    .ElementToBeClickable(nextPageButtonSelector));

                ExtractNoticesFromCurrentPage(notices);

                //If the current page is smaller than the total pages, we navigate to the next page.
                //If they are the same, we know we are in the last page and skip navigating to the next one
                if (currentPage < totalPages)
                {
                    NavigateToNextPage(nextPageButtonSelector);
                    currentPage++;
                }
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
        return notices;
    }

    private static void ExtractNoticesFromCurrentPage(List<Notice> notices)
    {
        var tableCells = ChromeDriver!.FindElements(By.XPath(BWApplicationConstants.XPathNoticeTableCells));

        //We increment i by 6 in each iteration so it jumps to the first cell of the next table row
        for (int i = 0; i < tableCells.Count; i += 6)
        {
            //Gets all the data from the Notice, saves into a variable and adds to the notice list
            var notice = CreateNoticeFromTableCells(tableCells, i);
            notices.Add(notice);
        }
    }

    private static Notice CreateNoticeFromTableCells(IReadOnlyList<IWebElement> tableCells, int startIndex)
    {
        //This method extracts the data from each cell and returns a new notice with the data

        DateTime dateOfPublication = DateTime.Parse(tableCells[startIndex].Text);
        DateTime submissionDeadline = DateTime.Parse(tableCells[startIndex + 1].Text);
        string shortDescription = tableCells[startIndex + 2].Text;
        NoticeType type = new() { Type = tableCells[startIndex + 3].Text };
        ContractingAuthority contractingAuthority = new() { Name = tableCells[startIndex + 4].Text };
        string urlElement = tableCells[startIndex + 5].FindElement(By.TagName("a")).GetAttribute("href");
        //The pattern says: "Start with "https://" until the first occurrence of a percentage sign "%""
        string url = ExtractUrlFromElement(urlElement, @"https://[^\%']+");

        Notice notice = new Notice
        {
            DateOfPublication = dateOfPublication,
            SubmissionDeadline = submissionDeadline,
            ShortDescription = shortDescription,
            Type = type,
            ContractingAuthority = contractingAuthority,
            Url = url
        };

        return notice;
    }

    private static string ExtractUrlFromElement(string urlElement, string pattern)
    {
        Match match = Regex.Match(urlElement, pattern);
        return match.Success ? match.Value : string.Empty;
    }

    private static void NavigateToNextPage(By nextPageButtonSelector)
    {
        var nextPageButton = ChromeDriver!.FindElement(nextPageButtonSelector);
        nextPageButton.Click();
    }
}
