using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaggageSortertingSystemWpf.Models;

public partial class FlyingPlan
{
    [JsonProperty("Flyveplan")]
    public Flyveplan[] Flyveplan { get; set; }
}

public partial class Flyveplan
{
    [JsonProperty("GateId")]
    public int GateId { get; set; }

    [JsonProperty("Afgangstid")]
    public DateTimeOffset Afgangstid { get; set; }

    [JsonProperty("Destination")]
    public string Destination { get; set; }
    [JsonProperty("MaxCustomers")]
    public int MaxCustomers { get; set; }
}
