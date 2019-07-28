using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{
    // single responsibility ???
    // encapsulation ???
    public class Damage
    {
        BattleDataCustom _battle;

        private IShipDataCustom _attacker;
        private IShipDataCustom _defender;

        private int _cap;

        private double _precapBase;
        private double _precapMods;
        private double _critMod;
        private double _postcapMods;
        private double _ammoMod = 1;

        private FleetDataCustom _attackerFleet;
        private FleetDataCustom _defenderFleet;
        private int _attackerFleetIndex;
        private int _defenderFleetIndex;

        private double Capped => Cap(_precapBase * _precapMods);
        private double Postcap => Math.Floor(Capped * _critMod) * _postcapMods;
        private double Min => Math.Floor((Postcap - (_defender?.BaseArmor ?? 0) * 1.3) * _ammoMod);
        private double Max => Math.Floor((Postcap - (_defender?.BaseArmor ?? 0) * 0.7) * _ammoMod);

        public double ShellingCapped
        {
            get
            {
                _cap = Constants.Softcap.DayShelling;
                _precapBase = _attacker.ShipType switch
                {
                    ShipTypes.AircraftCarrier => PrecapBaseShellingCarrier,
                    ShipTypes.ArmoredAircraftCarrier => PrecapBaseShellingCarrier,
                    ShipTypes.LightAircraftCarrier => PrecapBaseShellingCarrier,

                    _ => PrecapBaseShelling
                };
                _precapMods = PrecapModsShelling;
                _critMod = 1;
                _postcapMods = 1;

                return Capped;
            }
        }
        public double Shelling
        {
            get
            {
                _cap = Constants.Softcap.DayShelling;
                _precapBase = _attacker.ShipType switch
                {
                    ShipTypes.AircraftCarrier => PrecapBaseShellingCarrier,
                    ShipTypes.ArmoredAircraftCarrier => PrecapBaseShellingCarrier,
                    ShipTypes.LightAircraftCarrier => PrecapBaseShellingCarrier,

                    _ => PrecapBaseShelling
                };
                _precapMods = PrecapModsShelling;
                _critMod = 1;
                _postcapMods = PostcapModsShelling;

                return Postcap;
            }
        }
        public double ShellingCrit
        {
            get
            {
                _cap = Constants.Softcap.DayShelling;
                _precapBase = _attacker.ShipType switch
                    {
                    ShipTypes.AircraftCarrier => PrecapBaseShellingCarrier,
                    ShipTypes.ArmoredAircraftCarrier => PrecapBaseShellingCarrier,
                    ShipTypes.LightAircraftCarrier => PrecapBaseShellingCarrier,

                    _ => PrecapBaseShelling
                    };
                _precapMods = PrecapModsShelling;
                _critMod = CritMod;
                _postcapMods = PostcapModsShelling;

                return Postcap;
            }
        }
        public double ShellingMin
        {
            get
            {
                _cap = Constants.Softcap.DayShelling;
                _precapBase = _attacker.ShipType switch
                    {
                    ShipTypes.AircraftCarrier => PrecapBaseShellingCarrier,
                    ShipTypes.ArmoredAircraftCarrier => PrecapBaseShellingCarrier,
                    ShipTypes.LightAircraftCarrier => PrecapBaseShellingCarrier,

                    _ => PrecapBaseShelling
                    };
                _precapMods = PrecapModsShelling;
                _critMod = 1;
                _postcapMods = PostcapModsShelling;

                return Min;
            }
        }
        public double ShellingMax
        {
            get
            {
                _cap = Constants.Softcap.DayShelling;
                _precapBase = _attacker.ShipType switch
                    {
                    ShipTypes.AircraftCarrier => PrecapBaseShellingCarrier,
                    ShipTypes.ArmoredAircraftCarrier => PrecapBaseShellingCarrier,
                    ShipTypes.LightAircraftCarrier => PrecapBaseShellingCarrier,

                    _ => PrecapBaseShelling
                    };
                _precapMods = PrecapModsShelling;
                _critMod = 1;
                _postcapMods = PostcapModsShelling;

                return Max;
            }
        }
        public double ShellingCritMin
        {
            get
            {
                _cap = Constants.Softcap.DayShelling;
                _precapBase = _attacker.ShipType switch
                    {
                    ShipTypes.AircraftCarrier => PrecapBaseShellingCarrier,
                    ShipTypes.ArmoredAircraftCarrier => PrecapBaseShellingCarrier,
                    ShipTypes.LightAircraftCarrier => PrecapBaseShellingCarrier,

                    _ => PrecapBaseShelling
                    };
                _precapMods = PrecapModsShelling;
                _critMod = CritMod;
                _postcapMods = PostcapModsShelling;

                return Min;
            }
        }
        public double ShellingCritMax
        {
            get
            {
                _cap = Constants.Softcap.DayShelling;
                _precapBase = _attacker.ShipType switch
                    {
                    ShipTypes.AircraftCarrier => PrecapBaseShellingCarrier,
                    ShipTypes.ArmoredAircraftCarrier => PrecapBaseShellingCarrier,
                    ShipTypes.LightAircraftCarrier => PrecapBaseShellingCarrier,

                    _ => PrecapBaseShelling
                    };
                _precapMods = PrecapModsShelling;
                _critMod = CritMod;
                _postcapMods = PostcapModsShelling;

                return Max;
            }
        }


        // need opening and closing cause midget upgrades?
        public double TorpedoCapped
        {
            get
            {
                _cap = Constants.Softcap.Torpedo;
                _precapBase = PrecapBaseTorpedo;
                _precapMods = PrecapModsTorpedo;
                _critMod = 1;
                _postcapMods = 1;

                return Capped;
            }
        }
        public double Torpedo 
        {
            get
            {
                _cap = Constants.Softcap.Torpedo;
                _precapBase = PrecapBaseTorpedo;
                _precapMods = PrecapModsTorpedo;
                _critMod = 1;
                _postcapMods = PostcapModsTorpedo;

                return Postcap;
            }
        }
        public double TorpedoCrit
        {
            get
            {
                _cap = Constants.Softcap.Torpedo;
                _precapBase = PrecapBaseTorpedo;
                _precapMods = PrecapModsTorpedo;
                _critMod = CritMod;
                _postcapMods = PostcapModsTorpedo;

                return Postcap;
            }
        }

        public double AswCapped
        {
            get
            {
                _cap = Constants.Softcap.ASW;
                _precapBase = PrecapBaseASW;
                _precapMods = PrecapModsASW;
                _critMod = 1;
                _postcapMods = 1;

                return Capped;
            }
        }
        public double ASW
        {
            get
            {
                _cap = Constants.Softcap.ASW;
                _precapBase = PrecapBaseASW;
                _precapMods = PrecapModsASW;
                _critMod = 1;
                _postcapMods = PostcapModsASW;

                return Capped;
            }
        }
        public double AswCrit
        {
            get
            {
                _cap = Constants.Softcap.ASW;
                _precapBase = PrecapBaseASW;
                _precapMods = PrecapModsASW;
                _critMod = CritMod;
                _postcapMods = PostcapModsASW;

                return Postcap;
            }
        }
        public double AswMin
        {
            get
            {
                _cap = Constants.Softcap.ASW;
                _precapBase = PrecapBaseASW;
                _precapMods = PrecapModsASW;
                _critMod = 1;
                _postcapMods = PostcapModsASW;

                return Min;
            }
        }
        public double AswMax
        {
            get
            {
                _cap = Constants.Softcap.ASW;
                _precapBase = PrecapBaseASW;
                _precapMods = PrecapModsASW;
                _critMod = 1;
                _postcapMods = PostcapModsASW;

                return Max;
            }
        }
        public double AswCritMin
        {
            get
            {
                _cap = Constants.Softcap.ASW;
                _precapBase = PrecapBaseASW;
                _precapMods = PrecapModsASW;
                _critMod = CritMod;
                _postcapMods = PostcapModsASW;

                return Min;
            }
        }
        public double AswCritMax
        {
            get
            {
                _cap = Constants.Softcap.ASW;
                _precapBase = PrecapBaseASW;
                _precapMods = PrecapModsASW;
                _critMod = CritMod;
                _postcapMods = PostcapModsASW;

                return Max;
            }
        }


        public double NightCapped
        {
            get
            {
                _cap = Constants.Softcap.NightBattle;
                _precapBase = PrecapBaseNight;
                _precapMods = PrecapModsNight;
                _critMod = 1;
                _postcapMods = 1;

                return Capped;
            }
        }
        public double Night
        {
            get
            {
                _cap = Constants.Softcap.NightBattle;
                _precapBase = PrecapBaseNight;
                _precapMods = PrecapModsNight;
                _critMod = 1;
                _postcapMods = PostcapModsNight;

                return Postcap;
            }
        }
        public double NightCrit
        {
            get
            {
                _cap = Constants.Softcap.NightBattle;
                _precapBase = PrecapBaseNight;
                _precapMods = PrecapModsNight;
                _critMod = CritMod;
                _postcapMods = PostcapModsNight;

                return Postcap;
            }
        }
        public double NightMin
        {
            get
            {
                _cap = Constants.Softcap.NightBattle;
                _precapBase = PrecapBaseNight;
                _precapMods = PrecapModsNight;
                _critMod = 1;
                _postcapMods = PostcapModsNight;

                return Min;
            }
        }
        public double NightMax
        {
            get
            {
                _cap = Constants.Softcap.NightBattle;
                _precapBase = PrecapBaseNight;
                _precapMods = PrecapModsNight;
                _critMod = 1;
                _postcapMods = PostcapModsNight;

                return Max;
            }
        }
        public double NightCritMin
        {
            get
            {
                _cap = Constants.Softcap.NightBattle;
                _precapBase = PrecapBaseNight;
                _precapMods = PrecapModsNight;
                _critMod = CritMod;
                _postcapMods = PostcapModsNight;

                return Min;
            }
        }
        public double NightCritMax
        {
            get
            {
                _cap = Constants.Softcap.NightBattle;
                _precapBase = PrecapBaseNight;
                _precapMods = PrecapModsNight;
                _critMod = CritMod;
                _postcapMods = PostcapModsNight;

                return Max;
            }
        }

        public Damage(IShipDataCustom attacker, IShipDataCustom defender = null, BattleDataCustom battle = null, 
            FleetDataCustom attackerFleet = null, FleetDataCustom defenderFleet = null)
        {
            _attackerFleetIndex = battle?.AttackerFleetIndex ?? 1;
            _defenderFleetIndex = battle?.DefenderFleetIndex ?? 1;

            _attackerFleet = attackerFleet ?? new FleetDataCustom();
            _defenderFleet = defenderFleet ?? new FleetDataCustom();

            _battle = battle ?? new BattleDataCustom(_attackerFleet, _defenderFleet);

            _attacker = attacker;
            _defender = defender;

            _ammoMod = 1;
        }









        private double PrecapBaseShelling =>
            _attacker.Firepower
            + _attacker.Equipment.Where(eq => eq != null).Sum(eq => eq.Firepower)
            + CombinedFleetShellingDamageBonus + 5;

        private double PrecapBaseShellingCarrier =>
            55 + 1.5 * (_attacker.Firepower 
                        + _attacker.Torpedo
                        + _attacker.Equipment.Where(eq => eq != null)
                            .Sum(eq => eq.Firepower + eq.BaseTorpedo + Math.Floor(1.3 * eq.Bombing))
                        + CombinedFleetShellingDamageBonus);

        private double PrecapModsShelling =>
            _attackerFleet.ShellingDamageMod(_attacker)
            * _battle.EngagementMod;

        private double PostcapModsShelling =>
            DayAttackKindMod
            * ApDamageMod;



        private double PrecapBaseTorpedo =>
            _attacker.Torpedo
            + _attacker.Equipment.Where(eq => eq != null).Sum(eq => eq.Torpedo)
            + (_attackerFleet.IsCombinedFleet ? 0 : 5);

        private double PrecapModsTorpedo =>
            _attackerFleet.TorpedoDamageMod(_attacker)
            * _battle.EngagementMod;

        private double PostcapModsTorpedo => 1;




        private double PrecapBaseASW =>
            (2 * Math.Sqrt(_attacker.BaseASW)
             + 1.5 * _attacker.Equipment.Where(eq => eq?.CountsForAswDamage ?? false).Sum(eq => eq.ASW)
             + AswTypeConstant)
            * AswDamageMod;

        private double PrecapModsASW =>
            _attackerFleet.AswDamageMod(_attacker)
            * _battle.EngagementMod;

        private double PostcapModsASW => 1;




        private double PrecapBaseNight =>
            _defender?.IsInstallation ?? false ? _attacker.Firepower : _attacker.NightPower
            + _attacker.Equipment.Where(eq => eq != null).Sum(eq => eq.NightPower)
            + (_attackerFleet.NightRecon ? 5 : 0);

        private double PrecapModsNight =>
            _attackerFleet.NightDamageMod(_attacker)
            * NightAttackKindMod;

        private double PostcapModsNight => 1;












        private double CritMod => 1.5;

        private int CombinedFleetShellingDamageBonus =>
            (_attackerFleet.Type, _attackerFleetIndex, _defenderFleet.Type, _defenderFleetIndex) switch
            {
                (FleetType.Single, _, _, _) => 0,

                // against abyssal single fleet
                (FleetType.Carrier, 1, FleetType.Single, 1) => 2,
                (FleetType.Carrier, 2, FleetType.Single, 1) => 10,
                (FleetType.Surface, 1, FleetType.Single, 1) => 10,
                (FleetType.Surface, 2, FleetType.Single, 1) => -5,
                (FleetType.Transport, 1, FleetType.Single, 1) => -5,
                (FleetType.Transport, 2, FleetType.Single, 1) => 10,

                // against abyssal combined fleet
                (FleetType.Transport, 1, _, _) => -5,
                (FleetType.Transport, 2, _, 2) => 10,
                (_, 1, _, 1) => 2,
                (_, 1, _, 2) => -5,
                (_, 2, _, 2) => 10,

                _ => 0
            };

        private double DayAttackKindMod =>
            _battle.DayAttack switch
            {
                DayAttackKind.DoubleShelling => 1.2,
                DayAttackKind.CutinMainRadar => 1.2,
                DayAttackKind.CutinMainSub => 1.1,
                DayAttackKind.CutinMainAP => 1.3,
                DayAttackKind.CutinMainMain => 1.5,
                _ => 1
            };

        private double NightAttackKindMod =>
            _battle.NightAttack switch
            {
                NightAttackKind.DoubleShelling => 1.2,
                NightAttackKind.CutinMainSub => 1.75,
                NightAttackKind.CutinMainMain => 2,
                NightAttackKind.CutinMainTorpedo => 1.3,
                NightAttackKind.CutinTorpedoTorpedo => 1.5,
                _ => 1
            };

        private double ApDamageMod => CalculateApDamageMod();
        private double CalculateApDamageMod()
        {
            if (_defender == null)
                return 1;

            bool ap = false;
            bool main = false;
            bool sub = false;
            bool radar = false;

            foreach (IEquipmentDataCustom eq in _attacker.Equipment.Where(e => e != null))
            {
                switch (eq.CategoryType)
                {
                    case EquipmentTypes.APShell:
                        ap = true;
                        break;

                    case EquipmentTypes.MainGunSmall:
                    case EquipmentTypes.MainGunMedium:
                    case EquipmentTypes.MainGunLarge:
                    case EquipmentTypes.MainGunLarge2:
                        main = true;
                        break;

                    case EquipmentTypes.SecondaryGun:
                        sub = true;
                        break;

                    case EquipmentTypes.RadarLarge:
                    case EquipmentTypes.RadarLarge2:
                    case EquipmentTypes.RadarSmall:
                        radar = true;
                        break;
                }
            }

            if (!ap || !main)
                return 1;

            if (sub)
                return 1.15;

            if (radar)
                return 1.1;
            
            return 1.08;

        }

        private int AswTypeConstant => CalculateAswTypeConstant();
        private int CalculateAswTypeConstant()
        {
            IEnumerable<DayAttackKind> aswAttacks = _attacker.AswAttacks.ToList();

            // this shouldn't happen
            if (!aswAttacks.Any())
                return 0;

            if (aswAttacks.First() == DayAttackKind.AirAttack)
                return 8;

            return 13;
        }

        private double AswDamageMod => CalculateAswDamageMod();
        private double CalculateAswDamageMod()
        {
            bool depthChargeProjectorSpecial = false;
            bool depthChargeProjector = false;
            bool sonar = false;
            bool depthCharge = false;

            foreach (IEquipmentDataCustom eq in _attacker.Equipment.Where(eq => eq != null))
            {
                switch (eq.CategoryType)
                {
                    case EquipmentTypes.Sonar:
                        sonar = true;
                        break;

                    case EquipmentTypes.DepthCharge when eq.IsDepthCharge:
                        depthCharge = true;
                        break;

                    case EquipmentTypes.DepthCharge when eq.IsSpecialDepthChargeProjector:
                        depthChargeProjectorSpecial = true;
                        break;

                    case EquipmentTypes.DepthCharge:
                        depthChargeProjector = true;
                        break;
                }
            }

            double sonar_dcp = sonar && (depthChargeProjector || depthChargeProjectorSpecial) ? 1.15 : 1;
            double sonar_dc = sonar && depthCharge ? 0.15 : 0;
            double dcp_dc = depthChargeProjector && depthCharge ? 0.1 : 0;

            return sonar_dcp * (1 + sonar_dc + dcp_dc);
        }

        private double Cap(double damage)
        {
            if (damage > _cap)
                damage = _cap + Math.Sqrt(damage - _cap);

            return damage;
        }
    }
}
