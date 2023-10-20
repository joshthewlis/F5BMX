using F5BMX.Core;
using F5BMX.Core.IO;
using F5BMX.Interfaces;
using F5BMX.Models;
using System.Windows.Input;

namespace F5BMX.ViewModels;

internal class SelectRoundViewModel
{

    public SelectRoundViewModel() { }

    public SelectRoundViewModel(string seriesName)
    {
        Directories.SetSeries(seriesName);

        this.series = JSON.ReadFile<Series>("series");
    }

    public Series? series { get; set; }
    public SeriesRoundStatus? selectedRound { get; set; }


    #region Buttons
    public ICommand btnSelectRound => new RelayCommand<SeriesRoundStatus>(selectRound);
    private void selectRound(SeriesRoundStatus selectedRound)
    {
        this.selectedRound = selectedRound;
    }

    public ICommand btnLoadRound => new RelayCommand<IClosable>(loadRound, canLoadRound);
    private void loadRound(IClosable window)
    {
        new Views.Round() { DataContext = new Round(selectedRound.roundNumber) }.Show();
        window.Close();
    }
    private bool canLoadRound()
    {
        if (selectedRound == null)
            return false;

        return true;
    }

    #endregion

}
