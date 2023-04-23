using BaggageSortertingSystemWpf.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaggageSortertingSystemWpf
{
    public class ControlTower
    {
        public void ControlGates(object callback)
        {

            MainWindow.FlyingPlan = ConvertFileToFlyingPlan(ReadFile());
            if (Monitor.TryEnter(MainWindow.TerminalQueues))
            {
                if (Monitor.TryEnter(MainWindow.FlyingPlan.Flyveplan))
                {

                    for (int i = 0; i < MainWindow.FlyingPlan.Flyveplan.Count(); i++)
                    {

                        MainWindow.TerminalQueues.Add(MainWindow.FlyingPlan.Flyveplan[i].GateId, new Queue<Baggage>());
                        MainWindow.Planes.Add(MainWindow.FlyingPlan.Flyveplan[i].GateId, new Plane(MainWindow.FlyingPlan.Flyveplan[i].GateId, MainWindow.FlyingPlan.Flyveplan[i].MaxCustomers, MainWindow.FlyingPlan.Flyveplan[i].Destination, MainWindow.FlyingPlan.Flyveplan[i].Afgangstid));
                        Terminal terminal = new Terminal(MainWindow.FlyingPlan.Flyveplan[i].GateId);
                        MainWindow.Terminals.Add(MainWindow.FlyingPlan.Flyveplan[i].GateId, terminal);
                        ThreadPool.QueueUserWorkItem(MainWindow.Terminals[MainWindow.FlyingPlan.Flyveplan[i].GateId].ConsumeBaggage);
                        ThreadPool.QueueUserWorkItem(MainWindow.Planes[MainWindow.FlyingPlan.Flyveplan[i].GateId].Dock);
                        MainWindow.Logger.Information("Plane {0} added", MainWindow.FlyingPlan.Flyveplan[i].GateId);
                        MainWindow.Logger.Information("TerminalQueue {0} added", MainWindow.FlyingPlan.Flyveplan[i].GateId);
                    }
                    Monitor.Exit(MainWindow.FlyingPlan.Flyveplan);
                    Monitor.PulseAll(MainWindow.TerminalQueues);
                }
                Monitor.Exit(MainWindow.TerminalQueues);

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
