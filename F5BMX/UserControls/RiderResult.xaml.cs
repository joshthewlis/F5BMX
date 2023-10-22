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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace F5BMX.UserControls
{
    /// <summary>
    /// Interaction logic for RiderResult.xaml
    /// </summary>
    /// 
    public partial class RiderResult : UserControl
    {

        public static readonly DependencyProperty GateNumberProperty = DependencyProperty.Register("GateNumber", typeof(uint), typeof(RiderResult));
        public uint GateNumber
        {
            get => (uint)GetValue(GateNumberProperty);
            set => SetValue(GateNumberProperty, value);
        }

        public RiderResult()
        {
            InitializeComponent();
        }
    }
}
