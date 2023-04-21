using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagageSorteringsSystem
{
    public class Customer
    {
        public Customer()
        {
            
        }

        public void LoadFromFile()
        {

        }
        public void AutoGenerate(object callback)
        {
            while (true)
            {
                try
                {

                    if (Monitor.TryEnter(Program.CustomerLine))
                    {
                        if (Program.CustomerLine.Count < 100)
                        {
                            Random r = new Random();
                            Program.CustomerLine.Enqueue(new Baggage("Test", r.Next(1, 3)));
                            Monitor.PulseAll(Program.CustomerLine);
                        }
                        Monitor.Exit(Program.CustomerLine);
                    }
                }
                finally
                {
                    Thread.Sleep(200);
                }
            }
        }
    }
}
