using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace F5BMX.Models;

internal class Round
{

    public Round(int roundNumber, List<SeriesFormula> seriesFormulas)
    {
        this.roundNumber = roundNumber;

        foreach (var formula in seriesFormulas)
            this.formulas.Add(new RoundFormula(formula));

        this.progress = 1;
    }

    public int roundNumber { get; init; }
    public int progress { get; set; }

    //public List<RoundRider> riders { get; init; } = new List<RoundRider>();
    public List<RoundFormula> formulas { get; init; } = new List<RoundFormula>();

}
