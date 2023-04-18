using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerAndConsumer
{
    public class Consumer
    {
        private List<Cookie> consumedArrayCookies = new List<Cookie>();
        

        public Consumer()
        {

        }
        public void ConsumeArray()
        {
            //Stack

            try
            {
                if (Monitor.TryEnter(Program.cookieArray))
                {
                    for (int i = 0; i < Program.cookieArray.Length; i++)
                    {
                        if (Program.cookieArray[i] != null)
                        {
                            consumedArrayCookies.Add(Program.cookieArray[i]);
                            Program.cookieArray[i] = null;
                        }
                    }
                    Monitor.Wait(Program.cookieArray);
                }
            }
            finally
            {
                Monitor.Exit(Program.cookieArray);
            }
        }
        /// <summary>
        /// 
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
                    Thread.Sleep(500);
                }
            }
        }
    }
}
