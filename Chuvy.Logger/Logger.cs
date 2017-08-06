using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chuvy.Logger
{
   public class Logger : ILogger, IDisposable
    {

        private ILogger _logger;
        public Logger(ILogger logger)
        {
            this._logger = logger;
        }
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
            _logger.Info(info);
        }

        public void Error(string info, Exception ex)
        {
            _logger.Error(info,ex);
        }

        public void Fatal(string infor, Exception ex)
        {
            _logger.Fatal(infor,ex);
        }

        public void Dispose()
        {
            _logger = null;
        }
    }
}
