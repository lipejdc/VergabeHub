using OpenQA.Selenium;
using Vergabe.NRW.Constants;

namespace Vergabe.NRW;

internal static class NRWNoticeFilter
{

    internal static IWebDriver? ChromeDriver { get; set; }
    //Instead of using many By parameters, the "params" keyword allows you to
    //have a variable number of arguments of the same type.
    //The array is there for iteration purposes
    private static void SelectRelevantFilters(params By[] selectors)
    {
        foreach (var selector in selectors)
        {
            IWebElement element = ChromeDriver!.FindElement(selector);
            element.Click();
        }
    }

    internal static void SetTypesOfPublication(params By[] typesOfPublications)
    {
        SelectRelevantFilters(typesOfPublications);
    }

    internal static void SetProcurementRegulations(params By[] procurementRegulations)
    {
        SelectRelevantFilters(procurementRegulations);
    }

    internal static void SetSubjectMattersOfContract(params By[] subjectMattersOfContract)
    {
        IWebElement editSubjectMatterButton = ChromeDriver!
            .FindElement(By.Id(NRWApplicationConstants.IdEditSubjectMatterButton));

        editSubjectMatterButton.Click();

        SwitchToIFrame(By.Id("cpvCodeIFrame"));
        SelectRelevantFilters(subjectMattersOfContract);
        //If you don't switch back to the default content, you won't be able to select ANY
        //element outside of the iFrame and you will get a NoSuchElementException!
        ChromeDriver.SwitchTo().DefaultContent();
    }

    private static void SwitchToIFrame(By iFrameSelector)
    {
        //This is necessary to be able to access the elements inside the iFrame
        IWebElement iFrame = ChromeDriver!.FindElement(iFrameSelector);
        ChromeDriver.SwitchTo().Frame(iFrame);
    }

    internal static void StartSearch(By startSearchButtonSelector)
    {
        IWebElement startSearchButton = ChromeDriver!.FindElement(startSearchButtonSelector);
        startSearchButton.Click();
    }
}
