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
        /// Consumes baggeges from assigned gate id, and if no plane is docked, it will put baggs in lost baggage
        /// </summary>
        /// <param name="callback"></param>
        public void ConsumeBaggage(object callback)
        {
            while (true)
            {
                if (Program.TerminalQueues.ContainsKey(GateId) && Monitor.TryEnter(Program.TerminalQueues[GateId]))
                {
                    if (Program.Planes.ContainsKey(GateId) && PlaneDocked)
                    {

                        if (Monitor.TryEnter(Program.Planes[GateId].Baggages))
                        {
                            if (Program.TerminalQueues[GateId].Count() == 0)
                            {
                                Monitor.Wait(Program.TerminalQueues[GateId]);
                            }
                            Baggage baggage = Program.TerminalQueues[GateId].Dequeue();
                            baggage.Log.Add(String.Format("{0} | Baggage send to plane from gate {1}", DateTime.Now, GateId));
                            Program.Planes[GateId].Baggages.Enqueue(baggage);
                            Monitor.Exit(Program.Planes[GateId].Baggages);

                        }
                        
                    }
                    else
                    {
                        if(Monitor.TryEnter(Program.LostBaggage))
                        {
                            while (Program.TerminalQueues[GateId].Count != 0)
                            {
                                Baggage baggage = Program.TerminalQueues[GateId].Dequeue();
                                baggage.Log.Add(String.Format("{0} | Baggage send to lost baggage from gate {1}", DateTime.Now, GateId));
                                Program.LostBaggage.Enqueue(baggage);

                            }
                            Monitor.Exit(Program.LostBaggage);
                            //Program.TerminalQueues.Remove(_terminalId);
                        }
                    }
                    Monitor.Exit(Program.TerminalQueues[GateId]);
                }
                Thread.Sleep(1500);
            }
            
        }
    }
}
