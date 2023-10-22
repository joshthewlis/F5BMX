using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace F5BMX.Models;

internal class Race
{

    public Race()
    {
        this.gates = new Dictionary<uint, Guid>();
    }

    [JsonIgnore]
    public int finalNumber { get; set; }
    public int raceNumber { get; set; }

    public Dictionary<uint, Guid> gates { get; set; }

    public void setGateRider(uint gate, Guid rider)
    {
        this.gates[gate] = rider;
        return;
    }

    public uint findRiderGate(Guid rider)
    {
        return this.gates.FirstOrDefault(x => x.Value == rider).Key;
    }

    [JsonIgnore]
    public List<Guid> riderList => this.gates.Values.ToList();

}
