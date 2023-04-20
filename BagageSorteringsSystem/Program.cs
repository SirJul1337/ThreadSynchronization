using BagageSorteringsSystem;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;

public class Program
{
    public static Queue<Baggage> Baggages = new();
    public static Dictionary<int, Queue<Baggage>> TerminalQueues = new();
    public static Dictionary<int, Plane> Planes = new();    
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
        for (int i = 1; i <= 3; i++)
        {
            TerminalQueues.Add(i, new Queue<Baggage>());
            Planes.Add(i, new Plane(i));
            Logger.Information("Plane {0} added", i);
            Logger.Information("TerminalQueue {0} added", i);
        }
        for (int i = 1; i < Planes.Count; i++)
        {
            ThreadPool.QueueUserWorkItem(Planes[i].Dock);
        }
        for (int i = 0; i < TerminalQueues.Keys.Count; i++)
        {
            Terminal terminal = new Terminal(TerminalQueues.ElementAt(i).Key);
            ThreadPool.QueueUserWorkItem(terminal.ConsumeBaggage);
        }

        while (true)
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
                    if (Planes.ContainsKey(id)&&Monitor.TryEnter(Planes[id].Baggages))
                    {
                        Console.WriteLine("Plane {0} BaggageCount: {1}", id, Planes[id].Baggages.Count);
                        Monitor.Exit(Planes[id].Baggages);
                    }
                }
                //for (int i = 0; i < TerminalQueues.Count; i++)
                //{
                //    int id = TerminalQueues.ElementAt(i).Key;
                //    Console.WriteLine("----------------------------------------------");
                //    Console.WriteLine("Terminal {0} BaggageCount: {1}", id, TerminalQueues[id].Count);
                //    Console.WriteLine("Plane {0} BaggageCount: {1}", id, Planes[id].Baggages.Count);

                //}
                Monitor.Exit(TerminalQueues);
            }
            Thread.Sleep(50);

        }
    }
}