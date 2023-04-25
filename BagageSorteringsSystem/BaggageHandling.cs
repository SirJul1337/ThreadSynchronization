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
                Baggage baggage = AirPortManager.Baggages.Take();
                //Baggage baggage1 = Program.Baggages.Take();
                int _gateId = baggage.GateId;


                if (AirPortManager.TerminalQueues.ContainsKey(_gateId))
                {
                    //while (AirPortManager.TerminalQueues[_gateId].GetConsumingEnumerable().Count() >= 30)
                    //{
                    //}
                    //TODO: find a way to wait if count is 30 or above
                    baggage.Log.Add(String.Format("{0} | Arrived in sortingsystem", DateTime.Now));
                    AirPortManager.TerminalQueues[_gateId].Add(baggage);
                }
                else
                {

                    AirPortManager.LostBaggage.Add(baggage);
                }
                Thread.Sleep(1000);

            }
        }

    }
}
