using F5BMX.Core;
using F5BMX.Core.IO;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace F5BMX.Models;

internal class Series : ViewModelBase
{
    public ObservableCollection<SeriesFormula> Formulas { get; set; }
    public List<SeriesRoundInformation> Rounds { get; set; }

    private int _numberOfRounds;

    public int Year { get; init; }
    public string Name { get; set; }
    public int NumberOfRounds { get => _numberOfRounds; set { _numberOfRounds = value; NotifyPropertyChanged(); } }
    public string? Coordinator { get; set; }
    public string? CoordinatorEmail { get; set; }
    public bool DashForCash { get; set; }

    [JsonIgnore]
    public ObservableCollection<SeriesRider> Riders { get; set; }

    public Series()
    {
        if (Year == 0)
            Year = DateTime.Now.Year;
        Name ??= String.Empty;
        NumberOfRounds = 6;
        Formulas = [
            new SeriesFormula(1, "Formula 5", 4, 7, true),
            new SeriesFormula(2, "Formula 4", 8, 10, true),
            new SeriesFormula(3, "Formula 3", 11, 14, true),
            new SeriesFormula(4, "Formula 2", 15, 17, true),
            new SeriesFormula(5, "Formula 1", 18, 99, true),
        ];
        Rounds = [];
        Riders = JSON.ReadCollection<ObservableCollection<SeriesRider>>("riders") ?? [];
    }

    public Series(int year, string name) : this()
    {
        Year = year;
        Name = name;
    }

    public Series(string directoryName) : this()
    {
        var yearName = directoryName.Split('-');

        Year = int.Parse(yearName[0]);
        Name = yearName[1].Replace("_", " ");
    }

    [JsonIgnore]
    public string DashForCashFormulas
    {
        get
        {
            string tmp = String.Empty;
            Rounds.Where(x => x.DashForCashFormulaID != null).ToList()
                .ForEach(round => tmp += $"{Formulas.First(x => x.id == round.DashForCashFormulaID)?.name}, ");

            if (tmp.Length > 0)
                return tmp.Substring(0, tmp.Length - 2);

            return String.Empty;
        }
    }

}
