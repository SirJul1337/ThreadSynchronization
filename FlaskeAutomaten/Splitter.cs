using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten
{
    public class Splitter : IDisposable
    {
        /// <summary>
        /// This method will measure on the bottle object property name, and split to the specific Cola or Beerbelt.¨
        /// It will wait if there is not bottles on the belt
        /// </summary>
        /// <param name="obj"></param>
        public void SplitBottles(object obj)
        {
            CancellationToken cancellationToken = ((CancellationTokenSource)obj).Token;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Bottle bottle = Program.BottleBelt.Take();
                    switch (bottle.Name)
                    {
                        case "Cola":
                            while (Program.ColaBelt.Count >= 10)
                            {

                            }
                            Program.ColaBelt.Add((ColaBottle)bottle);
                            Program.Logger.Information("Bottle send to Cola buffer");
                            break;
                        case "Beer":

                            while (Program.BeerBelt.Count >= 10)
                            {
                                
                            }
                            Program.BeerBelt.Add((BeerBottle)bottle);
                            Program.Logger.Information("Bottle send to Beer buffer");

                            break;
                        default:
                            break;
                    }
                }
                finally
                {
                    Thread.Sleep(200);
                }

            }
        }
        public void Dispose()
        {

        }
    }
}
