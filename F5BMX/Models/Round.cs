using System.Collections.Generic;

namespace F5BMX.Models;

internal class Round
{

    public Round(int roundNumber)
    {
        this.roundNumber = roundNumber;
    }

    public int roundNumber { get; init; }

    public List<Rider> riders { get; init; } = new List<Rider>();
    public List<RoundFormula> formulas { get; init; } = new List<RoundFormula>();

}
