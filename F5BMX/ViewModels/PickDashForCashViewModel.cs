using F5BMX.Core;
using F5BMX.Interfaces;
using F5BMX.Models;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace F5BMX.ViewModels;

internal class PickDashForCashViewModel : ViewModelBase
{
    
    public PickDashForCashViewModel() : this(new Round(), String.Empty, new List<RoundFormula>())
    { }

    public PickDashForCashViewModel(Round round, string previousDashForCash, List<RoundFormula> formulasEligble)
    {
        this.round = round;
        this.previousDashForCash = previousDashForCash;
        this.formulasEligble = formulasEligble;
    }

    public Round round { get; init; }
    public string previousDashForCash { get; set; }
    public List<RoundFormula> formulasEligble { get; set; }
    public RoundFormula? selectedFormula { get; set; }

    public ICommand btnPick => new RelayCommand<IClosable>(
        (IClosable window) => { if (selectedFormula != null) { round.dashForCashFormulaID = selectedFormula.id; window.Close(); } },
        () => { return selectedFormula != null; }
    );

}
