using System.ComponentModel;

namespace DiningPhilosophers;

public class Philosopher
{
    public int Id;
    private int _rightFork;
    private int _leftFork;
    public Philosopher(int id )
    {
        Id = id;
        if(id == 0)
        {
            _rightFork = 0;
            _leftFork = 4;
        }
        else 
        {
            _rightFork = Id;
            _leftFork = Id-1;
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
                if (Monitor.TryEnter(Program.Forks[_rightFork]))
                {
                    if (Monitor.TryEnter(Program.Forks[_leftFork]))
                    {
                        Console.WriteLine("Philosopher{0} tried {1} times before eating", Id, tries);
                        tries = 0;
                        ate = true;
                        Eat();
                        Monitor.Exit(Program.Forks[_leftFork]);
                    }
                    Monitor.Exit(Program.Forks[_rightFork]);
                }
                tries++;
                
            }
            finally
            {

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
            //Thread.Abort is deprecated
        }
    }
    private void Eat()
    {
        Console.WriteLine("Philosopher{0} is eating", Id);
        Random r = new Random();
        Thread.Sleep(r.Next(500, 2000));
    }
    private void Think()
    {
        Console.WriteLine("Philosopher{0} is thinking", Id);
        Random r = new Random();
        Thread.Sleep(r.Next(500, 2000));

    }
}
