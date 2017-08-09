using System;
using System.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

namespace Chuvy.Logger.Serilog
{
    //article - https://www.codeproject.com/Articles/1041816/Serilog-An-Excellent-Logging-Framework-Integrated
    public sealed class SqlLogger : ILogger
    {
        private global::Serilog.Core.Logger _logger;
        private SqlLogger()
        {

            try
            {
                _logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo
                    .MSSqlServer(ConfigurationManager.ConnectionStrings["SqlLogger"].ConnectionString, "Logs")
                    .CreateLogger();

            }
            catch (Exception e)
            {

                throw(e);
            }
           
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
            _logger.Error(ex, ex.Message.ToString());
           
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
