using System.Linq;

namespace ElectronicObserverTypes
{
    public interface INightAccuracyShip<out TEquipType> where TEquipType : INightAccuracyEquipment
    {
        double BaseAccuracy { get; }
        int Condition { get; }
        double NightAccuracyFitBonus { get; }

        TEquipType[] Equipment { get; }
    }

    public interface INightAccuracyEquipment
    {
        double Accuracy { get; }
    }

    public interface INightAccuracyFleet
    {
        FormationType Formation { get; }
        bool NightRecon { get; }
        bool Flare { get; }
        bool Searchlight { get; }
    }
}
