using System.Collections.Concurrent;
using System.Collections.Generic;

namespace F5BMX.Models;

internal class Formula
{

    public Formula(int order, string name, int minAge, int maxAge)
    {
        this._order = order;
        this._name = name;
        this._minAge = minAge;
        this._maxAge = maxAge;

        this._riders = new List<Rider>();
        this._moto1 = new List<Moto>();
        this._moto2 = new List<Moto>();
        this._moto3 = new List<Moto>();
        this._final = new List<Moto>();
    }

    private int _order;
    private string _name;
    private int _minAge;
    private int _maxAge;

    private List<Rider> _riders;
    private List<Moto> _moto1;
    private List<Moto> _moto2;
    private List<Moto> _moto3;
    private List<Moto> _final;

    public int order { get => _order; }
    public string name { get => _name; }
    public int minAge { get => _minAge; }
    public int maxAge { get => _maxAge; }

    public List<Rider> riders { get => _riders; }
    public List<Moto> moto1 { get => _moto1; }
    public List<Moto> moto2 { get => _moto2; }
    public List<Moto> moto3 { get => _moto3; }
    public List<Moto> final { get => _final; }

}
