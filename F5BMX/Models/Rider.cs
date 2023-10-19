using F5BMX.Interfaces;
using System;

namespace F5BMX.Models
{
    internal class Rider : IRider
    {

        public Guid guid { get; init; } = Guid.NewGuid();
        public string firstName { get; set; } = String.Empty;
        public string lastName { get; set; } = String.Empty;
        public int yearOfBirth { get; set; }
        public int yearAge { get => DateTime.Now.Year - yearOfBirth; }

    }
}
