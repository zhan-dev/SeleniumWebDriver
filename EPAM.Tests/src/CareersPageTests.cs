using BusinessLayer.src;

namespace EPAM.Tests.src
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    internal class CareersPageTests : BaseSetup
    {
        private MainPage mainPage;
        private CareersPage careersPage;

        private static readonly string[] keywords = ["c#", ".net", "java", "javascript"];

        public override void SetUp()
        {
            base.SetUp();

            this.mainPage = new MainPage(this.driver);
            this.mainPage.MaximizeWindow();
            this.mainPage.GoToMainPage();
            this.mainPage.WaitUntilTitleIsPresented();
            this.mainPage.AcceptAllCookie();
            this.mainPage.GoToCareersViaClick();

            this.careersPage = new CareersPage(this.driver);
            this.careersPage.WaitUntilTitleIsPresented();
        }

        [Test]
        public void UserGoToUrl_WaitUntilTitleIsLoaded_TitleIsAsExpected()
        {
            //Arrange
            string expectedTitle = "Explore Professional Growth Opportunities | EPAM Careers";

            // Assert
            Assert.That(this.careersPage.GetTitle(), Is.EqualTo(expectedTitle));
        }

        [TestCaseSource(nameof(keywords))]
        public void UserGoToCareersSearch_UseCareersSearchPanel_SearchResultsIsAsExpected(string searchText)
        {
            //    //Act
            this.careersPage.ClickStartYourSearchHereButton();
            this.careersPage.AcceptAllCookie();
            this.careersPage.EnterTextToSearchInput(searchText);
            //this.careersPage.EnterTextToCountryInput("test");
            this.careersPage.AddRemoteFilter();
            //this.careersPage.ClickFindButton();

            //    var results = this.mainPage.GetSearchResultsCollection();
            //    this.mainPage.SearchResultsToConsole(results);
            //    bool isAllValid = this.mainPage.IsSearchResultsValid(results, searchText);

            //    // Assert
            //    Assert.Multiple(() =>
            //    {
            //        Assert.That(results, Is.Not.Empty);
            //        Assert.That(isAllValid, Is.True);
            //    });
        }

        [TearDown]
        public override void TearDown()
        {
            //base.TearDown();
        }
    }
}
