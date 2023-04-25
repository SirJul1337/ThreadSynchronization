using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten
{
    public class ColaConsumer : IConsumer
    {
        /// <summary>
        /// Method consuming beer from ColaBelt. If Cola.Count is 0 it will wait.
        /// It will dequeue from belt, and put into list called ConsumedColaBottles
        /// GetBottle is implemented by the interface IConsumer
        /// </summary>
        /// <param name="callback"></param>
        public void GetBottle(object obj)
        {
            //CancellationToken cancellationToken = (CancellationToken)obj;
            while (true)
            {
                try
                {
                    if (Program.ColaBelt.Count == 0)
                    {
                        Program.Logger.Information("ColaBelt is waiting");
                    }
                    if (Monitor.TryEnter(Program.ConsumedColaBottles))
                    {
                        Program.Logger.Information("Cola added to ConsumedColaBottles");
                        Program.ConsumedColaBottles.Add(Program.ColaBelt.Take());
                    }
                }
                finally
                {
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
