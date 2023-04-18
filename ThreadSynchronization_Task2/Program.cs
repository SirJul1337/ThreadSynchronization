public class Program
{
    public static int charCount = 0;
    public static readonly object _lock = new object();

    /// <summary>
    /// Creating Ojects with what charachter in parameter, and starting the work item
    /// </summary>
    public static void Main()
    {
        WriterClass star = new('*');
        WriterClass hashtag = new('#');
        ThreadPool.QueueUserWorkItem(star.WriteConsole);
        ThreadPool.QueueUserWorkItem(hashtag.WriteConsole);
        Console.ReadLine();
    }
}
/// <summary>
/// Class with constructor, with paramter c, the parameter is used when writing 60 charachters
/// </summary>
public class WriterClass
{
    private char _char;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="c"></param>
    public WriterClass(char c)
    {
        _char = c;
    }
    /// <summary>
    /// This method writes out the _char variable 60 times, and using lock from program class, so only one Writerclass object can write at a time
    /// After it wrote the 60 characters, it will print how many charachter the program has written in total, and then exit the lock, so next object can use the lock
    /// </summary>
    /// <param name="callback"></param>
    public void WriteConsole(object callback)
    {
        while (true)
        {
            Monitor.Enter(Program._lock);

            try
            {
                Console.WriteLine();
                for (int i = 0; i < 60; i++)
                {
                    Console.Write(_char);
                    Program.charCount++;
                }
                Console.Write(Program.charCount);
                Thread.Sleep(1000);
            }
            finally { Monitor.Exit(Program._lock); }
        }
    }

}
