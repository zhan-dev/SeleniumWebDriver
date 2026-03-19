using CoreLayer.Enums;
using OpenQA.Selenium;

namespace CoreLayer.WebDriver
{
    public interface IWebDriverFactory
    {
        public IWebDriver CreateDriver(WebBrowserMode mode);
    }
}