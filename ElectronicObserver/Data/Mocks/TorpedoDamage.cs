using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data.Damage;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.Mocks
{
    public class MockTorpedoDamageAttacker : ITorpedoDamageAttacker<MockTorpedoDamageAttackerEquipment>
    {
        public double Torpedo { get; set; } = 0;
        public MockTorpedoDamageAttackerEquipment[] Equipment { get; set; } = { };
    }

    public class MockTorpedoDamageAttackerEquipment : ITorpedoDamageAttackerEquipment
    {
        public double Torpedo { get; set; } = 0;
    }

    public class MockTorpedoAttackerFleetData : ITorpedoAttackerFleetData
    {
        public FormationType Formation { get; set; } = FormationType.LineAhead;

        public bool IsCombined => Type == FleetType.Carrier ||
                                  Type == FleetType.Surface ||
                                  Type == FleetType.Transport;
        public FleetType Type { get; set; } = FleetType.Single;
    }

    public class MockTorpedoDefenderData : ITorpedoDefenderData
    {
        public double Armor { get; set; } = 0;
    }

    public class MockTorpedoDefenderFleetData : ITorpedoDefenderFleetData
    {
    }
}
