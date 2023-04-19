using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public void ProduceArray(object callback)
        {
            int producedArrayCookies = 0;
            while (true)
            {

                string arrayWaitString = "";

                try
                {
                    Monitor.Enter(Program.cookieArray);
                    Console.Clear();
                    //TODO: Manager needed
                    if (Program.Index < 3)
                    {

                        for (int i = Program.Index; i <= Program.cookieArray.Length-1; i++)
                        {
                            Program.Index = i;
                            if(Program.cookieArray[Program.Index] == null)
                            {
                                producedArrayCookies++;
                                Program.cookieArray[Program.Index] = new Cookie();

                            }

                            
                        }
                        Monitor.PulseAll(Program.cookieArray);
                    }
                    else
                    {
                        arrayWaitString = "[Array] Producer Waits...";
                    }
                        


                }
                finally
                {
                    
                    Monitor.Exit(Program.cookieArray);
                    Console.WriteLine("[Array] Produced cookies: {0}", producedArrayCookies);

                    Console.WriteLine("[Array] Cookies in array: {0}", (Program.Index)+1);
                    Console.WriteLine("[Array] Consumed cookies: {0}", Program.consumedArrayCookies.Count);
                    Console.WriteLine("{0}", arrayWaitString);
                    Thread.Sleep(200);
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
            int ProducedCookies = 0;
            while (true)
            {
                string waitString = "";

                Monitor.Enter(Program.cookieQ);
                try
                {
                    Console.Clear();
                    if (Program.cookieQ.Count < 3)
                    {
                        while (Program.cookieQ.Count < 10)
                        {
                            ProducedCookies++;
                            Program.cookieQ.Enqueue(new Cookie());
                        }
                        Monitor.PulseAll(Program.cookieQ);
                    }
                    else
                    {
                        waitString = "[Queues] Producer Waits...";
                    }
                }
                finally
                {
                    Monitor.Exit(Program.cookieQ);
                    Console.WriteLine("[Queues] Produced cookies: {0}", ProducedCookies);
                    Console.WriteLine("[Queues] Cookies in queue: {0}", Program.cookieQ.Count);
                    Console.WriteLine("[Queues] Consumed cookies: {0}", Program.consumedQueueCookies.Count);
                    Console.WriteLine("{0}", waitString);
                    Thread.Sleep(200);
                }
            }
        }
    }
}
