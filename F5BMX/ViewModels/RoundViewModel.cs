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

    public RoundViewModel(Series series, uint roundNumber)
    {
        this.series = series;

        var round = JSON.ReadModel<Round>($"round{roundNumber}");
        this.round = round != null ? round : new Round(roundNumber, series.formulas.ToList(), series.numberOfRounds == roundNumber);
    }

    public Series series { get; set; }
    public Round round { get; set; }

    public bool roundSettingsEnabled => round.motosStatus == StageStatusEnum.NotGenerated;
    public bool registrationEnabled => round.motosStatus == StageStatusEnum.NotGenerated;
    public bool motosEnabled => round.registrationStatus == RegistrationStatusEnum.Closed && round.finalsStatus == StageStatusEnum.NotGenerated;
    public bool finalsEnabled => round.motosStatus == StageStatusEnum.Finished;
    public bool resultsEnabled => round.finalsStatus == StageStatusEnum.Finished;

    [JsonIgnore]
    public EnterResultsViewModel? motoResultsViewModel { get; set; }
    [JsonIgnore]
    public EnterResultsViewModel? finalResultsViewModel { get; set; }

    private void NotifyEnabled()
    {
        NotifyPropertyChanged(nameof(roundSettingsEnabled));
        NotifyPropertyChanged(nameof(registrationEnabled));
        NotifyPropertyChanged(nameof(motosEnabled));
        NotifyPropertyChanged(nameof(finalsEnabled));
        NotifyPropertyChanged(nameof(resultsEnabled));
    }

    #region RegistrationButtons
    public ICommand btnRegisterRiders => new RelayCommand(
        () => { new Views.RegisterRiders() { DataContext = new RegisterRidersViewModel(series, round) }.Show(); },
        () => { return round.registrationStatus == RegistrationStatusEnum.Open; }
    );
    public ICommand btnCloseRegistration => new RelayCommand(
        () =>
        {
            round.registrationStatus = RegistrationStatusEnum.Closed;
            round.Save();

            series.rounds[(int)round.roundNumber - 1].status = SeriesRoundStatusEnum.InProgress;
            series.rounds[(int)round.roundNumber - 1].date = DateOnly.FromDateTime(DateTime.Now);
            series.Save();

            NotifyEnabled();
        },
        () => { return round.registrationStatus == RegistrationStatusEnum.Open; }
    );
    public ICommand btnPrintRiderList => new RelayCommand(
        () =>
        {
            Registration.GenerateEntryList(series, round);
            MessageBox.Show("Opening Entry List In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{round.roundNumber}.entrylist.html") { UseShellExecute = true });
        },
        () => { return round.registrationStatus == RegistrationStatusEnum.Closed && round.motosStatus == StageStatusEnum.NotGenerated; }
    );
    public ICommand btnReOpenRegistration => new RelayCommand(
        () => { round.registrationStatus = RegistrationStatusEnum.Open; NotifyEnabled(); },
        () => { return round.registrationStatus == RegistrationStatusEnum.Closed && round.motosStatus == StageStatusEnum.NotGenerated; }
    );
    #endregion

    #region MotoControlButtons
    public ICommand btnGenerateMotos => new RelayCommand(
        () =>
        {
            Motos.Generate(round);
            round.motosStatus = StageStatusEnum.Generated;
            round.Save();
        },
        () => { return round.registrationStatus == RegistrationStatusEnum.Closed && round.motosStatus == StageStatusEnum.NotGenerated; }
    );
    public ICommand btnPrintMotoSheets => new RelayCommand(
        () =>
        {
            Motos.GenerateListing(series, round);
            MessageBox.Show("Opening Moto Listings In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{round.roundNumber}.motolist.html") { UseShellExecute = true });

            Motos.GenerateCommentary(round);
            MessageBox.Show("Opening Commentary In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{round.roundNumber}.motocommentary.html") { UseShellExecute = true });

            Motos.GenerateCallup(round);
            MessageBox.Show("Opening Call Up In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{round.roundNumber}.motocallup.html") { UseShellExecute = true });

            round.motosStatus = StageStatusEnum.SheetsPrinted;
            round.Save();
        },
        () => { return round.motosStatus == StageStatusEnum.Generated; }
    );
    public ICommand btnEnterMotoResults => new RelayCommand(
        () =>
        {
            if (motoResultsViewModel == null)
                motoResultsViewModel = new EnterResultsViewModel(round, EnterResultsTypeEnum.Moto);

            new Views.EnterResults() { DataContext = motoResultsViewModel }.ShowDialog();
            round.motosStatus = StageStatusEnum.ResultsEntered;
        },
        () => { return round.motosStatus == StageStatusEnum.SheetsPrinted || round.motosStatus == StageStatusEnum.ResultsEntered; }
    );
    public ICommand btnFinalizeMotos => new RelayCommand(
        () =>
        {
            if (motoResultsViewModel == null)
                return;

            Motos.Finalize(motoResultsViewModel.races, round.numberOfMotos);
            round.motosStatus = StageStatusEnum.Finished;
            round.Save();

            NotifyEnabled();
        },
        () => { return round.motosStatus == StageStatusEnum.ResultsEntered; }
    );
    #endregion

    #region FinalControlButtons
    public ICommand btnGenerateFinals => new RelayCommand(
        () =>
        {
            Finals.Generate(round);
            round.finalsStatus = StageStatusEnum.Generated;
            round.Save();
        },
        () => { return round.finalsStatus == StageStatusEnum.NotGenerated; }
    );
    public ICommand btnPrintFinalSheets => new RelayCommand(
        () =>
        {
            Finals.GenerateListing(series, round);
            MessageBox.Show("Opening Final Listings In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{round.roundNumber}.finallist.html") { UseShellExecute = true });

            Finals.GenerateCommentary(round);
            MessageBox.Show("Opening Commentary In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{round.roundNumber}.finalcommentary.html") { UseShellExecute = true });

            Finals.GenerateCallup(round);
            MessageBox.Show("Opening Call Up In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{round.roundNumber}.finalcallup.html") { UseShellExecute = true });

            round.finalsStatus = StageStatusEnum.SheetsPrinted;
            round.Save();
        },
        () => { return round.finalsStatus == StageStatusEnum.Generated; }
    );
    public ICommand btnEnterFinalResults => new RelayCommand(
        () =>
        {
            if (finalResultsViewModel == null)
                finalResultsViewModel = new EnterResultsViewModel(round, EnterResultsTypeEnum.Final);

            new Views.EnterResults() { DataContext = finalResultsViewModel }.ShowDialog();
            round.finalsStatus = StageStatusEnum.ResultsEntered;
        },
        () => { return round.finalsStatus == StageStatusEnum.SheetsPrinted || round.finalsStatus == StageStatusEnum.ResultsEntered; }
    );
    public ICommand btnFinalizeFinals => new RelayCommand(
        () =>
        {
            if (finalResultsViewModel == null)
                return;

            Finals.Finalize(series, round, finalResultsViewModel.races);
            round.finalsStatus = StageStatusEnum.Finished;
            round.Save();

            series.rounds[(int)round.roundNumber - 1].status = SeriesRoundStatusEnum.Complete;
            series.Save();

            NotifyEnabled();
        },
        () => { return round.finalsStatus == StageStatusEnum.ResultsEntered; }
    );
    #endregion

    #region ResultsButtons
    public ICommand btnRoundStandings => new RelayCommand(
        () =>
        {
            Standings.Round(series, round);
            MessageBox.Show("Opening Round Standings In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{round.roundNumber}.standings.html") { UseShellExecute = true });
        },
        () => { return round.finalsStatus == StageStatusEnum.Finished; }
    );
    public ICommand btnSeriesStandings => new RelayCommand(
        () => { },
        () => { return round.finalsStatus == StageStatusEnum.Finished; }
    );
    #endregion
}
