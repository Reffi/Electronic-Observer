using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using ElectronicObserverTypes;

namespace ElectronicObserverViewModels
{
    public class ShipViewModel : Observable
    {
        

        private int _level;
        private int _hp;
        private int _baseArmor;
        private int _baseEvasion;
        private ObservableCollection<int> _baseAircraft = new ObservableCollection<int>();
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

        private FitBonusCustom _synergy;

        public ShipDataCustom Ship
        {
            get => _ship;
            set
            {
                _ship = value;

                ShipID = (int)Ship.ShipID;
                Name = Ship.Name;

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

                Equipment = Ship.Equipment;
                Synergy = Ship.Synergies;
            }
        }

        public ObservableCollection<EquipmentSlotViewModel> EquipmentViewModels { get; }

        public FitBonusCustom Synergy
        {
            get => _synergy;
            set
            {
                SynergyViewModel.Synergy = value;
                SetField(ref _synergy, value);
            }
        }

        public SynergyViewModel SynergyViewModel { get; }

        public EquipmentDataCustom[] Equipment
        {
            get => Ship.Equipment;
            set
            {
                Ship.Equipment = value;

                for (int i = 0; i < 6; i++)
                {
                    if (EquipmentViewModels[i].Equip == value[i]) continue;

                    if (value[i] == null)
                    {
                        EquipmentViewModels[i].Equip = new EquipmentDataCustom();
                        continue;
                    }

                    int slotSize = 0;

                    if (i < Aircraft.Count)
                    {
                        slotSize = Aircraft[i];
                    }

                    /*EquipmentViewModels[i].Equip = value[i];
                    EquipmentViewModels[i].SlotSize = slotSize;*/

                    EquipmentViewModels[i].Slot = new Slot(value[i], slotSize);
                }

                Synergy = Ship.Synergies;
                // change all properties
                OnPropertyChanged(string.Empty);
            }
        }

        public int ShipID { get; set; }
        public string Name { get; set; }

        public int Level
        {
            get => _level;
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
            get => _hp;
            set
            {
                value = value.Clamp(4);
                _ship.HP = value;

                SetField(ref _hp, value);
            }
        }

        public int BaseArmor
        {
            get => _baseArmor;
            set
            {
                value = value.Clamp();
                _ship.BaseArmor = value;

                SetField(ref _baseArmor, value);
            }
        }

        public int BaseEvasion
        {
            get => _baseEvasion;
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
            get => _baseSpeed;
            set
            {
                value = value.Clamp();
                _ship.BaseSpeed = value;

                SetField(ref _baseSpeed, value);
            }
        }

        public int BaseRange
        {
            get => _baseRange;
            set
            {
                value = value.Clamp();
                _ship.BaseRange = value;

                SetField(ref _baseRange, value);
            }
        }

        public double BaseAccuracy => 2 * Math.Sqrt(Level) + 1.5 * Math.Sqrt(BaseLuck);




        public int Condition
        {
            get => _condition;
            set
            {
                value = value.Clamp(0, 100);
                _ship.Condition = value;

                SetField(ref _condition, value);
            }
        }

        public int BaseFirepower
        {
            get => _baseFirepower;
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
            get => _baseTorpedo;
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
            get => _baseAA;
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
            get => _baseASW;
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
            get => _baseLoS;
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
            get => _baseLuck;
            set
            {
                value = value.Clamp();
                _ship.BaseLuck = value;

                SetField(ref _baseLuck, value);
                OnPropertyChanged(nameof(BaseAccuracy));
            }
        }

        public int BaseNightPower => BaseFirepower + BaseTorpedo;


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


        public ShipViewModel()
        {
            EquipmentViewModels = new ObservableCollection<EquipmentSlotViewModel>(new EquipmentSlotViewModel[6]);

            for (int i = 0; i < 6; i++)
            {
                EquipmentViewModels[i] = new EquipmentSlotViewModel
                {
                    Equip = new EquipmentDataCustom(),
                    FitBonus = new FitBonusCustom()
                };

                EquipmentViewModels[i].PropertyChanged += EquipmentStatChange;
                EquipmentViewModels[i].PropertyChanged += SynergyParametersModified;
            }

            SynergyViewModel = new SynergyViewModel();
            SynergyViewModel.PropertyChanged += SynergyStatChange;
        }

        public ShipViewModel(ShipDataCustom ship) : this()
        {
            Ship = ship;
        }

        private void EquipmentStatChange(object sender, PropertyChangedEventArgs e)
        {
            // optimization point
            if (e.PropertyName == nameof(EquipmentSlotViewModel.SlotSize))
            {
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
            }

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

        private void SynergyParametersModified(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Level):
                    Synergy = Ship.Synergies;
                    break;
            }
        }
    }
}
