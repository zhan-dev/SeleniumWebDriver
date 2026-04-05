using BusinessLayer.PageObject;

namespace EPAM.Tests.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    internal class MainPageTests : TestSetup
    {
        private MainPage mainPage;

        private static readonly string[] searchCorrectKeywords = ["BLOCKCHAIN", "Cloud", "Automation"];
        public override void SetUp()
        {
            base.SetUp();

            this.mainPage = new MainPage(this.driver);
            this.mainPage.LoadMainPage();
        }

        [Test]
        public void UserGoToUrl_WaitUntilTitleIsLoaded_TitleIsAsExpected()
        {
            //Arrange
            string expectedTitle = "EPAM | Software Engineering & Product Development Services";

            // Assert
            Assert.That(this.mainPage.GetTitle(), Is.EqualTo(expectedTitle));
        }

        [TestCaseSource(nameof(searchCorrectKeywords))]
        public void UserGoToGlobalSearch_UseGlobalSearchPanel_SearchResultsIsAsExpected(string searchText)
        {
            //Act
            this.mainPage.ClickSearchButton();
            this.mainPage.InputDataIntoSearchInput(searchText);
            this.mainPage.ClickFindButton();

            var results = this.mainPage.GetSearchResultsCollection();
            this.mainPage.SearchResultsToConsole(results);
            bool isAllValid = this.mainPage.IsSearchResultsValid(results, searchText);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(results, Is.Not.Empty);
                Assert.That(isAllValid, Is.True);
            });
        }

        [Test]
        public void UserGoToFooterOnMainPage_UserClickCodeOfEthicalConductPDFLink_DownloadedFileAsExpected()
        {
            //Arrange
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string expectedFileName = "Code-Of-Conduct_01_26.pdf";

            //Act
            this.mainPage.ScrollToFooter();
            this.mainPage.ClickCodeOfEthicalConductPDFLink();
            string fullPath = Path.Combine(downloadsPath, expectedFileName);

            bool isExist = File.Exists(fullPath);

            //Assert
            Assert.That(isExist, Is.True);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }
    }
}
