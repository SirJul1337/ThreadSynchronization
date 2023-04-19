namespace FlaskeAutomaten;

public class Program
{
    public static Queue<Bottle> BottleBelt = new();
    public static Queue<ColaBottle> ColaBelt = new();
    public static Queue<BeerBottle> BeerBelt = new();
    public static List<ColaBottle> ConsumedColaBottles = new();
    public static List<BeerBottle> ConsumedBeerBottle = new();
    /// <summary>
    /// Main Method Instaciating BottleProducer and Splitter.
    /// Instanciating List of IConsumer called consumer, where i put in ColaConsumer and BeerConsumer
    /// Where I go through the list and I USE the Interface to Start thread (ThreadPool) on GetBottle that is IMPLEMENTED in ColaConsumer And BeerConsumer
    /// Starting Splitter and Producer With ThreadPool
    /// Main Thread wil then write to console count on different list and queues to see the application work
    /// </summary>
    public static void Main()
    {
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
        ThreadPool.QueueUserWorkItem(splitter.SplitBottles);
        ThreadPool.QueueUserWorkItem(producer.MakeBottle);

        while (true)
        {
            Console.Clear();
            if (Monitor.TryEnter(BottleBelt))
            {
                Console.WriteLine("Bottles in first buffer: {0}",BottleBelt.Count);
                Console.WriteLine("Cola Buffer: {0}",ColaBelt.Count);
                Console.WriteLine("Beer Buffer: {0}",BeerBelt.Count);
                Console.WriteLine("Consumed cola bottles: {0}", ConsumedColaBottles.Count);
                Console.WriteLine("Consumed beer bottles: {0}", ConsumedBeerBottle.Count);

                Monitor.Exit(BottleBelt);
            }
            Thread.Sleep(10);
        }

    }
}