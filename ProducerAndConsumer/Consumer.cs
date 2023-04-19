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
        public void ConsumeArray(object callback)
        {
            //Stack
            while (true)
            {

                try
                {
                    if (Monitor.TryEnter(Program.cookieArray))
                    {
                        if (Program.Index <=1)
                        {
                            Monitor.Wait(Program.cookieArray);
                        }
                        else
                        {
                            for (int i = Program.Index; i > 0; i--)
                            {

                                Program.consumedArrayCookies.Add(Program.cookieArray[i]);
                                Program.cookieArray[i] = null;
                                Program.Index--;
                            }
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
                    Thread.Sleep(200);
                }
            }
        }
    }
}
