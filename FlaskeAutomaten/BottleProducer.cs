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
        /// <summary>
        /// Cosntructor with parameter with minimum bottle, before stopping/starting generating bottles, 
        /// </summary>
        /// <param name="bottleMinimum"></param>
        public BottleProducer(int bottleMinimum)
        {

            BottleMinimum = bottleMinimum;

        }
        /// <summary>
        /// Method has random int generated 0-1, in the switch case, it will generate Beer or cola depending on the number,
        /// after generation it will pulse all and exit the lock
        /// </summary>
        /// <param name="obj"></param>
        public void MakeBottle(object obj)
        {
            CancellationToken cancellationToken = ((CancellationTokenSource)obj).Token;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {


                    if (Program.BottleBelt.Count < BottleMinimum)
                    {
                        Random rand = new Random();
                        int bottleInt = rand.Next(0, 2);
                        switch (bottleInt)
                        {
                            case 0:
                                ColaBottle cola = new ColaBottle();
                                Program.BottleBelt.Add(cola);
                                break;
                            case 1:
                                BeerBottle beer = new BeerBottle(5.4);
                                Program.BottleBelt.Add(beer);
                                break;
                        }
                        Thread.Sleep(200);
                    }
                }
                finally
                {

                }

            }
        }
    }
}
