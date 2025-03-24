using F5BMX.Core;
using F5BMX.Enums;
using F5BMX.Interfaces;
using System;
using System.Text.Json.Serialization;
using System.Windows.Media.TextFormatting;

namespace F5BMX.Models;

internal class RoundRider : ModelBase, IRider
{

    public RoundRider() : this(new SeriesRider())
    { }

    public RoundRider(SeriesRider seriesRider)
    {
        this.ID = seriesRider.ID;
        this.FirstName = seriesRider.FirstName;
        this.LastName = seriesRider.LastName;
        this.Club = seriesRider.Club;
        this.PlateNumber = seriesRider.PlateNumber;
        this.YearOfBirth = seriesRider.YearOfBirth;
    }

    public Guid ID { get; init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Club { get; set; }
    public string PlateNumber { get; set; }
    public int YearOfBirth { get; set; }
    public int YearAge => DateTime.Now.Year - YearOfBirth;


    public uint[] MotoPositions { get; set; } = new uint[3];
    public uint FinalPosition { get; set; }

    public uint RoundPoints { get; set; }

    public PromotionEnum Promotion { get; set; } = PromotionEnum.NoChange;

    /*
    public void setMotoPos(uint moto, uint pos)
    {
        switch(moto)
        {
            case 1:
                moto1pos = pos;
                break;
            case 2:
                moto2pos = pos;
                break;
            case 3:
                moto3pos = pos;
                break;
        }
    }
    */

}
