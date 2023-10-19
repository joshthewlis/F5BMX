using System.Windows;

namespace F5BMX.Views;

public partial class SelectSeries : Window
{

    public SelectSeries()
    {
        InitializeComponent();
    }

    private void btnCreateSeries_Click(object sender, RoutedEventArgs e)
    {
        CreateSeries createSeries = new CreateSeries();
        createSeries.ShowDialog();
    }

    private void btnLoadSeries_Click(object sender, RoutedEventArgs e)
    {

    }

}
