using F5BMX.Core;
using F5BMX.Core.IO;
using F5BMX.Interfaces;
using F5BMX.Views;
using System.Collections.Generic;
using System.Windows.Input;

namespace F5BMX.ViewModels;

internal class SelectSeriesViewModel : ViewModelBase
{

    public SelectSeriesViewModel()
    {
        _series = Directories.LoadSeries();
    }

    private List<string> _series;
    public List<string> series { get => _series; }

    public string selectedSeries { get; set; } = string.Empty;

    #region Buttons
    public ICommand btnCreateSeries => new RelayCommand(createSeries);
    private void createSeries()
    {
        new CreateSeries().ShowDialog();
        _series = Directories.LoadSeries();
        NotifyPropertyChanged(nameof(series));
    }

    public ICommand btnLoadSeries => new RelayCommand<IClosable>(loadSeries, canLoadSeries);
    private void loadSeries(IClosable window)
    {
        new SelectRound() { DataContext = new SelectRoundViewModel(selectedSeries) }.Show();
        window.Close();
    }
    private bool canLoadSeries()
    {
        return selectedSeries != string.Empty;
    }
    #endregion

}
