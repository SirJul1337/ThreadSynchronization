using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProducerAndConsumer
{
    public class Producer
    {
        /// <summary>
        /// Produces cookies in array, array manager should look where to put cookie
        /// </summary>
        public void ProduceArray()
        {
            while (true)
            {

                Monitor.Enter(Program.cookieArray);
                try
                {   
                    //TODO: Manager needed
                    for (int i = 0; i < Program.cookieArray.Length; i++)
                    {
                        Program.cookieArray[i] = new Cookie();
                    }
                }
                finally
                {
                    Monitor.Exit(Program.cookieArray);
                }
            }
        }
        /// <summary>
        /// Method producing cookies in the queue if there is less than n coockies (n = 3) 
        /// It will PulseAll waiting on the coockieQ everytime is preduces
        /// </summary>
        /// <param name="callback"></param>
        public void ProduceQueue(object callback)
        {
            
            while (true)
            {

                Monitor.Enter(Program.cookieQ);
                try
                {
                    Console.Clear();
                    if (Program.cookieQ.Count < 3)
                    {
                        Program.ProducedCookies++;
                        Program.cookieQ.Enqueue(new Cookie());
                        Monitor.PulseAll(Program.cookieQ);
                    }
                    else
                    {
                        Console.WriteLine("[Queues] Producer Waits...");
                    }
                }
                finally
                {
                    Monitor.Exit(Program.cookieQ);
                    Console.WriteLine("[Queues] Produced cookies: {0}", Program.ProducedCookies++);
                    Console.WriteLine("[Queues] Cookies in queue: {0}", Program.cookieQ.Count);
                    Console.WriteLine("[Queues] Consumed cookies: {0}", Program.consumedQueueCookies.Count);
                    Thread.Sleep(200);
                }
            }
        }
    }
}
