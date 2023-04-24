using BagageSorteringsSystem;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using System.Collections.Concurrent;
using System.Reflection;
namespace BagageSorteringsSystem;

public class Program
{
    public static BlockingCollection<Baggage> Baggages = new();
    public static BlockingCollection<Baggage> CustomerLine = new();
    public static BlockingCollection<Baggage> LostBaggage = new();
    public static Dictionary<int, BlockingCollection<Baggage>> TerminalQueues = new();
    public static Dictionary<int, Plane> Planes = new();
    public static Dictionary<int, Terminal> Terminals = new();
    public static Dictionary<ConsoleKey, Action> NavDictionary = new();
    public static CheckIn[] CheckIns = new CheckIn[3] { new CheckIn(), new CheckIn(), new CheckIn() };
    public static Logger Logger;
    public static FlyingPlan FlyingPlan;
    public static ConsoleKey NavKey = ConsoleKey.A;
    public static void Main()
    {
        Startup();


        while (true)
        {
            Console.Clear();
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("             {0}                          ", DateTime.Now);
            Console.WriteLine("----------------------------------------------");
            if (NavDictionary.ContainsKey(NavKey))
            {
                NavDictionary[NavKey].Invoke(); //TODO: fix problem with checkin Add and remove
            }
            Console.WriteLine("A. System overview  B. FlyingPlan  C. Checkin overview");
            Thread.Sleep(100);

        }

    }

    private static void Startup()
    {
        Logger = new LoggerConfiguration()
        .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
        CheckInManager checkInManager = new();
        checkInManager.Add();
        PopulateNavDirectory(checkInManager);
        Customer customer = new();
        BaggageHandling handling = new();
        ControlTower controlTower = new();
        ThreadPool.QueueUserWorkItem(controlTower.ControlGates);
        ConsoleNavigation navigation = new();
        Thread consoleNav = new(navigation.StartNavigations);
        consoleNav.Start();
        ThreadPool.QueueUserWorkItem(customer.AutoGenerate);
        ThreadPool.QueueUserWorkItem(handling.Sorting);
    }
    private static void PopulateNavDirectory(CheckInManager checkInManager)
    {
        NavDictionary.Add(ConsoleKey.A, ViewOverview);
        NavDictionary.Add(ConsoleKey.B, ViewFlyPlan);
        NavDictionary.Add(ConsoleKey.C, ViewCheckIns);
        NavDictionary.Add(ConsoleKey.P, checkInManager.Add);
        NavDictionary.Add(ConsoleKey.M, checkInManager.Remove);
    }
    private static void ViewOverview()
    {
        Console.WriteLine("Customers in queue: {0}", CustomerLine.Count);
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine("Baggages in sorting system: {0}", Baggages.Count);
        Console.WriteLine("----------------------------------------------");

        for (int i = 0; i < TerminalQueues.Count; i++)
        {

        }
        foreach (var item in TerminalQueues)
        {
            int id = item.Key;
            Console.WriteLine("Terminal {0} BaggageCount: {1}", id, TerminalQueues[id].Count);
            if (Planes.ContainsKey(id))
            {
                Console.WriteLine("Distination {0}", Planes[id].Destination);
                Console.WriteLine("Takeoff: {0}", Planes[id].Time);
                Console.WriteLine("Plane {0} baggage onboard: {1}/{2}", id, Planes[id].Baggages.Count, Planes[id].MaxCount);
            }

            Console.WriteLine("----------------------------------------------");
        }
    }
    private static void ViewFlyPlan()
    {

        for (int i = 0; i < FlyingPlan.FlyvePlaner.Count; i++)
        {
            Console.WriteLine("Gate {0} | Destination {1} | Time {2}", FlyingPlan.FlyvePlaner[i].GateId, FlyingPlan.FlyvePlaner[i].Destination, FlyingPlan.FlyvePlaner[i].Afgangstid);
            Console.WriteLine("---------------------------------------------------------------------------");

        }


    }
    private static void ViewCheckIns()
    {
        var openList = CheckIns.Where(c => c.Alive == true).ToList();
        for (int i = 0; i < openList.Count; i++)
        {
            Console.WriteLine("|----------------|");
            Console.WriteLine("|Check In box {0}  |", i);
            Console.WriteLine("|----------------|");
        }
        Console.WriteLine("P. Open checkin  M. Close checkin ");

    }
}