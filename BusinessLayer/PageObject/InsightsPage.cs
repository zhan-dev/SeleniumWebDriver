using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace BusinessLayer.PageObject
{
    public class InsightsPage
    {
        private readonly IWebDriver driver;

        private readonly By titleBy = By.TagName("title");
        private readonly By mainSliderRightArrowButtonBy = 
            By.XPath("//div[@class='slider section']//div[@class='slider__navigation']//button[contains(@class,'slider__right-arrow')]");
        private readonly By mainSliderBusinessValueTextBy =
            By.XPath("//div[@class='slider section']//div[contains(@class, 'single-slide__content')]//div[@class='text']" +
                "//p//span[contains(@class, 'museo-sans-light') or contains(@class, 'gradient-text')]");
        private readonly By readTheReportButtonBy = By.XPath("//a[contains(@href, 'ai-report-2025') and @tabindex='0']");
        private readonly By introductionHeaderBy = By.TagName("h1");

        public InsightsPage(IWebDriver driver)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(driver));

            this.driver = driver;
        }

        public string GetTitle()
        {
            return this.driver.Title;
        }

        public void ClickMainSliderRightArrow()
        {
            var rightArrow = this.driver.FindElement(mainSliderRightArrowButtonBy);
            new Actions(this.driver)
                .Click(rightArrow)
                .Pause(TimeSpan.FromSeconds(1))
                .Perform();
        }

        public string MainSliderGetBusinessValueText()
        {
            var spans = this.driver.FindElements(mainSliderBusinessValueTextBy);

            return string.Join("", spans.Select(s => s.Text)).Trim();
        }

        //public string MainSliderGetBusinessValueText()
        //{
        //    var spans = this.driver.FindElements(mainSliderBusinessValueTextBy);

        //    return string.Join("", spans.Select(s => s.Text)).Trim();
        //}

        public string IntroductionHeaderText()
        {
            WaitUntilTitleIsPresented();

            var headerText = this.driver.FindElement(introductionHeaderBy).Text;

            return headerText;
        }

        private void WaitUntilTitleIsPresented()
        {
            var titleWait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(5));
            titleWait.Until(driver => driver.FindElement(this.titleBy));
        }

        public void ClickReadTheReportButton()
        {
            this.driver.FindElement(readTheReportButtonBy).Click();
        }
    }
}
