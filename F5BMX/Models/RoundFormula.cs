using System.Collections.Generic;

namespace F5BMX.Models;

internal class RoundFormula
{

    public RoundFormula()
    {
        this._riders = new List<Rider>();
        this._moto1 = new List<Moto>();
        this._moto2 = new List<Moto>();
        this._moto3 = new List<Moto>();
        this._final = new List<Moto>();
    }

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
