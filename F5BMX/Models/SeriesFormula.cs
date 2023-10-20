using F5BMX.Core;
using System.Collections.Generic;
using System.ComponentModel;

namespace F5BMX.Models;

internal class SeriesFormula : ViewModelBase 
{

    public SeriesFormula() { }

    public SeriesFormula(int order) : this(order, string.Empty, 0, 0)
    { }

    public SeriesFormula(int order, string name, int minAge, int maxAge)
    {
        this.order = order;
        this.name = name;
        this.minAge = minAge;
        this.maxAge = maxAge;
    }

    private int _order;

    public int order { get => _order; set { _order = value; NotifyPropertyChanged(); } }
    public string name { get; set; }
    public int minAge { get; set; }
    public int maxAge { get; set; }
    public bool dashForCash { get; set; }


}
