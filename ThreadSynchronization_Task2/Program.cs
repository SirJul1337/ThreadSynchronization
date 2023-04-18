public class Program
{
    public static int charCount = 0;
    private static readonly object _lock = new object();

    public static void Main()
    {
        WriterObject star = new('*');
        WriterObject hashtag = new('#');
        ThreadPool.QueueUserWorkItem(star.WriteConsole);
        ThreadPool.QueueUserWorkItem(hashtag.WriteConsole);
        Console.ReadLine();
    }
}
/// <summary>
/// 
/// </summary>
public class WriterObject
{
    private char _char;
    private static readonly object _lock = new object();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="c"></param>
    public WriterObject(char c)
    {
        _char = c;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="callback"></param>
    public void WriteConsole(object callback)
    {
        while (true)
        {
            Monitor.Enter(_lock);

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
            finally { Monitor.Exit(_lock); }
        }
    }

}
