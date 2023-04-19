using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten
{
    public abstract class Bottle
    {
        public string Name;
        public Bottle(string name)
        {
            Name = name;
        }
    }
}
