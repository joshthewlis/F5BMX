using F5BMX.Core;
using System.Collections.Generic;
using System.ComponentModel;

namespace F5BMX.Models;

internal class Formula : ViewModelBase 
{

    public Formula(int order) : this(order, string.Empty, 0, 0)
    { }

    public Formula(int order, string name, int minAge, int maxAge)
    {
        this.order = order;
        this.name = name;
        this.minAge = minAge;
        this.maxAge = maxAge;

        this._riders = new List<Rider>();
        this._moto1 = new List<Moto>();
        this._moto2 = new List<Moto>();
        this._moto3 = new List<Moto>();
        this._final = new List<Moto>();
    }

    private int _order;

    public int order { get => _order; set { _order = value; NotifyPropertyChanged(); } }
    public string name { get; set; }
    public int minAge { get; set; }
    public int maxAge { get; set; }
    public bool dashForCash { get; set; }

    private List<Rider> _riders;
    private List<Moto> _moto1;
    private List<Moto> _moto2;
    private List<Moto> _moto3;
    private List<Moto> _final;

    public List<Rider> riders { get => _riders; }
    public List<Moto> moto1 { get => _moto1; }
    public List<Moto> moto2 { get => _moto2; }
    public List<Moto> moto3 { get => _moto3; }
    public List<Moto> final { get => _final; }

}
