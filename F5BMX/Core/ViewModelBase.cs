using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace F5BMX.Core;

internal class ViewModelBase : ModelBase, INotifyPropertyChanged
{

    public event PropertyChangedEventHandler? PropertyChanged;

    internal void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
