using F5BMX.Core;
using F5BMX.Interfaces;
using F5BMX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace F5BMX.ViewModels
{
    internal class EnterResultsViewModel : ViewModelBase
    {

        public EnterResultsViewModel(Round round)
        {
            this.round = round;

            var tmpRaces = new List<RaceResult>();
            foreach (var formula in round.formulas)
            {
                formula.moto1.ForEach(moto => { tmpRaces.Add(new RaceResult(formula, 1, moto)); });
                formula.moto2.ForEach(moto => { tmpRaces.Add(new RaceResult(formula, 2, moto)); });
                formula.moto3.ForEach(moto => { tmpRaces.Add(new RaceResult(formula, 3, moto)); });
            }

            this.races = tmpRaces.OrderBy(x => x.raceNumber).ToList();
        }

        public Round round { get; set; }

        #region Enumerator
        private List<RaceResult> races;
        private int idx = 0;
        public RaceResult race => races[idx];
        #endregion

        #region Buttons
        public ICommand btnPrevRace => new RelayCommand(
            () => { idx--; NotifyPropertyChanged(nameof(race)); },
            () => { return idx != 0; }
        );

        public ICommand btnNextRace => new RelayCommand(
            () => { idx++; NotifyPropertyChanged(nameof(race)); },
            () => { return idx < races.Count - 1; }
        );
        public ICommand btnReset => new RelayCommand(reset);
        private void reset()
        {
            race.nextResult = 0;

            foreach(var riderResult in race.gates)
                riderResult.Value.result = 0;
        }
        public ICommand btnClose => new RelayCommand<IClosable>(
            (IClosable window) => { window.Close(); },
            () => { return idx == races.Count; }
        );
        #endregion

        #region Rider Buttons
        public ICommand btnRiderResult => new RelayCommand<string>(riderResult);
        private void riderResult(string gate)
        {
            int pos = int.Parse(gate);
            race.gates[pos].result = race.nextResult;
            race.nextResult++;
        }
        public ICommand btnRiderDNF => new RelayCommand<string>(riderDNF);
        private void riderDNF(string gate)
        {
            int pos = int.Parse(gate);
            race.gates[pos].result = (uint)race.gates.Count;
        }
        public ICommand btnRiderDNS => new RelayCommand<string>(riderDNS);
        private void riderDNS(string gate)
        {
            int pos = int.Parse(gate);
            race.gates[pos].result = 99;
        }
        #endregion

    }
}
