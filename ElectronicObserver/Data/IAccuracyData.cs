using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data
{
    public interface IAccuracyData : IEquipmentAccuracyData, IFleetAccuracyData, IBattleAccuracyData
    {
        double BaseAccuracy { get; }
        double AccuracyFitBonus { get; }
        double NightAccuracyFitBonus { get; }

        int Condition { get; }

        int ApShellCount { get; }
        int MainGunCount { get; }
        int SecondaryGunCount { get; }
        int RadarCount { get; }
    }

    public interface IEquipmentAccuracyData
    {
        double EquipmentAccuracy { get; }
        double EquipmentAswAccuracy { get; }
    }

    public interface IFleetAccuracyData
    {
        FormationType FleetFormation { get; }
        FleetType FleetType { get; }
        FleetPositionDetail FleetPositionDetail { get; }

        bool NightRecon { get; }
        bool Flare { get; }
        bool Searchlight { get; }
    }

    public interface IBattleAccuracyData
    {
        DayAttackKind DayAttack { get; }
        NightAttackKind NightAttack { get; }
    }
}