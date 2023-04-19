using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten
{
    public class BottleProducer
    {
        int BottleMinimum;
        public BottleProducer(int bottleMinimum)
        {

            BottleMinimum = bottleMinimum;

        }
        public void MakeBottle(object callback)
        {
            while (true)
            {
                if (Monitor.TryEnter(Program.BottleBelt))
                {
                    if (Program.BottleBelt.Count < BottleMinimum)
                    {
                        Random rand = new Random();
                        int bottleInt = rand.Next(0, 2);
                        switch (bottleInt)
                        {
                            case 0:
                                ColaBottle cola = new ColaBottle();
                                Program.BottleBelt.Enqueue(cola);
                                break;
                            case 1:
                                BeerBottle beer = new BeerBottle(5.4);
                                Program.BottleBelt.Enqueue(beer);
                                break;
                        }
                        Monitor.PulseAll(Program.BottleBelt);
                    }
                    Monitor.Exit(Program.BottleBelt);
                }
                Thread.Sleep(200);

            }
        }
    }
}
