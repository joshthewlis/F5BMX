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
    public List<string> Series { get => _series; }

    public string SelectedSeries { get; set; } = string.Empty;

    #region Buttons
    public ICommand BtnCreateSeries => new RelayCommand(createSeries);
    private void createSeries()
    {
        new CreateSeries().ShowDialog();
        _series = Directories.LoadSeries();
        NotifyPropertyChanged(nameof(Series));
    }

    public ICommand BtnLoadSeries => new RelayCommand<IClosable>(loadSeries, canLoadSeries);
    private void loadSeries(IClosable window)
    {
        new SelectRound() { DataContext = new SelectRoundViewModel(SelectedSeries) }.Show();
        window.Close();
    }
    private bool canLoadSeries()
    {
        return SelectedSeries != string.Empty;
    }
    #endregion

}
