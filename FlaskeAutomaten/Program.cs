using Serilog;
using Serilog.Core;
using System.Collections.Concurrent;
using System.Net;

namespace FlaskeAutomaten;

public class Program
{
    public static BlockingCollection<Bottle> BottleBelt = new();
    public static BlockingCollection<ColaBottle> ColaBelt = new();
    public static BlockingCollection<BeerBottle> BeerBelt = new();
    public static List<ColaBottle> ConsumedColaBottles = new();
    public static List<BeerBottle> ConsumedBeerBottles = new();
    public static ILogger Logger;
    public static ConsoleKey ConsoleKey;
    /// <summary>
    /// Main Method Instaciating BottleProducer and Splitter.
    /// Instanciating List of IConsumer called consumer, where i put in ColaConsumer and BeerConsumer
    /// Where I go through the list and I USE the Interface to Start thread (ThreadPool) on GetBottle that is IMPLEMENTED in ColaConsumer And BeerConsumer
    /// Starting Splitter and Producer With ThreadPool
    /// Main Thread wil then write to console count on different list and queues to see the application work
    /// </summary>
    public static void Main()
    {
        Logger = new LoggerConfiguration()
        .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();
        BottleProducer producer = new(5);
        Splitter splitter = new Splitter();

        List<IConsumer> consumers = new();
        ColaConsumer colaConsumer = new();
        BeerConsumer beerConsumer = new();
        consumers.Add(colaConsumer);
        consumers.Add(beerConsumer);
        for (int i = 0; i < consumers.Count; i++)
        {


            ThreadPool.QueueUserWorkItem(consumers[i].GetBottle);
        }
        ConsoleReader reader = new ConsoleReader();
        ThreadPool.QueueUserWorkItem(reader.ReadConsole);
        CancellationTokenSource splitterToken = new CancellationTokenSource();
        Thread thread = new Thread(splitter.SplitBottles);
        ThreadPool.QueueUserWorkItem(splitter.SplitBottles, splitterToken);
        CancellationTokenSource createrToken = new CancellationTokenSource();
        ThreadPool.QueueUserWorkItem(producer.MakeBottle, createrToken);

        Random r = new Random();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Bottles in first buffer: {0}", BottleBelt.Count);
            Console.WriteLine("Cola Buffer: {0}", ColaBelt.Count);
            Console.WriteLine("Beer Buffer: {0}", BeerBelt.Count);
            Console.WriteLine("Consumed cola bottles: {0}", ConsumedColaBottles.Count);
            Console.WriteLine("Consumed beer bottles: {0}", ConsumedBeerBottles.Count);
            Thread.Sleep(10);
            if(ConsoleKey == ConsoleKey.C)
            {
                splitterToken.Cancel();
                createrToken.Cancel();
            }
        }
    }

}