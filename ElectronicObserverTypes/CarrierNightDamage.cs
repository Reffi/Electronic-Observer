using System;
using System.Linq;

namespace ElectronicObserverTypes
{
    public interface ICarrierNightDamageAttacker<out TEquipType> where TEquipType : ICarrierNightDamageEquipment
    {
        int BaseFirepower { get; }
        int[] Aircraft { get; }
        TEquipType[] Equipment { get; }
    }

    public interface ICarrierNightDamageEquipment
    {
        int BaseFirepower { get; }
        int BaseTorpedo { get; }
        int BaseASW { get; }
        int BaseBombing { get; }
        double UpgradeNightPower { get; }

        bool IsNightAircraft { get; }
        bool IsNightCapableAircraft { get; }
    }

    public interface ICarrierNightDamageDefender
    {
        int BaseArmor { get; }
        ShipTypes ShipType { get; }
    }

    public interface ICarrierNightDamageAttackerFleet
    {
        FormationType Formation { get; }
        FleetType Type { get; }
        bool IsMain { get; }
        bool IsVanguardTop { get; }
    }

    public interface ICarrierNightDamageDefenderFleet
    {
        FleetType Type { get; }
        bool IsMain { get; }
    }
}