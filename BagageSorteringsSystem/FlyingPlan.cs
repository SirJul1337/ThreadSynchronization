
using Newtonsoft.Json;

namespace BagageSorteringsSystem;

/// <summary>
/// Used to store all Deserialized object in json
/// </summary>
public partial class FlyingPlan
{
    [JsonProperty("Flyveplan")]
    public List<Flyveplan> FlyvePlaner { get; set; }
}

/// <summary>
/// Used for deserializing objects in json
/// </summary>
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
