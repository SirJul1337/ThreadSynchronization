using System.ComponentModel;

namespace DiningPhilosophers;

public class Philosopher
{
    public Enum Status;
    public enum Statuses { Eat, Think }
    public int Id;
    private int rightFork;
    private int leftFork;
    public Philosopher(int id )
    {
        Id = id;
        if(id == 0)
        {
            rightFork = 0;
            leftFork = 4;
        }
        else 
        {
            rightFork = Id;
            leftFork = Id-1;
        }
    }
    public void Do(object callback)
    {
        while (true)
        {
            Think();
            Wait();
        }
    }
    private void Wait()
    {
        int tries = 0;
        bool ate = false;
        Console.WriteLine("Philosopher{0} is waiting", Id);
        while (!ate)
        {
            try
            {
                Monitor.TryEnter(Program.forks[rightFork]);
                Monitor.TryEnter(Program.forks[leftFork]);
            }
            finally
            {
                if(Monitor.IsEntered(Program.forks[rightFork]) && Monitor.IsEntered(Program.forks[leftFork]))
                {
                    Console.WriteLine("Philosopher{0} tried {1} times before eating",Id,tries);
                    tries = 0;
                    ate = true;
                    Eat();
                }
                else if(Monitor.IsEntered(Program.forks[rightFork]))
                {
                    tries++;
                    Monitor.Exit(Program.forks[rightFork]);
                }
                else if (Monitor.IsEntered(Program.forks[leftFork]))
                {
                    tries++;
                    Monitor.Exit(Program.forks[leftFork]);
                }
                if(tries == 100)
                {
                    Dead();
                }
                Thread.Sleep(100);
            }

        }
    }
    private void Dead()
    {
        Console.WriteLine("Philosopher{0} is DEAD", Id);
        while (true)
        {

        }
    }
    private void Eat()
    {
        Console.WriteLine("Philosopher{0} is eating", Id);
        Random r = new Random();
        Thread.Sleep(r.Next(500, 2000));
        Monitor.Exit(Program.forks[rightFork]);
        Monitor.Exit(Program.forks[leftFork]);
    }
    private void Think()
    {
        Console.WriteLine("Philosopher{0} is thinking", Id);
        Random r = new Random();
        Thread.Sleep(r.Next(500, 2000));

    }
}
