using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten
{
    /// <summary>
    /// Class to represent Cola bottle, with superclass Bottle Containing name
    /// </summary>
    public class ColaBottle : Bottle
    {
        public ColaBottle():base("Cola")
        {
            
        }
    }
}
