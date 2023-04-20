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
        ControlTower controlTower = new ControlTower();

        ThreadPool.QueueUserWorkItem(customer.AutoGenerate);
        ThreadPool.QueueUserWorkItem(checkIn.Open);
        ThreadPool.QueueUserWorkItem(handling.Sorting);
        ThreadPool.QueueUserWorkItem(controlTower.ControlGates);



        foreach (var plane in Planes)
        {
            

        }
        //if (Monitor.TryEnter(TerminalQueues))
        //{
        //    if(TerminalQueues.Count == 0)
        //    {
        //        Monitor.Wait(TerminalQueues);
        //    }
        //    foreach (var terminalQueue in TerminalQueues.Keys)
        //    {
        //        //Terminal terminal = new Terminal(terminalQueue);
        //        //Terminals.Add(terminalQueue, terminal);
        //        ThreadPool.QueueUserWorkItem(Terminals[terminalQueue].ConsumeBaggage);

        //    }
        //    Monitor.Exit(TerminalQueues);

        //}

        

        while (true)
        {
            QueueOverview();
            Thread.Sleep(10);

        }
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
                if (Planes.ContainsKey(id) && Monitor.TryEnter(Planes[id].Baggages))
                {
                    Console.WriteLine("Terminal {0} BaggageCount: {1}", id, TerminalQueues[id].Count);
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