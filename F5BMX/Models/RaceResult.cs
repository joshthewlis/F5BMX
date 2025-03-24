using F5BMX.Core;
using F5BMX.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F5BMX.Models
{
    internal class RaceResult : ModelBase
    {

        public RaceResult(RoundFormula formula, Race race)
        {
            RaceNumber = race.RaceNumber;
            FormulaName = formula.Name;
            Gates = new Dictionary<uint, RoundRiderResult>();

            foreach (KeyValuePair<uint, Guid> kvp in race.Gates)
                Gates[kvp.Key] = new RoundRiderResult(formula.Riders.Where(x => x.ID == kvp.Value).First());
        }

        public int RaceNumber { get; init; }
        public string FormulaName { get; init; }
        public Dictionary<uint, RoundRiderResult> Gates { get; init; }
        public uint NextResult { get; set; } = 1;

    }
}
