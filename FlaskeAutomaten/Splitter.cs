using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten
{
    public class Splitter
    {
        public void SplitBottles(object callback)
        {
            while (true)
            {

                if (Monitor.TryEnter(Program.BottleBelt))
                {
                    if (Program.BottleBelt.Count == 0)
                    {
                        Monitor.Wait(Program.BottleBelt);
                    }
                    switch (Program.BottleBelt.Peek().Name)
                    {
                        case "Cola":
                            if (Monitor.TryEnter(Program.ColaBelt))
                            {
                                if(Program.ColaBelt.Count < 10)
                                {
                                    Program.ColaBelt.Enqueue((ColaBottle)Program.BottleBelt.Dequeue());
                                    Monitor.PulseAll(Program.ColaBelt);
                                }
                                Monitor.Exit(Program.ColaBelt);
                            }
                            break;
                        case "Beer":
                            if (Monitor.TryEnter(Program.BeerBelt))
                            {
                                if(Program.BeerBelt.Count < 10)
                                {
                                    Program.BeerBelt.Enqueue((BeerBottle)Program.BottleBelt.Dequeue());
                                    Monitor.PulseAll(Program.BeerBelt);
                                }
                                Monitor.Exit(Program.BeerBelt);
                            }
                            break;
                        default:
                            break;
                    }
                    Monitor.Exit(Program.BottleBelt);
                    Thread.Sleep(200);
                }
            }
        }
    }
}
