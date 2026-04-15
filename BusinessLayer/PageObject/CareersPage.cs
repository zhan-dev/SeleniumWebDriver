using CoreLayer;
using CoreLayer.WebDriver;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumKeys = OpenQA.Selenium.Keys;

namespace BusinessLayer.PageObject
{
    public class CareersPage : BasePage
    {
        private readonly By remoteCheckboxBy = By.XPath("//label[span[text()='Remote']]");
        private readonly By inputCountryBy = By.CssSelector("input[role='combobox'][aria-label='Choose your country']");
        private readonly By declineButtonBy = By.XPath("//div[contains(@class,'dropdown__clear-indicator')]");
        private readonly By searchDivWrapperBy = By.Id("anchor-list-wrapper");
        private readonly By searchInputBy = By.Name("search");
        private readonly By searchFormButtonBy = By.Name("submit_search_box_button");
        private readonly By startYourSearchButtonBy = By.CssSelector("a.button-body");
        private readonly By searchResultsElementsBy = By.ClassName("JobCard_panel__gTD7e");
        private readonly By revealArrowBy = By.CssSelector("[data-testid='accordion-section-header-icon']");
        private readonly By requirementsContainerBy = By.CssSelector("[data-testid='accordion-section-children-container']");
        private readonly By requirementsListBy = By.CssSelector("[data-testid='categories-container']");
        private readonly By requirementsElementBy = By.CssSelector("[data-testid='job-details-category-container']");

        public CareersPage(WebDriverWrapper driver, Logger logger) : base(driver, logger) { }

        public string GetTitle()
        {
            return this.DriverWrapper.GetTitle();
        }

        public void ClickStartYourSearchHereButton()
        {
            this.DriverWrapper.FindElement(this.startYourSearchButtonBy).Click();
            base.AcceptAllCookie();
        }

        public void EnterTextToSearchInput(string searchText)
        {
            this.DriverWrapper.FindChildByParent(this.searchDivWrapperBy, this.searchInputBy)
                .SendKeys(searchText);
        }

        public void EnterTextToCountryInput(string searchCountry)
        {
            var waitBtn = new WebDriverWait(this.DriverWrapper.Driver, TimeSpan.FromSeconds(10));
            waitBtn.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
            try
            {
                waitBtn.Until(drv =>
                {
                    var element = drv.FindElement(this.declineButtonBy);
                    if (element.Displayed && element.Enabled)
                    {
                        element.Click();
                        return true;
                    }

                    return false;
                });
            }
            catch (WebDriverTimeoutException ex)
            {
                this.Logger.Error(ex.Message);
                throw;
            }

            var waitInput = new WebDriverWait(this.DriverWrapper.Driver, TimeSpan.FromSeconds(10));
            waitInput.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            try
            {
                waitInput.Until(drv =>
                {
                    var element = drv.FindElement(this.inputCountryBy);

                    if (element.Displayed && element.Enabled)
                    {
                        element.Click();
                        element.SendKeys(searchCountry);
                        element.SendKeys(SeleniumKeys.Enter);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
            catch (WebDriverTimeoutException ex)
            {
                this.Logger.Error(ex.Message);
                throw;
            }
        }

        public void ClickFindButton()
        {
            this.DriverWrapper.FindChildByParent(this.searchDivWrapperBy, this.searchFormButtonBy).Click();
        }

        public void AddRemoteFilter()
        {
            var wait = new WebDriverWait(this.DriverWrapper.Driver, TimeSpan.FromSeconds(10));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));
            wait.Until(drv =>
            {
                var element = drv.FindElement(this.remoteCheckboxBy);
                if(element.Displayed && element.Enabled)
                {
                    element.Click();
                    return true;
                }
                return false;
            });
        }

        public void ExpandLastElement()
        {
            var wait = new WebDriverWait(this.DriverWrapper.Driver, TimeSpan.FromSeconds(5));
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            try
            {
                wait.Until(drv =>
                {
                    var elements = drv.FindElements(this.searchResultsElementsBy);
                    var last = elements.LastOrDefault();
                    if (last is null)
                    {
                        return false;
                    }
                    var arrow = last.FindElement(this.revealArrowBy);
                    if (arrow.Displayed && arrow.Enabled)
                    {
                        arrow.Click();
                        return true;
                    }
                    return false;
                });
            }
            catch (WebDriverTimeoutException ex)
            {
                this.Logger.Error(ex.Message);
                throw;
            }

            wait.Until(drv =>
            {
                var containers = drv.FindElements(this.requirementsContainerBy);
                return containers.Count > 0 && containers.Last().Displayed;
            });
        }

        public bool ValidateLastElementContains(string searchText)
        {
            var wait = new WebDriverWait(this.DriverWrapper.Driver, TimeSpan.FromSeconds(5));

            var expandedText = wait.Until(drv =>
            {
                var containers = drv.FindElement(this.requirementsListBy);
                var categoryBlocks = containers.FindElements(this.requirementsElementBy);

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
