using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Xml.Linq;

namespace BusinessLayer.src
{
    public class MainPage
    {
        private readonly IWebDriver driver;

        private readonly string url = "https://www.epam.com/";

        private readonly By titleBy = By.TagName("title");
        private readonly By searchButtonBy = By.ClassName("header__icon");
        private readonly By headerSearchPanelBy = By.ClassName("header-search__panel");
        private readonly By searchInputBy = By.Id("new_form_search");
        private readonly By findButtonBy = By.CssSelector(".search-results__action-section > button");
        private readonly By searchResultsCollectionElementsBy = By.ClassName("search-results__item");

        public MainPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void MaximizeWindow()
        {
            this.driver.Manage().Window.Maximize();
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
            var titleWait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
            return titleWait.Until(driver => driver.FindElement(this.titleBy));
        }

        public void SearchButtonClick()
        {
            this.driver.FindElement(this.searchButtonBy).Click();
        }

        public void InputDataIntoSearchInput(string searchText)
        {
            var searchPanel = this.driver.FindElement(headerSearchPanelBy);

            var waitInput = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));

            var activeInput = waitInput.Until(drv =>
            {
                var element = searchPanel.FindElement(searchInputBy);
                return (element.Displayed && element.Enabled) ? element : null;
            });

            activeInput.Click();
            activeInput.Clear();
            activeInput.SendKeys(searchText);
        }

        public void FindClick()
        {
            var searchPanel = this.driver.FindElement(headerSearchPanelBy);
            var findButton = searchPanel.FindElement(findButtonBy);
            findButton.Click();
        }

        public IReadOnlyCollection<IWebElement> GetSearchResultCollection()
        {
            var containerWait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));

            return containerWait.Until(drv =>
            {
                var elements = drv.FindElements(searchResultsCollectionElementsBy);
                return elements.Count > 0 ? elements : null;
            });

        }
    }
}
