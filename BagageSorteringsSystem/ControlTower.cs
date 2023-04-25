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
        public static Dictionary<int, Plane> Planes = new();
        public static FlyingPlan FlyingPlan;
        /// <summary>
        /// Create objects for terminals and starts consuming baggage, and terminalsqueues added, and calling method to get objects of flyingplan and terminals
        /// </summary>
        /// <param name="callback"></param>
        public async void ControlGates(object callback)
        {

            FlyingPlan = ConvertStringToFlyingPlan(ReadFile(@"../../../FileSystem/Flyveplan.json"));
            List<Terminal> terminalList = ConvertStringToTerminal(ReadFile(@"../../../FileSystem/Terminals.json"));
            for (int i = 0; i < terminalList.Count; i++)
            {
                AirPortManager.Terminals.Add(terminalList[i].GateId, terminalList[i]);
                AirPortManager.Logger.Information("Terminal created Gate {0}", terminalList[i].GateId);
                AirPortManager.TerminalQueues.Add(terminalList[i].GateId, new BlockingCollection<Baggage>());
                AirPortManager.Logger.Information("TerminalQueue {0} added", FlyingPlan.FlyvePlaner[i].GateId);
                ThreadPool.QueueUserWorkItem(AirPortManager.Terminals[terminalList[i].GateId].ConsumeBaggage);
                AirPortManager.Logger.Information("Terminal on Gate {0} start consuming", terminalList[i].GateId);
            }

            while (true)
            {

                foreach (Terminal terminal in AirPortManager.Terminals.Values)
                {

                    if (Monitor.TryEnter(FlyingPlan.FlyvePlaner))
                    {
                        var plan = FlyingPlan.FlyvePlaner.Where(x => x.GateId == terminal.GateId).FirstOrDefault();
                        if (plan != null)
                        {
                            if (!Planes.ContainsKey(plan.GateId))
                            {
                                Planes.Add(plan.GateId, new Plane(plan.GateId, plan.MaxCustomers, plan.Destination, plan.Afgangstid));
                                ThreadPool.QueueUserWorkItem(Planes[plan.GateId].Dock);
                            }
                        }
                        Monitor.Exit(FlyingPlan.FlyvePlaner);
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
