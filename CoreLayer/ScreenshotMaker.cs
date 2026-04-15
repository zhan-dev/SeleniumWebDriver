using OpenQA.Selenium;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace TestFramework.Core.BrowserUtils
{
    public class ScreenshotMaker
    {
        private static readonly string ScreenshotFolder = Path.Combine(Environment.CurrentDirectory, "Screenshots");

        static ScreenshotMaker()
        {
            if (!Directory.Exists(ScreenshotFolder))
            {
                Directory.CreateDirectory(ScreenshotFolder);
            }
        }

        private static string NewScreenshotName
        {
            get { return "_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-fff") + "." + ScreenshotImageFormat; }
        }
        private static ImageFormat ScreenshotImageFormat
        {
            get { return ImageFormat.Jpeg; }
        }
        public static string TakeBrowserScreenshot(ITakesScreenshot driver)
        {
            var now = DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-fff");
            var screenshotPath = Path.Combine(ScreenshotFolder, "Display" + NewScreenshotName);
            driver.GetScreenshot().SaveAsFile(screenshotPath);

            return screenshotPath;
        }
        public static string TakeFullDisplayScreenshot()
        {
            var screenshotPath = Path.Combine(ScreenshotFolder, "FullScreen" + NewScreenshotName);

            using (Bitmap bmpScreenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bmpScreenCapture))
                {
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                     Screen.PrimaryScreen.Bounds.Y,
                                     0, 0,
                                     bmpScreenCapture.Size,
                                     CopyPixelOperation.SourceCopy);
                }
                bmpScreenCapture.Save(screenshotPath, ScreenshotImageFormat);
            }
            return screenshotPath;
        }
    }
}
