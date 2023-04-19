using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten
{
    public class ColaConsumer : IConsumer
    {
        /// <summary>
        /// Method consuming beer from ColaBelt. If Cola.Count is 0 it will wait.
        /// It will dequeue from belt, and put into list called ConsumedColaBottles
        /// GetBottle is implemented by the interface IConsumer
        /// </summary>
        /// <param name="callback"></param>
        public void GetBottle(object callback)
        {
            while (true)
            {
                try
                {
                    if (Monitor.TryEnter(Program.ColaBelt))
                    {
                        if (Program.ColaBelt.Count == 0)
                        {
                            Monitor.Wait(Program.ColaBelt);
                        }
                        if (Monitor.TryEnter(Program.ConsumedColaBottles))
                        {
                            Program.ConsumedColaBottles.Add(Program.ColaBelt.Dequeue());
                            Monitor.Exit(Program.ConsumedColaBottles);
                        }
                        Monitor.Exit(Program.ColaBelt);

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
