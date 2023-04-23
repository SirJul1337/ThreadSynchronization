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
        public DateTimeOffset Time;
        public int MaxCount;
        public Queue<Baggage> Baggages = new Queue<Baggage>();
        /// <summary>
        /// Constructor to set id Maxcound Destination and time
        /// </summary>
        /// <param name="id"></param>
        /// <param name="maxCount"></param>
        /// <param name="destination"></param>
        /// <param name="takeOffTime"></param>
        public Plane(int id, int maxCount, string destination, DateTimeOffset takeOffTime)
        {
            Id = id;
            MaxCount = maxCount;
            Destination = destination;
            Time = takeOffTime;

        }
        /// <summary>
        /// Method to check how much baggage is filled up or it is time for flying, it will call Fly() method
        /// </summary>
        /// <param name="callback"></param>
        public void Dock(object callback)
        {

            Monitor.Enter(Program.Terminals[Id]);
            Program.Terminals[Id].PlaneDocked = true;
            Program.Logger.Information("Plane {0} Docked to gate");
            while (!_takeOff)
            {
                if (Baggages.Count >= MaxCount || DateTime.Now >= Time  )
                {
                    Fly();
                }
                else
                {
                    Thread.Sleep(1500);
                }
            }
            Monitor.Pulse(Program.Terminals[Id]);
            Monitor.Exit(Program.Terminals[Id]);
        }
        /// <summary>
        /// Fly method will change variables, to close the while loop, and remove the plan from the flyingplan list
        /// </summary>
        public void Fly()
        {
            Program.Logger.Information("Plane {0} takes off", Id);
            _takeOff = true;
            Program.Terminals[Id].PlaneDocked = false;
            Program.Planes.Remove(Id);
            Program.FlyingPlan.Flyveplan.Remove(Program.FlyingPlan.Flyveplan.Where(f => f.GateId == Id).FirstOrDefault());
        }
    }
}
