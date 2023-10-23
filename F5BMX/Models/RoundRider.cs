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
        this.id = seriesRider.id;
        this.firstName = seriesRider.firstName;
        this.lastName = seriesRider.lastName;
        this.club = seriesRider.club;
        this.plateNumber = seriesRider.plateNumber;
        this.yearOfBirth = seriesRider.yearOfBirth;
    }

    public Guid id { get; init; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string club { get; set; }
    public string plateNumber { get; set; }
    public int yearOfBirth { get; set; }
    public int yearAge => DateTime.Now.Year - yearOfBirth;


    public uint[] motoPositions { get; set; } = new uint[3];
    public uint finalPosition { get; set; }

    public uint roundPoints { get; set; }

    public PromotionEnum promotion { get; set; } = PromotionEnum.NoChange;

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
