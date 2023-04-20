using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagageSorteringsSystem
{
    public class BaggageHandling
    {
        public void Sorting(object callback)
        {
            while (true)
            {
                try
                {

                    if (Monitor.TryEnter(Program.Baggages))
                    {
                        if (Program.Baggages.Count == 0)
                        {
                            Monitor.Wait(Program.Baggages);
                        }
                        int _gateId = Program.Baggages.Peek().GateId;
                        if (Monitor.TryEnter(Program.TerminalQueues))
                        {

                            if (Program.TerminalQueues.ContainsKey(_gateId))
                            {
                                if (Monitor.TryEnter(Program.TerminalQueues[_gateId]))
                                {

                                    if (Program.TerminalQueues[_gateId].Count() < 30)
                                    {
                                        Program.TerminalQueues[_gateId].Enqueue(Program.Baggages.Dequeue());
                                    }
                                    Monitor.PulseAll(Program.TerminalQueues[_gateId]);
                                    Monitor.Exit(Program.TerminalQueues[_gateId]);
                                }

                            }
                            else
                            {
                                if (Monitor.TryEnter(Program.LostBaggage))
                                {
                                    Program.LostBaggage.Enqueue(Program.Baggages.Dequeue());
                                    Monitor.Exit(Program.LostBaggage);
                                    
                                }
                            }
                            Monitor.Exit(Program.TerminalQueues);
                        }
                        Monitor.Exit(Program.Baggages);

                    }
                }
                finally
                {
                    Thread.Sleep(1000);
                }
            }
        }
        public void LostBaggage()
        {

        }
    }
}
