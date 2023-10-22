using F5BMX.Core;

namespace F5BMX.Models;

internal class RoundRiderResult : ViewModelBase
{

    public RoundRiderResult(RoundRider? rider)
    {
        this.rider = rider;
    }

    public bool isEnabled => rider != null && result == 0;

    public RoundRider? rider { get; set; }

    private uint _result;
    public uint result { get => _result; set { _result = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(isEnabled)); } }

}
