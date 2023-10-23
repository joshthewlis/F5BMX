using F5BMX.Enums;
using System;

namespace F5BMX.Models;

internal class SeriesRoundInformation
{

    public uint roundNumber { get; set; }
    public DateOnly date { get; set; }
    public SeriesRoundStatusEnum status { get; set; } = SeriesRoundStatusEnum.Incomplete;

}
