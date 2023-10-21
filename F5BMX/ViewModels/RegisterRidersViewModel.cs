﻿using F5BMX.Core;
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

    public RegisterRidersViewModel() : this(new Series(), new Round(0, new List<SeriesFormula>()))
    { }

    public RegisterRidersViewModel(Series series, Round round)
    {
        this.series = series;
        this.round = round;

        _cvsSeriesRiders = new CollectionViewSource()
        {
            Source = series.riders,
            IsLiveSortingRequested = true,
            IsLiveFilteringRequested = true
        };
        cvsSeriesRiders.Filter += CvsSeriesRiders_Filter;
        cvsSeriesRiders.SortDescriptions.Add(new SortDescription("lastName", ListSortDirection.Ascending));
        cvsSeriesRiders.SortDescriptions.Add(new SortDescription("firstName", ListSortDirection.Ascending));
    }

    private void CvsSeriesRiders_Filter(object sender, FilterEventArgs e)
    {
        var rider = (SeriesRider)e.Item;
        foreach (var formula in round.formulas)
        {
            if (formula.riders.Where(x => x.id == rider.id).Count() > 0)
            {
                e.Accepted = false;
                return;
            }
        }

        e.Accepted = true;
    }

    public Series series { get; set; }
    public Round round { get; set; }

    private CollectionViewSource _cvsSeriesRiders;
    public CollectionViewSource cvsSeriesRiders { get => _cvsSeriesRiders; }

    private SeriesRider? _selectedRider;
    public SeriesRider? selectedRider
    {
        get => _selectedRider;
        set
        {
            if (_selectedRider != null)
                _selectedRider.PropertyChanged -= _selectedRider_PropertyChanged;

            _selectedRider = value;

            if (_selectedRider != null)
                _selectedRider.PropertyChanged += _selectedRider_PropertyChanged;

            NotifyPropertyChanged();
        }
    }

    public RoundRider? selectedRegisteredRider { get; set; }

    private void _selectedRider_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        var rider = (SeriesRider?)sender;

        if (rider != null)
        {
            if (e.PropertyName == "yearAge")
            {

                foreach (var formula in series.formulas)
                    if (formula.minAge <= rider.yearAge && formula.maxAge >= rider.yearAge)
                        rider.formulaID = formula.id;

                rider.NotifyPropertyChanged(nameof(rider.formulaID));
            }
        }
    }

    #region Buttons
    public ICommand btnNewRider => new RelayCommand(newRider);
    private void newRider()
    {
        selectedRider = new SeriesRider();
    }

    public ICommand btnRegisterRider => new RelayCommand(registerRider, canRegisterRider);
    private void registerRider()
    {
        if (selectedRider != null)
        {
            if (series.riders.Contains(selectedRider) == false)
                series.riders.Add(selectedRider);

            if (selectedRider != null)
            {
                var formula = round.formulas.Where(x => x.id == selectedRider.formulaID).FirstOrDefault();
                if (formula != null)
                    formula.riders.Add(new RoundRider(selectedRider));
            }

            Save();
        }
    }
    private bool canRegisterRider()
    {
        if (selectedRider != null)
            return true;

        return false;
    }

    /* FIX LATER */
    public ICommand btnUnregisterRider => new RelayCommand(unregisterRider, canUnregisterRider);
    private void unregisterRider()
    {
        if (selectedRegisteredRider == null)
            return;

        foreach (var formula in round.formulas)
            if (formula.riders.Contains(selectedRegisteredRider))
                formula.riders.Remove(selectedRegisteredRider);

        Save();
    }
    private bool canUnregisterRider()
    {
        if (selectedRegisteredRider == null)
            return false;

        return true;
    }
    public void unregisterRider(IRider rider)
    {
        foreach (var formula in round.formulas)
            if (formula.riders.Contains(rider))
                formula.riders.Remove((RoundRider)rider);

        Save();
    }
    #endregion


    private void Save()
    {
        cvsSeriesRiders.View.Refresh();

        JSON.WriteFile("riders", series.riders);
        round.Save();
    }
}