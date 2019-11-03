using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Helpers;

namespace ElectronicObserver.Window.ViewModel
{
    public class SynergyViewModel: Observable
    {
        private FitBonusCustom Synergy { get; }

        private int _firepower;
        private int _torpedo;
        private int _aa;
        private int _asw;
        private int _evasion;
        private int _armor;
        private int _los;
        private int _accuracy;

        public int Firepower
        {
            get => Synergy.Firepower;
            set
            {
                Synergy.Firepower = value;
                SetField(ref _firepower, value);
            }
        }
        public int Torpedo
        {
            get => Synergy.Torpedo;
            set
            {
                Synergy.Torpedo = value;
                SetField(ref _torpedo, value);
            }
        }
        public int AA
        {
            get => Synergy.AA;
            set
            {
                Synergy.AA = value;
                SetField(ref _aa, value);
            }
        }
        public int ASW
        {
            get => Synergy.ASW;
            set
            {
                Synergy.ASW = value;
                SetField(ref _asw, value);
            }
        }
        public int Evasion
        {
            get => Synergy.Evasion;
            set
            {
                Synergy.Evasion = value;
                SetField(ref _evasion, value);
            }
        }
        public int Armor
        {
            get => Synergy.Armor;
            set
            {
                Synergy.Armor = value;
                SetField(ref _armor, value);
            }
        }
        public int LoS
        {
            get => Synergy.LoS;
            set
            {
                Synergy.LoS = value;
                SetField(ref _los, value);
            }
        }

        /*public int Accuracy
        {
            get => _ship.CurrentSynergies.Accuracy;
            set
            {
                _ship.CurrentSynergies.Accuracy = value;
                SetField(ref _accuracy, value);
            }
        }*/

        public SynergyViewModel() => Synergy = new FitBonusCustom();

        public SynergyViewModel(FitBonusCustom synergy)
        {
            Synergy = synergy;

            _firepower = synergy.Firepower;
            _torpedo = synergy.Torpedo;
            _aa = synergy.AA;
            _asw = synergy.ASW;
            _evasion = synergy.Evasion;
            _armor = synergy.Armor;
            _los = synergy.LoS;
            _accuracy = synergy.Accuracy;
        }
    }
}
