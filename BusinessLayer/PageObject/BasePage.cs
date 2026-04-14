using CoreLayer;
using CoreLayer.WebDriver;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BusinessLayer.PageObject
{
    public abstract class BasePage
    {
        private readonly By acceptAllCookieBy = By.Id("onetrust-accept-btn-handler");
        protected WebDriverWrapper DriverWrapper { get; }
        protected Logger Logger { get; }
        protected readonly string mainPageTitle = "EPAM | Software Engineering & Product Development Services";
        protected readonly string careersPageTitle = "Explore Professional Growth Opportunities | EPAM Careers";
        protected readonly string insightsPageTitle = "Discover our Latest Insights | EPAM";

        protected BasePage(WebDriverWrapper driverWrapper, Logger logger)
        {
            this.DriverWrapper = driverWrapper;
            this.Logger = logger;
        }

        protected string WaitUntilTitleContains(string pageTitle)
        {
            var wait = new WebDriverWait(this.DriverWrapper.Driver, TimeSpan.FromSeconds(10));
            try
            {
                return wait.Until(drv =>
                {
                    return this.DriverWrapper.GetTitle().Contains(pageTitle) ? this.DriverWrapper.GetTitle() : null;
                });
            }
            catch (WebDriverTimeoutException ex)
            {
                this.Logger.Error(ex.Message);
                throw;
            }
        }
        protected void AcceptAllCookie()
        {
            var wait = new WebDriverWait(this.DriverWrapper.Driver, TimeSpan.FromSeconds(10));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
            try
            {
                wait.Until(drv =>
                {
                    var element = drv.FindElement(acceptAllCookieBy);
                    return (element.Displayed && element.Enabled) ? element : null;
                }).Click();
            }
            catch (WebDriverTimeoutException ex)
            {
                this.Logger.Error(ex.Message);
                throw;
            }

            wait.Until(drv => !drv.FindElement(acceptAllCookieBy).Displayed);
        }
    }
}
