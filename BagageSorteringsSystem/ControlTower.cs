using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
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

            Program.FlyingPlan = ConvertStringToFlyingPlan(ReadFile(@"../../../FileSystem/Flyveplan.json"));
            List<Terminal> terminalList = ConvertStringToTerminal(ReadFile(@"../../../FileSystem/Terminals.json"));
            for (int i = 0; i < terminalList.Count; i++)
            {
                Program.Terminals.Add(terminalList[i].GateId, terminalList[i]);
                Program.Logger.Information("Terminal created Gate {0}", terminalList[i].GateId);
                Program.TerminalQueues.Add(terminalList[i].GateId, new BlockingCollection<Baggage>());
                Program.Logger.Information("TerminalQueue {0} added", Program.FlyingPlan.FlyvePlaner[i].GateId);
                ThreadPool.QueueUserWorkItem(Program.Terminals[terminalList[i].GateId].ConsumeBaggage);
                Program.Logger.Information("Terminal on Gate {0} start consuming", terminalList[i].GateId);
            }

            while (true)
            {

                foreach (Terminal terminal in Program.Terminals.Values)
                {

                    if (Monitor.TryEnter(Program.FlyingPlan.FlyvePlaner))
                    {
                        var plan = Program.FlyingPlan.FlyvePlaner.Where(x => x.GateId == terminal.GateId).FirstOrDefault();
                        if (plan != null)
                        {
                            if (!Program.Planes.ContainsKey(plan.GateId))
                            {
                                Program.Planes.Add(plan.GateId, new Plane(plan.GateId, plan.MaxCustomers, plan.Destination, plan.Afgangstid));
                                ThreadPool.QueueUserWorkItem(Program.Planes[plan.GateId].Dock);
                            }
                        }
                        Monitor.Exit(Program.FlyingPlan.FlyvePlaner);
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
        private static FlyingPlan ConvertStringToFlyingPlan(string input)
        {
            FlyingPlan plan = JsonConvert.DeserializeObject<FlyingPlan>(input);
            return plan;
        }
        /// <summary>
        /// Converts string(json) into object
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static List<Terminal> ConvertStringToTerminal(string input)
        {
            List<Terminal> plan = JsonConvert.DeserializeObject<List<Terminal>>(input);
            return plan;
        }
    }
}
