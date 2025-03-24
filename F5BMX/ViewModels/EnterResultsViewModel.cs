using F5BMX.Core;
using F5BMX.Enums;
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

        public EnterResultsViewModel(Round round, EnterResultsTypeEnum enterResultsTypeEnum)
        {
            Round = round;
            EnterResultsTypeEnum = enterResultsTypeEnum;

            var tmpRaces = new List<RaceResult>();
            foreach (var formula in round.formulas)
            {
                if(enterResultsTypeEnum == EnterResultsTypeEnum.Moto)
                {
                    formula.moto1.ForEach(moto => { tmpRaces.Add(new RaceResult(formula, moto)); });
                    formula.moto2.ForEach(moto => { tmpRaces.Add(new RaceResult(formula, moto)); });
                    formula.moto3.ForEach(moto => { tmpRaces.Add(new RaceResult(formula, moto)); });
                }
                else
                {
                    formula.final.ForEach(final => { tmpRaces.Add(new RaceResult(formula, final)); });
                }
            }

            Races = tmpRaces.OrderBy(x => x.raceNumber).ToList();
        }

        public EnterResultsTypeEnum EnterResultsTypeEnum { get; init; }
        public Round Round { get; set; }

        #region Enumerator
        public List<RaceResult> Races;
        private int idx = 0;
        public RaceResult Race => Races[idx];
        #endregion

        #region Buttons
        public ICommand BtnPrevRace => new RelayCommand(
            () => { idx--; NotifyPropertyChanged(nameof(Race)); },
            () => { return idx != 0; }
        );

        public ICommand BtnNextRace => new RelayCommand(
            () => { idx++; NotifyPropertyChanged(nameof(Race)); },
            () => { return idx < Races.Count - 1; }
        );
        public ICommand BtnReset => new RelayCommand(Reset);
        private void Reset()
        {
            Race.nextResult = 0;

            foreach(var riderResult in Race.gates)
                riderResult.Value.result = 0;
        }
        public ICommand BtnFinish => new RelayCommand<IClosable>(Finish, CanFinish);
        private void Finish(IClosable window)
        {
            window.Close();
        }
        private bool CanFinish()
        {
            foreach (var race in Races)
                if (race.gates.Where(x => x.Value.result == 0).Any())
                    return false;

            return idx == Races.Count - 1;
        }
        #endregion

        #region Rider Buttons
        public ICommand BtnRiderResult => new RelayCommand<uint>(RiderResult);
        private void RiderResult(uint gate)
        {
            Race.gates[gate].result = Race.nextResult;
            Race.nextResult++;
        }
        public ICommand BtnRiderDNF => new RelayCommand<uint>(RiderDNF);
        private void RiderDNF(uint gate)
        {
            Race.gates[gate].result = (uint)Race.gates.Count;
        }
        public ICommand BtnRiderDNS => new RelayCommand<uint>(RiderDNS);
        private void RiderDNS(uint gate)
        {
            Race.gates[gate].result = 99;
        }
        #endregion

    }
}
