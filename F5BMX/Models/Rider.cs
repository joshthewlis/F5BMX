using F5BMX.Core;
using F5BMX.Interfaces;
using System;

namespace F5BMX.Models
{
    internal class Rider : ViewModelBase, IRider
    {

        private int _yearOfBirth;

        public Guid guid { get; init; } = Guid.NewGuid();
        public string firstName { get; set; } = String.Empty;
        public string lastName { get; set; } = String.Empty;
        public int yearOfBirth { get => _yearOfBirth; set { _yearOfBirth = value; NotifyPropertyChanged(); NotifyPropertyChanged("yearAge"); } }
        public string numberPlate { get; set; } = String.Empty;

        public int yearAge { get => DateTime.Now.Year - yearOfBirth; }

    }
}
