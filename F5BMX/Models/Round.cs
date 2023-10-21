using F5BMX.Core;
using F5BMX.Enums;
using F5BMX.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace F5BMX.Models;

internal class Round : ModelBase
{

    public Round() { }

    public Round(int roundNumber, List<SeriesFormula> seriesFormulas)
    {
        this.roundNumber = roundNumber;

        foreach (var formula in seriesFormulas.OrderByDescending(x => x.order))
            this.formulas.Add(new RoundFormula(formula));
    }

    public int roundNumber { get; init; }
    public int numberOfGates { get; set; } = 8;

    public RegistrationStatus registrationStatus { get; set; } = RegistrationStatus.Open;
    public StageStatus motosStatus { get; set; } = StageStatus.NotGenerated;
    public StageStatus finalsStatus { get; set; } = StageStatus.NotGenerated;

    public ObservableCollection<RoundFormula> formulas { get; set; } = new ObservableCollection<RoundFormula>();

    public void Save()
    {
        base.Save($"round{roundNumber}");
    }

}
