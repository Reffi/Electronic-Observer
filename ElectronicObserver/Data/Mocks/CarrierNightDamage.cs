using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data.Damage;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.Mocks
{
    public class MockCarrierNightDamageAttacker : ICarrierNightDamageAttacker<MockCarrierNightDamageEquipment>
    {
        public int BaseFirepower { get; set; } = 0;
        public int[] Aircraft { get; set; } = { };
        public MockCarrierNightDamageEquipment[] Equipment { get; set; } = { };

        public MockCarrierNightDamageAttacker Clone()
        {
            return (MockCarrierNightDamageAttacker) MemberwiseClone();
        }
    }

    public class MockCarrierNightDamageEquipment : ICarrierNightDamageEquipment
    {
        public int BaseFirepower { get; set; } = 0;
        public int BaseTorpedo { get; set; } = 0;
        public int BaseASW { get; set; } = 0;
        public int BaseBombing { get; set; } = 0;
        public double UpgradeNightPower { get; set; } = 0;
        public bool IsNightAircraft { get; set; } = false;
        public bool IsNightCapableAircraft { get; set; } = false;
    }

    public class MockCarrierNightDamageAttackerFleet : ICarrierNightDamageAttackerFleet
    {
        public FormationType Formation { get; set; } = FormationType.LineAhead;
        public FleetType Type { get; set; } = FleetType.Single;

        public bool IsMain => PositionDetail == FleetPositionDetail.Main ||
                              PositionDetail == FleetPositionDetail.MainFlag;

        public bool IsVanguardTop => PositionDetail == FleetPositionDetail.VanguardTop;

        public FleetPositionDetail PositionDetail { get; set; } = FleetPositionDetail.MainFlag;
    }

    public class MockCarrierNightDamageDefender : ICarrierNightDamageDefender
    {
        public int BaseArmor { get; set; } = 0;
        public ShipTypes ShipType { get; set; } = ShipTypes.Destroyer;
    }

    public class MockCarrierNightDamageDefenderFleet : ICarrierNightDamageDefenderFleet
    {
        public FleetType Type { get; set; } = FleetType.Single;

        public bool IsMain => PositionDetail == FleetPositionDetail.Main ||
                              PositionDetail == FleetPositionDetail.MainFlag;

        public FleetPositionDetail PositionDetail { get; set; } = FleetPositionDetail.MainFlag;
    }
}