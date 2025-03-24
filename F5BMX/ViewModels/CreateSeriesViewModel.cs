using F5BMX.Core;
using F5BMX.Core.IO;
using F5BMX.Interfaces;
using F5BMX.Models;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace F5BMX.ViewModels;

internal class CreateSeriesViewModel : ViewModelBase
{

    public CreateSeriesViewModel()
    {
        PropertyChanged += CreateSeriesViewModel_PropertyChanged;
    }

    private void CreateSeriesViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "selectedFormula")
            SelectedFormulaEditor = SelectedFormula?.Clone<SeriesFormula>();
    }

    public Series Series { get; init; } = new Series();

    private SeriesFormula? _selectedFormula;
    public SeriesFormula? SelectedFormula { get => _selectedFormula; set { _selectedFormula = value; NotifyPropertyChanged(); } }


    private SeriesFormula? _selectedFormulaEditor;
    public SeriesFormula? SelectedFormulaEditor { get => _selectedFormulaEditor; set { _selectedFormulaEditor = value; NotifyPropertyChanged(); } }

    #region FormulaEditorButtons
    public ICommand BtnMoveUp => new RelayCommand(MoveUp, CanMoveUp);
    public void MoveUp()
    {
        var formulaAbove = Series.Formulas?.Where(x => x.order == SelectedFormula?.order - 1).FirstOrDefault();

        if (SelectedFormula != null && formulaAbove != null)
        {
            SelectedFormula.order--;
            formulaAbove.order++;
        }
    }
    private bool CanMoveUp()
    {
        return SelectedFormula?.order != 1;
    }

    public ICommand BtnCreateFormula => new RelayCommand(CreateFormula);
    private void CreateFormula()
    {
        SelectedFormulaEditor = new SeriesFormula((uint)Series.Formulas.Count + 1);
    }

    public ICommand BtnMoveDown => new RelayCommand(MoveDown, CanMoveDown);
    private void MoveDown()
    {
        var formulaBelow = Series.Formulas?.Where(x => x.order == SelectedFormula?.order + 1).FirstOrDefault();
        if (SelectedFormula != null && formulaBelow != null)
        {
            SelectedFormula.order++;
            formulaBelow.order--;
        }
    }
    private bool CanMoveDown()
    {
        return SelectedFormula?.order != Series.Formulas.Count;
    }

    public ICommand BtnCreateUpdate => new RelayCommand(CreateUpdateFormula);
    private void CreateUpdateFormula()
    {
        PropertyChanged -= CreateSeriesViewModel_PropertyChanged;
        if (SelectedFormula != null && SelectedFormulaEditor != null)
        {
            if (SelectedFormula?.order == SelectedFormulaEditor?.order)
                if (SelectedFormula != null)
                    Series.Formulas?.Remove(SelectedFormula);

            if (SelectedFormulaEditor != null)
                Series.Formulas?.Add(SelectedFormulaEditor);

            SelectedFormula = SelectedFormulaEditor;
        }
        PropertyChanged += CreateSeriesViewModel_PropertyChanged;
    }

    public ICommand BtnCreateSeries => new RelayCommand<IClosable>(CreateSeries, CanCreateSeries);
    private void CreateSeries(IClosable window)
    {
        // CREATE DIRECTORY
        Directories.CreateSeriesDirectory(Series.Year, Series.Name);

        // CREATE ROUNDS STATUS
        for (uint i = 1; i <= Series.NumberOfRounds; i++)
            Series.Rounds.Add(new SeriesRoundInformation() { RoundNumber = i });

        // WRITE SERIES JSON FILE
        JSON.WriteFile<Series>($"{Series.Year}-{Series.Name.Replace(" ", "_")}/series", Series);

        // CLOSE WINDOW
        window.Close();
    }
    private bool CanCreateSeries()
    {
        if (Series.Name == null)
            return false;
        if (Series.Coordinator == null)
            return false;
        if (Series.CoordinatorEmail == null)
            return false;

        if (Series.Formulas?.Count == 0)
            return false;

        return true;
    }
    #endregion

}
