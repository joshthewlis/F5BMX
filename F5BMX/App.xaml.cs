using System.Globalization;
using System.Threading;
using System.Windows;

namespace F5BMX
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App() : base()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");
        }

    }
}
