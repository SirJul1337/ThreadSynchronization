public class Program
{
    public static int number = 0;
    /// <summary>
    /// Method instanciating Threads class, and starting work items in threadpool with CountUp and CountDown
    /// </summary>
    public static void Main()
    {
        Threads threads = new Threads();
        ThreadPool.QueueUserWorkItem(threads.CountUp);
        ThreadPool.QueueUserWorkItem(threads.CountDown);
        Console.ReadLine();
    }
}
public class Threads
{
    private static readonly object _lock = new object();
    /// <summary>
    /// This method is incrementing the number from Program class by 2,  it is locking  on the _lock object to ensure Countdown doesnt share the resource while doing it.
    /// After that it will exit the lock and sleep for 1000 ms
    /// </summary>
    /// <param name="callback"></param>
    public void CountUp(object callback)
    {
        while (true)
        {
            Monitor.Enter(_lock);
            try
            {
                Program.number += 2;
                Console.Clear();
                Console.WriteLine(Program.number);

            }
            finally
            {
                Monitor.Exit(_lock);
                Thread.Sleep(1000);

            }
        }
    }
    /// <summary>
    /// This method is locking on the _lock object and decrementing the number from program class,
    /// and exiting the lock after its done. It will sleep for 1000 ms before doing it again
    /// </summary>
    /// <param name="callback"></param>
    public void CountDown(object callback)
    {
        while (true)
        {
            Monitor.Enter(_lock);
            try
            {
                Program.number--;
                Console.Clear();
                Console.WriteLine(Program.number);

            }
            finally
            {
                Monitor.Exit(_lock);
                Thread.Sleep(1000);

            }
        }
    }
}

