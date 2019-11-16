using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data.Damage;
using ElectronicObserver.Data.HitRate;
using ElectronicObserver.Utility.Helpers;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Data
{
    /// <summary>
    ///
    /// Base = ships own stat, min + modernization <br/>
    /// no prefix = Base + fit + synergy
    /// 
    /// </summary>
    public class ShipDataCustom : IShellingDamageAttacker<EquipmentDataCustom>, IAswDamageAttacker<EquipmentDataCustom>,
        INightDamageAttacker<EquipmentDataCustom>, ICarrierShellingDamageAttacker<EquipmentDataCustom>,
        ICarrierNightDamageAttacker<EquipmentDataCustom>, IShellingDamageDefender, IAswDamageDefender,
        INightDamageDefender, ICarrierShellingDamageDefender, ICarrierNightDamageDefender,
        IShellingAccuracyShip<EquipmentDataCustom>, IAswAccuracyShip<EquipmentDataCustom>,
        INightAccuracyShip<EquipmentDataCustom>, INightEvasionShip<EquipmentDataCustom>,
        IHitRateDefender, IAntiInstallationAttacker<EquipmentDataCustom>, IInstallation
    {
        private int _level;

        private int _baseFirepower;
        private int _baseTorpedo;
        private int _baseLuck;


        private ShipData _ship;
        private EquipmentDataCustom[] _equipment;

        private VisibleFits CurrentFitBonus => Equipment.Where(eq => eq != null)
            .Sum(eq => eq.CurrentFitBonus.VisibleFit);

        public int FitFirepower => Equipment.Where(eq => eq != null).Sum(eq => eq.CurrentFitBonus?.Firepower ?? 0);
        public int FitTorpedo => Equipment.Where(eq => eq != null).Sum(eq => eq.CurrentFitBonus?.Torpedo ?? 0);
        public double FitAccuracy { get; set; }



        public int SynergyFirepower => Synergies.Firepower;
        public int SynergyTorpedo => Synergies.Torpedo;
        public int SynergyAA => Synergies.AA;
        public int SynergyArmor => Synergies.Armor;
        public int SynergyASW => Synergies.ASW;
        public int SynergyEvasion => Synergies.Evasion;
        public int SynergyLoS => Synergies.LoS;



        private int ASWMin { get; }
        private int ASWMax { get; }
        private int ASWMod { get; }
        private int EvasionMin { get; }
        private int EvasionMax { get; }
        private int LoSMin { get; }
        private int LoSMax { get; }

        public FitBonusCustom Synergies => new FitBonusCustom(this);

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

        public int HP { get; set; }

        public int BaseArmor { get; set; }

        public int BaseEvasion { get; set; }

        public int[] Aircraft { get; set; } = {0, 0, 0, 0, 0, 0};

        public int BaseSpeed { get; set; }

        public int BaseRange { get; set; }

        public double BaseAccuracy { get; private set; }


        public int Condition { get; set; }

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

        public int BaseAA { get; set; }

        public int BaseASW { get; set; }

        public int BaseLoS { get; set; }

        public int BaseLuck
        {
            get => _baseLuck;
            set
            {
                _baseLuck = value;
                SetBaseAccuracy();
            }
        }

        public int BaseNightPower { get; private set; }

        public int Fuel { get; set; } = 100;
        public int Ammo { get; set; } = 100;

        public EquipmentDataCustom[] Equipment
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

        public int Firepower => BaseFirepower + FitFirepower + SynergyFirepower;

        public int Torpedo => BaseTorpedo + FitTorpedo + SynergyTorpedo;



        public int EquipmentBaseFirepower => Equipment.Where(eq => eq != null).Sum(eq => eq.BaseFirepower);
        public int EquipmentBaseTorpedo => Equipment.Where(eq => eq != null).Sum(eq => eq.BaseTorpedo);
        public int EquipmentBaseAA => Equipment.Where(eq => eq != null).Sum(eq => eq.BaseAA);
        public int EquipmentBaseArmor => Equipment.Where(eq => eq != null).Sum(eq => eq.BaseArmor);
        public int EquipmentBaseASW => Equipment.Where(eq => eq != null).Sum(eq => eq.BaseASW);
        public int EquipmentBaseEvasion => Equipment.Where(eq => eq != null).Sum(eq => eq.BaseEvasion);
        public int EquipmentBaseLoS => Equipment.Where(eq => eq != null).Sum(eq => eq.BaseLoS);



        public double EquipmentEvasion => Equipment.Where(eq => eq != null).Sum(eq => eq.Accuracy);



        public int MainGunCount => Equipment.Where(eq => eq != null).Count(eq => eq.IsMainGun);
        public int SecondaryGunCount => Equipment.Where(eq => eq != null).Count(eq => eq.IsSecondaryGun);
        public int RadarCount => Equipment.Where(eq => eq != null).Count(eq => eq.IsRadar);
        public int ApShellCount => Equipment.Where(eq => eq != null).Count(eq => eq.IsApShell);


        public ShipDataCustom() { }

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
                .ToArray();

            foreach (EquipmentDataCustom equip in Equipment.Where(eq => eq != null))
            {
                equip.CurrentFitBonus = new FitBonusCustom(this, equip);
            }

            VisibleFits originalShipStats = new VisibleFits(
                firepower: ship.FirepowerTotal,
                torpedo: ship.TorpedoTotal,
                aa: ship.AATotal,
                asw: ship.ASWTotal,
                evasion: ship.EvasionTotal,
                armor: ship.ArmorTotal,
                los: ship.LOSTotal);

            VisibleFits shipStats = new VisibleFits(
                firepower: BaseFirepower,
                torpedo: BaseTorpedo,
                aa: BaseAA,
                asw: BaseASW,
                evasion: BaseEvasion,
                armor: BaseArmor,
                los: BaseLoS);

            VisibleFits equipStats = new VisibleFits(
                firepower: EquipmentBaseFirepower,
                torpedo: EquipmentBaseTorpedo,
                aa: EquipmentBaseAA,
                asw: EquipmentBaseASW,
                evasion: EquipmentBaseEvasion,
                armor: EquipmentBaseArmor,
                los: EquipmentBaseLoS);

            // Synergies = new FitBonusCustom(originalShipStats - shipStats - equipStats - CurrentFitBonus);
            // Synergies = new FitBonusCustom(this);
        }

        public ShipDataCustom(ShipDataMaster ship)
        {
            MasterShip = ship;

            ASWMin = ship.ASW.GetParameter(1);
            ASWMax = ship.ASW.GetParameter(99);

            LoSMin = ship.LOS.GetParameter(1);
            LoSMax = ship.LOS.GetParameter(99);

            EvasionMin = ship.Evasion.GetParameter(1);
            EvasionMax = ship.Evasion.GetParameter(99);

            Level = ship.IsSubmarine && ship.IsAbyssalShip ? 50 : 1;
            HP = ship.HPMax;
            BaseArmor = ship.ArmorMax;
            BaseEvasion = ScaledStat(EvasionMin, EvasionMax);
            Aircraft = ship.Aircraft.ToArray();
            BaseSpeed = ship.Speed;
            BaseRange = ship.Range;

            Condition = 49;
            BaseFirepower = ship.FirepowerMax;
            BaseTorpedo = ship.TorpedoMax;
            BaseAA = ship.AAMax;
            BaseASW = ScaledStat(ASWMin, ASWMax);
            BaseLoS = ScaledStat(LoSMin, LoSMax);
            BaseLuck = ship.LuckMin;


            List<EquipmentDataCustom> equip = ship.DefaultSlot?
                                                  .Select(id => id == -1
                                                      ? null
                                                      : new EquipmentDataCustom(
                                                          KCDatabase.Instance.MasterEquipments.Values.First(eq =>
                                                              eq.ID == id)))
                                                  .ToList() ?? new List<EquipmentDataCustom>();

            while (equip.Count < 6)
            {
                equip.Add(null);
            }

            Equipment = equip.ToArray();

            // Synergies = new FitBonusCustom(new VisibleFits());

            InstallationType = GetInstallationType(ship);
        }


        private int ScaledStat(int min, int max)
        {
            if (min == 0 || max == 9999)
                return 0;

            return min + (int) ((max - min) * _level / 99.0);
        }

        private InstallationType GetInstallationType(ShipDataMaster ship) => ship.ShipID switch
        {
            // 飛行場姫
            1556 => InstallationType.SoftSkin,
            1631 => InstallationType.SoftSkin,
            1632 => InstallationType.SoftSkin,
            1633 => InstallationType.SoftSkin,
            1650 => InstallationType.SoftSkin,
            1651 => InstallationType.SoftSkin,
            1652 => InstallationType.SoftSkin,
            1889 => InstallationType.SoftSkin,
            1890 => InstallationType.SoftSkin,
            1891 => InstallationType.SoftSkin,
            1892 => InstallationType.SoftSkin,
            1893 => InstallationType.SoftSkin,
            1894 => InstallationType.SoftSkin,

            // 港湾棲姫
            1573 => InstallationType.SoftSkin,
            1613 => InstallationType.SoftSkin,

            // 北方棲姫
            1581 => InstallationType.SoftSkin,
            1582 => InstallationType.SoftSkin,
            1587 => InstallationType.SoftSkin,
            1588 => InstallationType.SoftSkin,
            1589 => InstallationType.SoftSkin,
            1590 => InstallationType.SoftSkin,

            // 中間棲姫
            1583 => InstallationType.SoftSkin,
            1584 => InstallationType.SoftSkin,

            // 港湾水鬼
            1605 => InstallationType.SoftSkin,
            1606 => InstallationType.SoftSkin,
            1607 => InstallationType.SoftSkin,
            1608 => InstallationType.SoftSkin,

            // 泊地水鬼
            1609 => InstallationType.SoftSkin,
            1610 => InstallationType.SoftSkin,
            1611 => InstallationType.SoftSkin,
            1612 => InstallationType.SoftSkin,

            // リコリス棲姫
            1679 => InstallationType.SoftSkin,
            1680 => InstallationType.SoftSkin,
            1681 => InstallationType.SoftSkin,
            1682 => InstallationType.SoftSkin,
            1683 => InstallationType.SoftSkin,

            // 離島棲鬼 - todo verify this
            1574 => InstallationType.IsolatedIsland,
            1634 => InstallationType.IsolatedIsland,
            1635 => InstallationType.IsolatedIsland,
            1636 => InstallationType.IsolatedIsland,

            // 離島棲姫
            1668 => InstallationType.IsolatedIsland,
            1669 => InstallationType.IsolatedIsland,
            1670 => InstallationType.IsolatedIsland,
            1671 => InstallationType.IsolatedIsland,

            // 集積地棲姫
            1653 => InstallationType.SupplyDepot,
            1654 => InstallationType.SupplyDepot,
            1655 => InstallationType.SupplyDepot,
            1656 => InstallationType.SupplyDepot,
            1657 => InstallationType.SupplyDepot,
            1658 => InstallationType.SupplyDepot,

            // 集積地夏姫
            1753 => InstallationType.SupplyDepot,
            1754 => InstallationType.SupplyDepot,

            // 集積地棲姫 バカンスmode
            1809 => InstallationType.SupplyDepot,
            1810 => InstallationType.SupplyDepot,
            1811 => InstallationType.SupplyDepot,
            1812 => InstallationType.SupplyDepot,
            1813 => InstallationType.SupplyDepot,
            1814 => InstallationType.SupplyDepot,

            // 中枢棲姫
            1684 => InstallationType.Central,
            1685 => InstallationType.Central,
            1686 => InstallationType.Central,
            1687 => InstallationType.Central,
            1688 => InstallationType.Central,
            1689 => InstallationType.Central,

            // 港湾夏姫
            1699 => InstallationType.HarbourSummer,
            1700 => InstallationType.HarbourSummer,
            1701 => InstallationType.HarbourSummer,
            1702 => InstallationType.HarbourSummer,
            1703 => InstallationType.HarbourSummer,
            1704 => InstallationType.HarbourSummer,

            // 北端上陸姫
            1725 => InstallationType.NorthernmostLanding,
            1726 => InstallationType.NorthernmostLanding,
            1727 => InstallationType.NorthernmostLanding,

            // 泊地水鬼 バカンスmode
            1815 => InstallationType.AnchorageVacation,
            1816 => InstallationType.AnchorageVacation,
            1817 => InstallationType.AnchorageVacation,
            1818 => InstallationType.AnchorageVacation,
            1819 => InstallationType.AnchorageVacation,
            1820 => InstallationType.AnchorageVacation,

            _ => InstallationType.None
        };

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

            if (ShipType == ShipTypes.Destroyer)
                nightAttacks = nightAttacks.Concat(DestroyerNightAttacks());

            if (ShipType == ShipTypes.AircraftCarrier ||
                ShipType == ShipTypes.ArmoredAircraftCarrier ||
                ShipType == ShipTypes.LightAircraftCarrier)
                nightAttacks = nightAttacks.Concat(CarrierNightAttacks());

            nightAttacks = nightAttacks.Concat(SpecialNightAttacks());
            nightAttacks = nightAttacks.Concat(NormalNightAttacks());

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

            foreach (EquipmentDataCustom equip in Equipment.Where(eq => eq != null))
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
                        if (equip.IsZuiun)
                            zuiunCount++;
                        break;

                    case EquipmentTypes.CarrierBasedBomber:
                        if (equip.ID == 291 || equip.ID == 292 || equip.ID == 319) // 634 versions only
                            suiseiCount++;
                        break;
                }
            }

            if (ShipID == ShipID.IseKaiNi || ShipID == ShipID.HyuugaKaiNi)
            {
                if(suiseiCount > 1 && mainGunCount > 0)
                    dayAttacks.Add(DayAttackKind.SeaAirMultiAngle);

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

            /*int attackerCount = 0;
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

            if (attackerCount > 0 && bomberCount > 0)
                dayAttacks.Add(DayAttackKind.CutinAirAttack);*/

            dayAttacks.Add(DayAttackKind.AirAttack);

            return dayAttacks;
        }

        public List<DayAirAttackCutinKind> Cvcis()
        {
            List<DayAirAttackCutinKind> cvcis = new List<DayAirAttackCutinKind>();

            int attackerCount = 0;
            int bomberCount = 0;
            int fighterCount = 0;

            foreach (EquipmentDataCustom equip in Equipment.Where(eq => eq != null))
            {
                switch (equip.CategoryType)
                {
                    case EquipmentTypes.CarrierBasedTorpedo:
                        attackerCount++;
                        break;

                    case EquipmentTypes.CarrierBasedBomber:
                        bomberCount++;
                        break;

                    case EquipmentTypes.CarrierBasedFighter:
                        fighterCount++;
                        break;
                }
            }

            if (fighterCount > 0 && attackerCount > 0 && bomberCount > 0)
            {
                cvcis.Add(DayAirAttackCutinKind.FighterBomberAttacker);
            }

            if (attackerCount > 0 && bomberCount > 1)
            {
                cvcis.Add(DayAirAttackCutinKind.BomberBomberAttacker);
            }

            if (attackerCount > 0 && bomberCount > 0)
            {
                cvcis.Add(DayAirAttackCutinKind.BomberAttacker);
            }

            return cvcis;
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

        private List<NightAttackKind> SpecialNightAttacks()
        {
            List<NightAttackKind> nightAttacks = new List<NightAttackKind>();

            int mainGunCount = 0;
            int torpedoCount = 0;
            int secondaryCount = 0;

            foreach (EquipmentDataCustom equip in Equipment.Where(eq => eq != null))
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

            if (mainGunCount > 2)
                nightAttacks.Add(NightAttackKind.CutinMainMain);

            if (mainGunCount == 2 && secondaryCount == 1)
                nightAttacks.Add(NightAttackKind.CutinMainSub);

            if (torpedoCount > 0 && mainGunCount > 0)
                nightAttacks.Add(NightAttackKind.CutinMainTorpedo);

            if (mainGunCount + secondaryCount > 1)
                nightAttacks.Add(NightAttackKind.DoubleShelling);

            return nightAttacks;
        }

        private List<NightAttackKind> DestroyerNightAttacks()
        {
            List<NightAttackKind> nightAttacks = new List<NightAttackKind>();

            if (ShipType != ShipTypes.Destroyer)
                return nightAttacks;

            int mainGunCount = 0;
            int torpedoCount = 0;
            int radarCount = 0;
            int skilledLookoutsCount = 0;

            foreach (EquipmentDataCustom equip in Equipment.Where(eq => eq != null))
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

                    case EquipmentTypes.RadarSmall:
                    case EquipmentTypes.RadarLarge:
                    case EquipmentTypes.RadarLarge2:
                        radarCount++;
                        break;

                    case EquipmentTypes.SurfaceShipPersonnel:
                        skilledLookoutsCount++;
                        break;
                }
            }

            if (torpedoCount > 0 && skilledLookoutsCount > 0 && radarCount > 0)
                nightAttacks.Add(NightAttackKind.CutinTorpedoPicket);

            if (mainGunCount > 0 && torpedoCount > 0 && radarCount > 0)
                nightAttacks.Add(NightAttackKind.CutinTorpedoRadar);

            return nightAttacks;
        }

        private List<NightAttackKind> CarrierNightAttacks()
        {
            List<NightAttackKind> nightAttacks = new List<NightAttackKind>();

            /*if (HasNightPersonel)
                nightAttacks.Add(NightAttackKind.CutinAirAttack);*/

            if (HasNightPersonnel || (IsArkRoyal && HasSwordfish))
            {
                nightAttacks.Add(NightAttackKind.AirAttack);
            }

            return nightAttacks;
        }

        private List<NightAttackKind> NormalNightAttacks()
        {
            List<NightAttackKind> attacks = new List<NightAttackKind>();

            if (Equipment.Where(eq => eq != null).TakeWhile(equip => !equip.IsGun).Any(equip => equip.IsTorpedo))
            {
                attacks.Add(NightAttackKind.Torpedo);
            }

            attacks.Add(NightAttackKind.Shelling);

            return attacks;
        }

        public List<CvnciKind> Cvncis()
        {
            List<CvnciKind> cvncis = new List<CvnciKind>();

            if (!HasNightPersonnel)
                return cvncis;

            int nightCapableBomber = 0;
            int nightCapableAttacker = 0;
            int nightFighter = 0;
            int nightBomber = 0;
            int nightAttacker = 0;

            foreach (EquipmentDataCustom equip in Equipment.Where(eq => eq != null))
            {
                switch (equip.CategoryType)
                {
                    case EquipmentTypes.CarrierBasedFighter when equip.IsNightFighter:
                        nightFighter++;
                        break;

                    case EquipmentTypes.CarrierBasedBomber when equip.IsNightCapableBomber:
                        nightCapableBomber++;
                        break;

                    case EquipmentTypes.CarrierBasedBomber when equip.IsNightBomber:
                        nightBomber++;
                        break;

                    case EquipmentTypes.CarrierBasedTorpedo when equip.IsNightCapableAttacker:
                        nightCapableAttacker++;
                        break;

                    case EquipmentTypes.CarrierBasedTorpedo when equip.IsNightAttacker:
                        nightAttacker++;
                        break;
                }
            }

            if (nightFighter > 1 && nightAttacker > 0)
                cvncis.Add(CvnciKind.FFA);

            // there are fancier ways to do this but lets keep it simple
            if (nightFighter > 0 && nightBomber > 0 ||
                nightFighter > 0 && nightAttacker > 0 ||
                nightBomber > 0 && nightAttacker > 0)
                cvncis.Add(CvnciKind.Pair);

            if (nightFighter > 0 &&
                nightFighter + nightAttacker + nightBomber + nightCapableBomber + nightCapableAttacker > 2)
                cvncis.Add(CvnciKind.Other);

            return cvncis;
        }




        public string Name => MasterShip?.Name ?? "";
        public int SortID => MasterShip?.SortID ?? 0;
        [Obsolete("use ShipID")]
        public int ID => MasterShip?.ShipID ?? 0;
        public ShipID ShipID => (ShipID) ID;

        public ShipID BaseShipID()
        {
            ShipDataMaster ship = KCDatabase.Instance.MasterShips[ID];

            while (ship.RemodelBeforeShipID != 0)
            {
                ship = ship.RemodelBeforeShip;
            }

            return (ShipID)ship.ShipID;
        }

        // DropID
        public int MasterID => _ship?.MasterID ?? -1;

        public ShipDataMaster MasterShip { get; }

        public int EquipmentSlotCount => _ship?.SlotSize ?? MasterShip?.SlotSize ?? 0;

        public bool IsExpansionSlotAvailable => _ship?.IsExpansionSlotAvailable ?? true;

        public string NameWithLevel => $"{MasterShip.Name} Lv. {Level}";

        public ShipClasses ShipClass => (ShipClasses) (MasterShip?.ShipClass ?? 0);

        public ShipTypes ShipType => MasterShip?.ShipType ?? ShipTypes.Destroyer;

        public InstallationType InstallationType { get; }

        // todo localize enum
        public string ShipTypeName => MasterShip.ShipTypeName;

        public bool IsMarried => Level > 99;

        public bool IsInstallation => MasterShip?.IsLandBase ?? false;

        public bool IsCarrier => MasterShip?.IsAircraftCarrier ?? false;

        public bool IsSubmarine => MasterShip?.IsSubmarine ?? false;

        public bool IsAbyssal => MasterShip?.IsAbyssalShip ?? false;

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

        private bool HasAswAircraft => Equipment.Where(eq => eq != null).Any(eq => eq.IsAntiSubmarineAircraft);

        private bool HasNightPersonnel => Equipment.Where(eq => eq != null)
                                             .Any(eq => eq.IsNightAviationPersonnel) ||
                                         ShipID == ShipID.SaratogaMkII ||
                                         ShipID == ShipID.AkagiKaiNiE;

        private bool IsArkRoyal => BaseShipID() == ShipID.ArkRoyal;

        private bool HasSwordfish => Equipment.Where(eq => eq != null).Any(eq => eq.IsSwordfish);

        public bool HasSurfaceRadar => Equipment.Where(eq => eq != null).Any(eq => eq.IsSurfaceRadar);
        public bool HasAirRadar => Equipment.Where(eq => eq != null).Any(eq => eq.IsAirRadar);

        public IEnumerable<int> EquippableCategories => MasterShip.EquippableCategories;



        private void SetBaseAccuracy() => BaseAccuracy = 2 * Math.Sqrt(Level) + 1.5 * Math.Sqrt(BaseLuck);
        private void SetBaseNightPower() => BaseNightPower = _baseFirepower + _baseTorpedo;

    }
}
