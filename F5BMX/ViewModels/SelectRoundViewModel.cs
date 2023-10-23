using F5BMX.Core;
using F5BMX.Core.IO;
using F5BMX.Interfaces;
using F5BMX.Models;
using System;
using System.Windows.Input;

namespace F5BMX.ViewModels;

internal class SelectRoundViewModel
{

    public SelectRoundViewModel() : this(String.Empty)
    { }

    public SelectRoundViewModel(string seriesName)
    {
        Directories.SetSeries(seriesName);

        this.series = JSON.ReadModel<Series>("series");
    }

    public Series series { get; set; }
    public SeriesRoundInformation? selectedRound { get; set; }


    #region Buttons
    public ICommand btnSelectRound => new RelayCommand<SeriesRoundInformation>((SeriesRoundInformation selectedRound) => { this.selectedRound = selectedRound; });
    public ICommand btnLoadRound => new RelayCommand<IClosable>(loadRound, () => { return selectedRound == null ? false : true; });
    private void loadRound(IClosable window)
    {
        if (selectedRound != null)
        {
            new Views.Round() { DataContext = new RoundViewModel(series, selectedRound.roundNumber) }.Show();
            window.Close();
        }
    }
    #endregion

}
