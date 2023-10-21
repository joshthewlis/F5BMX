using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace F5BMX.Models;

internal class Race
{

    public int raceNumber { get; set; }
    public Guid? gate1 { get; set; }
    public Guid? gate2 { get; set; }
    public Guid? gate3 { get; set; }
    public Guid? gate4 { get; set; }
    public Guid? gate5 { get; set; }
    public Guid? gate6 { get; set; }
    public Guid? gate7 { get; set; }
    public Guid? gate8 { get; set; }

    public void setGateRider(int gate, Guid rider)
    {
        switch (gate)
        {
            case 1:
                this.gate1 = rider;
                break;
            case 2:
                this.gate2 = rider;
                break;
            case 3:
                this.gate3 = rider;
                break;
            case 4:
                this.gate4 = rider;
                break;
            case 5:
                this.gate5 = rider;
                break;
            case 6:
                this.gate6 = rider;
                break;
            case 7:
                this.gate7 = rider;
                break;
            case 8:
                this.gate8 = rider;
                break;
            default:
                throw new ArgumentException("Invalid Gate Number", "gate");
        }
    }

    public int findRiderGate(Guid rider)
    {
        if (gate1 == rider)
            return 1;
        if (gate2 == rider)
            return 2;
        if (gate3 == rider)
            return 3;
        if (gate4 == rider)
            return 4;
        if (gate5 == rider)
            return 5;
        if (gate6 == rider)
            return 6;
        if (gate7 == rider)
            return 7;
        if (gate8 == rider)
            return 8;

        return 0;
    }

    [JsonIgnore]
    public List<Guid?> riderList => new List<Guid?>() { gate1, gate2, gate3, gate4, gate5, gate6, gate7, gate8 };

}
