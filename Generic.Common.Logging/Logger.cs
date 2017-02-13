using log4net;
using System;
using System.IO;

namespace Generic.Common.Logging
{
    public static class Logger
    {
        #region Members
        private static readonly ILog logger = LogManager.GetLogger(typeof(Logger));
        private static readonly bool enableLog = true;
        #endregion

        #region Constructors
        static Logger()
        {
            var fileInfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            if (fileInfo.Exists)
                log4net.Config.XmlConfigurator.Configure(fileInfo);
        }
        #endregion

        #region Methods

        public static void WriteLog(LogLevel logType, string log)
        {
            if (enableLog)
                switch (logType)
                {
                    case LogLevel.DEBUG:
                        logger.Debug(log);
                        break;
                    case LogLevel.ERROR:
                        logger.Error(log);
                        break;
                    case LogLevel.FATAL:
                        logger.Fatal(log);
                        break;
                    case LogLevel.INFO:
                        logger.Info(log);
                        break;
                    case LogLevel.WARN:
                        logger.Warn(log);
                        break;
                    default:
                        break;
                }
        }

        public static void WriteException(LogLevel logType, string log, Exception ex)
        {
            if (logType.Equals(LogLevel.ERROR))
            {
                logger.Error(log, ex);
            }
        }

        public enum LogLevel
        {
            DEBUG = 0,
            INFO,
            WARN,
            ERROR,
            FATAL
        }

        #endregion
    }
}
