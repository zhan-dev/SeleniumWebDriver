using OpenQA.Selenium;

namespace CoreLayer.WebDriver
{
    public partial class WebDriverWrapper
    {
        private readonly IWebDriver _driver;
        private readonly TimeSpan _timeout;
        private const int WaitTimeInSeconds = 10;

        public WebDriverWrapper(IWebDriver driver, int explicitWaitTime = WaitTimeInSeconds)
        {
            this._driver = driver;
            this._timeout = TimeSpan.FromSeconds(explicitWaitTime);
        }

        public void SetImplicitWaitTime(int implicitWaitTime)
        {
            this._driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(implicitWaitTime);
        }

        public void NavigateTo(string url)
        {
            this._driver.Navigate().GoToUrl(url);
        }

        public void WindowMaximize()
        {
            this._driver.Manage().Window.Maximize();
        }

        public string GetTitle()
        {
            return this._driver.Title;
        }

        public string GetUrl()
        {
            return this._driver.Url;
        }

        public void Close()
        {
            this._driver.Dispose();
        }
    }
}
