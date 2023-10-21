using F5BMX.Core;
using F5BMX.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace F5BMX.ViewModels;

internal class RoundViewModel
{

    public RoundViewModel() : this(new Series(), 0)
    { }

    public RoundViewModel(Series series, int roundNumber)
    {
        this.series = series;
        this.round = new Round(roundNumber, series.formulas.ToList());
    }

    public Series series { get; set; }
    public Round round { get; set; }

    #region RegistrationButtons
    public ICommand btnRegisterRiders => new RelayCommand(registerRiders, canRegisterRiders);
    private void registerRiders()
    {
        new Views.RegisterRiders()
        {
            DataContext = new RegisterRidersViewModel(series, round)
        }.Show();
    }
    private bool canRegisterRiders()
    {
        if (round.progress == 1)
            return true;

        return false;
    }
    #endregion

}
