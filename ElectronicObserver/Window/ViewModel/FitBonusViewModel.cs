using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Helpers;

namespace ElectronicObserver.Window.ViewModel
{
    public class FitBonusViewModel: Observable
    {
        private FitBonusCustom _currentFitBonus;
        public FitBonusCustom CurrentFitBonus => _currentFitBonus;

        private IEquipmentDataCustom _equip;

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
            get => _equip.CurrentFitBonus.Firepower;
            set
            {
                _equip.CurrentFitBonus.Firepower = value;
                SetField(ref _firepower, value);
            }
        }
        public int Torpedo
        {
            get => _equip.CurrentFitBonus.Torpedo;
            set
            {
                _equip.CurrentFitBonus.Torpedo = value;
                SetField(ref _torpedo, value);
            }
        }
        public int AA
        {
            get => _equip.CurrentFitBonus.AA;
            set
            {
                _equip.CurrentFitBonus.AA = value;
                SetField(ref _aa, value);
            }
        }
        public int ASW
        {
            get => _equip.CurrentFitBonus.ASW;
            set
            {
                _equip.CurrentFitBonus.ASW = value;
                SetField(ref _asw, value);
            }
        }
        public int Evasion
        {
            get => _equip.CurrentFitBonus.Evasion;
            set
            {
                _equip.CurrentFitBonus.Evasion = value;
                SetField(ref _evasion, value);
            }
        }
        public int Armor
        {
            get => _equip.CurrentFitBonus.Armor;
            set
            {
                _equip.CurrentFitBonus.Armor = value;
                SetField(ref _armor, value);
            }
        }
        public int LoS
        {
            get => _equip.CurrentFitBonus.LoS;
            set
            {
                _equip.CurrentFitBonus.LoS = value;
                SetField(ref _los, value);
            }
        }
        public int? Accuracy
        {
            get => _equip.CurrentFitBonus.Accuracy;
            set
            {
                _equip.CurrentFitBonus.Accuracy = value;
                SetField(ref _accuracy, value);
            }
        }

        public FitBonusViewModel(IShipDataCustom ship, IEquipmentDataCustom equip, bool educatedFitGuessing)
        {
            _equip = equip;
            _currentFitBonus = new FitBonusCustom(ship, equip, educatedFitGuessing);
        }
    }
}
