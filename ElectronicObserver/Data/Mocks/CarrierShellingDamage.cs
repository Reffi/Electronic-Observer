using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data.Damage;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.Mocks
{
    public class MockCarrierShellingDamageAttacker : ICarrierShellingDamageAttacker<MockCarrierShellingDamageEquipment>
    {
        public int Firepower { get; set; } = 0;
        public int Torpedo { get; set; } = 0;
        public MockCarrierShellingDamageEquipment[] Equipment { get; set; } = { };
    }

    public class MockCarrierShellingDamageEquipment : ICarrierShellingDamageEquipment
    {
        public double Firepower { get; set; } = 0;
        public double Torpedo { get; set; } = 0;
        public double Bombing { get; set; } = 0;
    }

    public class MockCarrierShellingDamageAttackerFleet : ICarrierShellingDamageAttackerFleet
    {
        public FormationType Formation { get; set; } = FormationType.LineAhead;
        public FleetType Type { get; set; } = FleetType.Single;

        public bool IsMain => PositionDetail == FleetPositionDetail.Main ||
                              PositionDetail == FleetPositionDetail.MainFlag;

        public bool IsVanguardTop => PositionDetail == FleetPositionDetail.VanguardTop;

        public FleetPositionDetail PositionDetail { get; set; } = FleetPositionDetail.MainFlag;
    }

    public class MockCarrierShellingDamageDefender : ICarrierShellingDamageDefender
    {
        public int BaseArmor { get; set; } = 0;
        public ShipTypes ShipType { get; set; } = ShipTypes.Destroyer;
    }

    public class MockCarrierShellingDamageDefenderFleet : ICarrierShellingDamageDefenderFleet
    {
        public FleetType Type { get; set; } = FleetType.Single;

        public bool IsMain => PositionDetail == FleetPositionDetail.Main ||
                              PositionDetail == FleetPositionDetail.MainFlag;

        public FleetPositionDetail PositionDetail { get; set; } = FleetPositionDetail.MainFlag;
    }
}