using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F5BMX.Models
{
    internal class Rider
    {

        public string firstName { get; set; } = String.Empty;
        public string lastName { get; set; } = String.Empty;

        public int yearOfBirth { get; set; }

        public int yearAge { get => DateTime.Now.Year - yearOfBirth; }

    }
}
