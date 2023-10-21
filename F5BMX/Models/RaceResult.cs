using F5BMX.Core;
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
            raceNumber = race.raceNumber;
            formulaName = formula.name;
            gate1 = formula.riders.Where(x => x.id == race.gate1).FirstOrDefault();
            gate2 = formula.riders.Where(x => x.id == race.gate2).FirstOrDefault();
            gate3 = formula.riders.Where(x => x.id == race.gate3).FirstOrDefault();
            gate4 = formula.riders.Where(x => x.id == race.gate4).FirstOrDefault();
            gate5 = formula.riders.Where(x => x.id == race.gate5).FirstOrDefault();
            gate6 = formula.riders.Where(x => x.id == race.gate6).FirstOrDefault();
            gate7 = formula.riders.Where(x => x.id == race.gate7).FirstOrDefault();
            gate8 = formula.riders.Where(x => x.id == race.gate8).FirstOrDefault();
        }

        public int raceNumber { get; init; }
        public string formulaName { get; init; }
        public RoundRider? gate1 { get; init; }
        public RoundRider? gate2 { get; init; }
        public RoundRider? gate3 { get; init; }
        public RoundRider? gate4 { get; init; }
        public RoundRider? gate5 { get; init; }
        public RoundRider? gate6 { get; init; }
        public RoundRider? gate7 { get; init; }
        public RoundRider? gate8 { get; init; }

    }
}
