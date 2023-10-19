using F5BMX.ViewModels;
using System.Runtime.CompilerServices;
using System.Windows;

namespace F5BMX.Views;

public partial class SelectSeries : Window
{

    private readonly SelectSeriesViewModel viewModel;

    public SelectSeries()
    {
        InitializeComponent();

        viewModel = (SelectSeriesViewModel)this.DataContext;
    }

    private void btnCreateSeries_Click(object sender, RoutedEventArgs e)
    {
        CreateSeries createSeries = new CreateSeries();
        createSeries.ShowDialog();
    }

    private void btnLoadSeries_Click(object sender, RoutedEventArgs e)
    {
        SelectRound selectRound = new SelectRound()
        {
            DataContext = new SelectRoundViewModel()
            {
                series = viewModel.selectedSeries,
            }
        };
        selectRound.Show();
        this.Close();
    }

}
