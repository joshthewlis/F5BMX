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
        public List<RaceResult> races;
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
        public ICommand btnFinish => new RelayCommand<IClosable>(finish, canFinish);
        private void finish(IClosable window)
        {
            window.Close();
        }
        private bool canFinish()
        {
            foreach (var race in races)
                if (race.gates.Where(x => x.Value.result == 0).Any())
                    return false;

            return idx == races.Count - 1;
        }
        #endregion

        #region Rider Buttons
        public ICommand btnRiderResult => new RelayCommand<uint>(riderResult);
        private void riderResult(uint gate)
        {
            race.gates[gate].result = race.nextResult;
            race.nextResult++;
        }
        public ICommand btnRiderDNF => new RelayCommand<uint>(riderDNF);
        private void riderDNF(uint gate)
        {
            race.gates[gate].result = (uint)race.gates.Count;
        }
        public ICommand btnRiderDNS => new RelayCommand<uint>(riderDNS);
        private void riderDNS(uint gate)
        {
            race.gates[gate].result = 99;
        }
        #endregion

    }
}
