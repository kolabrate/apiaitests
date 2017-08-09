using System;
using System.Configuration;
using Serilog;


namespace Chuvy.Logger
{
    //article - https://www.codeproject.com/Articles/1041816/Serilog-An-Excellent-Logging-Framework-Integrated
    public sealed class SqlLogger : ILogger
    {

        private Serilog.ILogger _logger;
        private SqlLogger()
        {
                _logger = new LoggerConfiguration().WriteTo.MSSqlServer(ConfigurationManager.ConnectionStrings["SqlLogger"].ConnectionString, "Logs")
                    .CreateLogger();
  
        }

        #region Singleton Instantiation
        private static SqlLogger instance = null;
        static SqlLogger()
        {
            if (instance == null)
            {
                instance = new SqlLogger();
            }
        }

        public static SqlLogger Create()
        {
            return instance;
        }
        #endregion
        #region private methods.

        private int _counter;
        public void Verbose(string info)
        {
            _logger.Verbose(info);
        }

        public void Debug(string info)
        {
            _logger.Debug(info);
        }

        public void Info(string info)
        {
            _logger.Information(info);
        }

        public void Error(dynamic infor, Exception ex)
        {
            _logger.Error(ex, ex.StackTrace);
           
        }

        public void Fatal(dynamic infor, Exception ex)
        {
            throw new NotImplementedException();
        }

        //sample method for checking singleton
        public int ReturnCounter()
        {

            return ++_counter;
        }

        #endregion
    }
}
