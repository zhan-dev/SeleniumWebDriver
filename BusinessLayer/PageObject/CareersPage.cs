using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BusinessLayer.PageObject
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
        private readonly By searchResultsElementsBy = By.ClassName("JobCard_panel__gTD7e");
        private readonly By revealArrowBy = By.CssSelector("[data-testid='accordion-section-header-icon']");
        private readonly By requirementsContainerBy = By.CssSelector("[data-testid='accordion-section-children-container']");
        private readonly By requirementsListBy = By.CssSelector("[data-testid='categories-container']");
        private readonly By requirementsElementBy = By.CssSelector("[data-testid='job-details-category-container']");

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

        public void ExpandLastElement()
        {
            var wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(2));
            var revealArrow = wait.Until(drv =>
            {
                var elements = drv.FindElements(searchResultsElementsBy);
                var last = elements.LastOrDefault();
                if (last is null)
                {
                    return null;
                }
                try
                {
                    return last.FindElement(revealArrowBy);
                }
                catch (StaleElementReferenceException)
                {
                    return null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });

            revealArrow.Click();

            wait.Until(drv =>
            {
                var containers = drv.FindElements(requirementsContainerBy);
                return containers.Count > 0 && containers.Last().Displayed;
            });
        }

        public bool ValidateLastElementContains(string searchText)
        {
            var wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));

            var expandedText = wait.Until(drv =>
            {
                var containers = drv.FindElement(requirementsListBy);
                var categoryBlocks = containers.FindElements(requirementsElementBy);

                if (categoryBlocks.Count == 0)
                {
                    return null;
                }

                var result = new List<string>();

                foreach (var block in categoryBlocks)
                {
                    var heading = block.FindElement(By.CssSelector(".Category_heading__s3H5z")).Text;
                    Console.WriteLine("Section: " + heading);

                    var items = block.FindElements(By.CssSelector("ul li [data-testid='rich-text']"));
                    foreach (var item in items)
                    {
                        var text = item.GetAttribute("textContent");
                        Console.WriteLine(" - " + text);
                        result.Add(text);
                    }
                }

                return string.Join(" ", result);
            });

            return expandedText is not null && expandedText.Contains(searchText, StringComparison.OrdinalIgnoreCase);
        }
    }
}
