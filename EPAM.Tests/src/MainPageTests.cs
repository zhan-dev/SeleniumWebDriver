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

        public override void SetUp()
        {
            ChromeDriverFactory driverFactory = new ChromeDriverFactory();
            this.driver = driverFactory.CreateDriver(WebBrowserMode.UXUI);

            this.mainPage = new MainPage(this.driver);
            mainPage.GoToUrl();
            mainPage.WaitUntilTitleIsPresented();
        }

        [Test]
        public void UserGoToUrl_UserWaitUntilTitleIsLoaded_TitleIsAsExpected()
        {
            //Arrange
            string expectedTitle = "EPAM | Software Engineering & Product Development Services";

            // Assert
            Assert.That(mainPage.GetTitle(), Is.EqualTo(expectedTitle));
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }
    }
}
