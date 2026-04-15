using Serilog;
using Serilog.Events;

namespace CoreLayer
{
    public class Logger
    {
        private readonly ILogger _logger;

        public Logger()
        {
            var level = Enum.TryParse<LogEventLevel>(Configuration.LoggingLevel, true, out var parsedLevel)
                ? parsedLevel
                : LogEventLevel.Information;

            this._logger = new LoggerConfiguration()
                .MinimumLevel.Is(level)
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        public void Information(string message)
        {
            this._logger.Information("[INFO]: {Message}", message);
        }

        public void Warning(string message)
        {
            this._logger.Warning("[WARNING]: {Message}", message);
        }

        public void Error(string message)
        {
            this._logger.Error("[ERROR]: {Message}", message);
        }
    }
}
