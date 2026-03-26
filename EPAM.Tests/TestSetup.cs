using CoreLayer.Enums;
using CoreLayer.WebDriver;
using OpenQA.Selenium;

namespace EPAM.Tests
{
    internal abstract class TestSetup
    {
        protected IWebDriver driver;

        [SetUp]
        public virtual void SetUp()
        {
            ChromeDriverFactory driverFactory = new ChromeDriverFactory();
            this.driver = driverFactory.CreateDriver(WebBrowserMode.UXUI);
        }

        [TearDown]
        public virtual void TearDown() 
        {
            driver?.Dispose();
        }
    }
}
