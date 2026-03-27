using BusinessLayer.PageObject;

namespace EPAM.Tests.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    internal class CareersPageTests : TestSetup
    {
        private MainPage mainPage;
        private CareersPage careersPage;

        private static readonly string[] keywords = ["c#", ".net", "java", "javascript"];
        private static readonly string[] countries = ["Ukrain", "India", "Japan"];

        public static IEnumerable<TestCaseData> KeywordsAndCountries()
        {
            foreach (var keyword in keywords)
            {
                foreach (var country in countries)
                {
                    yield return new TestCaseData(keyword, country);
                }
            }
        }

        public override void SetUp()
        {
            base.SetUp();

            this.mainPage = new MainPage(this.driver);
            this.careersPage = new CareersPage(this.driver);

            this.mainPage.LoadMainPage();
            this.mainPage.GoToCareers();
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
        public void UserGoToCareersSearch_UseCareersSearchPanelWithoutFilters_SearchResultsAreValid(string searchText)
        {
            //Act
            this.careersPage.ClickStartYourSearchHereButton();
            this.careersPage.AcceptAllCookie();
            this.careersPage.EnterTextToSearchInput(searchText);
            this.careersPage.ClickFindButton();

            this.careersPage.ExpandLastElement();
            bool isAllValid = this.careersPage.ValidateLastElementContains(searchText);

                Assert.That(isAllValid, Is.True);
        }

        [TestCaseSource(nameof(KeywordsAndCountries))]
        public void UserGoToCareersSearch_UseCareersSearchPanelWithRemoteFilter_SearchResultsAreValid(
            string searchText, string countries)
        {
            //Act
            this.careersPage.ClickStartYourSearchHereButton();
            this.careersPage.AcceptAllCookie();
            this.careersPage.EnterTextToSearchInput(searchText);
            this.careersPage.EnterTextToCountryInput(countries);
            this.careersPage.AddRemoteFilter();

            this.careersPage.ExpandLastElement();
            bool isAllValid = this.careersPage.ValidateLastElementContains(searchText);

                Assert.That(isAllValid, Is.True);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }
    }
}
