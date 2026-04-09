using BusinessLayer.PageObject;
using CoreLayer;
using DataLayer;
using System.Text.Json;

namespace EPAM.Tests.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    internal class MainPageTests : TestSetup
    {
        private MainPage mainPage;

        private static readonly string[] searchKeywords = ["BLOCKCHAIN", "Cloud", "Automation"];
        private static IEnumerable<string> searchCorrectKeywords
        {
            get
            {
                //var path = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestData.json");
                //var json = File.ReadAllText(path);
                var json = File.ReadAllText(Configuration.TestDataPath);
                var models = JsonSerializer.Deserialize<List<SearchModel>>(json);

                return models.SelectMany(m => m.SearchKeywords);
            }
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            this.mainPage = new MainPage(this.DriverWrapper, this.Logger);
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

        [TestCaseSource(nameof(searchKeywords))]
        public void UserGoToGlobalSearch_UseGlobalSearchPanel_SearchResultsIsAsExpected(string searchText)
        {
            Logger.Information("Starting the test 'UserGoToGlobalSearch_UseGlobalSearchPanel_SearchResultsIsAsExpected'.");

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

            Logger.Information("Ending the test 'UserGoToGlobalSearch_UseGlobalSearchPanel_SearchResultsIsAsExpected'.");
        }

        [TestCaseSource(nameof(searchCorrectKeywords))]
        public void UserGoToGlobalSearch_UseGlobalSearchPanel_SearchResultsIsAsExpected_WithJsonData(string searchText)
        {
            Logger.Information("Starting the test 'UserGoToGlobalSearch_UseGlobalSearchPanel_SearchResultsIsAsExpected_WithJsonData'.");

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

            Logger.Information("Ending the test 'UserGoToGlobalSearch_UseGlobalSearchPanel_SearchResultsIsAsExpected_WithJsonData'.");
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
