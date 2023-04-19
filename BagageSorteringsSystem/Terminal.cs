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
                if (Monitor.TryEnter(Program.TerminalQueues[_terminalId]))
                {
                    if (Program.TerminalQueues[_terminalId].Count() == 0)
                    {
                        Monitor.Wait(Program.TerminalQueues[_terminalId]);
                    }
                    if (Monitor.TryEnter(Program.PlaneBaggage[_terminalId]))
                    {
                        Program.PlaneBaggage[_terminalId].Add(Program.TerminalQueues[_terminalId].Dequeue());
                        Monitor.Exit(Program.PlaneBaggage[_terminalId]);
                    }
                    Monitor.Exit(Program.TerminalQueues[_terminalId]);
                }
                Thread.Sleep(1000);
            }
            
        }
    }
}
