using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data.Damage;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.Mocks
{
    public class MockNightDamageAttacker : INightDamageAttacker<MockNightDamageAttackerEquipment>
    {
        public int Firepower { get; set; } = 0;
        public int Torpedo { get; set; } = 0;
        public MockNightDamageAttackerEquipment[] Equipment { get; set; } = { };
    }

    public class MockNightDamageAttackerEquipment : INightDamageAttackerEquipment
    {
        public int BaseFirepower { get; set; } = 0;
        public int BaseTorpedo { get; set; } = 0;
        public double UpgradeNightPower { get; set; } = 0;
    }

    public class MockNightDamageAttackerFleet : INightDamageAttackerFleet
    {
        public bool NightRecon { get; set; } = false;
        public FormationType Formation { get; set; } = FormationType.LineAhead;
        public bool IsVanguardTop => PositionDetail == FleetPositionDetail.VanguardTop;
        public FleetPositionDetail PositionDetail { get; set; } = FleetPositionDetail.MainFlag;
    }

    public class MockNightDamageDefender : INightDamageDefender
    {
        public int BaseArmor { get; set; } = 0;
        public bool IsInstallation { get; set; }
    }

    public class MockNightDamageDefenderFleet : INightDamageDefenderFleet
    {
    }
}
