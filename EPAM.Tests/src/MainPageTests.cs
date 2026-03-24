using BusinessLayer.src;

namespace EPAM.Tests.src
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    internal class MainPageTests : BaseSetup
    {
        private MainPage mainPage;

        private static readonly string[] searchCorrectKeywords = ["BLOCKCHAIN", "Cloud", "Automation"];

        public override void SetUp()
        {
            base.SetUp();

            this.mainPage = new MainPage(this.driver);
            this.mainPage.MaximizeWindow();
            this.mainPage.GoToMainPage();
            this.mainPage.WaitUntilTitleIsPresented();
            this.mainPage.AcceptAllCookie();
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

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }
    }
}
