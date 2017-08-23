using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apiai.tests
{

    public class PetOwner
    {
        public string name { get; set; }
        public string gender { get; set; }
        public int age { get; set; }
        public Pet[] pets { get; set; }
    }

    public class Pet
    {
        public string name { get; set; }
        public string type { get; set; }
    }


}
