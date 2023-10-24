using F5BMX.Core;
using F5BMX.Core.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace F5BMX.Models;

internal class Series : ViewModelBase
{

    public Series()
    {
        this.year = DateTime.Now.Year;
        this.numberOfRounds = 6;
        this.formulas = new ObservableCollection<SeriesFormula>()
        {
            new SeriesFormula(1, "Formula 5", 4, 7, true),
            new SeriesFormula(2, "Formula 4", 8, 10, true),
            new SeriesFormula(3, "Formula 3", 11, 14, true),
            new SeriesFormula(4, "Formula 2", 15, 17, true),
            new SeriesFormula(5, "Formula 1", 18, 99, true),
        };
        this.rounds = new List<SeriesRoundInformation>();
        this.riders = JSON.ReadCollection<ObservableCollection<SeriesRider>>("riders");
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

    private int _numberOfRounds;

    public int year { get; init; }
    public string name { get; set; }
    public int numberOfRounds { get => _numberOfRounds; set { _numberOfRounds = value; NotifyPropertyChanged(); } }
    public string? coordinator { get; set; }
    public string? coordinatorEmail { get; set; }
    public bool dashForCash { get; set; }

    [JsonIgnore]
    public string dashForCashFormulas
    {
        get
        {
            string tmp = String.Empty;
            rounds.Where(x => x.dashForCashFormulaID != null).ToList()
                .ForEach(round => tmp += $"{formulas.Where(x => x.id == round.dashForCashFormulaID).FirstOrDefault().name}, ");

            if (tmp.Length > 0)
                return tmp.Substring(0, tmp.Length - 2);

            return String.Empty;
        }
    }

    public ObservableCollection<SeriesFormula> formulas { get; set; }
    public List<SeriesRoundInformation> rounds { get; set; }

    [JsonIgnore]
    public ObservableCollection<SeriesRider> riders { get; set; }
}
