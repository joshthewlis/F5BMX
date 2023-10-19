using System;

namespace F5BMX.Interfaces;

internal interface IRider
{

    Guid guid { get; init; }
    string firstName { get; set; }
    string lastName { get; set; }

}
