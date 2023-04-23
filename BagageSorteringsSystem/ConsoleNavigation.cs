using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BagageSorteringsSystem
{
    public class ConsoleNavigation
    {
        /// <summary>
        /// Method to see what there is put into the console, used for navigation
        /// </summary>
        public void StartNavigations()
        {
            while (true)
            {
                Program.NavKey = Console.ReadKey().Key;

            }

        }
    }
}
