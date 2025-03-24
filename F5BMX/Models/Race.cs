using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace F5BMX.Models;

internal class Race
{

    public Race()
    {
        Gates = new Dictionary<uint, Guid>();
    }


    public uint FinalNumber { get; set; }
    public int RaceNumber { get; set; }

    public Dictionary<uint, Guid> Gates { get; set; }

    public void setGateRider(uint gate, Guid rider)
    {
        Gates[gate] = rider;
        return;
    }

    public uint findRiderGate(Guid rider)
    {
        return Gates.FirstOrDefault(x => x.Value == rider).Key;
    }

    [JsonIgnore]
    public List<Guid> RiderList => Gates.Values.ToList();

}
