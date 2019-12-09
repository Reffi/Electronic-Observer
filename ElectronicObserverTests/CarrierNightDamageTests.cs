using System;
using ElectronicObserver.Data.Damage;
using ElectronicObserver.Data.Mocks;
using ElectronicObserver.Utility.Data;
using Xunit;

namespace ElectronicObserverTests
{
    public class CarrierNightDamageTests
    {
        // https://wikiwiki.jp/kancolle/%E5%A4%9C%E6%88%A6#l127e796

        private MockCarrierNightDamageAttacker Saratoga = new MockCarrierNightDamageAttacker
        {
            BaseFirepower = 68,
            Aircraft = new[] {32, 24, 18, 6}
        };

        private MockCarrierNightDamageEquipment TBM3D = new MockCarrierNightDamageEquipment
        {
            BaseFirepower = 2,
            BaseTorpedo = 9,
            BaseASW = 8,
            IsNightAircraft = true
        };

        private MockCarrierNightDamageEquipment F6F3N = new MockCarrierNightDamageEquipment
        {
            BaseASW = 4,
            IsNightAircraft = true
        };

        private MockCarrierNightDamageEquipment SwordfishMk2 = new MockCarrierNightDamageEquipment
        {
            BaseFirepower = 3,
            BaseTorpedo = 5,
            BaseASW = 6,
            IsNightCapableAircraft = true
        };

        private MockCarrierNightDamageEquipment SwordfishMk3 = new MockCarrierNightDamageEquipment
        {
            BaseFirepower = 4,
            BaseTorpedo = 8,
            BaseASW = 10,
            IsNightCapableAircraft = true
        };

        private MockCarrierNightDamageEquipment Iwai = new MockCarrierNightDamageEquipment
        {
            BaseASW = 3,
            BaseBombing = 4,
            UpgradeNightPower = Math.Sqrt(10),
            IsNightCapableAircraft = true
        };

