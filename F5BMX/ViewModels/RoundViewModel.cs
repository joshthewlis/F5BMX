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
    public bool DashForCashEnabled => Round.MotosStatus == StageStatusEnum.NotGenerated;
    public bool RoundSettingsEnabled => Round.MotosStatus == StageStatusEnum.NotGenerated;
    public bool RegistrationEnabled => Round.MotosStatus == StageStatusEnum.NotGenerated;
    public bool MotosEnabled => Round.RegistrationStatus == RegistrationStatusEnum.Closed && Round.FinalsStatus == StageStatusEnum.NotGenerated;
    public bool FinalsEnabled => Round.MotosStatus == StageStatusEnum.Finished;
    public bool ResultsEnabled => Round.FinalsStatus == StageStatusEnum.Finished;

    [JsonIgnore]
    public EnterResultsViewModel? MotoResultsViewModel { get; set; }
    [JsonIgnore]
    public EnterResultsViewModel? FinalResultsViewModel { get; set; }
    [JsonIgnore]
    public RoundFormula? DashForCashFormula
    {
        get => Round.formulas.Where(x => x.ID == Round.DashForCashFormulaID).FirstOrDefault();
        set { if (value == null) return; Round.DashForCashFormulaID = value.ID; NotifyPropertyChanged(); }
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
                DataContext = new PickDashForCashViewModel(Round, Series.DashForCashFormulas, Round.formulas.Where(x => x.DashForCash == true).ToList())
            }.ShowDialog();
            NotifyPropertyChanged(nameof(DashForCashFormula));
        }
    );
    public ICommand BtnDashForCashRandom => new RelayCommand(
        () => 
        {
            var tmp = DashForCash.RandomDashForCashFormula(Series);
            DashForCashFormula = Round.formulas.Where(x => x.ID == tmp).First(); 
        }
    );
    #endregion

    #region RegistrationButtons
    public ICommand BtnRegisterRiders => new RelayCommand(
        () => { new Views.RegisterRiders() { DataContext = new RegisterRidersViewModel(Series, Round) }.Show(); },
        () => { return Round.RegistrationStatus == RegistrationStatusEnum.Open; }
    );
    public ICommand BtnCloseRegistration => new RelayCommand(
        () =>
        {
            Round.RegistrationStatus = RegistrationStatusEnum.Closed;
            Round.Save();

            Series.Rounds[(int)Round.RoundNumber - 1].Status = SeriesRoundStatusEnum.InProgress;
            Series.Rounds[(int)Round.RoundNumber - 1].Date = DateOnly.FromDateTime(DateTime.Now);
            Series.Save();

            NotifyEnabled();
        },
        () => { return Round.RegistrationStatus == RegistrationStatusEnum.Open; }
    );
    public ICommand BtnPrintRiderList => new RelayCommand(
        () =>
        {
            Registration.GenerateEntryList(Series, Round);
            MessageBox.Show("Opening Entry List In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{Round.RoundNumber}.entrylist.html") { UseShellExecute = true });
        },
        () => { return Round.RegistrationStatus == RegistrationStatusEnum.Closed && Round.MotosStatus == StageStatusEnum.NotGenerated; }
    );
    public ICommand BtnReOpenRegistration => new RelayCommand(
        () => { Round.RegistrationStatus = RegistrationStatusEnum.Open; NotifyEnabled(); },
        () => { return Round.RegistrationStatus == RegistrationStatusEnum.Closed && Round.MotosStatus == StageStatusEnum.NotGenerated; }
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
            Round.MotosStatus = StageStatusEnum.Generated;
            Round.Save();

            NotifyEnabled();
        },
        () => { return Round.RegistrationStatus == RegistrationStatusEnum.Closed && Round.MotosStatus == StageStatusEnum.NotGenerated; }
    );
    public ICommand BtnPrintMotoSheets => new RelayCommand(
        () =>
        {
            Motos.GenerateListing(Series, Round);
            MessageBox.Show("Opening Moto Listings In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{Round.RoundNumber}.motolist.html") { UseShellExecute = true });

            Motos.GenerateCommentary(Round);
            MessageBox.Show("Opening Commentary In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{Round.RoundNumber}.motocommentary.html") { UseShellExecute = true });

            Motos.GenerateCallup(Round);
            MessageBox.Show("Opening Call Up In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{Round.RoundNumber}.motocallup.html") { UseShellExecute = true });

            Round.MotosStatus = StageStatusEnum.SheetsPrinted;
            Round.Save();
        },
        () => { return Round.MotosStatus == StageStatusEnum.Generated; }
    );
    public ICommand BtnEnterMotoResults => new RelayCommand(
        () =>
        {
            MotoResultsViewModel ??= new EnterResultsViewModel(Round, EnterResultsTypeEnum.Moto);

            new Views.EnterResults() { DataContext = MotoResultsViewModel }.ShowDialog();
            Round.MotosStatus = StageStatusEnum.ResultsEntered;
        },
        () => { return Round.MotosStatus == StageStatusEnum.SheetsPrinted || Round.MotosStatus == StageStatusEnum.ResultsEntered; }
    );
    public ICommand BtnFinalizeMotos => new RelayCommand(
        () =>
        {
            if (MotoResultsViewModel == null)
                return;

            Motos.Finalize(MotoResultsViewModel.Races, Round.NumberOfMotos);
            Round.MotosStatus = StageStatusEnum.Finished;
            Round.Save();

            NotifyEnabled();
        },
        () => { return Round.MotosStatus == StageStatusEnum.ResultsEntered; }
    );
    #endregion

    #region FinalControlButtons
    public ICommand BtnGenerateFinals => new RelayCommand(
        () =>
        {
            Finals.Generate(Round);
            Round.FinalsStatus = StageStatusEnum.Generated;
            Round.Save();
        },
        () => { return Round.FinalsStatus == StageStatusEnum.NotGenerated; }
    );
    public ICommand BtnPrintFinalSheets => new RelayCommand(
        () =>
        {
            Finals.GenerateListing(Series, Round);
            MessageBox.Show("Opening Final Listings In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{Round.RoundNumber}.finallist.html") { UseShellExecute = true });

            Finals.GenerateCommentary(Round);
            MessageBox.Show("Opening Commentary In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{Round.RoundNumber}.finalcommentary.html") { UseShellExecute = true });

            Finals.GenerateCallup(Round);
            MessageBox.Show("Opening Call Up In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{Round.RoundNumber}.finalcallup.html") { UseShellExecute = true });

            Round.FinalsStatus = StageStatusEnum.SheetsPrinted;
            Round.Save();
        },
        () => { return Round.FinalsStatus == StageStatusEnum.Generated; }
    );
    public ICommand BtnEnterFinalResults => new RelayCommand(
        () =>
        {
            FinalResultsViewModel ??= new EnterResultsViewModel(Round, EnterResultsTypeEnum.Final);

            new Views.EnterResults() { DataContext = FinalResultsViewModel }.ShowDialog();
            Round.FinalsStatus = StageStatusEnum.ResultsEntered;
        },
        () => { return Round.FinalsStatus == StageStatusEnum.SheetsPrinted || Round.FinalsStatus == StageStatusEnum.ResultsEntered; }
    );
    public ICommand BtnFinalizeFinals => new RelayCommand(
        () =>
        {
            if (FinalResultsViewModel == null)
                return;

            Finals.Finalize(Series, Round, FinalResultsViewModel.Races);
            Round.FinalsStatus = StageStatusEnum.Finished;
            Round.Save();

            Series.Rounds[(int)Round.RoundNumber - 1].DashForCashFormulaID = DashForCashFormula?.ID;
            Series.Rounds[(int)Round.RoundNumber - 1].Status = SeriesRoundStatusEnum.Complete;
            Series.Save();

            NotifyEnabled();
        },
        () => { return Round.FinalsStatus == StageStatusEnum.ResultsEntered; }
    );
    #endregion

    #region ResultsButtons
    public ICommand BtnRoundStandings => new RelayCommand(
        () =>
        {
            Standings.Round(Series, Round);
            MessageBox.Show("Opening Round Standings In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/round{Round.RoundNumber}.standings.html") { UseShellExecute = true });
        },
        () => { return Round.FinalsStatus == StageStatusEnum.Finished; }
    );
    public ICommand BtnSeriesStandings => new RelayCommand(
        () =>
        {
            Standings.Series(Series, Round);
            MessageBox.Show("Opening Series Standings In Default Browser\r\nPlease Print.");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo($"{Directories.baseDirectory}/series.standings.round{Round.RoundNumber}.html") { UseShellExecute = true });
        },
        () => { return Round.FinalsStatus == StageStatusEnum.Finished; }
    );
    #endregion
}
