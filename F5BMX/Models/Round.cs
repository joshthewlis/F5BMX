using System.Collections.ObjectModel;

namespace F5BMX.Models;

internal class Round
{

    public int roundNumber { get; init; }

    public ObservableCollection<Rider> riders { get; init; } = new ObservableCollection<Rider>();

}
