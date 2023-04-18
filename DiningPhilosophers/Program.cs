using DiningPhilosophers;


public class Program
{
    public static Fork[] Forks = new Fork[5] { new Fork(), new Fork(), new Fork(), new Fork(),  new Fork() };
    static Philosopher[] Philosophers = new Philosopher[5];
    /// <summary>
    /// Array with 5 forks object, to lock them when they are in use
    /// Instanciating 5 philosophers, and parsing i as parameter, used as Id
    /// FOR loop to start work thread on each Philosoph
    /// </summary>
    public static void Main()
    {
        for (int i = 0; i < 5; i++)
        {
            Philosopher phil = new Philosopher(i);
            Philosophers[i] = phil;
        }
        for (int i = 0; i < Philosophers.Length; i++)
        {
            ThreadPool.QueueUserWorkItem(Philosophers[i].Live);
        }
        Console.Read();
    }
}
