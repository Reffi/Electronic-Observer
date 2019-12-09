using System.Linq;

namespace ElectronicObserverTypes
{
    public interface ITorpedoDamageAttacker<out TEquipType> where TEquipType: ITorpedoDamageAttackerEquipment
    {
        double Torpedo { get; }
        TEquipType[] Equipment { get; } 
    }

    public interface ITorpedoDamageAttackerEquipment
    {
        double Torpedo { get; }
    }

    public interface ITorpedoDefenderData
    {
        double Armor { get; }
    }

    public interface ITorpedoAttackerFleetData
    {
        FormationType Formation { get; }
        bool IsCombined { get; }
    }

    public interface ITorpedoDefenderFleetData
    {
    }
}