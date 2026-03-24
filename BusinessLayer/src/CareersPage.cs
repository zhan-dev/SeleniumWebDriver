using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Xml.Linq;

namespace BusinessLayer.src
{
    public class CareersPage
    {
        private readonly IWebDriver driver;
        private readonly By titleBy = By.TagName("title");
        private readonly By acceptAllCookieBy = By.Id("onetrust-accept-btn-handler");
        private readonly By searchDivWrapperBy = By.Id("anchor-list-wrapper");
        private readonly By searchFilterRemoteCheckBoxBy = By.Id("checkbox-vacancy_type-Remote-_r_0_");
        private readonly By searchInputBy = By.Name("search");
        private readonly By searchFormButtonBy = By.Name("submit_search_box_button");
        private readonly By startYourSearchButtonBy = By.CssSelector("a.button-body");
        private readonly By declineButtonBy = By.XPath("//div[contains(@class,'dropdown__clear-indicator')]");
        private readonly By searchFiltersBlockBy = By.ClassName("Filter_form__uZl4i");

        //span.pinned-button-text

        private readonly By pageHeaderBy = By.CssSelector("h1.remove-heading-style.scaling-of-text-wrapper");

        //private readonly By declineButtonBy = By.CssSelector("div.dropdown__dropdown-indicator");
        //private readonly By declineButtonBy = By.XPath("//div[contains(@class,'dropdown__indicator')]//svg");

        public CareersPage(IWebDriver driver)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(driver));
            this.driver = driver;
        }

        public string GetTitle()
        {
            return this.driver.Title;
        }

        public IWebElement WaitUntilTitleIsPresented()
        {
            var titleWait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
            return titleWait.Until(driver => driver.FindElement(this.titleBy));
        }

        public void AcceptAllCookie()
        {
            var wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
            wait.Until(drv =>
            {
                try
                {
                    var element = drv.FindElement(acceptAllCookieBy);
                    return (element.Displayed && element.Enabled) ? element : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            }).Click();
        }

        public void ClickStartYourSearchHereButton()
        {
            this.driver.FindElement(startYourSearchButtonBy).Click();
        }

        public void EnterTextToSearchInput(string searchText)
        {
            var searchPanelWrapper = this.driver.FindElement(searchDivWrapperBy);
            var searchInput = searchPanelWrapper.FindElement(searchInputBy);
            searchInput.SendKeys(searchText);
        }

        public void EnterTextToCountryInput(string searchCountry)
        {
            var searchPanelWrapper = this.driver.FindElement(searchDivWrapperBy);
            var declineButton = searchPanelWrapper.FindElement(declineButtonBy);
            declineButton.Click();
        }

        public void ClickFindButton()
        {
            var searchPanelWrapper = this.driver.FindElement(searchDivWrapperBy);
            var findButton = searchPanelWrapper.FindElement(searchFormButtonBy);
            findButton.Click();
        }

        public void AddRemoteFilter()
        {
            var searchPanelWrapper = this.driver.FindElement(searchFiltersBlockBy);
            var remoteFilter = searchPanelWrapper.FindElement(searchFilterRemoteCheckBoxBy);
            remoteFilter.Click();
        }
    }
}
