using ElectronicObserver.Data;
using ElectronicObserver.Data.Damage;
using ElectronicObserver.Utility.Data;
using Xunit;

namespace ElectronicObserverTests
{
    public class AntiInstallationDamageTests
    {
        // http://nga.178.com/read.php?tid=16936146

        private int precision = 4;

        private MockAntiInstallationAttacker DD = new MockAntiInstallationAttacker
        {
            ShipType = ShipTypes.Destroyer
        };

        private MockAntiInstallationAttacker SS = new MockAntiInstallationAttacker
        {
            ShipType = ShipTypes.Submarine
        };

        private MockAntiInstallationEquipment spb = new MockAntiInstallationEquipment
        {
            CategoryType = EquipmentTypes.SeaplaneBomber
        };

        private MockAntiInstallationEquipment spf = new MockAntiInstallationEquipment
        {
            CategoryType = EquipmentTypes.SeaplaneFighter
        };

        private MockAntiInstallationEquipment WG = new MockAntiInstallationEquipment
        {
            CategoryType = EquipmentTypes.Rocket,
            IsWG = true
        };

        private MockAntiInstallationEquipment daihatsu = new MockAntiInstallationEquipment
        {
            CategoryType = EquipmentTypes.LandingCraft
        };

        private MockAntiInstallationEquipment daihatsuTank = new MockAntiInstallationEquipment
        {
            CategoryType = EquipmentTypes.LandingCraft,
            IsDaihatsuTank = true
        };

        private MockAntiInstallationEquipment tokuDaihatsuTank = new MockAntiInstallationEquipment
        {
            CategoryType = EquipmentTypes.LandingCraft,
            IsTokuDaihatsuTank = true
        };

        private MockAntiInstallationEquipment t2tank = new MockAntiInstallationEquipment
        {
            CategoryType = EquipmentTypes.SpecialAmphibiousTank
        };

        private MockInstallation Wanko = new MockInstallation
        {
            InstallationType = InstallationType.SoftSkin
        };

        private MockInstallation DJ = new MockInstallation
        {
            InstallationType = InstallationType.SupplyDepot
        };

        [Fact]
        public void AntiInstallationDamageTest1()
        {
            MockAntiInstallationAttacker attacker = new MockAntiInstallationAttacker
            {
                Equipment = new[] {daihatsu, daihatsu}
            };

            AntiInstallationDamage damage = new AntiInstallationDamage(attacker, Wanko);

            Assert.Equal(1.4, damage.ShellingBonus().a13, precision);
        }

        [Fact]
        public void AntiInstallationDamageTest2()
        {
            MockAntiInstallationAttacker attacker = SS.Clone();
            attacker.Equipment = new[] { t2tank, WG, WG };

            AntiInstallationDamage damage = new AntiInstallationDamage(attacker, Wanko);

            Assert.Equal(2.73, damage.ShellingBonus().a13, precision);
            Assert.Equal(30, damage.ShellingBonus().b12, precision);
            Assert.Equal(110, damage.ShellingBonus().b13, precision);
        }

        [Fact]
        public void AntiInstallationDamageTest3()
        {
            MockAntiInstallationAttacker attacker = DD.Clone();
            attacker.Equipment = new[] { t2tank, WG, WG };

            AntiInstallationDamage damage = new AntiInstallationDamage(attacker, Wanko);

            Assert.Equal(2.73, damage.ShellingBonus().a13, precision);
            Assert.Equal(0, damage.ShellingBonus().b12, precision);
            Assert.Equal(110, damage.ShellingBonus().b13, precision);
        }

        [Fact]
        public void AntiInstallationDamageTest4()
        {
            MockAntiInstallationEquipment t2tankLvl2 = t2tank.Clone();
            t2tankLvl2.Level = 2;

            MockAntiInstallationEquipment t2tankLvl10 = t2tank.Clone();
            t2tankLvl10.Level = 10;

            MockAntiInstallationAttacker attacker = new MockAntiInstallationAttacker
            {
                Equipment = new[] { spb, spf, t2tankLvl2, t2tankLvl10 }
            };

            AntiInstallationDamage damage = new AntiInstallationDamage(attacker, Wanko);

            Assert.Equal(2.592, damage.ShellingBonus().a13, precision);
        }

        [Fact]
        public void AntiInstallationDamageTest5()
        {
            MockAntiInstallationEquipment daihatsuTankMax = daihatsuTank.Clone();
            daihatsuTankMax.Level = 10;

            MockAntiInstallationAttacker attacker = new MockAntiInstallationAttacker
            {
                Equipment = new[] { daihatsuTankMax, daihatsuTankMax, tokuDaihatsuTank }
            };

            AntiInstallationDamage damage = new AntiInstallationDamage(attacker, Wanko);

            Assert.Equal(5.5692, damage.ShellingBonus().a13, precision);
            Assert.Equal(25, damage.ShellingBonus().b13, precision);
        }

        [Fact]
        public void AntiInstallationDamageTest6()
        {
            MockAntiInstallationEquipment daihatsuTankMax = daihatsuTank.Clone();
            daihatsuTankMax.Level = 10;

            MockAntiInstallationAttacker attacker = new MockAntiInstallationAttacker
            {
                Equipment = new[] { daihatsuTankMax, daihatsuTankMax, tokuDaihatsuTank }
            };

            AntiInstallationDamage damage = new AntiInstallationDamage(attacker, DJ);

            Assert.Equal(5.5692, damage.ShellingBonus().a13, precision);
            Assert.Equal(25, damage.ShellingBonus().b13, precision);
            Assert.Equal(4.5418, damage.ShellingBonus().a6, precision);
        }
    }
}
