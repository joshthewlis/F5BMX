using F5BMX.Core;
using F5BMX.Interfaces;
using F5BMX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                formula.moto1.ForEach(moto => { tmpRaces.Add(new RaceResult(formula, moto)); });
                formula.moto2.ForEach(moto => { tmpRaces.Add(new RaceResult(formula, moto)); });
                formula.moto3.ForEach(moto => { tmpRaces.Add(new RaceResult(formula, moto)); });
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
            () => { return idx < races.Count-1; }
        );

        public ICommand btnClose => new RelayCommand<IClosable>(
            (IClosable window) => { window.Close(); },
            () => { return idx == races.Count; }
        );
        #endregion

    }
}
