using System;
namespace Chuvy.Logger.Tests
{
    class Program
    {
        private static Logger _logger = new Logger(SqlLogger.Create());
        static void Main(string[] args)
        {
            test t = new test() { param = "Priya", Address = "7 southksjdfk, Vic 3000" };
            _logger.Info("trying to retrieve {@T} records", t);
            int[] x = { 1, 2 };
            try
            {
               
                Console.WriteLine(x[5]);
                Console.Read();
            }
            catch (Exception e)
            {
                var prop = new {user = "Anand", param = x};
                test t1 = new test() { param = "Priya", Address = "7 southksjdfk, Vic 3000" };
                _logger.Error(t,e);
              //  throw;
            }
           
        }
    }
    public class test
    {
        public string param { get; set; }
        public string Address { get; set; }
    }
}
