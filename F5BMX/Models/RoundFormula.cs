using System.Collections.Generic;

namespace F5BMX.Models;

internal class RoundFormula
{

    public RoundFormula()
    {
        this._riders = new List<Rider>();
        this._moto1 = new List<Race>();
        this._moto2 = new List<Race>();
        this._moto3 = new List<Race>();
        this._final = new List<Race>();
    }

    private List<Rider> _riders;
    private List<Race> _moto1;
    private List<Race> _moto2;
    private List<Race> _moto3;
    private List<Race> _final;

    public List<Rider> riders { get => _riders; }
    public List<Race> moto1 { get => _moto1; }
    public List<Race> moto2 { get => _moto2; }
    public List<Race> moto3 { get => _moto3; }
    public List<Race> final { get => _final; }

}
