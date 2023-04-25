using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagageSorteringsSystem
{
    public class CheckInManager
    {
        public static CheckIn[] CheckIns = new CheckIn[3] { new CheckIn(), new CheckIn(), new CheckIn() };
        private static int _checkInIndex = 0;
        private static Dictionary<int, CancellationTokenSource> cancelationTokens = new Dictionary<int, CancellationTokenSource>();
        /// <summary>
        /// Used for adding more Checkin boxed
        /// </summary>
        public void Add()
        {
            if (_checkInIndex <= CheckIns.Length-1)
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                cancelationTokens.Add(_checkInIndex, cancellationTokenSource);
                ThreadPool.QueueUserWorkItem(CheckIns[_checkInIndex].Open,cancellationTokenSource );
                _checkInIndex++;
            }
        }
        /// <summary>
        /// Used for closing checkingboxes
        /// </summary>
        public void Remove() 
        {
            if(_checkInIndex >= 0)
            {
                    //cancelationTokens[_checkInIndex].Cancel();
                ThreadPool.QueueUserWorkItem(CheckIns[_checkInIndex].Close); 
                if(_checkInIndex != 0)
                {
                    _checkInIndex--;

                }

            }
        }
    }
}
