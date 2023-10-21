using F5BMX.Core;
using System;
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
        this.seriesFormula = seriesFormula;

        this.riders = new ObservableCollection<RoundRider>();
        this.moto1 = new List<Race>();
        this.moto2 = new List<Race>();
        this.moto3 = new List<Race>();
        this.final = new List<Race>();
    }

    /* SERIES FORMULA DATA */
    private SeriesFormula seriesFormula;

    public Guid id { get => seriesFormula.id; }
    public uint order { get => seriesFormula.order; }
    public string name { get => seriesFormula.name; }
    public uint minAge { get => seriesFormula.minAge; }
    public uint maxAge { get => seriesFormula.maxAge; }
    public bool dashForCash { get => seriesFormula.dashForCash; }

    [JsonIgnore]
    public ObservableCollection<RoundRider> riders { get; init; }
    public List<Race> moto1 { get; init; }
    public List<Race> moto2 { get; init; }
    public List<Race> moto3 { get; init; }
    public List<Race> final { get; init; }

}
