using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Helpers;
using ElectronicObserver.Window.ControlWpf;

namespace ElectronicObserver.Window.ViewModel
{
    public class ShipViewModel : Observable
    {
        private int _level;
        private int _hp;
        private int _baseArmor;
        private int _baseEvasion;
        private ObservableCollection<int> _baseAircraft;
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


        private ShipDataCustom _ship;

        private ObservableCollection<EquipmentSlotViewModel> _equipmentViewModels
            = new ObservableCollection<EquipmentSlotViewModel>(new EquipmentSlotViewModel[6]);

        private FitBonusCustom _synergy;
        private SynergyViewModel _synergyViewModel;

        public ShipDataCustom Ship
        {
            get => _ship;
            set
            {
                _ship = value;

                _level = Ship.Level;
                _hp = Ship.HP;
                _baseArmor = Ship.BaseArmor;
                _baseEvasion = Ship.BaseEvasion;
                _baseAircraft = new ObservableCollection<int>(Ship.Aircraft);
                _baseSpeed = Ship.BaseSpeed;
                _baseRange = Ship.BaseRange;
                _baseAccuracy = Ship.BaseAccuracy;

                _condition = Ship.Condition;
                _baseFirepower = Ship.BaseFirepower;
                _baseTorpedo = Ship.BaseTorpedo;
                _baseAA = Ship.BaseAA;
                _baseASW = Ship.BaseASW;
                _baseLoS = Ship.BaseLoS;
                _baseLuck = Ship.BaseLuck;
                _baseNightPower = Ship.BaseNightPower;

                Aircraft.CollectionChanged += AircraftChanged;

                Equipment = Ship.Equipment ?? new EquipmentDataCustom[6];
                Synergy = Ship.CurrentSynergies;
            }
        }

        public ObservableCollection<EquipmentSlotViewModel> EquipmentViewModels
        {
            get => _equipmentViewModels;
            set
            {
                //_ship.Equipment = value.Select(x => x.Equip).ToArray();
                SetField(ref _equipmentViewModels, value);
            }

        }

        public FitBonusCustom Synergy
        {
            get => _synergy;
            set
            {
                _synergy = value; 
                SynergyViewModel = new SynergyViewModel(Synergy);
                SynergyViewModel.PropertyChanged += SynergyStatChange;

                // OnPropertyChanged(string.Empty);
            }
        }

        public SynergyViewModel SynergyViewModel
        {
            get => _synergyViewModel;
            set
            {
                SetField(ref _synergyViewModel, value);
            }
        }

        public EquipmentDataCustom[] Equipment
        {
            get => Ship.Equipment;
            set
            {
                Ship.Equipment = value;

                for (int i = 0; i < 6; i++)
                {
                    if (EquipmentViewModels[i] == null)
                    {
                        EquipmentViewModels[i] = new EquipmentSlotViewModel();
                    }

                    if (EquipmentViewModels[i].Equip == value[i]) continue;

                    if (value[i] == null)
                    {
                        EquipmentViewModels[i] = new EquipmentSlotViewModel();
                        continue;
                    }

                    FitBonusCustom fitBonus = new FitBonusCustom(Ship, value[i]);

                    EquipmentViewModels[i] = new EquipmentSlotViewModel(value[i]) {FitBonus = fitBonus};
                    if (i < Aircraft.Count)
                    {
                        EquipmentViewModels[i].SlotSize = Aircraft[i];
                    }

                    EquipmentViewModels[i].PropertyChanged += EquipmentStatChange;
                }

                // change all properties
                OnPropertyChanged(string.Empty);
            }
        }

        public int ShipID => _ship.ID;
        public string Name => _ship.Name;

        public int Level
        {
            get => _ship.Level;
            set
            {
                value = value.Clamp(1);
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
                value = value.Clamp(4);
                _ship.HP = value;

                SetField(ref _hp, value);
            }
        }

        public int BaseArmor
        {
            get => _ship.BaseArmor;
            set
            {
                value = value.Clamp();
                _ship.BaseArmor = value;

                SetField(ref _baseArmor, value);
            }
        }

        public int BaseEvasion
        {
            get => _ship.BaseEvasion;
            set
            {
                value = value.Clamp();
                _ship.BaseEvasion = value;

                SetField(ref _baseEvasion, value);
            }
        }

