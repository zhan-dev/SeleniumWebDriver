using CoreLayer.Enums;
using Microsoft.Extensions.Configuration;

namespace CoreLayer;

public static class Configuration
{
    public static string BrowserType { get; private set; } = "Chrome";
    public static string AppUrl { get; private set; } = string.Empty;
    public static string TestDataPath { get; private set; } = string.Empty;
    public static string LoggingLevel { get; private set; } = "Information";
    public static WebBrowserMode WebBrowserMode { get; private set; } = WebBrowserMode.UXUI;

    static Configuration() => Init();

    public static void Init()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        BrowserType = configuration["Application:BrowserType"] ?? BrowserType;
        AppUrl = configuration["Application:ApplicationUrl"] ?? AppUrl;
        TestDataPath = configuration["Application:TestDataPath"] ?? TestDataPath;
        LoggingLevel = configuration["Logging:MinimumLevel:Default"] ?? LoggingLevel;

        var modeString = configuration["WebBrowserMode:Default"] ?? WebBrowserMode.ToString();
        if (Enum.TryParse<WebBrowserMode>(modeString, true, out var mode))
        {
            WebBrowserMode = mode;
        }
    }
}