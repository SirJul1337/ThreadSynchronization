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
        private int _id;
        public Queue<Baggage> Baggages = new Queue<Baggage>();
        public Plane(int id)
        {
            _id = id;
        }
        public void Dock(object callback)
        {
            while (!_takeOff)
            {
                if (Baggages.Count < 30)
                {
                    Program.Logger.Information("Plane {0} not yet filled", _id);
                    Thread.Sleep(2000);
                }
                else
                {
                    Fly();
                }
            }
        }
        public void Fly()
        {
            Program.Logger.Information("Plane {0} takes off", _id);
            _takeOff = true;
            Program.Planes.Remove(_id);
        }
    }
}
