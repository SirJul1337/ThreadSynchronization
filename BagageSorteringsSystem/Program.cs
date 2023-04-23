using BagageSorteringsSystem;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using System.Reflection;
namespace BagageSorteringsSystem;

public class Program
{
    public static Queue<Baggage> Baggages = new();
    public static Dictionary<int, Queue<Baggage>> TerminalQueues = new();
    public static Dictionary<int, Plane> Planes = new();
    public static Dictionary<int, Terminal> Terminals = new();
    public static CheckIn[] CheckIns = new CheckIn[3] { new CheckIn(), new CheckIn(), new CheckIn() };
    public static Queue<Baggage> CustomerLine = new();
    public static Queue<Baggage> LostBaggage = new();
    public static Logger Logger;
    public static Dictionary<ConsoleKey, Action> NavDictionary = new();
    public static ConsoleKey NavKey = ConsoleKey.A;
    public static FlyingPlan FlyingPlan;
    public static void Main()
    {
        Startup();
        CheckInManager checkInManager = new();
        checkInManager.Add();
        NavDictionary.Add(ConsoleKey.A, QueueOverview);
        NavDictionary.Add(ConsoleKey.B, ViewFlyPlan);
        NavDictionary.Add(ConsoleKey.C, CheckInOverview);
        NavDictionary.Add(ConsoleKey.P, checkInManager.Add);
        NavDictionary.Add(ConsoleKey.M, checkInManager.Remove);
        while (true)
        {
            Console.Clear();
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
    private static void QueueOverview()
    {



        Console.WriteLine("----------------------------------------------");
        Console.WriteLine("             {0}                          ", DateTime.Now);
        Console.WriteLine("----------------------------------------------");
        if (Monitor.TryEnter(CustomerLine))
        {
            Console.WriteLine("Customers in queue: {0}", CustomerLine.Count);
            Monitor.Exit(CustomerLine);

        }
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine("Baggages in sorting system: {0}", Baggages.Count);
        if (Monitor.TryEnter(TerminalQueues))
        {
            for (int i = 0; i < TerminalQueues.Count; i++)
            {

            }
            foreach (var item in TerminalQueues)
            {
                int id = item.Key;
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("Terminal {0} BaggageCount: {1}", id, TerminalQueues[id].Count);
                if (Planes.ContainsKey(id))
                {
                    Console.WriteLine("Distination {0}", Planes[id].Destination);
                    Console.WriteLine("Takeoff: {0}", Planes[id].Time);
                    Console.WriteLine("Plane {0} baggage onboard: {1}/{2}", id, Planes[id].Baggages.Count, Planes[id].MaxCount);
                    //Monitor.Exit(Planes[id].Baggages);
                }
            }
            Monitor.Exit(TerminalQueues);
            Console.WriteLine("----------------------------------------------");
        }
    }
    private static void ViewFlyPlan()
    {
        if (Monitor.TryEnter(FlyingPlan.Flyveplan))
        {
            for (int i = 0; i < FlyingPlan.Flyveplan.Count; i++)
            {
                Console.WriteLine("Gate {0} | Destination {1} | Time {2}", FlyingPlan.Flyveplan[i].GateId, FlyingPlan.Flyveplan[i].Destination, FlyingPlan.Flyveplan[i].Afgangstid);
                Console.WriteLine("---------------------------------------------------------------------------");

            }

        }
    }
    private static void CheckInOverview()
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