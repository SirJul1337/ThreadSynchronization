namespace FlaskeAutomaten;

public class Program
{
    public static Queue<Bottle> BottleBelt = new();
    public static Queue<ColaBottle> ColaBelt = new();
    public static Queue<BeerBottle> BeerBelt = new();
    public static List<ColaBottle> ConsumedColaBottles = new();
    public static List<BeerBottle> ConsumedBeerBottle = new();
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
                Console.WriteLine("Split cola bottles: {0}",ColaBelt.Count);
                Console.WriteLine("Split beer bottles: {0}",BeerBelt.Count);
                Console.WriteLine("Consumed cola bottles: {0}", ConsumedColaBottles.Count);
                Console.WriteLine("Consumed beer bottles: {0}", ConsumedBeerBottle.Count);

                Monitor.Exit(BottleBelt);
            }
            Thread.Sleep(10);
        }

    }
}