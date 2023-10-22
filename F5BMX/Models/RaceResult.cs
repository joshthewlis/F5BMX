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
            this.raceNumber = race.raceNumber;
            this.formulaName = formula.name;
            this.gates = new Dictionary<uint, RoundRiderResult>();

            foreach (KeyValuePair<uint, Guid> kvp in race.gates)
                this.gates[kvp.Key] = new RoundRiderResult(formula.riders.Where(x => x.id == kvp.Value).First());
        }

        public int raceNumber { get; init; }
        public string formulaName { get; init; }
        public Dictionary<uint, RoundRiderResult> gates { get; init; }
        public uint nextResult { get; set; } = 1;

    }
}
