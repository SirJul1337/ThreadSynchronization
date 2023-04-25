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
        /// <summary>
        /// When checkin box is open it will check if there is space in sortingsystem and it will consume from customerline, and put it into system
        /// </summary>
        /// <param name="callback"></param>
        public void Open(object obj)
        {
            CancellationToken token = ((CancellationTokenSource)obj).Token;
            _abort = false;
            Alive = true;
            while (!_abort)
            {
                try
                {
                    if (AirPortManager.Baggages.Count < 50)
                    {
                        Baggage baggage = CheckInManager.CustomerLine.Take();
                        AirPortManager.Baggages.Add(baggage);
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
        /// TODO: CancelationToken
        /// </summary>
        /// <param name="callback"></param>
        public void Close(object callback)
        {
            Alive = false;
            _abort = true;
        }
    }
}
