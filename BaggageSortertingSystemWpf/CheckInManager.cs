using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaggageSortertingSystemWpf
{
    public class CheckInManager
    {
        private static int _checkInIndex = 0;
        public void Add()
        {
            if (_checkInIndex <= MainWindow.CheckIns.Length - 1)
            {
                ThreadPool.QueueUserWorkItem(MainWindow.CheckIns[_checkInIndex].Open);
                if(_checkInIndex != MainWindow.CheckIns.Length - 1)
                {
                    _checkInIndex++;

                }
            }
        }
        public void Remove()
        {
            if (_checkInIndex >= 0)
            {
                ThreadPool.QueueUserWorkItem(MainWindow.CheckIns[_checkInIndex].Close);
                if (_checkInIndex != 0)
                {
                    _checkInIndex--;

                }

            }
        }
    }
}
