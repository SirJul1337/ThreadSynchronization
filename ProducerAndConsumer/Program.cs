using ProducerAndConsumer;

public class Program
{
    public static Cookie[] cookieArray = new Cookie[10];
    public static Queue<Cookie> cookieQ = new Queue<Cookie>();
    public static List<Cookie> consumedQueueCookies = new List<Cookie>();
    public static int ProducedCookies = 0;
    public static void Main()
    {
        Console.WriteLine("1. Array\n2. Queue");
        switch (Console.ReadLine())
        {
            case "1":
                break;
            case "2":
                Console.Clear();
                Producer producer= new Producer();
                Consumer consumer= new Consumer();
                ThreadPool.QueueUserWorkItem(producer.ProduceQueue);
                ThreadPool.QueueUserWorkItem(consumer.ConsumeQueue);
                break;
            default: Console.WriteLine("Wrong Input"); break;
        }
        Console.ReadLine();
    }
}