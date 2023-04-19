using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomaten
{
    /// <summary>
    /// Interface with GetBottle 
    /// </summary>
    public interface IConsumer
    {
        public void GetBottle(object callback);
    }
}
