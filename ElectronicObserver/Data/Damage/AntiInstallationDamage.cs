using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.Damage
{
    public interface IAntiInstallationAttacker<out TEquipData> where TEquipData : IAntiInstallationEquipment
    {
        ShipTypes ShipType { get; }
        TEquipData[] Equipment { get; }
    }

    public interface IAntiInstallationEquipment
    {
        int ID { get; }
        int Level { get; }
        bool IsWG { get; }
        bool IsAntiInstallationRocket { get; }
        bool IsMortar { get; }
        bool IsTokuDaihatsu { get; }
        bool IsDaihatsuTank { get; }
        bool IsTokuDaihatsuTank { get; }
        bool IsAmericanDaihatsuTank { get; }
        EquipmentTypes CategoryType { get; }
    }

    public interface IInstallation
    {
        InstallationType InstallationType { get; }
    }

    public class MockAntiInstallationAttacker : IAntiInstallationAttacker<MockAntiInstallationEquipment>
    {
        public ShipTypes ShipType { get; set; } = ShipTypes.Unknown;
        public MockAntiInstallationEquipment[] Equipment { get; set; } = { };

        public MockAntiInstallationAttacker Clone()
        {
            return (MockAntiInstallationAttacker) MemberwiseClone();
        }
    }

    public class MockAntiInstallationEquipment : IAntiInstallationEquipment
    {
        public int ID { get; set; } = 0;
        public int Level { get; set; } = 0;
        public bool IsWG { get; set; } = false;
        public bool IsAntiInstallationRocket { get; set; } = false;
        public bool IsMortar { get; set; } = false;
        public bool IsTokuDaihatsu { get; set; } = false;
        public bool IsDaihatsuTank { get; set; } = false;
        public bool IsTokuDaihatsuTank { get; set; } = false;
        public bool IsAmericanDaihatsuTank { get; set; } = false;
        public EquipmentTypes CategoryType { get; set; } = EquipmentTypes.Unknown;

        public MockAntiInstallationEquipment Clone()
        {
            return (MockAntiInstallationEquipment) MemberwiseClone();
        }
    }

    public class MockInstallation : IInstallation
    {
        public InstallationType InstallationType { get; set; } = InstallationType.None;
    }

    public class AntiInstallationDamage
    {
        private IAntiInstallationAttacker<IAntiInstallationEquipment> Attacker { get; }
        private IInstallation Defender { get; }

        public AntiInstallationDamage(IAntiInstallationAttacker<IAntiInstallationEquipment> attacker,
            IInstallation defender)
        {
            Attacker = attacker;
            Defender = defender;
        }

        public ExtraDamageBonus ShellingBonus() => Defender.InstallationType switch
        {
            InstallationType.SupplyDepot => AllInstallationBonus(Attacker)
                                            + SoftSkinBonus(Attacker)
                                            + SupplyDepotBonus(Attacker),

            InstallationType.SoftSkin => AllInstallationBonus(Attacker)
                                         + SoftSkinBonus(Attacker),

            InstallationType.ArtilleryImp => AllInstallationBonus(Attacker)
                                             + ArtilleryImpBonus(Attacker),

            InstallationType.IsolatedIsland => AllInstallationBonus(Attacker)
                                               + IsolatedIslandBonus(Attacker),

            InstallationType.HarbourSummer => AllInstallationBonus(Attacker)
                                              + HarbourSummerBonus(Attacker),

            _ => new ExtraDamageBonus()
        };

        private ExtraDamageBonus AllInstallationBonus(IAntiInstallationAttacker<IAntiInstallationEquipment> attacker)
        {
            ExtraDamageBonus bonus = new ExtraDamageBonus();

            bonus.b12 += attacker.ShipType switch
            {
                ShipTypes.Submarine => 30,
                ShipTypes.SubmarineAircraftCarrier => 30,

                _ => 0
            };

            // WG42(Wurfgerät 42)
            bonus.b13 += attacker.Equipment.Count(eq => eq?.IsWG ?? false) switch
            {
                0 => 0,
                1 => 75,
                2 => 110,
                3 => 140,
                _ => 160
            };

            // 艦載型 四式20cm対地噴進砲
            bonus.b13 += attacker.Equipment.Count(eq => eq?.ID == 348) switch
            {
                0 => 0,
                1 => 55,
                2 => 115,
                3 => 160,
                _ => 195
            };

            // 四式20cm対地噴進砲 集中配備
            bonus.b13 += attacker.Equipment.Count(eq => eq?.ID == 349) switch
            {
                0 => 0,
                _ => 80
            };

            // 二式12cm迫撃砲改
            bonus.b13 += attacker.Equipment.Count(eq => eq?.ID == 346) switch
            {
                0 => 0,
                1 => 30,
                2 => 55,
                3 => 75,
                _ => 90
            };

            // 二式12cm迫撃砲改 集中配備
            bonus.b13 += attacker.Equipment.Count(eq => eq?.ID == 347) switch
            {
                0 => 0,
                1 => 60,
                _ => 110
            };

            bonus.b13 += attacker.Equipment.Count(eq => eq?.IsTokuDaihatsuTank ?? false) switch
            {
                0 => 0,
                _ => 25
            };

            return bonus;
        }

        private ExtraDamageBonus SoftSkinBonus(IAntiInstallationAttacker<IAntiInstallationEquipment> attacker)
        {
            ExtraDamageBonus bonus = new ExtraDamageBonus();

            if (attacker.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.AAShell))
            {
                bonus.a13 *= 2.5;
            }

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.IsWG ?? false) switch
            {
                0 => 1,
                1 => 1.3,
                _ => 1.3 * 1.4
            };

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.IsAntiInstallationRocket ?? false) switch
            {
                0 => 1,
                1 => 1.25,
                _ => 1.25 * 1.5
            };

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.IsMortar ?? false) switch
            {
                0 => 1,
                1 => 1.2,
                _ => 1.2 * 1.3
            };

            if (attacker.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.LandingCraft))
            {
                bonus.a13 *= 1.4;
            }

            if (attacker.Equipment.Any(eq => eq?.IsTokuDaihatsu ?? false))
            {
                bonus.a13 *= 1.15;
            }

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.IsDaihatsuTank ?? false) switch
            {
                0 => 1,
                1 => 1.5,
                _ => 1.5 * 1.3
            };

            if (attacker.Equipment.Any(eq => eq?.IsTokuDaihatsuTank ?? false))
            {
                bonus.a13 *= 1.8;
            }

            bonus.a13 *= 1 + attacker.Equipment.Where(eq => eq?.CategoryType == EquipmentTypes.LandingCraft)
                             .DefaultIfEmpty().Average(eq => eq?.Level ?? 0) / 50;

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank) switch
            {
                0 => 1,
                1 => 1.5,
                _ => 1.5 * 1.2
            };

            bonus.a13 *= 1 + attacker.Equipment.Where(eq => eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank)
                             .DefaultIfEmpty().Average(eq => eq?.Level ?? 0) / 30;

            if (attacker.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.SeaplaneBomber ||
                                             eq?.CategoryType == EquipmentTypes.SeaplaneFighter))
            {
                bonus.a13 *= 1.2;
            }

            return bonus;
        }

        private ExtraDamageBonus SupplyDepotBonus(IAntiInstallationAttacker<IAntiInstallationEquipment> attacker)
        {
            ExtraDamageBonus bonus = new ExtraDamageBonus();

            bonus.a6 *= attacker.Equipment.Count(eq => eq?.IsWG ?? false) switch
            {
                0 => 1,
                1 => 1.25,
                _ => 1.25 * 1.3
            };

            bonus.a6 *= attacker.Equipment.Count(eq => eq?.IsAntiInstallationRocket ?? false) switch
            {
                0 => 1,
                1 => 1.2,
                _ => 1.2 * 1.4
            };

            bonus.a6 *= attacker.Equipment.Count(eq => eq?.IsMortar ?? false) switch
            {
                0 => 1,
                1 => 1.15,
                _ => 1.15 * 1.2
            };

            if (attacker.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.LandingCraft))
            {
                bonus.a6 *= 1.7;
            }

            if (attacker.Equipment.Any(eq => eq?.IsTokuDaihatsu ?? false))
            {
                bonus.a6 *= 1.2;
            }

            bonus.a6 *= attacker.Equipment.Count(eq => eq?.IsDaihatsuTank ?? false) switch
            {
                0 => 1,
                1 => 1.3,
                _ => 1.3 * 1.6
            };

            if (attacker.Equipment.Any(eq => eq?.IsAmericanDaihatsuTank ?? false))
            {
                bonus.a6 *= 1.2;
            }

            bonus.a6 *= Math.Pow(1 + attacker.Equipment.Where(eq => eq?.CategoryType == EquipmentTypes.LandingCraft)
                                     .DefaultIfEmpty().Average(eq => eq?.Level ?? 0) / 50, 2);

            bonus.a6 *= attacker.Equipment.Count(eq => eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank) switch
            {
                0 => 1,
                1 => 1.7,
                _ => 1.7 * 1.5
            };

            bonus.a6 *= 1 + attacker.Equipment.Where(eq => eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank)
                            .DefaultIfEmpty().Average(eq => eq?.Level ?? 0) / 30;

            return bonus;
        }

        private ExtraDamageBonus ArtilleryImpBonus(IAntiInstallationAttacker<IAntiInstallationEquipment> attacker)
        {
            ExtraDamageBonus bonus = new ExtraDamageBonus();

            if (attacker.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.APShell))
            {
                bonus.a13 *= 1.85;
            }

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.IsWG ?? false) switch
            {
                0 => 1,
                1 => 1.6,
                _ => 1.6 * 1.7
            };

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.IsAntiInstallationRocket ?? false) switch
            {
                0 => 1,
                1 => 1.25,
                _ => 1.25 * 1.5
            };

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.IsMortar ?? false) switch
            {
                0 => 1,
                1 => 1.5,
                _ => 1.5 * 1.8
            };

            if (attacker.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.LandingCraft))
            {
                bonus.a13 *= 1.8;
            }

            if (attacker.Equipment.Any(eq => eq?.IsTokuDaihatsu ?? false))
            {
                bonus.a13 *= 1.15;
            }

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.IsDaihatsuTank ?? false) switch
            {
                0 => 1,
                1 => 1.5,
                _ => 1.5 * 1.4
            };

            if (attacker.Equipment.Any(eq => eq?.IsTokuDaihatsuTank ?? false))
            {
                bonus.a13 *= 1.8;
            }

            bonus.a13 *= 1 + attacker.Equipment.Where(eq => eq?.CategoryType == EquipmentTypes.LandingCraft)
                             .DefaultIfEmpty().Average(eq => eq?.Level ?? 0) / 50;

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank) switch
            {
                0 => 1,
                1 => 2.4,
                _ => 2.4 * 1.35
            };

            bonus.a13 *= 1 + attacker.Equipment.Where(eq => eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank)
                             .DefaultIfEmpty().Average(eq => eq?.Level ?? 0) / 30;

            if (attacker.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedBomber ||
                                             eq?.CategoryType == EquipmentTypes.SeaplaneBomber ||
                                             eq?.CategoryType == EquipmentTypes.SeaplaneFighter))
            {
                bonus.a13 *= 1.5;
            }

            bonus.a13 *= attacker.ShipType switch
            {
                ShipTypes.Destroyer => 1.4,
                ShipTypes.LightCruiser => 1.4,

                _ => 1
            };

            return bonus;
        }

        private ExtraDamageBonus IsolatedIslandBonus(IAntiInstallationAttacker<IAntiInstallationEquipment> attacker)
        {
            ExtraDamageBonus bonus = new ExtraDamageBonus();

            if (attacker.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.AAShell))
            {
                bonus.a13 *= 1.75;
            }

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.IsWG ?? false) switch
            {
                0 => 1,
                1 => 1.4,
                _ => 1.4 * 1.5
            };

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.IsAntiInstallationRocket ?? false) switch
            {
                0 => 1,
                1 => 1.3,
                _ => 1.3 * 1.65
            };

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.IsMortar ?? false) switch
            {
                0 => 1,
                1 => 1.2,
                _ => 1.2 * 1.4
            };

            if (attacker.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.LandingCraft))
            {
                bonus.a13 *= 1.8;
            }

            if (attacker.Equipment.Any(eq => eq?.IsTokuDaihatsu ?? false))
            {
                bonus.a13 *= 1.15;
            }

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.IsDaihatsuTank ?? false) switch
            {
                0 => 1,
                1 => 1.2,
                _ => 1.2 * 1.4
            };

            if (attacker.Equipment.Any(eq => eq?.IsTokuDaihatsuTank ?? false))
            {
                bonus.a13 *= 1.8;
            }

            bonus.a13 *= 1 + attacker.Equipment.Where(eq => eq?.CategoryType == EquipmentTypes.LandingCraft)
                             .DefaultIfEmpty().Average(eq => eq?.Level ?? 0) / 50;

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank) switch
            {
                0 => 1,
                1 => 2.4,
                _ => 2.4 * 1.35
            };

            bonus.a13 *= 1 + attacker.Equipment.Where(eq => eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank)
                             .DefaultIfEmpty().Average(eq => eq?.Level ?? 0) / 30;

            if (attacker.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedBomber))
            {
                bonus.a13 *= 1.4;
            }

            return bonus;
        }

        private ExtraDamageBonus HarbourSummerBonus(IAntiInstallationAttacker<IAntiInstallationEquipment> attacker)
        {
            ExtraDamageBonus bonus = new ExtraDamageBonus();

            if (attacker.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.AAShell))
            {
                bonus.a13 *= 1.75;
            }

            if (attacker.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.APShell))
            {
                bonus.a13 *= 1.3;
            }

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.IsWG ?? false) switch
            {
                0 => 1,
                1 => 1.4,
                _ => 1.4 * 1.2
            };

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.IsAntiInstallationRocket ?? false) switch
            {
                0 => 1,
                1 => 1.25,
                _ => 1.25 * 1.4
            };

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.IsMortar ?? false) switch
            {
                0 => 1,
                1 => 1.1,
                _ => 1.1 * 1.15
            };

            if (attacker.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.LandingCraft))
            {
                bonus.a13 *= 1.7;
            }

            if (attacker.Equipment.Any(eq => eq?.IsTokuDaihatsu ?? false))
            {
                bonus.a13 *= 1.2;
            }

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.IsDaihatsuTank ?? false) switch
            {
                0 => 1,
                1 => 1.6,
                _ => 1.6 * 1.5
            };

            if (attacker.Equipment.Any(eq => eq?.IsTokuDaihatsuTank ?? false))
            {
                bonus.a13 *= 1.8;
            }

            if (attacker.Equipment.Any(eq => eq?.IsAmericanDaihatsuTank ?? false))
            {
                bonus.a13 *= 2.8;
            }

            bonus.a13 *= 1 + attacker.Equipment.Where(eq => eq?.CategoryType == EquipmentTypes.LandingCraft)
                             .DefaultIfEmpty().Average(eq => eq?.Level ?? 0) / 50;

            bonus.a13 *= attacker.Equipment.Count(eq => eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank) switch
            {
                0 => 1,
                1 => 2.8,
                _ => 2.8 * 1.5
            };

            bonus.a13 *= 1 + attacker.Equipment.Where(eq => eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank)
                             .DefaultIfEmpty().Average(eq => eq?.Level ?? 0) / 30;

            if (attacker.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedBomber ||
                                             eq?.CategoryType == EquipmentTypes.SeaplaneBomber ||
                                             eq?.CategoryType == EquipmentTypes.SeaplaneFighter))
            {
                bonus.a13 *= 1.3;
            }

            return bonus;
        }
    }
}
