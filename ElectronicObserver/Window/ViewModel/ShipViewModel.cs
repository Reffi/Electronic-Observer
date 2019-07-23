using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Helpers;
using ElectronicObserver.Window.ControlWpf;

namespace ElectronicObserver.Window.ViewModel
{
    public class ShipViewModel: Observable
    {
        private int _level;
        private int _hp;
        private int _baseArmor;
        private int _baseEvasion;
        private int _baseAircraft;
        private int _baseSpeed;
        private int _baseRange;
        private double _baseAccuracy;

        private int _condition;
        private int _baseFirepower;
        private int _baseTorpedo;
        private int _baseAA;
        private int _baseASW;
        private int _baseLoS;
        private int _baseLuck;
        private int _baseNightPower;


        private IShipDataCustom _ship;
        private ObservableCollection<EquipmentViewModel> _equipmentViewModels;

        private VisibleFits _synergies;

        public IShipDataCustom Ship
        {
            get => _ship;
            set
            {
                _ship = value;
                Equipment = _ship.Equipment;
            }
        }

        public ObservableCollection<EquipmentViewModel> EquipmentViewModels
        {
            get => _equipmentViewModels;
            set
            {
                //_ship.Equipment = value.Select(x => x.Equip).ToArray();
                SetField(ref _equipmentViewModels, value);
            }

        }

        private IEquipmentDataCustom[] _equipment;

        public IEquipmentDataCustom[] Equipment
        {
            get => _ship.Equipment;
            set
            {
                _synergies = new VisibleFits();
                _ship.Equipment = value;
                EquipmentViewModel[] evms = new EquipmentViewModel[6];
                for (int i = 0; i<6; i++)
                {
                    if (_equipment?[i] == value[i])
                        continue;

                    if (value[i] == null)
                    {
                        evms[i] = null;
                        continue;
                    }

                    FitBonusViewModel fbvm = new FitBonusViewModel(_ship, value[i], true);
                    fbvm.PropertyChanged += EquipmentStatChange;

                    evms[i] = new EquipmentViewModel(value[i]);
                    evms[i].CurrentFitBonus = fbvm;
                    evms[i].PropertyChanged += EquipmentStatChange;
                }

                EquipmentViewModels = new ObservableCollection<EquipmentViewModel>(evms);

                // change all properties
                OnPropertyChanged(string.Empty);
            }
        }

        public string Name => _ship.Name;

        public int Level
        {
            get => _ship.Level;
            set
            {
                value = ValidRange(value, 1);
                _ship.Level = value;

                SetField(ref _level, value);
                OnPropertyChanged(nameof(BaseEvasion));
                OnPropertyChanged(nameof(BaseASW));
                OnPropertyChanged(nameof(BaseLoS));
                OnPropertyChanged(nameof(BaseAccuracy));
            }
        }
        public int HP
        {
            get => _ship.HP;
            set
            {
                value = ValidRange(value, 4);
                _ship.HP = value;

                SetField(ref _hp, value);
            }
        }
        public int BaseArmor
        {
            get => _ship.BaseArmor;
            set
            {
                value = ValidRange(value);
                _ship.BaseArmor = value;

                SetField(ref _baseArmor, value);
            }
        }
        public int BaseEvasion
        {
            get => _ship.BaseEvasion;
            set
            {
                value = ValidRange(value);
                _ship.BaseEvasion = value;

                SetField(ref _baseEvasion, value);
            }
        }

        public int[] Aircraft => _ship.Aircraft;
        public int BaseSpeed
        {
            get => _ship.BaseSpeed;
            set
            {
                value = ValidRange(value);
                _ship.BaseSpeed = value;

                SetField(ref _baseSpeed, value);
            }
        }
        public int BaseRange
        {
            get => _ship.BaseRange;
            set
            {
                value = ValidRange(value);
                _ship.BaseRange = value;

                SetField(ref _baseRange, value);
            }
        }
        public double BaseAccuracy => _ship.BaseAccuracy;




