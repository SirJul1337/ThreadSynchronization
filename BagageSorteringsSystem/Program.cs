using BagageSorteringsSystem;

public class Program
{
    public static Queue<Baggage> Baggages = new();
    public static Dictionary<int, Queue<Baggage>> TerminalQueues = new();
    public static Dictionary<int, List<Baggage>> PlaneBaggage = new();
    public static void Main()
    {
        CheckIn checkIn = new();
        BaggageHandling handling = new BaggageHandling();
        ThreadPool.QueueUserWorkItem(checkIn.Open);
        ThreadPool.QueueUserWorkItem(handling.Sorting);
        for (int i = 1; i <= 5; i++)
        {
            TerminalQueues.Add(i, new Queue<Baggage>());
            PlaneBaggage.Add(i, new List<Baggage>());
        }
        for (int i = 0; i < TerminalQueues.Keys.Count; i++)
        {
            Terminal terminal = new Terminal(TerminalQueues.ElementAt(i).Key);
            ThreadPool.QueueUserWorkItem(terminal.ConsumeBaggage);
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Baggages in sorting system: {0}", Baggages.Count);
            if (Monitor.TryEnter(TerminalQueues))
            {

                for (int i = 0; i < TerminalQueues.Count; i++)
                {
                    int id = TerminalQueues.ElementAt(i).Key;
                    Console.WriteLine("----------------------------------------------");
                    Console.WriteLine("Terminal {0} BaggageCount: {1}", id, TerminalQueues[id].Count);
                    Console.WriteLine("Plane {0} BaggageCount: {1}", id, PlaneBaggage[id].Count);

                }
                Monitor.Exit(TerminalQueues);
            }
            Thread.Sleep(50);

        }
    }
}