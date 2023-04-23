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
        /// <summary>
        /// Create objects for terminals and starts consuming baggage, and terminalsqueues added, and calling method to get objects of flyingplan and terminals
        /// </summary>
        /// <param name="callback"></param>
        public async void ControlGates(object callback)
        {

            Program.FlyingPlan = ConvertFileToFlyingPlan(ReadFile(@"../../../FileSystem/Flyveplan.json"));
            List<Terminal> list = ConvertFileToTerminal(ReadFile(@"../../../FileSystem/Terminals.json"));
            for (int i = 0; i < list.Count; i++)
            {
                Program.Terminals.Add(list[i].GateId, list[i]);
                Program.Logger.Information("Terminal created Gate {0}", list[i].GateId);
                Program.TerminalQueues.Add(list[i].GateId, new Queue<Baggage>());
                Program.Logger.Information("TerminalQueue {0} added", Program.FlyingPlan.Flyveplan[i].GateId);
                ThreadPool.QueueUserWorkItem(Program.Terminals[list[i].GateId].ConsumeBaggage);
                Program.Logger.Information("Terminal on Gate {0} start consuming", list[i].GateId);
            }

            while (true)
            {

                foreach (Terminal terminal in Program.Terminals.Values)
                {
                    if (Monitor.TryEnter(terminal))
                    {
                        if (Monitor.TryEnter(Program.FlyingPlan.Flyveplan))
                        {
                            var plan = Program.FlyingPlan.Flyveplan.Where(x => x.GateId == terminal.GateId).FirstOrDefault();
                            if(plan != null)
                            {
                                if (!Program.Planes.ContainsKey(plan.GateId))
                                {
                                    Program.Planes.Add(plan.GateId,  new Plane(plan.GateId, plan.MaxCustomers, plan.Destination, plan.Afgangstid));
                                    ThreadPool.QueueUserWorkItem(Program.Planes[plan.GateId].Dock);
                                }
                            }
                            Monitor.Exit(terminal);

                        }
                    }
                }
            }
        }
        /// <summary>
        /// Reads all text from filePath
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static string ReadFile(string filePath)
        {
            
            string file = File.ReadAllText(filePath);
            return file;
        }
        /// <summary>
        /// Converts string(json) into object
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static FlyingPlan ConvertFileToFlyingPlan(string input)
        {
            FlyingPlan plan = JsonConvert.DeserializeObject<FlyingPlan>(input);
            return plan;
        }
        /// <summary>
        /// Converts string(json) into object
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static List<Terminal> ConvertFileToTerminal(string input)
        {
            List<Terminal> plan = JsonConvert.DeserializeObject<List<Terminal>>(input);
            return plan;
        }
    }
}
