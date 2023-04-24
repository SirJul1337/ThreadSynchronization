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

                    while (Program.CustomerLine.Count == 0 || Program.Baggages.Count >= 50)
                    {

                    }

                    Baggage baggage = Program.CustomerLine.Take();
                    Program.Baggages.Add(baggage);


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
