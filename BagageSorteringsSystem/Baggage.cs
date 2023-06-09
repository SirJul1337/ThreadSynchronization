﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagageSorteringsSystem
{
    /// <summary>
    /// Class for baggage object
    /// </summary>
    public class Baggage
    {
        public string Name { get; set; }
        public int GateId { get; set; }
        public List<string> Log = new List<string>();
        public Baggage(string name, int gateId)
        {
            Name = name;
            GateId = gateId;
        }
    }
}
