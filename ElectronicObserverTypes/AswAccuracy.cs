using System.Linq;

namespace ElectronicObserverTypes
{
    public interface IAswAccuracyShip<out TEquipType> where TEquipType : IAswAccuracyEquipment
    {
        double BaseAccuracy { get; }
        int Condition { get; }
        TEquipType[] Equipment { get; }
    }

    public interface IAswAccuracyEquipment
    {
        double AswAccuracy { get; }
    }

    public interface IAswAccuracyFleet
    {
        FormationType Formation { get; }
    }
}
