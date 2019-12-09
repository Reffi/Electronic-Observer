using System.Linq;

namespace ElectronicObserverTypes
{
    public interface IShellingAccuracyShip<out TEquipType> where TEquipType : IShellingAccuracyEquipment
    {
        double BaseAccuracy { get; }
        int Condition { get; }
        double AccuracyFitBonus { get; }

        TEquipType[] Equipment { get; }
    }

    public interface IShellingAccuracyEquipment
    {
        double Accuracy { get; }

        bool IsApShell { get; }
        bool IsMainGun { get; }
        bool IsSecondaryGun { get; }
        bool IsRadar { get; }
    }

    public interface IShellingAccuracyFleet
    {
        FormationType Formation { get; }
        FleetType Type { get; }
        bool IsMain { get; }
        bool IsVanguardTop { get; }
    }
}