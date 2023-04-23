using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaggageSortertingSystemWpf
{
    public class CheckIn
    {
        private bool _abort;
        public bool Alive;
        public CheckIn()
        {

        }
        public void Open(object callback)
        {
            _abort = false;
            Alive = true;
            while (!_abort)
            {
                try
                {

                    if (Monitor.TryEnter(MainWindow.CustomerLine))
                    {
                        if (Monitor.TryEnter(MainWindow.Baggages))
                        {
                            if (MainWindow.CustomerLine.Count == 0)
                            {
                                Monitor.Wait(MainWindow.CustomerLine);
                            }
                            if (MainWindow.Baggages.Count < 50)
                            {
                                MainWindow.Baggages.Enqueue(MainWindow.CustomerLine.Dequeue());
                                Monitor.PulseAll(MainWindow.Baggages);

                            }
                            Monitor.Exit(MainWindow.Baggages);
                        }
                        Monitor.Exit(MainWindow.CustomerLine);
                    }

                }
                finally
                {
                    Thread.Sleep(700);
                }
            }
        }
        public void Close(object callback)
        {
            Alive = false;
            _abort = true;
        }
    }
}
