using System;
using System.Runtime.CompilerServices;
using System.Text;
using NLog;

namespace PhotoAlbum.Logging
{
    public static class LogExt
    {
        private static Logger _logger; //NLog logger

        public static void Initialize()
        {
            _logger = LogManager.GetLogger("Default") ?? LogManager.GetCurrentClassLogger();
            var config = new NLog.Config.LoggingConfiguration();
            var logFile = new NLog.Targets.FileTarget()
            {
                FileName = "nlog.log",
                Name = "logfile",
                Layout = "${level:upperCase=true}:${message}${exception:format=ToString}"
            };
            var logConsole = new NLog.Targets.ColoredConsoleTarget()
            {
                Name = "logconsole",
                Layout = "${level:upperCase=true}:${message}"
            };

            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Trace, logConsole));
            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Debug, logFile));

            LogManager.Configuration = config;
            typeof(LogExt).Trace($"LogExt started. LogExt file: {logFile.FileName}");
        }

        public static void Trace<T>(
            this T caller,
            string message = "",
            Exception ex = null,
            [CallerMemberName] string callerFunction = "")
        {
            if (!_logger.IsTraceEnabled) return;

            var line = BuildLogLine(caller,  message, callerFunction);
            _logger.Trace(ex, line);
        }

        public static void Info<T>(
            this T caller,
            string message,
            Exception ex = null,
            [CallerMemberName] string callerFunction = "")
        {
            if (!_logger.IsInfoEnabled) return;

            var line = BuildLogLine(caller,  message, callerFunction);
            _logger.Info(ex, line);
        }

        public static void Warn<T>(
            this T caller,
            string message,
            Exception ex = null,
            [CallerMemberName] string callerFunction = "")
        {
            if (!_logger.IsWarnEnabled) return;

            var line = BuildLogLine(caller,  message, callerFunction);
            _logger.Warn(ex, line);
        }

        public static void Warn<T>(
            this T caller,
            Exception ex,
            [CallerMemberName] string callerFunction = "")
        {
            if (!_logger.IsWarnEnabled) return;

            var line = BuildLogLine(caller,  ex.Message, callerFunction);
            _logger.Warn(ex, line);
        }


        public static void Error<T>(
            this T caller,
            Exception ex,
            [CallerMemberName] string callerFunction = "")
        {
            if (!_logger.IsErrorEnabled) return;

            var line = BuildLogLine(caller,  ex.Message, callerFunction);
            _logger.Error(ex, line);
        }

        public static void Error<T>(
            this T caller,
            string message,
            Exception ex,
            [CallerMemberName] string callerFunction = "")
        {
            if (!_logger.IsErrorEnabled) return;

            var line = BuildLogLine(caller, message, callerFunction);
            _logger.Error(ex, line);
        }

        private static string BuildLogLine<T>(
            T caller,
            string message = "",
            string callerFunction = "")
        {
            StringBuilder sb = new StringBuilder();

            var type = caller as Type;
            var callerName = (type != null ? type : caller.GetType()).Name;
            sb.Append($@" {callerName}");

            if (!String.IsNullOrWhiteSpace(callerFunction))
                sb.Append($@".{callerFunction}");

            if (!String.IsNullOrWhiteSpace(message))
                sb.Append($@": {message}");

            return sb.ToString();
        }
    }
}
