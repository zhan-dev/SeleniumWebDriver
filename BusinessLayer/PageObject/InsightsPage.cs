using CoreLayer;
using CoreLayer.WebDriver;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace BusinessLayer.PageObject
{
    public class InsightsPage : BasePage
    {
        private readonly By mainSliderRightArrowButtonBy = 
            By.XPath("//div[@class='slider section']//div[@class='slider__navigation']//button[contains(@class,'slider__right-arrow')]");
        
        //private readonly By mainSliderBusinessValueTextBy =
        //    By.XPath("//div[@class='slider section']//div[contains(@class, 'single-slide__content')]//div[@class='text']" +
        //        "//p//span[contains(@class, 'museo-sans-light') or contains(@class, 'gradient-text')]");
        private readonly By mainSliderBusinessValueTextBy =
            By.XPath("//div[@class='single-slide__content-container']//span[contains(text(),'Business Value')]");

        private readonly By readTheReportButtonBy = By.XPath("//a[contains(@href, 'ai-report-2025') and @tabindex='0']");
        private readonly By introductionHeaderBy = By.TagName("h1");

        public InsightsPage(WebDriverWrapper driver, Logger logger) : base(driver, logger) { }

        public string GetTitle()
        {
            return this.DriverWrapper.GetTitle();
        }

        public void ClickMainSliderRightArrow()
        {
            var rightArrow = this.DriverWrapper.FindElement(mainSliderRightArrowButtonBy);
            new Actions(this.DriverWrapper.Driver)
                .Click(rightArrow)
                .Pause(TimeSpan.FromSeconds(1))
                .Perform();
        }

        public string MainSliderGetBusinessValueText()
        {
            var spans = this.DriverWrapper.FindElements(mainSliderBusinessValueTextBy);

            return string.Join("", spans.Select(s => s.Text)).Trim();
        }

        public string IntroductionHeaderText()
        {
            base.WaitUntilTitleContains(base.insightsPageTitle);

            return this.DriverWrapper.FindElement(introductionHeaderBy).Text;
        }

        public void ClickReadTheReportButton()
        {
            this.DriverWrapper.FindElement(readTheReportButtonBy).Click();
        }
    }
}
