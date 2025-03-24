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

        public EnterResultsViewModel() : this(new Round(), EnterResultsTypeEnum.Moto) { }

        public EnterResultsViewModel(Round round, EnterResultsTypeEnum enterResultsTypeEnum)
        {
            Round = round;
            EnterResultsTypeEnum = enterResultsTypeEnum;

            var tmpRaces = new List<RaceResult>();
            foreach (var formula in round.Formulas)
            {
                if(enterResultsTypeEnum == EnterResultsTypeEnum.Moto)
                {
                    formula.Moto1.ForEach(moto => { tmpRaces.Add(new RaceResult(formula, moto)); });
                    formula.Moto2.ForEach(moto => { tmpRaces.Add(new RaceResult(formula, moto)); });
                    formula.Moto3.ForEach(moto => { tmpRaces.Add(new RaceResult(formula, moto)); });
                }
                else
                {
                    formula.Final.ForEach(final => { tmpRaces.Add(new RaceResult(formula, final)); });
                }
            }

            Races = tmpRaces.OrderBy(x => x.RaceNumber).ToList();
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
            Race.NextResult = 1;

            foreach(var riderResult in Race.Gates)
                riderResult.Value.Result = 0;
        }
        public ICommand BtnFinish => new RelayCommand<IClosable>(Finish, CanFinish);
        private void Finish(IClosable window)
        {
            window.Close();
        }
        private bool CanFinish()
        {
            foreach (var race in Races)
                if (race.Gates.Where(x => x.Value.Result == 0).Any())
                    return false;

            return idx == Races.Count - 1;
        }
        #endregion

        #region Rider Buttons
        public ICommand BtnRiderResult => new RelayCommand<uint>(RiderResult);
        private void RiderResult(uint gate)
        {
            Race.Gates[gate].Result = Race.NextResult;
            Race.NextResult++;
        }
        public ICommand BtnRiderDNF => new RelayCommand<uint>(RiderDNF);
        private void RiderDNF(uint gate)
        {
            Race.Gates[gate].Result = (uint)Race.Gates.Count;
        }
        public ICommand BtnRiderDNS => new RelayCommand<uint>(RiderDNS);
        private void RiderDNS(uint gate)
        {
            Race.Gates[gate].Result = 99;
        }
        #endregion

    }
}
