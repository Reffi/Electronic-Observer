using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserver.Data
{
    /// <summary>
    ///
    /// Base = ships own stat, min + modernization
    /// no prefix = Base + fit + synergy
    /// 
    /// </summary>
    public class ShipDataCustom : IShipDataCustom
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


        private ShipData _ship;
        private ShipDataMaster _shipMaster;
        private IEquipmentDataCustom[] _equipment;

        private VisibleFits _visibleFits;
        private VisibleFits _synergies;

        public int FitFirepower => Equipment.Where(eq => eq != null).Sum(eq => eq.CurrentFitBonus?.Firepower ?? 0) 
                                   + _synergies.Firepower;
        public int FitTorpedo => Equipment.Where(eq => eq != null).Sum(eq => eq.CurrentFitBonus?.Torpedo ?? 0)
                                 + _synergies.Torpedo;

        public double FitAccuracy { get; set; }


        private int ASWMin { get; }
        private int ASWMax { get; }
        private int ASWMod { get; }
        private int EvasionMin { get; }
        private int EvasionMax { get; }
        private int LoSMin { get; }
        private int LoSMax { get; }

        public FitBonusCustom FitBonus { get; set; }

        public int Level
        {
            get => _level;
            set
            {
                _level = value;

                BaseASW = ScaledStat(ASWMin, ASWMax) + ASWMod;
                BaseLoS = ScaledStat(LoSMin, LoSMax);
                BaseEvasion = ScaledStat(EvasionMin, EvasionMax);
                SetBaseAccuracy();
            }
        }
        public int HP
        {
            get => _hp;
            set => _hp = value;
        }
        public int BaseArmor
        {
            get => _baseArmor;
            set => _baseArmor = value;
        }
        public int BaseEvasion
        {
            get => _baseEvasion;
            set => _baseEvasion = value;
        }
        public int[] Aircraft { get; set; }
        public int BaseSpeed
        {
            get => _baseSpeed;
            set => _baseSpeed = value;
        }
        public int BaseRange
        {
            get => _baseRange;
            set => _baseRange = value;
        }
        public double BaseAccuracy => _baseAccuracy;




        public int Condition
        {
            get => _condition;
            set => _condition = value;
        }
        public int BaseFirepower
        {
            get => _baseFirepower;
            set
            {
                _baseFirepower = value;
                SetBaseNightPower();
            } 
        }
        public int BaseTorpedo
        {
            get => _baseTorpedo;
            set
            {
                _baseTorpedo = value;
                SetBaseNightPower();
            }
        }
        public int BaseAA
        {
            get => _baseAA;
            set => _baseAA = value;
        }
        public int BaseASW
        {
            get => _baseASW;
            set => _baseASW = value;
        }
        public int BaseLoS
        {
            get => _baseLoS;
            set => _baseLoS = value;
        }
        public int BaseLuck
        {
            get => _baseLuck;
            set
            {
                _baseLuck = value;
                SetBaseAccuracy();
            }
        }
        public int BaseNightPower => _baseNightPower;
        public int Fuel { get; set; }
        public int Ammo { get; set; }

        public IEquipmentDataCustom[] Equipment
        {
            get => _equipment;
            set
            {
                _equipment = value;
                FitAccuracy = AccuracyFitBonus;
            }

        }

        public IEnumerable<DayAttackKind> DayAttacks => GetDayAttacks(); /*new []
        {
            DayAttackKind.NormalAttack,
            DayAttackKind.DoubleShelling,
            DayAttackKind.CutinMainSub,
            DayAttackKind.CutinMainRadar,
            DayAttackKind.CutinMainAP,
            DayAttackKind.CutinMainMain
        };*/

        public IEnumerable<NightAttackKind> NightAttacks => GetNightAttacks(); /*new []
        {
            NightAttackKind.NormalAttack,
            NightAttackKind.DoubleShelling,
            NightAttackKind.CutinMainSub,
            NightAttackKind.CutinMainTorpedo,
            NightAttackKind.CutinMainMain,
            NightAttackKind.CutinTorpedoTorpedo
        };*/

        public IEnumerable<DayAttackKind> AswAttacks => GetAswAttacks();

        public int Firepower => BaseFirepower + FitFirepower;
        public int Torpedo => BaseTorpedo + FitTorpedo;
        public int NightPower => Firepower + Torpedo;






        public ShipDataCustom(ShipData ship) : this(ship.MasterShip)
        {
            _ship = ship;

            ASWMod = ship.ASWModernized;

            Level = ship.Level;
            HP = ship.HPCurrent;
            BaseArmor = ship.ArmorBase;
            BaseEvasion = ship.EvasionBase;
            Aircraft = ship.Aircraft.ToArray();
            BaseSpeed = ship.Speed;
            BaseRange = ship.Range;

            Condition = ship.Condition;
            BaseFirepower = ship.FirepowerBase;
            BaseTorpedo = ship.TorpedoBase;
            BaseAA = ship.AABase;
            BaseASW = ship.ASWBase;
            BaseLoS = ship.LOSBase;
            BaseLuck = ship.LuckBase;


            Equipment = ship.AllSlotInstance
                .Select(eq => eq == null ? null : new EquipmentDataCustom(eq))
                .ToArray<IEquipmentDataCustom>();
        }

        public ShipDataCustom(ShipDataMaster ship)
        {
            _shipMaster = ship;

            ASWMin = ship.ASW.GetParameter(1);
            ASWMax = ship.ASW.GetParameter(99);

            LoSMin = ship.LOS.GetParameter(1);
            LoSMax = ship.LOS.GetParameter(99);

            EvasionMin = ship.Evasion.GetParameter(1);
            EvasionMax = ship.Evasion.GetParameter(99);
        }

        private int ScaledStat(int min, int max)
        {
            if (min == 0 || max == 9999)
                return 0;

            return min + (int) ((max - min) * _level / 99.0);
        }

        public double NightAccuracyFitBonus => 0;

        public double AccuracyFitBonus => ShipType switch
        {
            ShipTypes.Battleship => BattleshipAccuracyFitBonus,
            ShipTypes.AviationBattleship => BattleshipAccuracyFitBonus,
            ShipTypes.Battlecruiser => BattleshipAccuracyFitBonus,

            _ => 0
        };

        private double BattleshipAccuracyFitBonus => Equipment
            .Where(eq => eq != null && (eq.CategoryType == EquipmentTypes.MainGunLarge || 
                                        eq.CategoryType == EquipmentTypes.MainGunLarge2))
            .GroupBy(eq => eq.CategoryType)
            .Sum(groupedEquip =>
            {
                double bonus = groupedEquip.Sum(eq => eq.CurrentFitBonus?.Accuracy ?? 0);
                if (bonus < 0 && _ship.IsMarried)
                    bonus *= 0.6;

                return bonus / Math.Sqrt(groupedEquip.Count());
            });
        

        private List<DayAttackKind> GetDayAttacks()
        {
            if (ShipType == ShipTypes.AircraftCarrier ||
                ShipType == ShipTypes.ArmoredAircraftCarrier ||
                ShipType == ShipTypes.LightAircraftCarrier)
                return CarrierDayAttacks();

            return MainGunDayAttacks();
        }

        private IEnumerable<NightAttackKind> GetNightAttacks()
        {
            IEnumerable<NightAttackKind> nightAttacks = new List<NightAttackKind>();

            if (ShipType == ShipTypes.AircraftCarrier ||
                ShipType == ShipTypes.ArmoredAircraftCarrier ||
                ShipType == ShipTypes.LightAircraftCarrier)
                return CarrierNightAttacks();

            if (ShipType == ShipTypes.Destroyer)
                nightAttacks = nightAttacks.Concat(DestroyerNightAttacks());

            nightAttacks = nightAttacks.Concat(RegularNightAttacks());

            return nightAttacks;
        }

        private List<DayAttackKind> MainGunDayAttacks()
        {
            List<DayAttackKind> dayAttacks = new List<DayAttackKind>();

            int mainGunCount = 0;
            int secondaryGunCount = 0;
            int seaplaneCount = 0;
            int radarCount = 0;
            int apShellCount = 0;
            int zuiunCount = 0;
            int suiseiCount = 0;

            foreach (IEquipmentDataCustom equip in Equipment.Where(eq => eq != null))
            {
                switch (equip.CategoryType)
                {
                    case EquipmentTypes.MainGunSmall:
                    case EquipmentTypes.MainGunMedium:
                    case EquipmentTypes.MainGunLarge:
                    case EquipmentTypes.MainGunLarge2:
                        mainGunCount++;
                        break;

                    case EquipmentTypes.SecondaryGun:
                        secondaryGunCount++;
                        break;

                    case EquipmentTypes.APShell:
                        apShellCount++;
                        break;

                    case EquipmentTypes.RadarSmall:
                    case EquipmentTypes.RadarLarge:
                    case EquipmentTypes.RadarLarge2:
                        radarCount++;
                        break;

                    case EquipmentTypes.SeaplaneBomber:
                    case EquipmentTypes.SeaplaneRecon:
                        seaplaneCount++;
                        if(equip.IsZuiun)
                            zuiunCount++;
                        break;

                    case EquipmentTypes.CarrierBasedBomber:
                        if (equip.ID == 291 || equip.ID == 292 || equip.ID == 319) // 634 versions only
                            suiseiCount++;
                        break;
                }
            }

            if (ShipID == 553 || ShipID == 554)
            {
                /*if(suiseiCount > 1 && mainGunCount > 0)
                    dayAttacks.Add(DayAttackKind.Suisei);*/

                if (zuiunCount > 1 && mainGunCount > 0)
                    dayAttacks.Add(DayAttackKind.ZuiunMultiAngle);
            }

            if (seaplaneCount > 0)
            {
                if (apShellCount > 0 && mainGunCount > 1)
                    dayAttacks.Add(DayAttackKind.CutinMainMain);

                if (mainGunCount > 0 && apShellCount > 0 && secondaryGunCount > 0)
                    dayAttacks.Add(DayAttackKind.CutinMainAP);

                if (mainGunCount > 0 && secondaryGunCount > 0 && radarCount > 0)
                    dayAttacks.Add(DayAttackKind.CutinMainRadar);

                if (mainGunCount > 0 && secondaryGunCount > 0)
                    dayAttacks.Add(DayAttackKind.CutinMainSub);

                if (mainGunCount > 1)
                    dayAttacks.Add(DayAttackKind.DoubleShelling);
            }

            dayAttacks.Add(DayAttackKind.Shelling);

            return dayAttacks;
        }

        private List<DayAttackKind> CarrierDayAttacks()
        {
            List<DayAttackKind> dayAttacks = new List<DayAttackKind>();

            int attackerCount = 0;
            int bomberCount = 0;

            foreach (IEquipmentDataCustom equip in Equipment.Where(eq => eq != null))
            {
                switch (equip.CategoryType)
                {
                    case EquipmentTypes.CarrierBasedTorpedo:
                        attackerCount++;
                        break;

                    case EquipmentTypes.CarrierBasedBomber:
                        bomberCount++;
                        break;
                }
            }

            if(attackerCount > 0 && bomberCount > 0)
                dayAttacks.Add(DayAttackKind.CutinAirAttack);

            dayAttacks.Add(DayAttackKind.AirAttack);

            return dayAttacks;
        }

        private List<DayAttackKind> GetAswAttacks()
        {
            List<DayAttackKind> aswAttacks = new List<DayAttackKind>();

            if (!CanAttackSubmarine)
                return aswAttacks;

            if (HasAswAircraft)
                aswAttacks.Add(DayAttackKind.AirAttack);
            else
                aswAttacks.Add(DayAttackKind.DepthCharge);

            return aswAttacks;
        }

        private List<NightAttackKind> RegularNightAttacks()
        {
            List<NightAttackKind> nightAttacks = new List<NightAttackKind>();

            int mainGunCount = 0;
            int torpedoCount = 0;
            int secondaryCount = 0;

            foreach (IEquipmentDataCustom equip in Equipment.Where(eq => eq != null))
            {
                switch (equip.CategoryType)
                {
                    case EquipmentTypes.MainGunSmall:
                    case EquipmentTypes.MainGunMedium:
                    case EquipmentTypes.MainGunLarge:
                    case EquipmentTypes.MainGunLarge2:
                        mainGunCount++;
                        break;

                    case EquipmentTypes.Torpedo:
                        torpedoCount++;
                        break;

                    case EquipmentTypes.SecondaryGun:
                        secondaryCount++;
                        break;
                }
            }

            if (torpedoCount > 1)
                nightAttacks.Add(NightAttackKind.CutinTorpedoTorpedo);

            if(mainGunCount > 2)
                nightAttacks.Add(NightAttackKind.CutinMainMain);

            if(mainGunCount == 2 && secondaryCount == 1)
                nightAttacks.Add(NightAttackKind.CutinMainSub);

            if (torpedoCount > 0 && mainGunCount > 0)
                nightAttacks.Add(NightAttackKind.CutinMainTorpedo);

            if(mainGunCount+secondaryCount > 1)
                nightAttacks.Add(NightAttackKind.DoubleShelling);

            if(torpedoCount > 0)
                nightAttacks.Add(NightAttackKind.Torpedo);
            else
                nightAttacks.Add(NightAttackKind.NormalAttack);

            return nightAttacks;
        }

        private List<NightAttackKind> DestroyerNightAttacks()
        {
            List<NightAttackKind> nightAttacks = new List<NightAttackKind>();


            return nightAttacks;
        }

        private List<NightAttackKind> CarrierNightAttacks()
        {
            List<NightAttackKind> nightAttacks = new List<NightAttackKind>();

            nightAttacks.Add(NightAttackKind.CutinAirAttack);

            nightAttacks.Add(NightAttackKind.AirAttack);

            nightAttacks.Add(NightAttackKind.Shelling);

            return nightAttacks;
        }




        public string Name => _ship?.Name ?? _shipMaster.Name;
        public int SortID => _ship?.MasterShip.SortID ?? _shipMaster.SortID;

        public int ShipID => _ship?.ShipID ?? _shipMaster.ShipID;

        public int MasterID => _ship?.MasterID ?? -1;

        public ShipDataMaster MasterShip => _ship?.MasterShip ?? _shipMaster;

        public int EquipmentSlotCount => _ship?.SlotSize ?? _shipMaster.SlotSize;

        public bool IsExpansionSlotAvailable => _ship?.IsExpansionSlotAvailable ?? true;

        public string NameWithLevel => $"{MasterShip.Name} Lv. {Level}";

        public ShipClasses ShipClass => (ShipClasses)(_ship?.MasterShip.ShipClass ?? _shipMaster.ShipClass);

        public ShipTypes ShipType => _ship?.MasterShip.ShipType ?? _shipMaster.ShipType;

        public string ShipTypeName => _ship?.MasterShip.ShipTypeName ?? _shipMaster.ShipTypeName;

        public bool IsMarried => Level > 99;

        public bool IsInstallation => false;

        public bool CanAttackSubmarine => ShipType switch
        {
            ShipTypes.Escort => DepthChargeCondition,
            ShipTypes.Destroyer => DepthChargeCondition,
            ShipTypes.LightCruiser => DepthChargeCondition,
            ShipTypes.TorpedoCruiser => DepthChargeCondition,
            ShipTypes.TrainingCruiser => DepthChargeCondition,
            ShipTypes.FleetOiler => DepthChargeCondition,

            ShipTypes.AviationCruiser => HasAswAircraft,
            ShipTypes.LightAircraftCarrier => HasAswAircraft,
            ShipTypes.AviationBattleship => HasAswAircraft,
            ShipTypes.SeaplaneTender => HasAswAircraft,
            ShipTypes.AmphibiousAssaultShip => HasAswAircraft,

            _ => false
        };

        private bool DepthChargeCondition => BaseASW > 0;
        private bool HasAswAircraft => Equipment.Any(eq => eq != null && eq.IsAntiSubmarineAircraft);


        public IEnumerable<int> EquippableCategories => _ship?.MasterShip.EquippableCategories ?? _shipMaster.EquippableCategories;



        private void SetBaseAccuracy() => _baseAccuracy = 2 * Math.Sqrt(Level) + 1.5 * Math.Sqrt(BaseLuck);
        private void SetBaseNightPower() =>_baseNightPower = _baseFirepower + _baseTorpedo;

    }
}
