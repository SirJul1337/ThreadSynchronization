using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten
{
    public class ConsoleReader
    {
        public void ReadConsole(object callback)
        {
            while (true)
            {
                Program.ConsoleKey = Console.ReadKey().Key;

            }
        }
    }
}
