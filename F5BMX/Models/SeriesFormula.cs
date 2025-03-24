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
        Order = order;
        Name = name;
        MinAge = minAge;
        MaxAge = maxAge;
        Promotion = promotion;
    }

    private uint _order;

    public Guid ID { get; init; } = Guid.NewGuid();
    public uint Order { get => _order; set { _order = value; NotifyPropertyChanged(); } }
    public string Name { get; set; }
    public uint MinAge { get; set; }
    public uint MaxAge { get; set; }
    public bool DashForCash { get; set; }
    public bool Promotion { get; set; }


}
