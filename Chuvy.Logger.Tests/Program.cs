using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chuvy.Logger;
namespace Chuvy.Logger.Tests
{
    class Program
    {
        private static Logger _logger = new Logger(SqlLogger.Create());
        static void Main(string[] args)
        {
            _logger.Info("Inside Main");
            int[] x = { 1, 2 };
            try
            {
               
                Console.WriteLine(x[5]);
                Console.Read();
            }
            catch (Exception e)
            {
                var prop = new {user = "Anand", param = x};
                _logger.Error(prop, e);
              //  throw;
            }
           
        }
    }
}
