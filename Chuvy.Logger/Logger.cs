﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chuvy.Logger
{
   public class Logger : ILogger
    {

        private ILogger _logger;
        public Logger(ILogger logger)
        {
            this._logger = logger;
        }
        public void Verbose(string info)
        {
            
        }

        public void Debug(string info)
        {
            throw new NotImplementedException();
        }

        public void Info(string info)
        {
            throw new NotImplementedException();
        }

        public void Error(string info, Exception ex)
        {
            throw new NotImplementedException();
        }

        public void Fatal(string infor, Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
