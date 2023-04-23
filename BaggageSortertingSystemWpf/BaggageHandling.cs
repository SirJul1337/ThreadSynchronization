using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaggageSortertingSystemWpf
{
    public class BaggageHandling
    {
        public void Sorting(object callback)
        {
            while (true)
            {
                try
                {

                    if (Monitor.TryEnter(MainWindow.Baggages))
                    {
                        if (MainWindow.Baggages.Count == 0)
                        {
                            Monitor.Wait(MainWindow.Baggages);
                        }
                        int _gateId = MainWindow.Baggages.Peek().GateId;
                        if (Monitor.TryEnter(MainWindow.TerminalQueues))
                        {

                            if (MainWindow.TerminalQueues.ContainsKey(_gateId))
                            {
                                if (Monitor.TryEnter(MainWindow.TerminalQueues[_gateId]))
                                {

                                    if (MainWindow.TerminalQueues[_gateId].Count() < 30)
                                    {
                                        MainWindow.TerminalQueues[_gateId].Enqueue(MainWindow.Baggages.Dequeue());
                                        Monitor.PulseAll(MainWindow.TerminalQueues[_gateId]);
                                    }
                                    Monitor.Exit(MainWindow.TerminalQueues[_gateId]);
                                }

                            }
                            else
                            {
                                if (Monitor.TryEnter(MainWindow.LostBaggage))
                                {
                                    MainWindow.LostBaggage.Enqueue(MainWindow.Baggages.Dequeue());
                                    Monitor.Exit(MainWindow.LostBaggage);

                                }
                            }
                            Monitor.Exit(MainWindow.TerminalQueues);
                        }
                        Monitor.Exit(MainWindow.Baggages);

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
