using F5BMX.Core;
using F5BMX.Core.IO;
using F5BMX.Interfaces;
using F5BMX.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;

namespace F5BMX.ViewModels;

internal class RegisterRidersViewModel : ViewModelBase
{

    public RegisterRidersViewModel() : this(new Series(), new Round(0, new List<SeriesFormula>(), false))
    { }

    public RegisterRidersViewModel(Series series, Round round)
    {
        Series = series;
        Round = round;

        _cvsSeriesRiders = new CollectionViewSource()
        {
            Source = Series.Riders,
            IsLiveSortingRequested = true,
            IsLiveFilteringRequested = true
        };
        CvsSeriesRiders.Filter += CvsSeriesRiders_Filter;
        CvsSeriesRiders.SortDescriptions.Add(new SortDescription("LastName", ListSortDirection.Ascending));
        CvsSeriesRiders.SortDescriptions.Add(new SortDescription("FirstName", ListSortDirection.Ascending));
    }

    private void CvsSeriesRiders_Filter(object sender, FilterEventArgs e)
    {
        var rider = (SeriesRider)e.Item;
        foreach (var formula in Round.Formulas)
        {
            if (formula.Riders.Where(x => x.ID == rider.ID).Count() > 0)
            {
                e.Accepted = false;
                return;
            }
        }

        e.Accepted = true;
    }

    public Series Series { get; set; }
    public Round Round { get; set; }

    private CollectionViewSource _cvsSeriesRiders;
    public CollectionViewSource CvsSeriesRiders { get => _cvsSeriesRiders; }

    private SeriesRider? _selectedRider;
    public SeriesRider? SelectedRider
    {
        get => _selectedRider;
        set
        {
            if (_selectedRider != null)
                _selectedRider.PropertyChanged -= SelectedRider_PropertyChanged;

            _selectedRider = value;

            if (_selectedRider != null)
                _selectedRider.PropertyChanged += SelectedRider_PropertyChanged;

            NotifyPropertyChanged();
        }
    }

    public RoundRider? SelectedRegisteredRider { get; set; }

    private void SelectedRider_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var rider = (SeriesRider?)sender;

        if (rider != null)
        {
            if (e.PropertyName == nameof(SeriesRider.YearAge))
            {
                foreach (var formula in Series.Formulas)
                    if (formula.MinAge <= rider.YearAge && formula.MaxAge >= rider.YearAge)
                        rider.FormulaID = formula.ID;

                rider.NotifyPropertyChanged(nameof(rider.FormulaID));
            }
        }
    }

    #region Buttons
    public ICommand BtnNewRider => new RelayCommand(NewRider);
    private void NewRider()
    {
        SelectedRider = new SeriesRider();
    }

    public ICommand BtnRegisterRider => new RelayCommand(RegisterRider, canRegisterRider);
    private void RegisterRider()
    {
        if (SelectedRider != null)
        {
            if (Series.Riders.Contains(SelectedRider) == false)
                Series.Riders.Add(SelectedRider);

            if (SelectedRider != null)
            {
                var formula = Round.Formulas.FirstOrDefault(x => x.ID == SelectedRider.FormulaID);
                if (formula != null)
                    formula.Riders.Add(new RoundRider(SelectedRider));
            }

            Save();
        }
    }
    private bool canRegisterRider()
    {
        if (SelectedRider != null)
            return true;

        return false;
    }

    /* FIX LATER */
    public ICommand BtnUnregisterRider => new RelayCommand(UnregisterRider, CanUnregisterRider);
    private void UnregisterRider()
    {
        if (SelectedRegisteredRider == null)
            return;

        foreach (var formula in Round.Formulas)
            if (formula.Riders.Contains(SelectedRegisteredRider))
                formula.Riders.Remove(SelectedRegisteredRider);

        Save();
    }
    private bool CanUnregisterRider()
    {
        if (SelectedRegisteredRider == null)
            return false;

        return true;
    }
    public void UnregisterRider(IRider rider)
    {
        foreach (var formula in Round.Formulas)
            if (formula.Riders.Contains(rider))
                formula.Riders.Remove((RoundRider)rider);

        Save();
    }
    #endregion


    private void Save()
    {
        CvsSeriesRiders.View.Refresh();

        JSON.WriteFile("riders", Series.Riders);
        Round.Save();
    }
}
