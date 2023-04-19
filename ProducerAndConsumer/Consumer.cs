using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerAndConsumer
{
    public class Consumer
    {


        public Consumer()
        {

        }
        /// <summary>
        /// Method trying to lock Cookie array, if  index is 0 there is no cookies and i will Wait  until pulse
        /// If there is cookies in array in will take the cookie on the spot and put it into a consumedArrayCookies, and set the index it took it from to null.
        /// And decrement the index and exit the lock
        /// </summary>
        /// <param name="callback"></param>
        public void ConsumeArray(object callback)
        {
            while (true)
            {

                try
                {
                    if (Monitor.TryEnter(Program.cookieArray))
                    {
                        if (Program.Index == 0)
                        {
                            Monitor.Wait(Program.cookieArray);
                        }
                        else
                        {


                            Program.consumedArrayCookies.Add(Program.cookieArray[Program.Index]);
                            Program.cookieArray[Program.Index] = null;
                            Program.Index--;
                            
                            Monitor.Exit(Program.cookieArray);
                        }

                    }

                }
                finally
                {
                    Thread.Sleep(200);
                }
            }
        }
        /// <summary>
        /// The method will try to enter the queue check IF count is 0'
        /// Else it will dequeue from the queue and add it to the list of consumedQueueCookies
        /// </summary>
        /// <param name="callback"></param>
        public void ConsumeQueue(object callback)
        {
            while (true)
            {
                try
                {
                    if (Monitor.TryEnter(Program.cookieQ))
                    {
                        if (Program.cookieQ.Count == 0)
                        {
                            Monitor.Wait(Program.cookieQ);
                        }
                        else
                        {
                            Program.consumedQueueCookies.Add(Program.cookieQ.Dequeue());
                        }
                        Monitor.Exit(Program.cookieQ);
                    }

                }
                finally
                {
                    Thread.Sleep(200);
                }
            }
        }
    }
}