        [Fact]
        public void CarrierNightDamageTest01()
        {
            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {TBM3D, SwordfishMk2, SwordfishMk2};

            CarrierNightDamage night = new CarrierNightDamage(attacker);

            Assert.Equal(277.8, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest02()
        {
            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {TBM3D, SwordfishMk2};

            CarrierNightDamage night = new CarrierNightDamage(attacker);

            Assert.Equal(251.9, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest03()
        {
            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {F6F3N, SwordfishMk2};

            CarrierNightDamage night = new CarrierNightDamage(attacker);

            Assert.Equal(202.8, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest04()
        {
            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {TBM3D};

            CarrierNightDamage night = new CarrierNightDamage(attacker);

            Assert.Equal(223.4, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest05()
        {
            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {F6F3N};

            CarrierNightDamage night = new CarrierNightDamage(attacker);

            Assert.Equal(174.2, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest06()
        {
            MockNightBattle battle = new MockNightBattle
            {
                NightAttack = NightAttackKind.CutinAirAttack,
                CvnciKind = CvnciKind.Other
            };

            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {F6F3N, SwordfishMk3, Iwai};

            CarrierNightDamage night = new CarrierNightDamage(attacker, battle: battle);

            Assert.Equal(272.1, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest07()
        {
            MockNightBattle battle = new MockNightBattle
            {
                NightAttack = NightAttackKind.CutinAirAttack,
                CvnciKind = CvnciKind.Other
            };

            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {F6F3N, SwordfishMk2, Iwai};

            CarrierNightDamage night = new CarrierNightDamage(attacker, battle: battle);

            Assert.Equal(253.5, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest08()
        {
            MockNightBattle battle = new MockNightBattle
            {
                NightAttack = NightAttackKind.CutinAirAttack,
                CvnciKind = CvnciKind.Other
            };

            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {F6F3N, SwordfishMk2, SwordfishMk3};

            CarrierNightDamage night = new CarrierNightDamage(attacker, battle: battle);

            Assert.Equal(286.5, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest09()
        {
            MockNightBattle battle = new MockNightBattle
            {
                NightAttack = NightAttackKind.CutinAirAttack,
                CvnciKind = CvnciKind.Other
            };

            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {F6F3N, SwordfishMk2, SwordfishMk2};

            CarrierNightDamage night = new CarrierNightDamage(attacker, battle: battle);

            Assert.Equal(269.7, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest10()
        {
            MockNightBattle battle = new MockNightBattle
            {
                NightAttack = NightAttackKind.CutinAirAttack,
                CvnciKind = CvnciKind.Other
            };

            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {F6F3N, TBM3D, Iwai};

            CarrierNightDamage night = new CarrierNightDamage(attacker, battle: battle);

            Assert.Equal(308.2, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest11()
        {
            MockNightBattle battle = new MockNightBattle
            {
                NightAttack = NightAttackKind.CutinAirAttack,
                CvnciKind = CvnciKind.Other
            };

            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {F6F3N, TBM3D, SwordfishMk3};

            CarrierNightDamage night = new CarrierNightDamage(attacker, battle: battle);

            Assert.Equal(310.0, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest12()
        {
            MockNightBattle battle = new MockNightBattle
            {
                NightAttack = NightAttackKind.CutinAirAttack,
                CvnciKind = CvnciKind.Other
            };

            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {F6F3N, TBM3D, SwordfishMk2};

            CarrierNightDamage night = new CarrierNightDamage(attacker, battle: battle);

            Assert.Equal(309.1, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest13()
        {
            MockNightBattle battle = new MockNightBattle
            {
                NightAttack = NightAttackKind.CutinAirAttack,
                CvnciKind = CvnciKind.Other
            };

            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {F6F3N, F6F3N, Iwai};

            CarrierNightDamage night = new CarrierNightDamage(attacker, battle: battle);

            Assert.Equal(303.9, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest14()
        {
            MockNightBattle battle = new MockNightBattle
            {
                NightAttack = NightAttackKind.CutinAirAttack,
                CvnciKind = CvnciKind.Other
            };

            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {F6F3N, F6F3N, SwordfishMk3};

            CarrierNightDamage night = new CarrierNightDamage(attacker, battle: battle);

            Assert.Equal(306.9, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest15()
        {
            MockNightBattle battle = new MockNightBattle
            {
                NightAttack = NightAttackKind.CutinAirAttack,
                CvnciKind = CvnciKind.Other
            };

            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {F6F3N, F6F3N, SwordfishMk2};

            CarrierNightDamage night = new CarrierNightDamage(attacker, battle: battle);

            Assert.Equal(305.6, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest16()
        {
            MockNightBattle battle = new MockNightBattle
            {
                NightAttack = NightAttackKind.CutinAirAttack,
                CvnciKind = CvnciKind.Other
            };

            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {F6F3N, F6F3N, F6F3N};

            CarrierNightDamage night = new CarrierNightDamage(attacker, battle: battle);

            Assert.Equal(308.6, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest17()
        {
            MockNightBattle battle = new MockNightBattle
            {
                NightAttack = NightAttackKind.CutinAirAttack,
                CvnciKind = CvnciKind.Pair
            };

            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {F6F3N, TBM3D};

            CarrierNightDamage night = new CarrierNightDamage(attacker, battle: battle);

            Assert.Equal(307.7, night.Capped, 1);
        }

        [Fact]
        public void CarrierNightDamageTest18()
        {
            MockNightBattle battle = new MockNightBattle
            {
                NightAttack = NightAttackKind.CutinAirAttack,
                CvnciKind = CvnciKind.FFA
            };

            MockCarrierNightDamageAttacker attacker = Saratoga.Clone();
            attacker.Equipment = new[] {F6F3N, F6F3N, TBM3D};

            CarrierNightDamage night = new CarrierNightDamage(attacker, battle: battle);

            Assert.Equal(312.1, night.Capped, 1);
        }
    }
}