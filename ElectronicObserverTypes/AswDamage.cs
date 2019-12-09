using System;
using System.Linq;

namespace ElectronicObserverTypes
{
    public interface IAswDamageAttacker<out TEquipType> where TEquipType : IAswDamageAttackerEquipment
    {
        int BaseASW { get; }
        TEquipType[] Equipment { get; }
    }

    public interface IAswDamageAttackerEquipment
    {
        double ASW { get; }

        bool CountsForAswDamage { get; }
        bool IsSonar { get; }
        bool IsSmallSonar { get; }
        bool IsDepthCharge { get; }
        bool IsDepthChargeProjector { get; }
        bool IsSpecialDepthChargeProjector { get; }
    }

    public interface IAswDamageAttackerFleet
    {
        FormationType Formation { get; }
        bool IsVanguardTop { get; }
    }

    public interface IAswDamageDefender
    {
        int BaseArmor { get; }
    }

    public interface IAswDamageDefenderFleet
    {

    }
}