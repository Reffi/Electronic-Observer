using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserverTypes;
using Xunit;

namespace ElectronicObserverTests
{
    public class FitBonusTests
    {
        ShipDataCustom IseK2 = new ShipDataCustom
        {
            ShipID = ShipID.IseKaiNi,
            ShipClass = ShipClasses.Ise
        };

        private EquipmentDataCustom gun41tripleK2_1 = new EquipmentDataCustom
        {
            EquipID = EquipID.LargeCalibreMainGun_41cmTripleGunKai2
        };

        private EquipmentDataCustom gun41tripleK2_2 = new EquipmentDataCustom
        {
            EquipID = EquipID.LargeCalibreMainGun_41cmTripleGunKai2
        };

        [Fact(DisplayName = "Fit bonus gets applied")]
        public void FitBonusTest1()
        {
            ShipDataCustom ship = IseK2.Clone();

            ship.Equipment = new[] {gun41tripleK2_1};

            Assert.True(IseK2.CurrentFitBonus == new VisibleFits(firepower: 3, aa: 2, evasion: 1));
        }

        [Fact(DisplayName = "Modified fit bonus doesn't get overridden")]
        public void FitBonusTest2()
        {
            ShipDataCustom ship = IseK2.Clone();
            EquipmentDataCustom modified41 = gun41tripleK2_1.Clone();

            ship.Equipment = new[] {modified41};

            Assert.True(IseK2.CurrentFitBonus == new VisibleFits(firepower: 3, aa: 2, evasion: 1));

            modified41.CurrentFitBonus = new FitBonusCustom(new VisibleFits(firepower: 100));

            ship.Equipment = new[] {modified41};

            Assert.True(IseK2.CurrentFitBonus == new VisibleFits(firepower: 100));

        }

        [Fact(DisplayName = "Modified fits stack normally")]
        public void FitBonusTest3()
        {
            ShipDataCustom ship = IseK2.Clone();
            EquipmentDataCustom modified41 = gun41tripleK2_1.Clone();

            ship.Equipment = new[] { modified41 };

            Assert.True(IseK2.CurrentFitBonus == new VisibleFits(firepower: 3, aa: 2, evasion: 1));

            modified41.CurrentFitBonus = new FitBonusCustom(new VisibleFits(firepower: 100));

            ship.Equipment = new[] { modified41 };

            Assert.True(IseK2.CurrentFitBonus == new VisibleFits(firepower: 100));

            ship.Equipment = new[] { modified41, gun41tripleK2_2 };

            Assert.True(IseK2.CurrentFitBonus == new VisibleFits(firepower: 103, aa: 2, evasion: 1));

        }
    }
}
