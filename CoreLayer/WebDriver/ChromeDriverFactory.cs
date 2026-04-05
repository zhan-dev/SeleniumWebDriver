using CoreLayer.Enums;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CoreLayer.WebDriver
{
    public class ChromeDriverFactory : IWebDriverFactory
    {
        public IWebDriver CreateDriver(WebBrowserMode mode)
        {
            var service = ChromeDriverService.CreateDefaultService();
            var options = new ChromeOptions();

            options.AddExcludedArgument("enable-automation");
            options.AddArgument("--incognito");

            if (mode is WebBrowserMode.Silent)
            {
                options.AddArgument("--headless");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--window-size=1920,1080");
            }

            var driver = new ChromeDriver(service, options, TimeSpan.FromSeconds(30));

            if (mode is WebBrowserMode.UXUI)
            {
                driver.Manage().Window.Maximize();
            }

            return driver;
        }
    }
}
