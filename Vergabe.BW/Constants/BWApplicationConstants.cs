namespace Vergabe.BW.Constants;

internal class BWApplicationConstants
{
    //URL
    internal const string NoticeAdvancedSearchUrl = "https://ausschreibungen.landbw.de/Center/company/announcements/categoryOverview.do?method=showCategoryOverview";

    //Type of publications
    internal const string IdNoticeCheckBox = "Tender";

    //Procurement regulations
    internal const string IdSuppliesAndServicesCheckbox = "VOL";

    internal const string IdDefenseAndSecurityCheckbox = "VSVGV";

    internal const string IdUtilitiesCheckbox = "SEKTVO";

    internal const string IdOtherCheckbox = "Sonstige";

    //Subject matters of contract
    internal const string IdEditSubjectMatterButton = "openCPVWizard";

    internal const string XPathITServicesButton =
        """//a[contains(@href,"selectCPVCode&cpv=72000000-5")]""";

    internal const string XPathBusinessServicesButton =
        """//a[contains(@href,"selectCPVCode&cpv=79000000-4")]""";

    internal const string XPathSoftwarePackageButton =
       """//a[contains(@href,"selectCPVCode&cpv=48000000-8")]""";

    internal const string IdApplySettingsButton = "wizardAssume";

    internal const string IdStartSearchButton = "searchStart";

    //Pages
    internal const string IdNextPageButton = "nextPage";

    internal const string XPathBrowsePagesText = "//div[@class='browsePagesText']";

    //Table cells
    internal const string XPathNoticeTableCells = "//table/tbody//td";
}
