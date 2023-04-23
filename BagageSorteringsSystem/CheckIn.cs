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
        private bool _abort;
        public bool Alive;
        public CheckIn()
        {

        }
        /// <summary>
        /// When checkin box is open it will consume from customer line, and put bagges into sortingSystem baggges queue if there is space
        /// </summary>
        /// <param name="callback"></param>
        public void Open(object callback)
        {
            _abort = false;
            Alive = true;
            while (!_abort)
            {
                try
                {

                    if (Monitor.TryEnter(Program.CustomerLine))
                    {
                        if (Monitor.TryEnter(Program.Baggages))
                        {
                            if (Program.CustomerLine.Count == 0)
                            {
                                Monitor.Wait(Program.CustomerLine);
                            }
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
                    Thread.Sleep(700);
                }
            }
        }
        /// <summary>
        /// Used for closing thread, cause thread.Abort is deprecated
        /// </summary>
        /// <param name="callback"></param>
        public void Close(object callback)
        {
            Alive = false;
            _abort = true;
        }
    }
}
