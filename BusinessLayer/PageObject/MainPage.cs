using CoreLayer;
using CoreLayer.WebDriver;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumKeys = OpenQA.Selenium.Keys;

namespace BusinessLayer.PageObject
{
    public class MainPage : BasePage
    {
        private readonly By searchInputBy = By.Id("new_form_search");
        private readonly By searchButtonBy = By.ClassName("header__icon");
        private readonly By headerSearchPanelBy = By.ClassName("header-search__panel");
        private readonly By searchResultsCollectionElementsBy = By.ClassName("search-results__item");
        private readonly By viewMoreSearchResultsButtonBy = By.ClassName("search-results__view-more");
        private readonly By topNavigationList = By.ClassName("top-navigation__row");
        private readonly By findButtonBy = By.CssSelector(".search-results__action-section > button");
        private readonly By careersLinkBy = By.LinkText("Careers");
        private readonly By insightsPageLinkBy = By.LinkText("Insights");
        private readonly By policiesCodeOfEthicalConductBy = 
            By.XPath("//div[@class='policies']//li/a[contains(text(),'Code of Ethical Conduct')]");

        public string Url { get;  } = "https://www.epam.com/";

        public MainPage(WebDriverWrapper driver, Logger logger) : base(driver, logger) { }

        public void LoadMainPage()
        {
            this.DriverWrapper.NavigateTo(Url);

            base.WaitUntilTitleContains(base.mainPageTitle);
            base.AcceptAllCookie();
        }

        public void GoToCareers()
        {
            this.DriverWrapper.FindChildByParent(this.topNavigationList, this.careersLinkBy).Click();

            base.WaitUntilTitleContains(base.careersPageTitle);
        }

        public void GoToInsightsPage()
        {
            this.DriverWrapper.FindChildByParent(this.topNavigationList, this.insightsPageLinkBy).Click();

            base.WaitUntilTitleContains(base.insightsPageTitle);
        }

        public string GetTitle()
        {
            return this.DriverWrapper.GetTitle();
        }

        public void ClickSearchButton()
        {
            this.DriverWrapper.FindElement(this.searchButtonBy).Click();
        }

        public void InputDataIntoSearchInput(string searchText)
        {
            var waitInput = new WebDriverWait(this.DriverWrapper.Driver, TimeSpan.FromSeconds(10));
            var activeInput = waitInput.Until(drv =>
            {
                var searchPanel = this.DriverWrapper.FindElement(this.headerSearchPanelBy);
                var element = searchPanel.FindElement(this.searchInputBy);

                return element.Displayed && element.Enabled ? element : null;
            });

            this.DriverWrapper.ClickAndSendAction(activeInput, searchText);
        }

        public void ClickFindButton()
        {
            this.DriverWrapper.FindChildByParent(this.headerSearchPanelBy, this.findButtonBy).Click();
        }

        public IReadOnlyCollection<IWebElement> GetSearchResultsCollection()
        {
            //Emulate "FindMore" click
            //ScrollAndClickForAllResults();

            var containerWait = new WebDriverWait(this.DriverWrapper.Driver, TimeSpan.FromSeconds(10));
            return containerWait.Until(drv =>
            {
                var elements = drv.FindElements(this.searchResultsCollectionElementsBy);
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

        public void ScrollToFooter()
        {
            new Actions(this.DriverWrapper.Driver)
                .SendKeys(SeleniumKeys.End)
                .Pause(TimeSpan.FromSeconds(1))
                .Perform();
        }

        public void ClickCodeOfEthicalConductPDFLink()
        {
            var item = this.DriverWrapper.FindElement(this.policiesCodeOfEthicalConductBy);

            new Actions(this.DriverWrapper.Driver)
                .MoveToElement(item)
                .Pause(TimeSpan.FromSeconds(1))
                .Click()
                .Pause(TimeSpan.FromSeconds(1))
                .Perform();
        }

        private void ScrollAndClickForAllResults()
        {
            var waitViewMoreButton = new WebDriverWait(this.DriverWrapper.Driver, TimeSpan.FromSeconds(5));
            while (true)
            {

                new WebDriverWait(this.DriverWrapper.Driver, TimeSpan.FromSeconds(5))
                    .Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

                new Actions(this.DriverWrapper.Driver)
                    .SendKeys(SeleniumKeys.End)
                    .Perform();

                try
                {
                    var showMoreButton = waitViewMoreButton.Until(drv =>
                    {
                        var element = this.DriverWrapper.FindElement(viewMoreSearchResultsButtonBy);

                        return element.Displayed && element.Enabled ? element : null;
                    });

                    new Actions(this.DriverWrapper.Driver)
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
