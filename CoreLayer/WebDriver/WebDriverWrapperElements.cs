using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.Runtime.CompilerServices;

namespace CoreLayer.WebDriver
{
    public partial class WebDriverWrapper
    {
        public void Click(By by, int? waitTime = null)
        {
            this.WaitForElementToBePresent(this._driver, by, this.Timeout(waitTime)).Click();
        }

        public void EnterText(By by, string text, int? waitTime = null)
        {
            var element = this.WaitForElementToBePresent(this._driver, by, this.Timeout(waitTime));
            element.Clear();
            element.SendKeys(text);
        }

        public void EnterText(IWebElement element, string text)
        {
            element.Clear();
            element.SendKeys(text);
        }

        public void ClearText(By by, int? waitTime = null)
        {
            var element = this.WaitForElementToBePresent(this._driver, by, this.Timeout(waitTime));
            element.Click();
            element.SendKeys(Keys.Control + "a");
            element.SendKeys(Keys.Delete);
        }

        public IWebElement FindElement(By by, int? waitTime = null)
        {
            return this.WaitForElementToBePresent(this._driver, by, this.Timeout(waitTime));
        }

        public IReadOnlyCollection<IWebElement> FindElements(By by, int? waitTime = null)
        {
            this.WaitForElementToBePresent(this._driver, by, this.Timeout(waitTime));
            return this._driver.FindElements(by);
        }

        public IWebElement FindChildByParent(IWebElement parent, By childBy, int? waitTime = null)
        {
            return WaitForChildElementToBePresent(parent, childBy, this.Timeout(waitTime));
        }

        public IWebElement FindChildByParent(By parentBy, By childBy, int? waitTime = null)
        {
            return WaitForElementToBePresent(this._driver, parentBy, childBy, this.Timeout(waitTime));
        }

        public void ClickAndSendAction(IWebElement element, string textToSend)
        {
            var clickAndSendKeysActions = new Actions(this._driver);
            clickAndSendKeysActions.Click(element)
                .Pause(TimeSpan.FromSeconds(1))
                .SendKeys(textToSend)
                .Pause(TimeSpan.FromSeconds(1))
                .Perform();
        }

        private IWebElement WaitForChildElementToBePresent(IWebElement parent, By childBy, TimeSpan timeout)
        {
            ArgumentNullException.ThrowIfNull(parent);
            ArgumentNullException.ThrowIfNull(childBy);

            var wait = new DefaultWait<IWebElement>(parent)
            {
                Timeout = timeout,
                PollingInterval = TimeSpan.FromMilliseconds(500)
            };
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            try
            {
                return wait.Until(p =>
                {
                    var element = p.FindElement(childBy);
                    return element.Displayed ? element : null;
                });
            }
            catch (WebDriverTimeoutException ex)
            {
                this.Logger(ex);
                throw;
            }
        }

        private IWebElement WaitForElementToBePresent(IWebDriver driver, By parentBy, By childBy, TimeSpan timeout)
        {
            ArgumentNullException.ThrowIfNull(driver);
            ArgumentNullException.ThrowIfNull(parentBy);
            ArgumentNullException.ThrowIfNull(childBy);

            var wait = new WebDriverWait(driver, timeout);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            try
            {
                return wait.Until(drv =>
                {
                    var parent = drv.FindElement(parentBy);
                    if (!parent.Displayed)
                    {
                        return null;
                    }
                    
                    var element = parent.FindElement(childBy);
                    return element.Displayed ? element : null;
                });
            }
            catch (WebDriverTimeoutException ex)
            {
                this.Logger(ex);
                throw;
            }
        }

        private IWebElement WaitForElementToBePresent(IWebDriver driver, By by, TimeSpan timeout)
        {
            ArgumentNullException.ThrowIfNull(driver);
            ArgumentNullException.ThrowIfNull(by);

            var wait = new WebDriverWait(driver, timeout);
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(StaleElementReferenceException));

            try
            {
                return wait.Until(drv =>
                {
                    var element = drv.FindElement(by);
                    return element.Displayed ? element : null;
                });
            }
            catch (WebDriverTimeoutException ex)
            {
                this.Logger(ex);
                throw;
            }
        }

        private void Logger(Exception ex, [CallerMemberName] string caller = "")
        {
            Console.WriteLine($"[{caller}] {ex.Message}");
        }

        private TimeSpan Timeout(int? waitTime)
        {
            return waitTime is null ? this._timeout : TimeSpan.FromSeconds(waitTime.Value);
        }
    }  
}
