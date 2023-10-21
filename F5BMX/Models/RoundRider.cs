using F5BMX.Core;
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
        this.plateNumber = seriesRider.plateNumber;
        this.yearOfBirth = seriesRider.yearOfBirth;
    }

    public Guid id { get; init; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string plateNumber { get; set; }
    public int yearOfBirth { get; set; }


    public uint moto1pos { get; set; }
    public uint moto2pos { get;set; }
    public uint moto3pos { get; set; }
    public uint finalNumber { get; set; }
    public uint finalpos { get; set; }

}
