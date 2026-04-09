using CoreLayer;
using CoreLayer.WebDriver;

namespace EPAM.Tests
{
    internal abstract class TestSetup
    {
        protected WebDriverWrapper DriverWrapper { get; private set; }
        protected Logger Logger { get; private set; }

        [SetUp]
        public virtual void SetUp()
        {
            ChromeDriverFactory driverFactory = new ChromeDriverFactory();
            var driver = driverFactory.CreateDriver(Configuration.WebBrowserMode);
            this.DriverWrapper = new WebDriverWrapper(driver);
            this.Logger ??= new Logger();
        }

        [TearDown]
        public virtual void TearDown()
        {
            this.DriverWrapper.Close();
        }
    }
}
