using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Vergabe.BW;
using Vergabe.BW.Constants;
using VergabeHub.Data;
using VergabeHub.DatabaseFunctions;


VergabeHubDbContext dbContext = new();

int noticePlatformId = dbContext.NoticePlatforms
    .SingleOrDefault(pn => pn.Name == "Vergabe BW")!.Id;

//This allows the sharing of the same instance of ChromeDriver among different classes
var chromeDriver = new ChromeDriver();
BWNoticeFilter.ChromeDriver = chromeDriver;
BWNoticeScraper.ChromeDriver = chromeDriver;

chromeDriver
    .Navigate()
    .GoToUrl(url: BWApplicationConstants.NoticeAdvancedSearchUrl);

BWNoticeFilter.SetTypesOfPublication(By.Id(BWApplicationConstants.IdNoticeCheckBox));

//When using the 'params' keyword as a parameter, it is not possible to add parameter names.
BWNoticeFilter.SetProcurementRegulations(
    By.Id(BWApplicationConstants.IdSuppliesAndServicesCheckbox),
    By.Id(BWApplicationConstants.IdDefenseAndSecurityCheckbox),
    By.Id(BWApplicationConstants.IdUtilitiesCheckbox),
    By.Id(BWApplicationConstants.IdOtherCheckbox)
    );

BWNoticeFilter.SetSubjectMattersOfContract(
    By.XPath(BWApplicationConstants.XPathITServicesButton),
    By.XPath(BWApplicationConstants.XPathBusinessServicesButton),
    By.XPath(BWApplicationConstants.XPathSoftwarePackageButton),
    By.Id(BWApplicationConstants.IdApplySettingsButton)
    );

BWNoticeFilter.StartSearch(
    startSearchButtonSelector: By.Id(BWApplicationConstants.IdStartSearchButton));

var notices = BWNoticeScraper.ScrapeAllNoticesToList(
    nextPageButtonSelector: By.Id(BWApplicationConstants.IdNextPageButton)
    );

await DatabaseOperator.ProcessNoticesAndAddToDatabase(notices, noticePlatformId);

chromeDriver.Quit();

