using System;
using System.Linq;

namespace ElectronicObserverTypes
{
    public interface ICarrierShellingDamageAttacker<out TEquipType> where TEquipType : ICarrierShellingDamageEquipment
    {
        int Firepower { get; }
        int Torpedo { get; }
        TEquipType[] Equipment { get; }
    }

    public interface ICarrierShellingDamageEquipment
    {
        double Firepower { get; }
        double Torpedo { get; }
        double Bombing { get; }
    }

    public interface ICarrierShellingDamageDefender
    {
        int BaseArmor { get; }
        ShipTypes ShipType { get; }
    }

    public interface ICarrierShellingDamageAttackerFleet
    {
        FormationType Formation { get; }
        FleetType Type { get; }
        bool IsMain { get; }
        bool IsVanguardTop { get; }
    }

    public interface ICarrierShellingDamageDefenderFleet
    {
        FleetType Type { get; }
        bool IsMain { get; }
    }
}