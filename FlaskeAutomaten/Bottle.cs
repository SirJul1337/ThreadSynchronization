using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten
{
    /// <summary>
    /// Abstract class bottle with name variable 
    /// </summary>
    public abstract class Bottle
    {
        public string Name;
        public Bottle(string name)
        {
            Name = name;
        }
    }
}
