using ElectronicObserverTypes;

namespace ElectronicObserverViewModels
{
    public struct Slot
    {
        public EquipmentDataCustom Equip { get; }
        public int Size { get; }

        public Slot(EquipmentDataCustom equip, int size = 0)
        {
            Equip = equip;
            Size = size;
        }
    }

    public class EquipmentSlotViewModel : Observable
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
        private int _slotSize;



        private EquipmentDataCustom _equip;

        public EquipmentDataCustom Equip
        {
            get => _equip;
            set
            {
                _equip = value ?? new EquipmentDataCustom();
                OnPropertyChanged(string.Empty);
            }
        }

        public Slot Slot
        {
            get => new Slot(_equip, _slotSize);
            set
            {
                _equip = value.Equip;
                _slotSize = value.Size;
                OnPropertyChanged(string.Empty);
            }
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
                value = value.Clamp(0, 10);
                _equip.Level = value;

                SetField(ref _level, value);
            }
        }

        public int Proficiency
        {
            get => _equip.Proficiency;
            set
            {
                value = value.Clamp(0, 7);
                _equip.Proficiency = value;

                SetField(ref _proficiency, value);
            }
        }

        public int[] Levels { get; } = {10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0};

        public int[] Proficiencies { get; } = {7, 6, 5, 4, 3, 2, 1, 0};

        public int SlotSize
        {
            get => _slotSize;
            set
            {
                SetField(ref _slotSize, value);
            }
        }


        private int _fitFirepower;
        private int _fitTorpedo;
        private int _fitAA;
        private int _fitASW;
        private int _fitEvasion;
        private int _fitArmor;
        private int _fitLoS;
        private int _fitAccuracy;

        public int FitFirepower
        {
            get => FitBonus.Firepower;
            set
            {
                FitBonus.Firepower = value;
                SetField(ref _fitFirepower, value);
            }
        }

        public int FitTorpedo
        {
            get => FitBonus.Torpedo;
            set
            {
                FitBonus.Torpedo = value;
                SetField(ref _fitTorpedo, value);
            }
        }

        public int FitAA
        {
            get => FitBonus.AA;
            set
            {
                FitBonus.AA = value;
                SetField(ref _fitAA, value);
            }
        }

        public int FitASW
        {
            get => FitBonus.ASW;
            set
            {
                FitBonus.ASW = value;
                SetField(ref _fitASW, value);
            }
        }

        public int FitEvasion
        {
            get => FitBonus.Evasion;
            set
            {
                FitBonus.Evasion = value;
                SetField(ref _fitEvasion, value);
            }
        }

        public int FitArmor
        {
            get => FitBonus.Armor;
            set
            {
                FitBonus.Armor = value;
                SetField(ref _fitArmor, value);
            }
        }

        public int FitLoS
        {
            get => FitBonus.LoS;
            set
            {
                FitBonus.LoS = value;
                SetField(ref _fitLoS, value);
            }
        }

        public int FitAccuracy
        {
            get => FitBonus.Accuracy;
            set
            {
                FitBonus.Accuracy = value;
                SetField(ref _fitAccuracy, value);
            }
        }

        public FitBonusCustom FitBonus
        {
            get => Equip.CurrentFitBonus;
            set => Equip.CurrentFitBonus = value;
        }


        /*private FitBonusViewModel _currentFitBonus;

        public FitBonusViewModel CurrentFitBonus
        {
            get => _currentFitBonus;
            set
            {
                SetField(ref _currentFitBonus, value);
                _equip.CurrentFitBonus = value.CurrentFitBonus ?? new FitBonusCustom();
                CurrentFitBonus.PropertyChanged += (sender, args) => SetField(ref _currentFitBonus, value);
            }
        }*/




        public EquipmentSlotViewModel() => Equip = new EquipmentDataCustom();

        public EquipmentSlotViewModel(EquipmentDataCustom equip)
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
    }
}
