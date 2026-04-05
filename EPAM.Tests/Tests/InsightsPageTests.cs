using BusinessLayer.PageObject;

namespace EPAM.Tests.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    internal class InsightsPageTests : TestSetup
    {
        private MainPage mainPage;
        private InsightsPage insightsPage;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            this.mainPage = new MainPage(this.driver);
            this.insightsPage = new InsightsPage(this.driver);

            this.mainPage.LoadMainPage();
            this.mainPage.GoToInsightsPage();
        }

        [Test]
        public void UserGoToUrl_WaitUntilTitleIsLoaded_TitleIsAsExpected()
        {
            //Arrange
            string expectedTitle = "Discover our Latest Insights | EPAM";

            // Assert
            Assert.That(this.insightsPage.GetTitle(), Is.EqualTo(expectedTitle));
        }

        [Test]
        public void UserNavigateToInsights_UserSwapCarouselAndClickReadMore_CarouselTitleIsSameAsReadMoreTitle()
        {
            //Arrange
            string expectedText = "From Hype to Impact: How Enterprises Can Unlock Real Business Value with AI";

            //Act
            this.insightsPage.ClickMainSliderRightArrow();
            this.insightsPage.ClickMainSliderRightArrow();
            var getSliderText = this.insightsPage.MainSliderGetBusinessValueText();
            this.insightsPage.ClickReadTheReportButton();
            var textHeader = this.insightsPage.IntroductionHeaderText();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(getSliderText, Is.EqualTo(expectedText));
                Assert.That(expectedText, Is.EqualTo(textHeader));
            });
        }

        [TearDown]
        public override void TearDown()
        {
            Thread.Sleep(3000);
            base.TearDown();
        }
    }
}
