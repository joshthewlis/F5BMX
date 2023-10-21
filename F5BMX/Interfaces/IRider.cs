using System;

namespace F5BMX.Interfaces;

internal interface IRider
{

    Guid id { get; init; }
    string firstName { get; set; }
    string lastName { get; set; }
    int yearOfBirth { get; set; }
    string plateNumber { get; set; }

}