        public ObservableCollection<int> Aircraft
        {
            get => _baseAircraft;
            set
            {
                _ship.Aircraft = value.ToArray();
                SetField(ref _baseAircraft, value);
            }
        }

        public int BaseSpeed
        {
            get => _ship.BaseSpeed;
            set
            {
                value = value.Clamp();
                _ship.BaseSpeed = value;

                SetField(ref _baseSpeed, value);
            }
        }

        public int BaseRange
        {
            get => _ship.BaseRange;
            set
            {
                value = value.Clamp();
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
                value = value.Clamp(0, 100);
                _ship.Condition = value;

                SetField(ref _condition, value);
            }
        }

        public int BaseFirepower
        {
            get => _ship.BaseFirepower;
            set
            {
                value = value.Clamp();
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
                value = value.Clamp();
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
                value = value.Clamp();
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
                value = value.Clamp();
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
                value = value.Clamp();
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
                value = value.Clamp();
                _ship.BaseLuck = value;

                SetField(ref _baseLuck, value);
                OnPropertyChanged(nameof(BaseAccuracy));
            }
        }

        public int BaseNightPower => _ship.BaseNightPower;


        public bool IsExpansionSlotAvailable => _ship.IsExpansionSlotAvailable;



        public int DisplayFirepower => BaseFirepower
                                       + EquipmentViewModels.Where(x => x != null)
                                           .Sum(x => x.BaseFirepower + x.FitBonus.Firepower)
                                       + SynergyViewModel.Firepower;

        public int DisplayTorpedo => BaseTorpedo
                                     + EquipmentViewModels.Where(x => x != null)
                                         .Sum(x => x.BaseTorpedo + x.FitBonus.Torpedo)
                                     + SynergyViewModel.Torpedo;

        public int DisplayAA => BaseAA
                                + EquipmentViewModels.Where(x => x != null)
                                    .Sum(x => x.BaseAA + x.FitBonus.AA)
                                + SynergyViewModel.AA;

        public int DisplayArmor => BaseArmor
                                   + EquipmentViewModels.Where(x => x != null)
                                       .Sum(x => x.BaseArmor + x.FitBonus.Armor)
                                   + SynergyViewModel.Armor;

        public int DisplayASW => BaseASW
                                 + EquipmentViewModels.Where(x => x != null)
                                     .Sum(x => x.BaseASW + x.FitBonus.ASW)
                                 + SynergyViewModel.ASW;

        public int DisplayEvasion => BaseEvasion
                                     + EquipmentViewModels.Where(x => x != null)
                                         .Sum(x => x.BaseEvasion + x.FitBonus.Evasion)
                                     + SynergyViewModel.Evasion;

        public int DisplayLoS => BaseLoS
                                 + EquipmentViewModels.Where(x => x != null)
                                     .Sum(x => x.BaseLoS + x.FitBonus.LoS)
                                 + SynergyViewModel.LoS;



        public double DisplayAccuracy => BaseAccuracy
                                         + EquipmentViewModels.Where(x => x != null)
                                             .Sum(x => x.BaseAccuracy);

        public int DisplayNightPower => DisplayFirepower + DisplayTorpedo;

        public int TotalAircraft => _baseAircraft.Sum();


        public ShipViewModel() => Ship = new ShipDataCustom();

        public ShipViewModel(ShipDataCustom ship)
        {
            Ship = ship;
        }

        private void EquipmentStatChange(object sender, PropertyChangedEventArgs e)
        {
            // optimization point

            Aircraft = new ObservableCollection<int>
            (
                new[]
                {
                    EquipmentViewModels[0].SlotSize,
                    EquipmentViewModels[1].SlotSize,
                    EquipmentViewModels[2].SlotSize,
                    EquipmentViewModels[3].SlotSize,
                    EquipmentViewModels[4].SlotSize,
                    EquipmentViewModels[5].SlotSize,
                }
            );

            /*for (int i = 0; i < Aircraft.Count; i++)
            {
                Aircraft[i] = EquipmentViewModels[i].SlotSize;
            }*/

            OnPropertyChanged(string.Empty);
        }

        private void AircraftChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var x in e.NewItems)
            {
                // do something
            }

            foreach (var y in e.OldItems)
            {
                //do something
            }
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                //do something
            }
        }

        private void SynergyStatChange(object sender, PropertyChangedEventArgs e)
        {
            // optimization point
            OnPropertyChanged(string.Empty);
        }
    }
}
