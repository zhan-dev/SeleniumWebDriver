using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BusinessLayer.src
{
    public class MainPage
    {
        private readonly IWebDriver driver;

        private readonly By titleBy = By.TagName("title");
        private readonly By searchButtonBy = By.ClassName("header__icon");
        private readonly By headerSearchPanelBy = By.ClassName("header-search__panel");
        private readonly By searchInputBy = By.Id("new_form_search");
        private readonly By findButtonBy = By.CssSelector(".search-results__action-section > button");
        private readonly By searchResultsCollectionElementsBy = By.ClassName("search-results__item");

        public string Url { get;  } = "https://www.epam.com/";

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
            this.driver.Navigate().GoToUrl(Url);
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
            var waitInput = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
            var activeInput = waitInput.Until(drv =>
            {
                var searchPanel = this.driver.FindElement(headerSearchPanelBy);
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

        public IReadOnlyCollection<IWebElement> GetSearchResultsCollection()
        {
            var containerWait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
            return containerWait.Until(drv =>
            {
                var elements = drv.FindElements(searchResultsCollectionElementsBy);
                return elements.Count > 0 ? elements : null;
            });

        }

        public bool IsSearchResultsValid(IReadOnlyCollection<IWebElement> results, string searchText)
        {
            if (results.Count == 0)
            {
                return false;
            }

            return results.All(key => 
                searchText.Any(keyword => 
                key.Text.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
        }

        public void SearchResultsToConsole(IReadOnlyCollection<IWebElement> results)
        {
            foreach (var result in results)
            {
                var title = result.FindElement(By.CssSelector(".search-results__title-link")).Text;
                var link = result.FindElement(By.CssSelector(".search-results__title-link")).GetAttribute("href");
                var description = result.FindElement(By.CssSelector(".search-results__description")).Text;

                Console.WriteLine($"{title} -> {link}");
                Console.WriteLine(description);
            }
        }
    }
}
