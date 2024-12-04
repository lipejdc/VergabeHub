using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Vergabe.NRW;
using Vergabe.NRW.Constants;
using VergabeHub.Data;
using VergabeHub.DatabaseFunctions;


VergabeHubDbContext _dbContext = new();

int noticePlatformId = _dbContext.NoticePlatforms
    .SingleOrDefault(pn => pn.Name == "Vergabe NRW")!.Id;

//This allows the sharing of the same instance of ChromeDriver among different classes
var chromeDriver = new ChromeDriver();
NRWNoticeFilter.ChromeDriver = chromeDriver;
NRWNoticeScraper.ChromeDriver = chromeDriver;

chromeDriver
    .Navigate()
    .GoToUrl(url: NRWApplicationConstants.NoticeAdvancedSearchUrl);

NRWNoticeFilter.SetTypesOfPublication(By.Id(NRWApplicationConstants.IdNoticeCheckBox));

//When using the 'params' keyword as a parameter, it is not possible to add parameter names.
NRWNoticeFilter.SetProcurementRegulations(
    By.Id(NRWApplicationConstants.IdSuppliesAndServicesCheckbox),
    By.Id(NRWApplicationConstants.IdDefenseAndSecurityCheckbox),
    By.Id(NRWApplicationConstants.IdUtilitiesCheckbox),
    By.Id(NRWApplicationConstants.IdOtherCheckbox)
    );

//XPATH is not working, even when I can uniquely identify the element when using DevTools
//CssSelector is also not working and when examining with DevTools, it finds the element
//After some research I found out that if the element is nested inside an iframe element
//it is not possible to find it. You first have to switch to the iframe and then find the element.
NRWNoticeFilter.SetSubjectMattersOfContract(
    By.XPath(NRWApplicationConstants.XPathITServicesButton),
    By.XPath(NRWApplicationConstants.XPathBusinessServicesButton),
    By.XPath(NRWApplicationConstants.XPathSoftwarePackageButton),
    By.Id(NRWApplicationConstants.IdApplySettingsButton)
    );

NRWNoticeFilter.StartSearch(
    startSearchButtonSelector: By.Id(NRWApplicationConstants.IdStartSearchButton)
    );

var notices = NRWNoticeScraper.ScrapeAllNoticesToList(
    nextPageButtonSelector: By.Id(NRWApplicationConstants.IdNextPageButton)
    );

await DatabaseOperator.ProcessNoticesAndAddToDatabase(notices, noticePlatformId);

chromeDriver.Quit();

