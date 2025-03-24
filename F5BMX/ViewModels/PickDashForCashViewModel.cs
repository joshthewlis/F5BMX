using F5BMX.Core;
using F5BMX.Interfaces;
using F5BMX.Models;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace F5BMX.ViewModels;

internal class PickDashForCashViewModel : ViewModelBase
{
    
    public PickDashForCashViewModel() : this(new Round(), String.Empty, [])
    { }

    public PickDashForCashViewModel(Round round, string previousDashForCash, List<RoundFormula> formulasEligble)
    {
        Round = round;
        PreviousDashForCash = previousDashForCash;
        FormulasEligble = formulasEligble;
    }

    public Round Round { get; init; }
    public string PreviousDashForCash { get; set; }
    public List<RoundFormula> FormulasEligble { get; set; }
    public RoundFormula? SelectedFormula { get; set; }

    public ICommand BtnPick => new RelayCommand<IClosable>(
        (IClosable window) => { if (SelectedFormula != null) { Round.DashForCashFormulaID = SelectedFormula.ID; window.Close(); } },
        () => { return SelectedFormula != null; }
    );

}
