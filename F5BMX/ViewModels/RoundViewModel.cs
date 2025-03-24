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
    {
        Series.DashForCash = true;
    }

    public RoundViewModel(Series series, uint roundNumber)
    {
        this.Series = series;

        var round = JSON.ReadModel<Round>($"round{roundNumber}");
        this.Round = round ?? new Round(roundNumber, series.Formulas.ToList(), series.NumberOfRounds == roundNumber);
    }

    public Series Series { get; set; }
    public Round Round { get; set; }

    public Visibility DashForCashVisible => Series.DashForCash == true ? Visibility.Visible : Visibility.Collapsed;
    public bool DashForCashEnabled => Round.motosStatus == StageStatusEnum.NotGenerated;
    public bool RoundSettingsEnabled => Round.motosStatus == StageStatusEnum.NotGenerated;
    public bool RegistrationEnabled => Round.motosStatus == StageStatusEnum.NotGenerated;
    public bool MotosEnabled => Round.registrationStatus == RegistrationStatusEnum.Closed && Round.finalsStatus == StageStatusEnum.NotGenerated;
    public bool FinalsEnabled => Round.motosStatus == StageStatusEnum.Finished;
    public bool ResultsEnabled => Round.finalsStatus == StageStatusEnum.Finished;

    [JsonIgnore]
    public EnterResultsViewModel? MotoResultsViewModel { get; set; }
    [JsonIgnore]
    public EnterResultsViewModel? FinalResultsViewModel { get; set; }
    [JsonIgnore]
    public RoundFormula? DashForCashFormula
    {
        get => Round.formulas.Where(x => x.id == Round.dashForCashFormulaID).FirstOrDefault();
        set { if (value == null) return; Round.dashForCashFormulaID = value.id; NotifyPropertyChanged(); }
    }

    private void NotifyEnabled()
    {
        NotifyPropertyChanged(nameof(DashForCashEnabled));
        NotifyPropertyChanged(nameof(RoundSettingsEnabled));
        NotifyPropertyChanged(nameof(RegistrationEnabled));
        NotifyPropertyChanged(nameof(MotosEnabled));
        NotifyPropertyChanged(nameof(FinalsEnabled));
        NotifyPropertyChanged(nameof(ResultsEnabled));
    }

    #region DashForCashButtons
    public ICommand BtnDashForCashPick => new RelayCommand(
        () => 
        {
            new Views.PickDashForCash()
            {
                DataContext = new PickDashForCashViewModel(Round, Series.DashForCashFormulas, Round.formulas.Where(x => x.dashForCash == true).ToList())
            }.ShowDialog();
            NotifyPropertyChanged(nameof(DashForCashFormula));
        }
    );
    public ICommand BtnDashForCashRandom => new RelayCommand(
        () => 
        {
            var tmp = DashForCash.RandomDashForCashFormula(Series);
            DashForCashFormula = Round.formulas.Where(x => x.id == tmp).First(); 
        }
    );
    #endregion

    #region RegistrationButtons
    public ICommand BtnRegisterRiders => new RelayCommand(
        () => { new Views.RegisterRiders() { DataContext = new RegisterRidersViewModel(Series, Round) }.Show(); },
        () => { return Round.registrationStatus == RegistrationStatusEnum.Open; }
    );
    public ICommand BtnCloseRegistration => new RelayCommand(
        () =>
        {
            Round.registrationStatus = RegistrationStatusEnum.Closed;
            Round.Save();

            Series.Rounds[(int)Round.roundNumber - 1].Status = SeriesRoundStatusEnum.InProgress;
            Series.Rounds[(int)Round.roundNumber - 1].Date = DateOnly.FromDateTime(DateTime.Now);
            Series.Save();

            NotifyEnabled();
        },
        () => { return Round.registrationStatus == RegistrationStatusEnum.Open; }
    );
    public ICommand BtnPrintRiderList => new RelayCommand(
        () =>
        {
            Registration.GenerateEntryList(Series, Round);
            MessageBox.Show("Opening Entry List In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{Round.roundNumber}.entrylist.html") { UseShellExecute = true });
        },
        () => { return Round.registrationStatus == RegistrationStatusEnum.Closed && Round.motosStatus == StageStatusEnum.NotGenerated; }
    );
    public ICommand BtnReOpenRegistration => new RelayCommand(
        () => { Round.registrationStatus = RegistrationStatusEnum.Open; NotifyEnabled(); },
        () => { return Round.registrationStatus == RegistrationStatusEnum.Closed && Round.motosStatus == StageStatusEnum.NotGenerated; }
    );
    #endregion

    #region MotoControlButtons
    public ICommand BtnGenerateMotos => new RelayCommand(
        () =>
        {
            if(Series.DashForCash == true && DashForCashFormula == null)
            {
                var msg = MessageBox.Show("No Dash for Cash has been selected.\r\nDo you want to continue?", "Caption", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (msg == MessageBoxResult.No)
                    return;
            }

            Motos.Generate(Round);
            Round.motosStatus = StageStatusEnum.Generated;
            Round.Save();

            NotifyEnabled();
        },
        () => { return Round.registrationStatus == RegistrationStatusEnum.Closed && Round.motosStatus == StageStatusEnum.NotGenerated; }
    );
    public ICommand BtnPrintMotoSheets => new RelayCommand(
        () =>
        {
            Motos.GenerateListing(Series, Round);
            MessageBox.Show("Opening Moto Listings In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{Round.roundNumber}.motolist.html") { UseShellExecute = true });

            Motos.GenerateCommentary(Round);
            MessageBox.Show("Opening Commentary In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{Round.roundNumber}.motocommentary.html") { UseShellExecute = true });

            Motos.GenerateCallup(Round);
            MessageBox.Show("Opening Call Up In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{Round.roundNumber}.motocallup.html") { UseShellExecute = true });

            Round.motosStatus = StageStatusEnum.SheetsPrinted;
            Round.Save();
        },
        () => { return Round.motosStatus == StageStatusEnum.Generated; }
    );
    public ICommand BtnEnterMotoResults => new RelayCommand(
        () =>
        {
            MotoResultsViewModel ??= new EnterResultsViewModel(Round, EnterResultsTypeEnum.Moto);

            new Views.EnterResults() { DataContext = MotoResultsViewModel }.ShowDialog();
            Round.motosStatus = StageStatusEnum.ResultsEntered;
        },
        () => { return Round.motosStatus == StageStatusEnum.SheetsPrinted || Round.motosStatus == StageStatusEnum.ResultsEntered; }
    );
    public ICommand BtnFinalizeMotos => new RelayCommand(
        () =>
        {
            if (MotoResultsViewModel == null)
                return;

            Motos.Finalize(MotoResultsViewModel.Races, Round.numberOfMotos);
            Round.motosStatus = StageStatusEnum.Finished;
            Round.Save();

            NotifyEnabled();
        },
        () => { return Round.motosStatus == StageStatusEnum.ResultsEntered; }
    );
    #endregion

    #region FinalControlButtons
    public ICommand BtnGenerateFinals => new RelayCommand(
        () =>
        {
            Finals.Generate(Round);
            Round.finalsStatus = StageStatusEnum.Generated;
            Round.Save();
        },
        () => { return Round.finalsStatus == StageStatusEnum.NotGenerated; }
    );
    public ICommand BtnPrintFinalSheets => new RelayCommand(
        () =>
        {
            Finals.GenerateListing(Series, Round);
            MessageBox.Show("Opening Final Listings In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{Round.roundNumber}.finallist.html") { UseShellExecute = true });

            Finals.GenerateCommentary(Round);
            MessageBox.Show("Opening Commentary In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{Round.roundNumber}.finalcommentary.html") { UseShellExecute = true });

            Finals.GenerateCallup(Round);
            MessageBox.Show("Opening Call Up In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{Round.roundNumber}.finalcallup.html") { UseShellExecute = true });

            Round.finalsStatus = StageStatusEnum.SheetsPrinted;
            Round.Save();
        },
        () => { return Round.finalsStatus == StageStatusEnum.Generated; }
    );
    public ICommand BtnEnterFinalResults => new RelayCommand(
        () =>
        {
            FinalResultsViewModel ??= new EnterResultsViewModel(Round, EnterResultsTypeEnum.Final);

            new Views.EnterResults() { DataContext = FinalResultsViewModel }.ShowDialog();
            Round.finalsStatus = StageStatusEnum.ResultsEntered;
        },
        () => { return Round.finalsStatus == StageStatusEnum.SheetsPrinted || Round.finalsStatus == StageStatusEnum.ResultsEntered; }
    );
    public ICommand BtnFinalizeFinals => new RelayCommand(
        () =>
        {
            if (FinalResultsViewModel == null)
                return;

            Finals.Finalize(Series, Round, FinalResultsViewModel.Races);
            Round.finalsStatus = StageStatusEnum.Finished;
            Round.Save();

            Series.Rounds[(int)Round.roundNumber - 1].DashForCashFormulaID = DashForCashFormula?.id;
            Series.Rounds[(int)Round.roundNumber - 1].Status = SeriesRoundStatusEnum.Complete;
            Series.Save();

            NotifyEnabled();
        },
        () => { return Round.finalsStatus == StageStatusEnum.ResultsEntered; }
    );
    #endregion

    #region ResultsButtons
    public ICommand BtnRoundStandings => new RelayCommand(
        () =>
        {
            Standings.Round(Series, Round);
            MessageBox.Show("Opening Round Standings In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{Round.roundNumber}.standings.html") { UseShellExecute = true });
        },
        () => { return Round.finalsStatus == StageStatusEnum.Finished; }
    );
    public ICommand BtnSeriesStandings => new RelayCommand(
        () =>
        {
            Standings.Series(Series, Round);
            MessageBox.Show("Opening Series Standings In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/series.standings.round{Round.roundNumber}.html") { UseShellExecute = true });
        },
        () => { return Round.finalsStatus == StageStatusEnum.Finished; }
    );
    #endregion
}
