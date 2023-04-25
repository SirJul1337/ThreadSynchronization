using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagageSorteringsSystem.DTO
{
    public class DashboardOverviewDTO
    {
        public BlockingCollection<Baggage> customerLine;
        public BlockingCollection<Baggage> systemBaggages;
        public Dictionary<int, BlockingCollection<Baggage>> TerminalQueues;
        public Dictionary<int, Plane> Planes;
    }
}
