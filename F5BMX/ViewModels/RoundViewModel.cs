using F5BMX.Core;
using F5BMX.Core.IO;
using F5BMX.Enums;
using F5BMX.Helpers;
using F5BMX.Models;
using System;
using System.Linq;
using System.Windows.Input;

namespace F5BMX.ViewModels;

internal class RoundViewModel : ViewModelBase
{

    public RoundViewModel() : this(new Series(), 0)
    { }

    public RoundViewModel(Series series, int roundNumber)
    {
        this.series = series;

        var round = JSON.ReadModel<Round>($"round{roundNumber}");
        this.round = round != null ? round : new Round(roundNumber, series.formulas.ToList());
    }

    public Series series { get; set; }
    public Round round { get; set; }

    public bool roundSettingsEnabled => round.motosStatus == StageStatus.NotGenerated;
    public bool registrationEnabled => round.motosStatus == StageStatus.NotGenerated;
    public bool motosEnabled => round.registrationStatus == RegistrationStatus.Closed && round.finalsStatus == StageStatus.NotGenerated;
    public bool finalsEnabled => round.motosStatus == StageStatus.Finished;

    private void NotifyEnabled()
    {
        NotifyPropertyChanged(nameof(registrationEnabled));
        NotifyPropertyChanged(nameof(motosEnabled));
        NotifyPropertyChanged(nameof(finalsEnabled));
    }

    #region RegistrationButtons
    public ICommand btnRegisterRiders => new RelayCommand(
        () => { new Views.RegisterRiders() { DataContext = new RegisterRidersViewModel(series, round) }.Show(); },
        () => { return round.registrationStatus == RegistrationStatus.Open; }
    );
    public ICommand btnCloseRegistration => new RelayCommand(
        () => { round.registrationStatus = RegistrationStatus.Closed; NotifyEnabled(); },
        () => { return round.registrationStatus == RegistrationStatus.Open; }
    );
    public ICommand btnPrintRiderList => new RelayCommand(
        () =>
        {
            Registration.GenerateEntryList(series, round);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{round.roundNumber}.entrylist.html") { UseShellExecute = true });
        },
        () => { return round.registrationStatus == RegistrationStatus.Closed && round.motosStatus == StageStatus.NotGenerated; }
    );
    public ICommand btnReOpenRegistration => new RelayCommand(
        () => { round.registrationStatus = RegistrationStatus.Open; NotifyEnabled(); },
        () => { return round.registrationStatus == RegistrationStatus.Closed && round.motosStatus == StageStatus.NotGenerated; }
    );
    #endregion

    #region MotoControlButtons
    public ICommand btnGenerateMotos => new RelayCommand(
        () =>
        {
            Motos.Generate(round);
            round.motosStatus = StageStatus.Generated;
            round.Save();
        },
        () => { return round.registrationStatus == RegistrationStatus.Closed && round.motosStatus == StageStatus.NotGenerated; }
    );
    public ICommand btnPrintMotoSheets => new RelayCommand(
        () => {
            Motos.GenerateMotoListing(series, round);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{round.roundNumber}.motolist.html") { UseShellExecute = true });
            round.motosStatus = StageStatus.SheetsPrinted;
            round.Save();
        },
        () => { return round.motosStatus == StageStatus.Generated; }
    );
    public ICommand btnEnterMotoResults => new RelayCommand(
        () => {
            new Views.EnterResults() { DataContext = new EnterResultsViewModel(round) }.ShowDialog();
            round.motosStatus = StageStatus.ResultsEntered;
            round.Save();
        },
        () => { return round.motosStatus == StageStatus.SheetsPrinted || round.motosStatus == StageStatus.ResultsEntered; }
    );
    public ICommand btnFinalizeMotos => new RelayCommand(
        () => { },
        () => { return round.motosStatus == StageStatus.ResultsEntered; }
    );
    #endregion
}
