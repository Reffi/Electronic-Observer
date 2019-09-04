using ElectronicObserver.Data.Damage;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.Mocks
{
    public class MockAswDamageAttacker : IAswDamageAttacker<IAswDamageAttackerEquipment>
    {
        public int BaseASW { get; set; } = 0;
        public IAswDamageAttackerEquipment[] Equipment { get; set; } = { };
    }

    public class MockAswDamageAttackerEquipment : IAswDamageAttackerEquipment
    {
        public double ASW { get; set; } = 0;
        public bool CountsForAswDamage { get; set; } = false;
        public bool IsSonar { get; set; } = false;
        public bool IsSmallSonar { get; set; } = false;
        public bool IsDepthCharge { get; set; } = false;
        public bool IsDepthChargeProjector { get; set; } = false;
        public bool IsSpecialDepthChargeProjector { get; set; } = false;
    }

    public class MockAswDamageAttackerFleet : IAswDamageAttackerFleet
    {
        public FormationType Formation { get; set; } = FormationType.LineAhead;
        public bool IsVanguardTop => PositionDetail == FleetPositionDetail.VanguardTop;
        public FleetPositionDetail PositionDetail { get; set; } = FleetPositionDetail.MainFlag;
    }

    public class MockAswDamageDefender : IAswDamageDefender
    {
        public int BaseArmor { get; set; } = 0;
    }

    public class MockAswDamageDefenderFleet : IAswDamageDefenderFleet
    {

    }
}
