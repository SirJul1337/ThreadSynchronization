using BagageSorteringsSystem;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;
using System.Reflection;

public class Program
{
    public static Queue<Baggage> Baggages = new();
    public static Dictionary<int, Queue<Baggage>> TerminalQueues = new();
    public static Dictionary<int, Plane> Planes = new();    
    public static Dictionary<int, Terminal> Terminals = new();
    public static Queue<Baggage> CustomerLine = new();
    public static Queue<Baggage> LostBaggage = new();
    public static Logger Logger;
    public static void Main()
    {
        Logger = new LoggerConfiguration()
        .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

        Customer customer = new Customer();
        CheckIn checkIn = new();
        BaggageHandling handling = new BaggageHandling();

        ThreadPool.QueueUserWorkItem(customer.AutoGenerate);
        ThreadPool.QueueUserWorkItem(checkIn.Open);
        ThreadPool.QueueUserWorkItem(handling.Sorting);
        FlyingPlan flyingPlan = ConvertFileToFlyingPlan(ReadFile());

        for (int i = 0; i < flyingPlan.Flyveplan.Count(); i++)
        {
            TerminalQueues.Add(flyingPlan.Flyveplan[i].GateId, new Queue<Baggage>());
            Planes.Add(flyingPlan.Flyveplan[i].GateId, new Plane(flyingPlan.Flyveplan[i].GateId, flyingPlan.Flyveplan[i].MaxCustomers, flyingPlan.Flyveplan[i].Destination));
            Logger.Information("Plane {0} added", flyingPlan.Flyveplan[i].GateId);
            Logger.Information("TerminalQueue {0} added", flyingPlan.Flyveplan[i].GateId);

        }

        foreach (var plane in Planes)
        {
            ThreadPool.QueueUserWorkItem(plane.Value.Dock);

        }
        foreach (var terminalQueue in TerminalQueues.Keys)
        {
            Terminal terminal = new Terminal(terminalQueue);
            Terminals.Add(terminalQueue, terminal);
            ThreadPool.QueueUserWorkItem(Terminals[terminalQueue].ConsumeBaggage);

        }

        

        while (true)
        {
            QueueOverview();
            Thread.Sleep(50);

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
    private static void QueueOverview()
    {
        Console.Clear();
        if (Monitor.TryEnter(CustomerLine))
        {
            Console.WriteLine("Customers in queue: {0}", CustomerLine.Count);
            Monitor.Exit(CustomerLine);

        }
        Console.WriteLine("----------------------------------------------");
        Console.WriteLine("Baggages in sorting system: {0}", Baggages.Count);
        if (Monitor.TryEnter(TerminalQueues))
        {
            foreach (var item in TerminalQueues)
            {
                int id = item.Key;
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine("Terminal {0} BaggageCount: {1}", id, TerminalQueues[id].Count);
                if (Planes.ContainsKey(id) && Monitor.TryEnter(Planes[id].Baggages))
                {
                    Console.WriteLine("Distination {0} with max {1} customers ", Planes[id].Destination, Planes[id].MaxCount);
                    Console.WriteLine("Plane {0} baggage onboard: {1}", id, Planes[id].Baggages.Count);
                    Monitor.Exit(Planes[id].Baggages);
                }
            }
            Monitor.Exit(TerminalQueues);
            Console.WriteLine("----------------------------------------------");
        }
    }
}