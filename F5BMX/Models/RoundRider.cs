using F5BMX.Core;
using F5BMX.Interfaces;
using System;
using System.Text.Json.Serialization;
using System.Windows.Media.TextFormatting;

namespace F5BMX.Models;

internal class RoundRider : ModelBase
{

    public RoundRider(SeriesRider seriesRider)
    {
        this.seriesRider = seriesRider;
    }

    private SeriesRider seriesRider;
    public Guid id { get => seriesRider.id; }
    public string firstName { get => seriesRider.firstName; }
    public string lastName { get => seriesRider.lastName; }
    public string plateNumber { get => seriesRider.plateNumber; }


    public uint moto1pos { get; set; }
    public uint moto2pos { get;set; }
    public uint moto3pos { get; set; }
    public uint finalNumber { get; set; }
    public uint finalpos { get; set; }

}
