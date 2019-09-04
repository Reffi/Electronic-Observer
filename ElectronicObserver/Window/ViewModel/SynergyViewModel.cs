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
        private ShipDataCustom _ship;

        private int _firepower;
        private int _torpedo;
        private int _aa;
        private int _asw;
        private int _evasion;
        private int _armor;
        private int _los;
        private int? _accuracy;

        public int Firepower
        {
            get => _ship.CurrentSynergies.Firepower;
            set
            {
                _ship.CurrentSynergies.Firepower = value;
                SetField(ref _firepower, value);
            }
        }
        public int Torpedo
        {
            get => _ship.CurrentSynergies.Torpedo;
            set
            {
                _ship.CurrentSynergies.Torpedo = value;
                SetField(ref _torpedo, value);
            }
        }
        public int AA
        {
            get => _ship.CurrentSynergies.AA;
            set
            {
                _ship.CurrentSynergies.AA = value;
                SetField(ref _aa, value);
            }
        }
        public int ASW
        {
            get => _ship.CurrentSynergies.ASW;
            set
            {
                _ship.CurrentSynergies.ASW = value;
                SetField(ref _asw, value);
            }
        }
        public int Evasion
        {
            get => _ship.CurrentSynergies.Evasion;
            set
            {
                _ship.CurrentSynergies.Evasion = value;
                SetField(ref _evasion, value);
            }
        }
        public int Armor
        {
            get => _ship.CurrentSynergies.Armor;
            set
            {
                _ship.CurrentSynergies.Armor = value;
                SetField(ref _armor, value);
            }
        }
        public int LoS
        {
            get => _ship.CurrentSynergies.LoS;
            set
            {
                _ship.CurrentSynergies.LoS = value;
                SetField(ref _los, value);
            }
        }
        /*public int? Accuracy
        {
            get => _ship.CurrentSynergies.Accuracy;
            set
            {
                _ship.CurrentSynergies.Accuracy = value;
                SetField(ref _accuracy, value);
            }
        }*/

        public SynergyViewModel(ShipDataCustom ship)
        {
            _ship = ship;
        }
    }
}
