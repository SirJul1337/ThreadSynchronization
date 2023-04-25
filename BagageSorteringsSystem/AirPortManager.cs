using BagageSorteringsSystem.DTO;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagageSorteringsSystem
{
    public class AirPortManager
    {
        public static BlockingCollection<Baggage> Baggages = new();
        public static BlockingCollection<Baggage> LostBaggage = new();
        public static Dictionary<int, BlockingCollection<Baggage>> TerminalQueues = new();
        public static Dictionary<int, Terminal> Terminals = new();
        public static Dictionary<ConsoleKey, Action> NavDictionary = new();

        public static Logger Logger;
        

        public void Run()
        {
            Startup();
        }
        public DashboardOverviewDTO GetDashboardInfo()
        {
            return new DashboardOverviewDTO { customerLine = CheckInManager.CustomerLine, systemBaggages = Baggages, TerminalQueues = TerminalQueues, Planes = ControlTower.Planes };
        }
        public List<Flyveplan> GetFlyvePlan()
        {
            return ControlTower.FlyingPlan.FlyvePlaner;
        }
        public CheckIn[] GetCheckins()
        {
            var openList = CheckInManager.CheckIns.Where(c => c.Alive == true).ToList();
            return openList.ToArray();
        }

        private static void Startup()
        {
            Logger = new LoggerConfiguration()
            .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
            CustomerGenerater customer = new();
            BaggageHandling handling = new();
            ControlTower controlTower = new();
            CheckInManager checkInManager = new();
            ConsoleNavigation navigation = new();
            Thread consoleNav = new(navigation.StartNavigations);
            checkInManager.Add();
            consoleNav.Start();
            PopulateNavDirectory(checkInManager);
            ThreadPool.QueueUserWorkItem(controlTower.ControlGates);
            ThreadPool.QueueUserWorkItem(customer.AutoGenerate);
            ThreadPool.QueueUserWorkItem(handling.Sorting);

        }

        /// <summary>
        /// Method to populate my dictionary for my navigation
        /// </summary>
        /// <param name="checkInManager"></param>
        private static void PopulateNavDirectory(CheckInManager checkInManager)
        {
            NavDictionary.Add(ConsoleKey.A, Program.ViewOverview);
            NavDictionary.Add(ConsoleKey.B, Program.ViewFlyPlan);
            NavDictionary.Add(ConsoleKey.C, Program.ViewCheckIns);
            NavDictionary.Add(ConsoleKey.P, checkInManager.Add);
            NavDictionary.Add(ConsoleKey.M, checkInManager.Remove);
        }
    }
}
