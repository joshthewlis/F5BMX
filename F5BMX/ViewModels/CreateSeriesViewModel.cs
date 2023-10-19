using F5BMX.Core;
using F5BMX.Models;

namespace F5BMX.ViewModels;

internal class CreateSeriesViewModel : NotifyBase
{

    public Series series { get; init; } = new Series();

    public Formula? selectedFormula { get; set; }

}
