using F5BMX.Enums;
using System;
using System.Text.Json.Serialization;

namespace F5BMX.Models;

internal class SeriesRoundInformation
{

    public uint RoundNumber { get; set; }
    public DateOnly Date { get; set; }
    public SeriesRoundStatusEnum Status { get; set; } = SeriesRoundStatusEnum.Incomplete;
    public Guid? DashForCashFormulaID { get; set; }

    [JsonIgnore]
    public bool Complete => Status == SeriesRoundStatusEnum.Complete;

}
