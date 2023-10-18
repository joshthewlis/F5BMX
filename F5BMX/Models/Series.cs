using F5BMX.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace F5BMX.Models;

internal class Series : NotifyBase
{

    public Series()
    {
        this.year = DateTime.Now.Year;
        this.numberOfRounds = 5;
        this._formulas = new ObservableCollection<Formula>()
        {
            new Formula(1, "Formula 5", 4, 7),
            new Formula(2, "Formula 4", 8, 10),
            new Formula(3, "Formula 3", 11, 14),
            new Formula(4, "Formula 2", 15, 17),
            new Formula(5, "Formula 1", 18, 99),
        };
    }

    public Series(int year, string name)
    {
        this.year = year;
        this.name = name;
    }

    public Series(string directoryName)
    {
        var yearName = directoryName.Split('-');

        this.year = int.Parse(yearName[0]);
        this.name = yearName[1].Replace("_", " ");
    }

    private int _year;
    private string _name;
    private int _numberOfRounds;
    private string _coordinator;
    private string _coordinatorEmail;

    private ObservableCollection<Formula> _formulas;

    public int year { get => _year; init => _year = value; }
    public string name { get => _name; set => _name = value; }
    public int numberOfRounds { get => _numberOfRounds; set { _numberOfRounds = value; NotifyPropertyChanged(); } }
    public string coordinator { get => _coordinator; set => _coordinator = value; }
    public string coordinatorEmail { get => _coordinatorEmail; set => _coordinatorEmail = value; }

    public ObservableCollection<Formula> formulas { get => _formulas; }


}
