using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten
{
    public class ColaConsumer : IConsumer
    {
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
