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
        this.id = seriesFormula.id;
        this.order = seriesFormula.order;
        this.name = seriesFormula.name;
        this.minAge = seriesFormula.minAge;
        this.maxAge = seriesFormula.maxAge;
        this.dashForCash = seriesFormula.dashForCash;

        this.riders = new ObservableCollection<RoundRider>();
        this.moto1 = new List<Race>();
        this.moto2 = new List<Race>();
        this.moto3 = new List<Race>();
        this.final = new List<Race>();
    }

    public Guid id { get; set; }
    public uint order { get; set; }
    public string name { get; set; }
    public uint minAge { get; set; }
    public uint maxAge { get; set; }
    public bool dashForCash { get; set; }

    public ObservableCollection<RoundRider> riders { get; init; }
    public List<Race> moto1 { get; init; }
    public List<Race> moto2 { get; init; }
    public List<Race> moto3 { get; init; }
    public List<Race> final { get; init; }

}
