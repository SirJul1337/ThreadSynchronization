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

            Program.FlyingPlan = ConvertFileToFlyingPlan(ReadFile());
            if (Monitor.TryEnter(Program.TerminalQueues))
            {
                if (Monitor.TryEnter(Program.FlyingPlan.Flyveplan))
                {

                    for (int i = 0; i < Program.FlyingPlan.Flyveplan.Count(); i++)
                    {

                        Program.TerminalQueues.Add(Program.FlyingPlan.Flyveplan[i].GateId, new Queue<Baggage>());
                        Program.Planes.Add(Program.FlyingPlan.Flyveplan[i].GateId, new Plane(Program.FlyingPlan.Flyveplan[i].GateId, Program.FlyingPlan.Flyveplan[i].MaxCustomers, Program.FlyingPlan.Flyveplan[i].Destination));
                        Terminal terminal = new Terminal(Program.FlyingPlan.Flyveplan[i].GateId);
                        Program.Terminals.Add(Program.FlyingPlan.Flyveplan[i].GateId, terminal);
                        ThreadPool.QueueUserWorkItem(Program.Terminals[Program.FlyingPlan.Flyveplan[i].GateId].ConsumeBaggage);
                        ThreadPool.QueueUserWorkItem(Program.Planes[Program.FlyingPlan.Flyveplan[i].GateId].Dock);
                        Program.Logger.Information("Plane {0} added", Program.FlyingPlan.Flyveplan[i].GateId);
                        Program.Logger.Information("TerminalQueue {0} added", Program.FlyingPlan.Flyveplan[i].GateId);
                    }
                    Monitor.Exit(Program.FlyingPlan.Flyveplan);
                    Monitor.PulseAll(Program.TerminalQueues);
                }
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
