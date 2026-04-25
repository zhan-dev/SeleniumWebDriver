using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CoreLayer.WebDriver
{
    public class ChromeDriverFactory : IWebDriverFactory
    {
        public IWebDriver CreateDriver()
        {
            var service = ChromeDriverService.CreateDefaultService();
            var options = new ChromeOptions();

            options.AddExcludedArgument("enable-automation");

            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            options.AddArgument("--incognito");

            if (Configuration.IsHeadless)
            {
                options.AddArgument("--headless=new");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--window-size=1920,1080");
            }

            var driver = new ChromeDriver(service, options, TimeSpan.FromSeconds(30));

            if (!Configuration.IsHeadless)
            {
                driver.Manage().Window.Maximize();
            }

            return driver;
        }
    }
}
