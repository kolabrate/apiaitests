using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chuvy.Logger
{
    public interface ILogger
    {
        void Verbose(string info); //example : calculated cvv number for a customer
        void Debug(string info); // example : retrieved cvv number
        void Info(string info); // example : finsihed calc cvv
        void Error(dynamic info, Exception ex); //error : something went wrong
        void Fatal(dynamic info, Exception ex); // error : terminating.

    }
}
