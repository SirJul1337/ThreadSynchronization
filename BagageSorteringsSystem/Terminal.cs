using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagageSorteringsSystem
{
    public class Terminal
    {
        private int _terminalId;
        public Terminal(int terminalId)
        {
            _terminalId= terminalId;
        }
        public void ConsumeBaggage(object callback)
        {
            while (true)
            {
                if (Program.TerminalQueues.ContainsKey(_terminalId) &&Monitor.TryEnter(Program.TerminalQueues[_terminalId]))
                {
                    if (Program.Planes.ContainsKey(_terminalId))
                    {
                        //TODO: fix problem with Queue is empty
                        if (Monitor.TryEnter(Program.Planes[_terminalId].Baggages))
                        {
                            if (Program.TerminalQueues[_terminalId].Count() == 0)
                            {
                                Monitor.Wait(Program.TerminalQueues[_terminalId]);
                            }
                            Program.Planes[_terminalId].Baggages.Enqueue(Program.TerminalQueues[_terminalId].Dequeue());
                            Monitor.Exit(Program.Planes[_terminalId].Baggages);

                        }
                        Monitor.Exit(Program.TerminalQueues[_terminalId]);
                    }
                    else
                    {
                        if(Monitor.TryEnter(Program.LostBaggage))
                        {
                            while (Program.TerminalQueues[_terminalId].Count != 0)
                            {
                                Program.LostBaggage.Enqueue(Program.TerminalQueues[_terminalId].Dequeue());

                            }
                            Monitor.Exit(Program.LostBaggage);
                            Program.TerminalQueues.Remove(_terminalId);
                        }
                    }
                }
                Thread.Sleep(2000);
            }
            
        }
    }
}
