using Serilog.Context;
using webapi_bilheteria_c.Domain.Interface;

namespace webapi_bilheteria_c.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> _logger;
        private const string ApplicationName = "web-api-bilheteria";
        public LoggerService(ILogger<LoggerService> logger){
            _logger = logger;
        }

        public async Task LogInformation(string message){
            var date = DateTime.Now;
            using (LogContext.PushProperty("ApplicationName", ApplicationName))
            using (LogContext.PushProperty("LogMessage", message))
            using (LogContext.PushProperty("Date", date)){
                await Task.Run(() => _logger.LogInformation(
                    $"Log level: Information ApplicationName: {ApplicationName} LogMessage: {message} Date: {date}"
                ));
            }
        }

        public async Task LogWarning(string message){
            var date = DateTime.Now;
            using (LogContext.PushProperty("ApplicationName", ApplicationName))
            using (LogContext.PushProperty("LogMessage", message))
            using (LogContext.PushProperty("Date", date)){
                await Task.Run(() => _logger.LogWarning(
                    $"Log level: Information ApplicationName: {ApplicationName} LogMessage: {message} Date: {date}"
                ));
            }
        }

        public async Task LogError(string message){
            var date = DateTime.Now;
            using (LogContext.PushProperty("ApplicationName", ApplicationName))
            using (LogContext.PushProperty("LogMessage", message))
            using (LogContext.PushProperty("Date", date)){
                await Task.Run(() => _logger.LogError(
                    $"Log level: Information ApplicationName: {ApplicationName} LogMessage: {message} Date: {date}"
                ));
            }
        }
    }
}