        public int Condition
        {
            get => _ship.Condition;
            set
            {
                value = ValidRange(value, 0, 100);
                _ship.Condition = value;

                SetField(ref _condition, value);
            }
        }
        public int BaseFirepower
        {
            get => _ship.BaseFirepower;
            set
            {
                value = ValidRange(value);
                _ship.BaseFirepower = value;

                SetField(ref _baseFirepower, value);
                OnPropertyChanged(nameof(BaseNightPower));
                OnPropertyChanged(nameof(DisplayFirepower));
                OnPropertyChanged(nameof(DisplayNightPower));
            }
        }
        public int BaseTorpedo
        {
            get => _ship.BaseTorpedo;
            set
            {
                value = ValidRange(value);
                _ship.BaseTorpedo = value;

                SetField(ref _baseTorpedo, value);
                OnPropertyChanged(nameof(BaseNightPower));
                OnPropertyChanged(nameof(DisplayTorpedo));
                OnPropertyChanged(nameof(DisplayNightPower));
            }
        }
        public int BaseAA
        {
            get => _ship.BaseAA;
            set
            {
                value = ValidRange(value);
                _ship.BaseAA = value;

                SetField(ref _baseAA, value);
                OnPropertyChanged(nameof(DisplayAA));
            }
        }
        public int BaseASW
        {
            get => _ship.BaseASW;
            set
            {
                value = ValidRange(value);
                _ship.BaseASW = value;

                SetField(ref _baseASW, value);
                OnPropertyChanged(nameof(DisplayASW));
            }
        }
        public int BaseLoS
        {
            get => _ship.BaseLoS;
            set
            {
                value = ValidRange(value);
                _ship.BaseLoS = value;

                SetField(ref _baseLoS, value);
                OnPropertyChanged(nameof(DisplayLoS));
            }
        }
        public int BaseLuck
        {
            get => _ship.BaseLuck;
            set
            {
                value = ValidRange(value);
                _ship.BaseLuck = value;

                SetField(ref _baseLuck, value);
                OnPropertyChanged(nameof(BaseAccuracy));
            }
        }
        public int BaseNightPower => _ship.BaseNightPower;


        public bool IsExpansionSlotAvailable => _ship.IsExpansionSlotAvailable;




        public int DisplayArmor => BaseArmor 
                                   + EquipmentViewModels
                                       .Where(x => x != null)
                                       .Sum(x => x.BaseArmor + x.CurrentFitBonus.Armor);
        public int DisplayEvasion => BaseEvasion 
                                     + EquipmentViewModels
                                         .Where(x => x != null)
                                         .Sum(x => x.BaseEvasion + x.CurrentFitBonus.Evasion);
        public double DisplayAccuracy => BaseAccuracy 
                                         + EquipmentViewModels
                                             .Where(x => x != null)
                                             .Sum(x => x.BaseAccuracy);

        public int DisplayFirepower => BaseFirepower
                                       + EquipmentViewModels
                                           .Where(x => x != null)
                                           .Sum(x => x.BaseFirepower + x.CurrentFitBonus.Firepower);
        public int DisplayTorpedo => BaseTorpedo
                                     + EquipmentViewModels.Where(x => x != null)
                                         .Sum(x => x.BaseTorpedo + x.CurrentFitBonus.Torpedo);
        public int DisplayAA => BaseAA
                                + EquipmentViewModels.Where(x => x != null)
                                    .Sum(x => x.BaseAA + x.CurrentFitBonus.AA);
        public int DisplayASW => BaseASW 
                                 + EquipmentViewModels.Where(x => x != null)
                                     .Sum(x => x.BaseASW + x.CurrentFitBonus.ASW);
        public int DisplayLoS => BaseLoS
                                 + EquipmentViewModels.Where(x => x != null)
                                     .Sum(x => x.BaseLoS + x.CurrentFitBonus.LoS);
        public int DisplayNightPower => DisplayFirepower + DisplayTorpedo;




        public ShipViewModel()
        {

        }

        public ShipViewModel(IShipDataCustom ship)
        {
            Ship = ship;
        }

        private int ValidRange(int value, int min = 0, int? max = null)
        {
            if (value < min)
                return min;

            if (max != null && value > max)
                return max.Value;

            return value;
        }
        
        private void EquipmentStatChange(object sender, PropertyChangedEventArgs e)
        {
            // optimization point
            OnPropertyChanged(string.Empty);
        }
    }
}
