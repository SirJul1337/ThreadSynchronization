using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BagageSorteringsSystem
{
    public class ControlTower
    {
        public void ControlGates(object callback)
        {

            FlyingPlan flyingPlan = ConvertFileToFlyingPlan(ReadFile());
            if(Monitor.TryEnter(Program.TerminalQueues))
            { 
                for (int i = 0; i < flyingPlan.Flyveplan.Count(); i++)
                {
                    Program.TerminalQueues.Add(flyingPlan.Flyveplan[i].GateId, new Queue<Baggage>());
                    Program.Planes.Add(flyingPlan.Flyveplan[i].GateId, new Plane(flyingPlan.Flyveplan[i].GateId, flyingPlan.Flyveplan[i].MaxCustomers, flyingPlan.Flyveplan[i].Destination));
                    Terminal terminal = new Terminal(flyingPlan.Flyveplan[i].GateId);
                    Program.Terminals.Add(flyingPlan.Flyveplan[i].GateId, terminal);
                    ThreadPool.QueueUserWorkItem(Program.Terminals[flyingPlan.Flyveplan[i].GateId].ConsumeBaggage);
                    ThreadPool.QueueUserWorkItem(Program.Planes[flyingPlan.Flyveplan[i].GateId].Dock);
                    Program.Logger.Information("Plane {0} added", flyingPlan.Flyveplan[i].GateId);
                    Program.Logger.Information("TerminalQueue {0} added", flyingPlan.Flyveplan[i].GateId);
                }
                Monitor.PulseAll(Program.TerminalQueues);
                Monitor.Exit(Program.TerminalQueues);

            }
        }
        private static string ReadFile()
        {
            string path = @"../../../FileSystem/Flyveplan.json";
            string file = File.ReadAllText(path);
            return file;
        }
        private static FlyingPlan ConvertFileToFlyingPlan(string input)
        {
            FlyingPlan plan = JsonConvert.DeserializeObject<FlyingPlan>(input);
            return plan;
        }
    }
}
