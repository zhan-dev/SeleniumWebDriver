using BusinessLayer.src;
using CoreLayer.Enums;
using CoreLayer.WebDriver;
using OpenQA.Selenium;

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
            ChromeDriverFactory driverFactory = new ChromeDriverFactory();
            this.driver = driverFactory.CreateDriver(WebBrowserMode.UXUI);

            this.mainPage = new MainPage(this.driver);
            this.mainPage.MaximizeWindow();
            this.mainPage.GoToUrl();
            this.mainPage.WaitUntilTitleIsPresented();
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
            this.mainPage.SearchButtonClick();
            this.mainPage.InputDataIntoSearchInput(searchText);
            this.mainPage.FindClick();

            var results = this.mainPage.GetSearchResultCollection();

            // Assert
            Assert.That(results, Is.Not.Empty);

            foreach (var result in results)
            {
                var title = result.FindElement(By.CssSelector(".search-results__title-link")).Text;
                var link = result.FindElement(By.CssSelector(".search-results__title-link")).GetAttribute("href");
                var description = result.FindElement(By.CssSelector(".search-results__description")).Text;

                Console.WriteLine($"{title} -> {link}");
                Console.WriteLine(description);
            }

            bool allValid = results.All(key =>
                searchText.Any(keyword =>
                    key.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase)));

            Assert.That(allValid, Is.True);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }
    }
}
