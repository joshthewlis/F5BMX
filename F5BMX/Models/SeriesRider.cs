using F5BMX.Core;
using F5BMX.Interfaces;
using System;
using System.Text.Json.Serialization;

namespace F5BMX.Models
{
    internal class SeriesRider : ViewModelBase, IRider
    {

        private int _yearOfBirth;

        public Guid ID { get; init; } = Guid.NewGuid();
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Club { get; set; } = String.Empty;
        public string PlateNumber { get; set; } = String.Empty;
        public int YearOfBirth { get => _yearOfBirth; set { _yearOfBirth = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(YearAge)); } }
        public uint SeriesPoints { get; set; }
        public Guid FormulaID { get; set; }

        [JsonIgnore]
        public int YearAge { get => DateTime.Now.Year - YearOfBirth; }

    }
}
