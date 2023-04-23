using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaggageSortertingSystemWpf
{
    public class Terminal
    {
        private int _terminalId;
        public Terminal(int terminalId)
        {
            _terminalId = terminalId;
        }
        public void ConsumeBaggage(object callback)
        {
            while (true)
            {
                if (MainWindow.TerminalQueues.ContainsKey(_terminalId) && Monitor.TryEnter(MainWindow.TerminalQueues[_terminalId]))
                {
                    if (MainWindow.Planes.ContainsKey(_terminalId))
                    {

                        if (Monitor.TryEnter(MainWindow.Planes[_terminalId].Baggages))
                        {
                            if (MainWindow.TerminalQueues[_terminalId].Count() == 0)
                            {
                                Monitor.Wait(MainWindow.TerminalQueues[_terminalId]);
                            }
                            MainWindow.Planes[_terminalId].Baggages.Enqueue(MainWindow.TerminalQueues[_terminalId].Dequeue());
                            Monitor.Exit(MainWindow.Planes[_terminalId].Baggages);

                        }
                        Monitor.Exit(MainWindow.TerminalQueues[_terminalId]);
                    }
                    else
                    {
                        if (Monitor.TryEnter(MainWindow.LostBaggage))
                        {
                            while (MainWindow.TerminalQueues[_terminalId].Count != 0)
                            {
                                MainWindow.LostBaggage.Enqueue(MainWindow.TerminalQueues[_terminalId].Dequeue());

                            }
                            Monitor.Exit(MainWindow.LostBaggage);
                            MainWindow.TerminalQueues.Remove(_terminalId);
                        }
                    }
                }
                Thread.Sleep(1500);
            }

        }
    }
}
