using CoreLayer;
using CoreLayer.WebDriver;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using TestFramework.Core.BrowserUtils;

namespace EPAM.Tests
{
    internal abstract class TestSetup
    {
        protected WebDriverWrapper DriverWrapper { get; private set; }
        protected Logger Logger { get; private set; }

        [SetUp]
        public virtual void SetUp()
        {
            ChromeDriverFactory driverFactory = new ChromeDriverFactory();
            var driver = driverFactory.CreateDriver();
            this.DriverWrapper = new WebDriverWrapper(driver);
            this.Logger ??= new Logger();
        }

        [TearDown]
        public virtual void TearDown()
        {
            //for screenshots
            //if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            //{
            //    ScreenshotMaker.TakeBrowserScreenshot((ITakesScreenshot)this.DriverWrapper.Driver);
            //    ScreenshotMaker.TakeFullDisplayScreenshot();
            //}

            this.DriverWrapper.Close();
        }
    }
}
