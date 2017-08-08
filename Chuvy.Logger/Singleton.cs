using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// article - https://dzone.com/articles/understanding-and-implementing-singleton-pattern-i
// profiler - https://marketplace.visualstudio.com/items?itemName=JetBrains.dotTrace


namespace Chuvy.Logger
{
    public sealed class Singleton //make sure the class is sealed as it must not be inherited.
    {
        private int _counter;
        private Singleton() //keep the constructor private as this cant be initialised from outside.
        {

        }
        private static readonly Singleton Instance = null;
        static Singleton()
        {
            Instance = new Singleton();
        }
        public static Singleton GetInstance() //is it thread safe. -- check if the instance is already null.
        {
            return Instance;
          // return new Singleton();
        }

        public int ReturnCounter()
        {
            return ++_counter;
        }

    }
}
