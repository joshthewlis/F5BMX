using System;

namespace F5BMX.Models;

internal class Race
{

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

}
