using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace BusinessLayer.src
{
    public class MainPage
    {
        private readonly IWebDriver driver;

        private readonly By titleBy = By.TagName("title");
        private readonly By searchInputBy = By.Id("new_form_search");
        private readonly By acceptAllCookieBy = By.Id("onetrust-accept-btn-handler");
        private readonly By searchButtonBy = By.ClassName("header__icon");
        private readonly By headerSearchPanelBy = By.ClassName("header-search__panel");
        private readonly By searchResultsCollectionElementsBy = By.ClassName("search-results__item");
        private readonly By viewMoreSearchResultsButtonBy = By.ClassName("search-results__view-more");
        private readonly By topNavigationList = By.ClassName("top-navigation__row");
        private readonly By findButtonBy = By.CssSelector(".search-results__action-section > button");
        private readonly By careersLinkBy = By.LinkText("Careers");

        public string Url { get;  } = "https://www.epam.com/";

        public MainPage(IWebDriver driver)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(driver));
            this.driver = driver;
        }

        public void AcceptAllCookie()
        {
            var wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
            wait.Until(drv =>
            {
                try
                {
                    var element =  drv.FindElement(acceptAllCookieBy);
                    return (element.Displayed && element.Enabled) ? element : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            }).Click();
        }

        public void MaximizeWindow()
        {
            this.driver.Manage().Window.Maximize();
        }

        public void GoToMainPage()
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

        public void ClickSearchButton()
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

            var clickAndSendKeysActions = new Actions(this.driver);
            clickAndSendKeysActions.Click(activeInput)
                .Pause(TimeSpan.FromSeconds(0.5))
                .SendKeys(searchText)
                .Pause(TimeSpan.FromSeconds(0.5))
                .Perform();
        }

        public void ClickFindButton()
        {
            var searchPanel = this.driver.FindElement(headerSearchPanelBy);
            var findButton = searchPanel.FindElement(findButtonBy);
            findButton.Click();
        }

        public IReadOnlyCollection<IWebElement> GetSearchResultsCollection()
        {
            //Emulate "FindMore" click
            //ScrollAndClicForAllResults();

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

            var keywords = searchText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            return results.All(key =>
                keywords.Any(word =>
                    key.Text.Contains(word, StringComparison.OrdinalIgnoreCase)));
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

        public void GoToCareersViaClick()
        {
            var navList = NavigationList();
            var careersLink = navList.FindElement(careersLinkBy);
            careersLink.Click();
        }

        private IWebElement NavigationList()
        {
            return this.driver.FindElement(topNavigationList);
        }

        private void ScrollAndClicForAllResults()
        {
            var waitViewMoreButton = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
            while (true)
            {

                new WebDriverWait(this.driver, TimeSpan.FromSeconds(5))
                    .Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

                new Actions(this.driver)
                    .SendKeys(Keys.End)
                    .Perform();

                try
                {
                    var showMoreButton = waitViewMoreButton.Until(drv =>
                    {
                        var element = this.driver.FindElement(viewMoreSearchResultsButtonBy);

                        return (element.Displayed && element.Enabled) ? element : null;
                    });

                    new Actions(this.driver)
                        .MoveToElement(showMoreButton)
                        .Pause(TimeSpan.FromSeconds(1))
                        .Click(showMoreButton)
                        .Perform();
                }

                catch (WebDriverTimeoutException)
                {
                    break;
                }
            }
        }
    }
}
