using F5BMX.Interfaces;
using F5BMX.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace F5BMX.Views
{
    /// <summary>
    /// Interaction logic for RegisterRiders.xaml
    /// </summary>
    public partial class RegisterRiders : Window, IClosable
    {
        public RegisterRiders()
        {
            InitializeComponent();
        }

        private void lstSeriesRiders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (RegisterRidersViewModel)DataContext;

            viewModel.btnRegisterRider.Execute(null);
        }

        private void lstFormulaRider_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var listView = (ListView)sender;

            if (listView == null)
                return;

            var selectedRider = (IRider)listView.SelectedItem;
            var viewModel = (RegisterRidersViewModel)DataContext;

            viewModel.unregisterRider(selectedRider);
        }

    }
}
