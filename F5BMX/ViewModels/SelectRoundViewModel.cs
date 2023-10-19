using F5BMX.Core.IO;
using F5BMX.Models;

namespace F5BMX.ViewModels;

internal class SelectRoundViewModel
{

    public SelectRoundViewModel()
    {

    }

    public SelectRoundViewModel(string seriesName)
    {
        Directories.SetSeries(seriesName);

        this.series = JSON.ReadFile<Series>("series");
    }

    public Series series { get; set; }
    public Round selectedRound { get; set; }

}
