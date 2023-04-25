using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagageSorteringsSystem
{
    public class Terminal
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("GateId")]
        public int GateId { get; set; }

        public bool PlaneDocked = false;
        /// <summary>
        /// Consumes baggages from assigned gate id, and if no plane is docked, it will put baggs in lost baggage
        /// </summary>
        /// <param name="callback"></param>
        public void ConsumeBaggage(object callback)
        {
            while (true)
            {
                if (Program.TerminalQueues.ContainsKey(GateId))
                {
                    Baggage baggage = Program.TerminalQueues[GateId].Take();
                    if (Program.Planes.ContainsKey(GateId) && PlaneDocked)
                    {
                        SendToPlaneBaggage(baggage);
                        Thread.Sleep(1500);

                    }
                    else
                    {
                        SendToLostBaggage(baggage);
                    }

                }
            }
        }
        /// <summary>
        /// Method to log on the baggage and send to the planes baggage
        /// </summary>
        /// <param name="baggage"></param>
        private void SendToPlaneBaggage(Baggage baggage)
        {
            baggage.Log.Add(String.Format("{0} | Baggage send to plane from gate {1}", DateTime.Now, GateId));
            Program.Planes[GateId].Baggages.Enqueue(baggage);
        }
        /// <summary>
        /// Method to log on the baggage and send the baggage to the lostBaggage 
        /// </summary>
        /// <param name="baggage"></param>
        private void SendToLostBaggage(Baggage baggage)
        {
            Program.LostBaggage.Add(baggage);
            baggage.Log.Add(String.Format("{0} | Baggage send to lost baggage from gate {1}", DateTime.Now, GateId));
        }
    }

}
