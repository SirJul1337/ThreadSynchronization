using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaggageSortertingSystemWpf
{
    public class Plane
    {
        private bool _takeOff = false;
        public int Id;
        public string Destination;
        public DateTimeOffset Time;
        public int MaxCount;
        public Queue<Baggage> Baggages = new Queue<Baggage>();
        public Plane(int id, int maxCount, string destination, DateTimeOffset takeOffTime)
        {
            Id = id;
            MaxCount = maxCount;
            Destination = destination;
            Time = takeOffTime;

        }
        public void Dock(object callback)
        {

            Monitor.Enter(MainWindow.Terminals[Id]);

            MainWindow.Logger.Information("Plane {0} Docked to gate");
            while (!_takeOff)
            {
                if (Baggages.Count >= MaxCount || DateTime.Now >= Time)
                {
                    Fly();
                }
                else
                {
                    Thread.Sleep(1500);
                }
            }
            Monitor.Pulse(MainWindow.Terminals[Id]);
            Monitor.Exit(MainWindow.Terminals[Id]);
        }
        public void Fly()
        {
            MainWindow.Logger.Information("Plane {0} takes off", Id);
            _takeOff = true;
            MainWindow.Planes.Remove(Id);
        }
    }
}
