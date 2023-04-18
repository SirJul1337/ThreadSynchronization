public class Program
{
    static int number = 0;
    
    public static void Main()
    {
        Threads threads = new Threads();
        ThreadPool.QueueUserWorkItem(threads.CountUp);
        ThreadPool.QueueUserWorkItem(threads.CountDown);
        Console.WriteLine("Done");
        Console.ReadLine();
    }

    public class Threads
    {
        private static readonly object _lock = new object();
        public void CountUp(object callback)
        {
            while (true)
            {
                Monitor.Enter(_lock);
                try
                {
                    number += 2;
                    Console.Clear();
                    Console.WriteLine(number);

                }
                finally
                {
                    Monitor.Exit(_lock);
                    Thread.Sleep(1000);

                }
            }
        }
        public void CountDown(object callback)
        {
            while (true)
            {
                Monitor.Enter(_lock);
                try
                {
                    number--;
                    Console.Clear();
                    Console.WriteLine(number);

                }
                finally
                {
                    Monitor.Exit(_lock);
                    Thread.Sleep(1000);

                }
            }
        }
    }
}
