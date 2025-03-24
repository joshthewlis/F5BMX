using F5BMX.Core;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace F5BMX.Models;

internal class RoundFormula : ViewModelBase
{

    public RoundFormula() : this(new SeriesFormula())
    { }

    public RoundFormula(SeriesFormula seriesFormula)
    {
        ID = seriesFormula.ID;
        Order = seriesFormula.Order;
        Name = seriesFormula.Name;
        MinAge = seriesFormula.MinAge;
        MaxAge = seriesFormula.MaxAge;
        DashForCash = seriesFormula.DashForCash;

        Riders = new ObservableCollection<RoundRider>();
        Moto1 = new List<Race>();
        Moto2 = new List<Race>();
        Moto3 = new List<Race>();
        Final = new List<Race>();
    }

    public Guid ID { get; set; }
    public uint Order { get; set; }
    public string Name { get; set; }
    public uint MinAge { get; set; }
    public uint MaxAge { get; set; }
    public bool DashForCash { get; set; }

    public ObservableCollection<RoundRider> Riders { get; init; }
    public List<Race> Moto1 { get; init; }
    public List<Race> Moto2 { get; init; }
    public List<Race> Moto3 { get; init; }
    public List<Race> Final { get; init; }

}
