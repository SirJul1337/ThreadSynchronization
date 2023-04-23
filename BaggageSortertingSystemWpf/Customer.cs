using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaggageSortertingSystemWpf
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

                    if (Monitor.TryEnter(MainWindow.CustomerLine))
                    {
                        if (MainWindow.CustomerLine.Count < 100)
                        {
                            Random r = new Random();
                            MainWindow.CustomerLine.Enqueue(new Baggage("Test", r.Next(1, 3)));
                            Monitor.PulseAll(MainWindow.CustomerLine);
                        }
                        Monitor.Exit(MainWindow.CustomerLine);
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
