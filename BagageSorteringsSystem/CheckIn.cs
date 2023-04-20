using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagageSorteringsSystem
{
    /// <summary>
    /// Class contains private bool _abort to close thread, cause Thread.Abort is not recommended anymore
    /// </summary>
    public class CheckIn
    {
        private bool _abort = false;
        public CheckIn()
        {

        }
        public void Open(object callback)
        {
            while (!_abort)
            {
                try
                {

                    if (Monitor.TryEnter(Program.CustomerLine))
                    {
                        if (Program.CustomerLine.Count == 0)
                        {
                            Monitor.Wait(Program.CustomerLine);
                        }
                        if (Monitor.TryEnter(Program.Baggages))
                        {
                            if(Program.Baggages.Count < 50)
                            {
                                Program.Baggages.Enqueue(Program.CustomerLine.Dequeue());
                                Monitor.PulseAll(Program.Baggages);

                            }
                            Monitor.Exit(Program.Baggages);
                        }
                        Monitor.Exit(Program.CustomerLine);
                    }

                }
                finally
                {
                    Thread.Sleep(500);
                }
            }
        }
        public void Close()
        {
            _abort = true;
        }
    }
}
