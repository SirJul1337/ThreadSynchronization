using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagageSorteringsSystem
{
    public class BaggageHandling
    {
        /// <summary>
        /// Used for checking what id a bag is to sort the bag out to the right terminals
        /// </summary>
        /// <param name="callback"></param>
        public void Sorting(object callback)
        {
            while (true)
            {
                try
                {

                    Baggage baggage = Program.Baggages.Take();
                    //Baggage baggage1 = Program.Baggages.Take();
                    int _gateId = baggage.GateId;


                    if (Program.TerminalQueues.ContainsKey(_gateId))
                    {
                        if (Program.TerminalQueues[_gateId].GetConsumingEnumerable().Count() >= 30)
                        {
                            baggage.Log.Add(String.Format("{0} | Arrived in sortingsystem", DateTime.Now));
                            Program.TerminalQueues[_gateId].Add(baggage);
                        }
                    }
                    else
                    {

                        Program.LostBaggage.Add(baggage);
                    }

                }
                finally
                {
                    Thread.Sleep(1000);
                }
            }
        }

    }
}
