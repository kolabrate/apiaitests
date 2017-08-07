using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//article for singleton.
namespace Chuvy.Logger
{
    public sealed class Singleton //make sure the class is sealed as it must not be inherited.
    {

        private  Singleton() //keep the constructor private as this cant be initialised from outside.
        {

        }

        public static Singleton InstanceCreation() 
        {
            
            return new Singleton();
        }

    }
}
