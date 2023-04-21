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
        public void StartNavigations()
        {
            while (true)
            {
                Program.NavKey = Console.ReadKey().Key;

            }

        }
    }
}
