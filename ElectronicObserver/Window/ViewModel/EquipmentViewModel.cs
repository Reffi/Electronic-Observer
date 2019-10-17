using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Helpers;

namespace ElectronicObserver.Window.ViewModel
{
    public class EquipmentViewModel: Observable
    {
        private int _id;

        private int _baseFirepower;
        private int _baseTorpedo;
        private int _baseAA;
        private int _baseArmor;
        private int _baseASW;
        private int _baseEvasion;
        private int _baseLoS;
        private int _baseAccuracy;
        private int _baseBombing;

        private int _level;
        private int _proficiency;




        private IEquipmentDataCustom _equip;

        public IEquipmentDataCustom Equip
        {
            get => _equip;
            set => _equip = value;
        }

        public int ID => _equip.ID;
        public string Name => _equip.Name;
        public EquipmentTypes CategoryType => _equip.CategoryType;


        public int BaseFirepower
        {
            get => _equip.BaseFirepower;
            set
            {
                _equip.BaseFirepower = value;

                SetField(ref _baseFirepower, value);
            }

        }
        public int BaseTorpedo
        {
            get => _equip.BaseTorpedo;
            set
            {
                _equip.BaseTorpedo = value;

                SetField(ref _baseTorpedo, value);
            }
        }
        public int BaseAA
        {
            get => _equip.BaseAA;
            set
            {
                _equip.BaseAA = value;

                SetField(ref _baseAA, value);
            }
        }
        public int BaseArmor
        {
            get => _equip.BaseArmor;
            set
            {
                _equip.BaseArmor = value;

                SetField(ref _baseArmor, value);
            }
        }
        public int BaseASW
        {
            get => _equip.BaseASW;
            set
            {
                _equip.BaseASW = value;

                SetField(ref _baseASW, value);
            }
        }
        public int BaseEvasion
        {
            get => _equip.BaseEvasion;
            set
            {
                _equip.BaseEvasion = value;

                SetField(ref _baseEvasion, value);
            }
        }
        public int BaseLoS
        {
            get => _equip.BaseLoS;
            set
            {
                _equip.BaseLoS = value;

                SetField(ref _baseLoS, value);
            }
        }
        public int BaseAccuracy
        {
            get => _equip.BaseAccuracy;
            set
            {
                _equip.BaseAccuracy = value;

                SetField(ref _baseAccuracy, value);
            }
        }
        public int BaseBombing
        {
            get => _equip.BaseBombing;
            set
            {
                _equip.BaseBombing = value;

                SetField(ref _baseBombing, value);
            }
        }


        public int Level
        {
            get => _equip.Level;
            set
            {
                value = ValidRange(value, 0, 10);
                _equip.Level = value;

                SetField(ref _level, value);
            }
        }
        public int Proficiency
        {
            get => _equip.Proficiency;
            set
            {
                value = ValidRange(value, 0, 7);
                _equip.Proficiency = value;

                SetField(ref _proficiency, value);
            }
        }
        public int SlotSize { get; set; }


        private FitBonusViewModel _currentFitBonus;
        public FitBonusViewModel CurrentFitBonus
        {
            get => _currentFitBonus;
            set
            {
                _equip.CurrentFitBonus = value.CurrentFitBonus;
                _currentFitBonus = value;
            }
        }




        public EquipmentViewModel() => Equip = new EquipmentDataCustom();

        public EquipmentViewModel(IEquipmentDataCustom equip)
        {
            _equip = equip;

            _id = equip.ID;

            _baseFirepower = equip.BaseFirepower;
            _baseTorpedo = equip.BaseTorpedo;
            _baseAA = equip.BaseAA;
            _baseArmor = equip.BaseArmor;
            _baseASW = equip.BaseASW;
            _baseEvasion = equip.BaseEvasion;
            _baseLoS = equip.BaseLoS;
            _baseAccuracy = equip.BaseAccuracy;
            _baseBombing = equip.BaseBombing;

            _level = equip.Level;
            _proficiency = equip.Proficiency;
    }

        private int ValidRange(int value, int min = 0, int? max = null)
        {
            if (value < min)
                return min;

            if (max != null && value > max)
                return max.Value;

            return value;
        }
    }
}
