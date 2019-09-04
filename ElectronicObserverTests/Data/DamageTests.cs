using System;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Damage;
using ElectronicObserver.Data.Mocks;
using ElectronicObserver.Utility.Data;
using Xunit;

namespace ElectronicObserverTests.Data
{
    public class DamageTests
    {
        [Fact]
        public void ShellingDamageTest1()
        {
            // Kamikaze no gear
            MockShellingDamageAttacker attacker = new MockShellingDamageAttacker
            {
                Firepower = 41
            };

            MockShellingDamageDefender defender = new MockShellingDamageDefender
            {
                BaseArmor = 10
            };

            ShellingDamage shelling = new ShellingDamage(attacker, defender: defender);

            Assert.Equal(46, shelling.Capped);
            Assert.Equal(46, shelling.Postcap);

            Assert.Equal(33, Math.Floor(shelling.Min));
            Assert.Equal(39, Math.Floor(shelling.Max));
        }

        [Fact]
        public void ShellingDamageTest2()
        {
            MockDayBattle battle = new MockDayBattle
            {
                DayAttack = DayAttackKind.CutinMainMain
            };

            // Haruna 2*46kai max, Yura plane skilled, T1AP shell max
            MockShellingDamageAttacker attacker = new MockShellingDamageAttacker
            {
                Firepower = 96,
                Equipment = new[]
                {
                    new MockShellingDamageAttackerEquipment {Firepower = 27 + 1.5 * Math.Sqrt(10), IsMainGun = true},
                    new MockShellingDamageAttackerEquipment {Firepower = 27 + 1.5 * Math.Sqrt(10), IsMainGun = true},
                    new MockShellingDamageAttackerEquipment {Firepower = 9 + Math.Sqrt(10), IsApShell = true},
                    new MockShellingDamageAttackerEquipment {Firepower = 2}
                }
            };

            ShellingDamage shelling = new ShellingDamage(attacker, battle: battle);

            Assert.Equal(267, shelling.Postcap);
        }

        [Fact]
        public void ShellingDamageCritTest1()
        {
            MockDayBattle battle = new MockDayBattle
            {
                HitType = HitType.Critical
            };

            // Kamikaze no gear
            MockShellingDamageAttacker attacker = new MockShellingDamageAttacker
            {
                Firepower = 41
            };

            MockShellingDamageDefender defender = new MockShellingDamageDefender
            {
                BaseArmor = 10
            };

            ShellingDamage shelling = new ShellingDamage(attacker, battle: battle, defender: defender);

            Assert.Equal(46, shelling.Capped);
            Assert.Equal(69, shelling.Postcap);

            Assert.Equal(56, Math.Floor(shelling.Min));
            Assert.Equal(62, Math.Floor(shelling.Max));
        }

        [Fact]
        public void TorpedoDamageTest1()
        {
            // Yuu 2*後期型艦首魚雷(6門)
            MockTorpedoDamageAttacker attacker = new MockTorpedoDamageAttacker
            {
                Torpedo = 64,
                Equipment = new[]
                {
                    new MockTorpedoDamageAttackerEquipment{Torpedo = 15},
                    new MockTorpedoDamageAttackerEquipment{Torpedo = 15}
                }
            };

            TorpedoDamage torpedo = new TorpedoDamage(attacker);

            Assert.Equal(99, torpedo.Postcap);
        }

        [Fact]
        public void AswDamageTest1()
        {
            // Kamikaze no gear
            MockAswDamageAttacker attacker = new MockAswDamageAttacker
            {
                BaseASW = 114
            };

            MockAswDamageAttackerFleet attackerFleet = new MockAswDamageAttackerFleet
            {
                Formation = FormationType.ThirdPatrolFormation
            };

            AswDamage asw = new AswDamage(attacker, attackerFleet);

            Assert.Equal(34, asw.Postcap);
        }

        [Fact]
        public void NightDamageTest1()
        {
            MockNightBattle battle = new MockNightBattle
            {
                NightAttack = NightAttackKind.CutinTorpedoTorpedo,
                TorpedoCutinKind = NightTorpedoCutinKind.LateModelTorpedo2
            };

            // Yuu 2*後期型艦首魚雷(6門)
            MockNightDamageAttacker attacker = new MockNightDamageAttacker
            {
                Firepower = 12,
                Torpedo = 64,
                Equipment = new []
                {
                    new MockNightDamageAttackerEquipment{BaseTorpedo = 15},
                    new MockNightDamageAttackerEquipment{BaseTorpedo = 15}
                }
            };

            NightDamage night = new NightDamage(attacker, battle: battle);

            Assert.Equal(169, night.Postcap);
        }
    }
}