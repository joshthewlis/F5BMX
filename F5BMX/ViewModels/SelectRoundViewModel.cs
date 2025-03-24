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

        this.Series = JSON.ReadModel<Series>("series");
    }

    public Series? Series { get; set; }
    public SeriesRoundInformation? SelectedRound { get; set; }


    #region Buttons
    public ICommand BtnSelectRound => new RelayCommand<SeriesRoundInformation>((SeriesRoundInformation selectedRound) => { this.SelectedRound = selectedRound; });
    public ICommand BtnLoadRound => new RelayCommand<IClosable>(LoadRound, () => { return SelectedRound != null; });
    private void LoadRound(IClosable window)
    {
        if (Series == null)
            return;

        if (SelectedRound != null)
        {
            new Views.Round() { DataContext = new RoundViewModel(Series, SelectedRound.roundNumber) }.Show();
            window.Close();
        }
    }
    #endregion

}
