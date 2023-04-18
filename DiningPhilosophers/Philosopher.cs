using System.ComponentModel;

namespace DiningPhilosophers;

public class Philosopher
{
    public int Id;
    private int _rightFork;
    private int _leftFork;
    /// <summary>
    /// Constructor has a parameter to use as Id, The id is use by the Philosoph to know what forks the Philosoph is going to use
    /// </summary>
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
    /// <summary>
    /// Method to simulate the philosoph living, He thinks and waits for forks
    /// </summary>
    /// <param name="callback"></param>
    public void Live(object callback)
    {
        while (true)
        {
            Think();
            WaitForks();
        }
    }
    /// <summary>
    /// Method for philosoph trying to lock forks, Philosoph is trying to lock  forks, if it doesnt work,
    /// it will Exit any locks if it got any depends of how far the philosoph came in the nested if statements
    /// If he fails, it will increment tries counter, if he havent ate for a 100 tries, he dies
    /// </summary>
    private void WaitForks()
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
    /// <summary>
    /// Dead forever, cant abort anymore, cause it can cause unstable code
    /// </summary>
    private void Dead()
    {
        Console.WriteLine("Philosopher{0} is DEAD", Id);
        while (true)
        {
            //Thread.Abort is deprecated
        }
    }
    /// <summary>
    /// Eating for a random amount of time 500ms-2000ms to simulate eating
    /// </summary>
    private void Eat()
    {
        Console.WriteLine("Philosopher{0} is eating", Id);
        Random r = new Random();
        Thread.Sleep(r.Next(500, 2000));
    }
    /// <summary>
    /// Thinking for a random amount of time 500ms-2000ms to simulate thinking
    /// </summary>
    private void Think()
    {
        Console.WriteLine("Philosopher{0} is thinking", Id);
        Random r = new Random();
        Thread.Sleep(r.Next(500, 2000));

    }
}
