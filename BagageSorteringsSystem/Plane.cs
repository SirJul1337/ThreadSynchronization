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
        /// Constructor used to construct Plane with all neccesary variables
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
        /// Method to check how much baggage is filled up or if it is time for flying, it will call Fly() method
        /// </summary>
        /// <param name="callback"></param>
        public void Dock(object callback)
        {
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

        }
        /// <summary>
        /// Fly method will change variables, to close the while loop, and remove the plan from the flyingplan list
        /// </summary>
        private void Fly()
        {
            Program.Logger.Information("Plane {0} takes off", Id);
            _takeOff = true;
            Monitor.Enter(Program.FlyingPlan.FlyvePlaner);
            Program.FlyingPlan.FlyvePlaner.Remove(Program.FlyingPlan.FlyvePlaner.Where(f => f.GateId == Id).FirstOrDefault());
            Monitor.Exit(Program.FlyingPlan.FlyvePlaner);
            Program.Terminals[Id].PlaneDocked = false;
            Program.Planes.Remove(Id);
        }
    }
}
