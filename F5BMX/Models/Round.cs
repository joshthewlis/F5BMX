using F5BMX.Core;
using F5BMX.Enums;
using F5BMX.Helpers;
using F5BMX.Interfaces;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace F5BMX.Models;

internal class Round : ModelBase
{

    public Round() { }

    public Round(uint roundNumber, List<SeriesFormula> seriesFormulas, bool finalRound)
    {
        RoundNumber = roundNumber;

        foreach (var formula in seriesFormulas.OrderByDescending(x => x.Order))
            Formulas.Add(new RoundFormula(formula));

        FinalRound = finalRound;
    }

    public DateOnly Date { get; init; } = DateOnly.FromDateTime(DateTime.Now);
    public uint RoundNumber { get; init; }
    public bool FinalRound { get; set; }

    public uint NumberOfGates { get; set; } = 8;
    public uint NumberOfMotos { get; set; } = 3;

    public Guid DashForCashFormulaID { get; set; }

    public RegistrationStatusEnum RegistrationStatus { get; set; } = RegistrationStatusEnum.Open;
    public StageStatusEnum MotosStatus { get; set; } = StageStatusEnum.NotGenerated;
    public StageStatusEnum FinalsStatus { get; set; } = StageStatusEnum.NotGenerated;

    public ObservableCollection<RoundFormula> Formulas { get; set; } = new ObservableCollection<RoundFormula>();

    public void Save()
    {
        base.Save($"round{RoundNumber}");
    }

}
