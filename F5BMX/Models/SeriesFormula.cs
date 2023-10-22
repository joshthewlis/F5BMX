using F5BMX.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace F5BMX.Models;

internal class SeriesFormula : ViewModelBase 
{

    public SeriesFormula() : this(0)
    { }

    public SeriesFormula(uint order) : this(order, string.Empty, 0, 0, true)
    { }

    public SeriesFormula(uint order, string name, uint minAge, uint maxAge, bool promotion)
    {
        this.order = order;
        this.name = name;
        this.minAge = minAge;
        this.maxAge = maxAge;
        this.promotion = promotion;
    }

    private uint _order;

    public Guid id { get; init; } = Guid.NewGuid();
    public uint order { get => _order; set { _order = value; NotifyPropertyChanged(); } }
    public string name { get; set; }
    public uint minAge { get; set; }
    public uint maxAge { get; set; }
    public bool dashForCash { get; set; }
    public bool promotion { get; set; }


}
