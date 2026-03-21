using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BusinessLayer.src
{
    public class MainPage
    {
        private readonly IWebDriver driver;

        private readonly string url = "https://www.epam.com/";

        private readonly By title = By.TagName("title");

        public MainPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void GoToUrl()
        {
            this.driver.Navigate().GoToUrl(url);
        }

        public string GetTitle()
        {
            return this.driver.Title;
        }

        public IWebElement WaitUntilTitleIsPresented()
        {
            var titleWait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            return titleWait.Until(driver => driver.FindElement(this.title));
        }
    }
}
