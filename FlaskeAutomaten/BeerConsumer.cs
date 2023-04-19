using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten
{
    public class BeerConsumer : IConsumer
    {
        public void GetBottle(object callback)
        {
            while (true)
            {
                try
                {
                    if (Monitor.TryEnter(Program.BeerBelt))
                    {
                        if (Program.BeerBelt.Count == 0)
                        {
                            Monitor.Wait(Program.BeerBelt);
                        }
                        if (Monitor.TryEnter(Program.ConsumedBeerBottle))
                        {
                            Program.ConsumedBeerBottle.Add(Program.BeerBelt.Dequeue());
                            Monitor.Exit(Program.ConsumedBeerBottle);
                        }
                        Monitor.Exit(Program.BeerBelt);

                    }
                }
                finally
                {
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
