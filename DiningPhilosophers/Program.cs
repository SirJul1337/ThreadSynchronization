using DiningPhilosophers;

public class Program
{
    public static Fork[] forks = new Fork[5] { new Fork(), new Fork(), new Fork(), new Fork(),  new Fork() };
    static Philosopher[] Philosophers = new Philosopher[5];
    public static void Main()
    {
        for (int i = 0; i < 5; i++)
        {
            Philosopher phil = new Philosopher(i);
            Philosophers[i] = phil;
        }
        for (int i = 0; i < Philosophers.Length; i++)
        {
            ThreadPool.QueueUserWorkItem(Philosophers[i].Do);
        }
        Console.Read();
    }
}
