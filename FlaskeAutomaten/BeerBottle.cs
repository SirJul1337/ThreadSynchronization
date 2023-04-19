using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten
{
    /// <summary>
    /// Class to represent Beer bottle, with superclass Bottle Containing name
    /// Beerbottle contains Alchohol variable
    /// </summary>
    public class BeerBottle:Bottle
    {
        private double Alchohol;
        public BeerBottle(double alchohol) : base("Beer")
        {

            Alchohol = alchohol;

        }
    }
}
