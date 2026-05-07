using Microsoft.Extensions.Configuration;

namespace CoreLayer;

public static class Configuration
{
    public static string BrowserType { get; private set; } = "Chrome";
    public static string AppUrl { get; private set; } = string.Empty;
    public static string TestDataPath { get; private set; } = string.Empty;
    public static string LoggingLevel { get; private set; } = "Information";
    public static bool IsHeadless { get; private set; } = false;

    static Configuration() => Init();

    public static void Init()
    {
        string? browserEnv = Environment.GetEnvironmentVariable("BROWSER");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        BrowserType = !string.IsNullOrWhiteSpace(browserEnv)
        ? browserEnv
        : configuration["Application:BrowserType"] ?? BrowserType;
        Console.WriteLine($"[DEBUG] Browser from env: {browserEnv}");
        AppUrl = configuration["Application:ApplicationUrl"] ?? AppUrl;
        TestDataPath = configuration["Application:TestDataPath"] ?? TestDataPath;
        LoggingLevel = configuration["Logging:MinimumLevel:Default"] ?? LoggingLevel;

        string headless = configuration["WebBrowserMode:IsHeadless"] ?? IsHeadless.ToString();
        IsHeadless = bool.TryParse(headless, out bool result) && result;
    }
}