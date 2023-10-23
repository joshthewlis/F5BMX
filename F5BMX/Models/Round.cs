using F5BMX.Core;
using F5BMX.Enums;
using F5BMX.Interfaces;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace F5BMX.Models;

internal class Round : ModelBase
{

    public Round() { }

    public Round(uint roundNumber, List<SeriesFormula> seriesFormulas)
    {
        this.roundNumber = roundNumber;

        foreach (var formula in seriesFormulas.OrderByDescending(x => x.order))
            this.formulas.Add(new RoundFormula(formula));
    }

    public DateOnly date { get; init; } = DateOnly.FromDateTime(DateTime.Now);
    public uint roundNumber { get; init; }
    public uint numberOfGates { get; set; } = 8;
    public uint numberOfMotos { get; set; } = 3;

    public RegistrationStatusEnum registrationStatus { get; set; } = RegistrationStatusEnum.Open;
    public StageStatusEnum motosStatus { get; set; } = StageStatusEnum.NotGenerated;
    public StageStatusEnum finalsStatus { get; set; } = StageStatusEnum.NotGenerated;

    public ObservableCollection<RoundFormula> formulas { get; set; } = new ObservableCollection<RoundFormula>();

    public void Save()
    {
        base.Save($"round{roundNumber}");
    }

}
