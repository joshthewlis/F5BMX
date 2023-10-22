using F5BMX.Core;
using F5BMX.Core.IO;
using F5BMX.Enums;
using F5BMX.Helpers;
using F5BMX.Models;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows;
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

    [JsonIgnore]
    public EnterResultsViewModel enterResultsViewModel { get; set; }

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
            MessageBox.Show("Opening Entry List In Default Browser\r\nPlease Print.");
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
        () =>
        {
            Motos.GenerateMotoListing(series, round);
            MessageBox.Show("Opening Moto Listings In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{round.roundNumber}.motolist.html") { UseShellExecute = true });

            Motos.GenerateMotoCommentary(series, round);
            MessageBox.Show("Opening Commentary In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{round.roundNumber}.commentary.html") { UseShellExecute = true });

            Motos.GenerateMotoCallup(series, round);
            MessageBox.Show("Opening Call Up In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{round.roundNumber}.callup.html") { UseShellExecute = true });

            round.motosStatus = StageStatus.SheetsPrinted;
            round.Save();
        },
        () => { return round.motosStatus == StageStatus.Generated; }
    );
    public ICommand btnEnterMotoResults => new RelayCommand(
        () =>
        {
            if (enterResultsViewModel == null)
                enterResultsViewModel = new EnterResultsViewModel(round);

            new Views.EnterResults() { DataContext = enterResultsViewModel }.ShowDialog();
            round.motosStatus = StageStatus.ResultsEntered;
            round.Save();
        },
        () => { return round.motosStatus == StageStatus.SheetsPrinted || round.motosStatus == StageStatus.ResultsEntered; }
    );
    public ICommand btnFinalizeMotos => new RelayCommand(
        () =>
        {
            round.motosStatus = StageStatus.Finished;
            NotifyPropertyChanged(nameof(finalsEnabled));
            round.Save();
        },
        () => { return round.motosStatus == StageStatus.ResultsEntered; }
    );
    #endregion

    #region FinalControlButtons
    public ICommand btnGenerateFinals => new RelayCommand(
        () => {
            Finals.Generate(round);
        },
        () => { return round.finalsStatus == StageStatus.NotGenerated; }
    );
    public ICommand btnPrintFinalSheets => new RelayCommand(
        () => {
            
        },
        () => { return round.finalsStatus == StageStatus.Generated; }
    );
    public ICommand btnEnterFinalResults => new RelayCommand(
        () => {
            Finals.Generate(round);
        },
        () => { return round.finalsStatus == StageStatus.SheetsPrinted || round.finalsStatus == StageStatus.ResultsEntered; }
    );
    public ICommand btnFinalizeFinals => new RelayCommand(
        () => {
        },
        () => { return round.finalsStatus == StageStatus.ResultsEntered; }
    );
    #endregion
}
