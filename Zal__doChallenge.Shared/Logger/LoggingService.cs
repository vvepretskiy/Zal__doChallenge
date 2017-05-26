using System;
using MetroLog;
using MetroLog.Targets;

namespace Zal__doChallenge.Shared.Logger
{
	public class LoggingService
	{

		public static LoggingService Instance { get; }

		public static int RetainDays { get; } = 3;

		public static bool Enabled { get; } = true;

		static LoggingService()
		{
			Instance = Instance ?? new LoggingService();

#if DEBUG
			LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new FileStreamingTarget { RetainDays = RetainDays });
#else
			LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Info, LogLevel.Fatal, new FileStreamingTarget {RetainDays = RetainDays});
#endif
		}

		public void WriteLine<T>(string message, LogLevel logLevel = LogLevel.Trace, Exception ex = null)
		{
			if (Enabled)
			{
				var logger = LogManagerFactory.DefaultLogManager.GetLogger<T>();

				if (logLevel == LogLevel.Trace && logger.IsTraceEnabled)
				{
					logger.Trace(message, ex);
				}
				if (logLevel == LogLevel.Debug && logger.IsDebugEnabled)
				{
					System.Diagnostics.Debug.WriteLine($"{DateTime.Now.TimeOfDay} {message}");
					logger.Debug(message, ex);
				}
				if (logLevel == LogLevel.Error && logger.IsErrorEnabled)
				{
					logger.Error(message, ex);
				}
				if (logLevel == LogLevel.Fatal && logger.IsFatalEnabled)
				{
					logger.Fatal(message, ex);
				}
				if (logLevel == LogLevel.Info && logger.IsInfoEnabled)
				{
					logger.Info(message, ex);
				}
				if (logLevel == LogLevel.Warn && logger.IsWarnEnabled)
				{
					logger.Warn(message, ex);
				}
			}
		}
	}
}
