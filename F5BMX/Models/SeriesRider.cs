using F5BMX.Core;
using F5BMX.Interfaces;
using System;
using System.Text.Json.Serialization;

namespace F5BMX.Models
{
    internal class SeriesRider : ViewModelBase, IRider
    {

        private int _yearOfBirth;

        public Guid id { get; init; } = Guid.NewGuid();
        public string firstName { get; set; } = String.Empty;
        public string lastName { get; set; } = String.Empty;
        public string club { get; set; } = String.Empty;
        public string plateNumber { get; set; } = String.Empty;
        public int yearOfBirth { get => _yearOfBirth; set { _yearOfBirth = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(yearAge)); } }
        public int seriesPoints { get; set; }
        public Guid formulaID { get; set; }

        [JsonIgnore]
        public int yearAge { get => DateTime.Now.Year - yearOfBirth; }

    }
}
