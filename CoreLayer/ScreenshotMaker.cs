using OpenQA.Selenium;
using System.Drawing.Imaging;

namespace TestFramework.Core.BrowserUtils
{
    public static class ScreenshotMaker
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
            get 
            { 
                return "_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss-fff") + "." + ScreenshotImageFormat; 
            }
        }
        private static ImageFormat ScreenshotImageFormat
        {
            get 
            { 
                return ImageFormat.Jpeg; 
            }
        }
        public static string TakeBrowserScreenshot(ITakesScreenshot driver)
        {
            var screenshotPath = Path.Combine(ScreenshotFolder, "Display" + NewScreenshotName);
            driver.GetScreenshot().SaveAsFile(screenshotPath);

            return screenshotPath;
        }
        public static string TakeFullDisplayScreenshot()
        {
            var primaryScreen = Screen.PrimaryScreen ?? throw new InvalidOperationException("No primary screen detected.");
            var screenshotPath = Path.Combine(ScreenshotFolder, "FullScreen" + NewScreenshotName);

            using (Bitmap bmpScreenCapture = new(primaryScreen.Bounds.Width, primaryScreen.Bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bmpScreenCapture))
                {
                    g.CopyFromScreen(primaryScreen.Bounds.X,
                                     primaryScreen.Bounds.Y,
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
