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
            
            Program.Logger.Information("Plane {0} Docked to gate");
            while (!_takeOff)
            {
                if (Baggages.Count < MaxCount)
                {
                    Thread.Sleep(2000);
                }
                else
                {
                    Fly();
                }
            }
            Monitor.Pulse(Program.Terminals[Id]);
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
