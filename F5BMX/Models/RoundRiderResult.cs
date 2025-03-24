using F5BMX.Core;

namespace F5BMX.Models;

internal class RoundRiderResult : ViewModelBase
{

    public RoundRiderResult(RoundRider rider)
    {
        Rider = rider;
    }

    public bool IsEnabled => Rider != null && Result == 0;

    public RoundRider Rider { get; set; }

    private uint _result;
    public uint Result { get => _result; set { _result = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(isEnabled)); } }

}
