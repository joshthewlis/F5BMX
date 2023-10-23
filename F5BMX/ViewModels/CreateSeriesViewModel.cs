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
            selectedFormulaEditor = selectedFormula?.Clone<SeriesFormula>();
    }

    public Series series { get; init; } = new Series();

    private SeriesFormula? _selectedFormula;
    public SeriesFormula? selectedFormula { get => _selectedFormula; set { _selectedFormula = value; NotifyPropertyChanged(); } }


    private SeriesFormula? _selectedFormulaEditor;
    public SeriesFormula? selectedFormulaEditor { get => _selectedFormulaEditor; set { _selectedFormulaEditor = value; NotifyPropertyChanged(); } }

    #region FormulaEditorButtons
    public ICommand btnMoveUp => new RelayCommand(moveUp, canMoveUp);
    public void moveUp()
    {
        var formulaAbove = series.formulas?.Where(x => x.order == selectedFormula?.order - 1).FirstOrDefault();

        if (selectedFormula != null && formulaAbove != null)
        {
            selectedFormula.order--;
            formulaAbove.order++;
        }
    }
    private bool canMoveUp()
    {
        return selectedFormula?.order != 1;
    }

    public ICommand btnCreateFormula => new RelayCommand(createFormula);
    private void createFormula()
    {
        selectedFormulaEditor = new SeriesFormula((uint)series.formulas.Count + 1);
    }

    public ICommand btnMoveDown => new RelayCommand(moveDown, canMoveDown);
    private void moveDown()
    {
        var formulaBelow = series.formulas?.Where(x => x.order == selectedFormula?.order + 1).FirstOrDefault();
        if (selectedFormula != null && formulaBelow != null)
        {
            selectedFormula.order++;
            formulaBelow.order--;
        }
    }
    private bool canMoveDown()
    {
        return selectedFormula?.order != series.formulas.Count;
    }

    public ICommand btnCreateUpdate => new RelayCommand(createUpdateFormula);
    private void createUpdateFormula()
    {
        PropertyChanged -= CreateSeriesViewModel_PropertyChanged;
        if (selectedFormula != null && selectedFormulaEditor != null)
        {
            if (selectedFormula?.order == selectedFormulaEditor?.order)
                if (selectedFormula != null)
                    series.formulas?.Remove(selectedFormula);

            if (selectedFormulaEditor != null)
                series.formulas?.Add(selectedFormulaEditor);

            selectedFormula = selectedFormulaEditor;
        }
        PropertyChanged += CreateSeriesViewModel_PropertyChanged;
    }

    public ICommand btnCreateSeries => new RelayCommand<IClosable>(createSeries, canCreateSeries);
    private void createSeries(IClosable window)
    {
        // CREATE DIRECTORY
        Directories.CreateSeriesDirectory(series.year, series.name);

        // CREATE ROUNDS STATUS
        for (uint i = 1; i <= series.numberOfRounds; i++)
            series.rounds.Add(new SeriesRoundInformation() { roundNumber = i });

        // WRITE SERIES JSON FILE
        JSON.WriteFile<Series>($"{series.year}-{series.name.Replace(" ", "_")}/series", series);

        // CLOSE WINDOW
        window.Close();
    }
    private bool canCreateSeries()
    {
        if (series.name == null)
            return false;
        if (series.coordinator == null)
            return false;
        if (series.coordinatorEmail == null)
            return false;

        if (series.formulas?.Count == 0)
            return false;

        return true;
    }
    #endregion

}
