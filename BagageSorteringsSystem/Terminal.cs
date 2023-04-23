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
                            Program.Planes[GateId].Baggages.Enqueue(Program.TerminalQueues[GateId].Dequeue());
                            Monitor.Exit(Program.Planes[GateId].Baggages);

                        }
                        
                    }
                    else
                    {
                        if(Monitor.TryEnter(Program.LostBaggage))
                        {
                            while (Program.TerminalQueues[GateId].Count != 0)
                            {
                                Program.LostBaggage.Enqueue(Program.TerminalQueues[GateId].Dequeue());

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
