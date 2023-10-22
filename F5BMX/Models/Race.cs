﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace F5BMX.Models;

internal class Race
{

    public Race()
    {
        this.gates = new Dictionary<int, Guid>();
    }

    public int raceNumber { get; set; }

    public Dictionary<int, Guid> gates { get; set; }

    public void setGateRider(int gate, Guid rider)
    {
        this.gates[gate] = rider;
        return;
    }

    public int findRiderGate(Guid rider)
    {
        return this.gates.FirstOrDefault(x => x.Value == rider).Key;
    }

    [JsonIgnore]
    public List<Guid> riderList => this.gates.Values.ToList();

}
