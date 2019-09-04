using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data.Damage;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.Mocks
{
    public class MockShellingDamageAttacker : IShellingDamageAttacker<MockShellingDamageAttackerEquipment>
    {
        public int Firepower { get; set; } = 0;
        public MockShellingDamageAttackerEquipment[] Equipment { get; set; } = { };
    }

    public class MockShellingDamageAttackerEquipment : IShellingDamageAttackerEquipment
    {
        public double Firepower { get; set; } = 0;
        public bool IsApShell { get; set; } = false;
        public bool IsMainGun { get; set; } = false;
        public bool IsSecondaryGun { get; set; } = false;
        public bool IsRadar { get; set; } = false;
    }

    public class MockShellingDamageAttackerFleet : IShellingDamageAttackerFleet
    {
        public FormationType Formation { get; set; } = FormationType.LineAhead;
        public FleetType Type { get; set; } = FleetType.Single;
        public bool IsMain => PositionDetail == FleetPositionDetail.Main ||
                              PositionDetail == FleetPositionDetail.MainFlag;
        public bool IsVanguardTop => PositionDetail == FleetPositionDetail.VanguardTop;
        public FleetPositionDetail PositionDetail { get; set; } = FleetPositionDetail.MainFlag;
    }

    public class MockShellingDamageDefender : IShellingDamageDefender
    {
        public int BaseArmor { get; set; } = 0;
        public ShipTypes ShipType { get; set; } = ShipTypes.Destroyer;
    }

    public class MockShellingDamageDefenderFleet : IShellingDamageDefenderFleet
    {
        public FleetType Type { get; set; } = FleetType.Single;
        public bool IsMain => PositionDetail == FleetPositionDetail.Main ||
                              PositionDetail == FleetPositionDetail.MainFlag;
        public FleetPositionDetail PositionDetail { get; set; } = FleetPositionDetail.MainFlag;
    }
}
