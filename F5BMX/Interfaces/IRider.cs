using System;

namespace F5BMX.Interfaces;

internal interface IRider
{

    Guid ID { get; init; }
    string FirstName { get; set; }
    string LastName { get; set; }
    int YearOfBirth { get; set; }
    string PlateNumber { get; set; }

}
