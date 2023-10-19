using F5BMX.Models;
using System.Collections.Generic;

namespace F5BMX.ViewModels;

internal class SelectRoundViewModel
{

    public SelectRoundViewModel()
    {
        this.series = new Series();
        this.rounds = new List<Round>()
        {
            new Round(),
            new Round(),
            new Round(),
            new Round(),
            new Round(),
            new Round()
        };
    }

    public Series series { get; set; }

    public List<Round> rounds { get; set; }
    public Round selectedRound { get; set; }

}
