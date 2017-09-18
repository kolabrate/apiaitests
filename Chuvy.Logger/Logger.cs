using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        #region private methods


        public void Verbose(string info)
        {
            _logger.Verbose(info);
        }

        public void Debug(string info)
        {
           _logger.Debug(info);
        }

        public void Info(string info, params object[] data)
        {
            _logger.Info(info,data);
        }

        public void Error(object properties, Exception ex)
        {
            _logger.Error(properties, ex);
        }

        public void Fatal(dynamic infor, Exception ex)
        {
            _logger.Fatal(infor,ex);
        }

        public void Dispose()
        {
            _logger = null;
        }


        #endregion

    }
}
