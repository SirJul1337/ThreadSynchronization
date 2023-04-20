using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagageSorteringsSystem
{
    public class Plane
    {
        private bool _takeOff = false;
        public int Id;
        public string Destination;
        public int MaxCount;
        public Queue<Baggage> Baggages = new Queue<Baggage>();
        public Plane(int id, int maxCount, string destination)
        {
            Id = id;
            MaxCount = maxCount;
            Destination = destination;
        }
        public void Dock(object callback)
        {
            Monitor.Enter(Program.Terminals[Id]);
            while (!_takeOff)
            {
                if (Baggages.Count < MaxCount)
                {
                    Program.Logger.Information("Plane {0} not yet filled", Id);
                    Thread.Sleep(2000);
                }
                else
                {
                    Fly();
                }
            }
            Monitor.Exit(Program.Terminals[Id]);
        }
        public void Fly()
        {
            Program.Logger.Information("Plane {0} takes off", Id);
            _takeOff = true;
            Program.Planes.Remove(Id);
        }
    }
}
