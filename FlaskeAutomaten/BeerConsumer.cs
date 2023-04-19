using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten
{
    public class BeerConsumer : IConsumer
    {
        /// <summary>
        /// Method consuming beer from beerBelt. If beercount is 0 it will wait.
        /// It will dequeue from belt, and put into list called ConsumedBeerBottle
        /// GetBottle is implemented by the interface IConsumer
        /// </summary>
        /// <param name="callback"></param>
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
