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


                    if (Monitor.TryEnter(Program.Baggages))
                    {
                        if (Program.Baggages.Count < 50)
                        {
                            Random r = new Random();
                            Program.Baggages.Enqueue(new Baggage("Test", r.Next(1, 3)));
                            Monitor.PulseAll(Program.Baggages);
                        }
                        Monitor.Exit(Program.Baggages);
                    }
                }
                finally
                {
                    Thread.Sleep(100);
                }
            }
        }
        public void Close()
        {
            _abort = true;
        }
    }
}
