﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagageSorteringsSystem
{
    public class CheckInManager
    {
        private static int _checkInIndex = 0;
        /// <summary>
        /// Used for adding more Checkin boxed
        /// </summary>
        public void Add()
        {
            if (_checkInIndex <= Program.CheckIns.Length-1)
            {
                ThreadPool.QueueUserWorkItem(Program.CheckIns[_checkInIndex].Open);
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
                ThreadPool.QueueUserWorkItem(Program.CheckIns[_checkInIndex].Close); 
                if(_checkInIndex != 0)
                {
                    _checkInIndex--;

                }

            }
        }
    }
}
