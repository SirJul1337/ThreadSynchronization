using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagageSorteringsSystem
{
    public class Customer
    {
        public Customer()
        {

        }

        public void LoadFromFile()
        {

        }
        /// <summary>
        /// Customer that just keep generating Customer baggeges
        /// </summary>
        /// <param name="callback"></param>
        public void AutoGenerate(object callback)
        {
            while (true)
            {
                try
                {
                    if (Program.CustomerLine.Count < 100)
                    {
                        Random r = new Random();
                        Baggage baggage = new Baggage("Test", r.Next(1, 3));
                        baggage.Log.Add(String.Format("{0} | Baggage auto generated", DateTime.Now));
                        Program.CustomerLine.Add(baggage);
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
