using System.Windows;

namespace F5BMX.Views;

public partial class Start : Window
{

    public Start()
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
