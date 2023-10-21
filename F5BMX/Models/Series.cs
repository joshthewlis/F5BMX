using F5BMX.Core;
using F5BMX.Core.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace F5BMX.Models;

internal class Series : ViewModelBase
{

    public Series()
    {
        this.year = DateTime.Now.Year;
        this.numberOfRounds = 5;
        this.formulas = new ObservableCollection<SeriesFormula>()
        {
            new SeriesFormula(1, "Formula 5", 4, 7),
            new SeriesFormula(2, "Formula 4", 8, 10),
            new SeriesFormula(3, "Formula 3", 11, 14),
            new SeriesFormula(4, "Formula 2", 15, 17),
            new SeriesFormula(5, "Formula 1", 18, 99),
        };
        this.rounds = new List<SeriesRoundStatus>();
        this.riders = JSON.ReadFile<ObservableCollection<SeriesRider>>("riders");
        if (this.riders == null)
            this.riders = new ObservableCollection<SeriesRider>();
    }

    public Series(int year, string name) : this()
    {
        this.year = year;
        this.name = name;
    }

    public Series(string directoryName) : this()
    {
        var yearName = directoryName.Split('-');

        this.year = int.Parse(yearName[0]);
        this.name = yearName[1].Replace("_", " ");
    }

    private int _year;
    private string _name = String.Empty;
    private int _numberOfRounds;
    private string? _coordinator;
    private string? _coordinatorEmail;

    public int year { get => _year; init => _year = value; }
    public string name { get => _name; set => _name = value; }
    public int numberOfRounds { get => _numberOfRounds; set { _numberOfRounds = value; NotifyPropertyChanged(); } }
    public string? coordinator { get => _coordinator; set => _coordinator = value; }
    public string? coordinatorEmail { get => _coordinatorEmail; set => _coordinatorEmail = value; }


    public ObservableCollection<SeriesFormula> formulas { get; set; }
    public List<SeriesRoundStatus> rounds { get; set; }
    [JsonIgnore]
    public ObservableCollection<SeriesRider> riders { get; set; }

}
