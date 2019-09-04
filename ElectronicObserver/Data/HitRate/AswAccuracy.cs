using System.Linq;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.HitRate
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

    public class AswAccuracy : IAccuracy
    {
        private IAswAccuracyShip<IAswAccuracyEquipment> Ship { get; }
        private IAswAccuracyFleet Fleet { get; }
        private IDayBattle Battle { get; }

        public AswAccuracy(IAswAccuracyShip<IAswAccuracyEquipment> ship, IAswAccuracyFleet fleet = null,
            IDayBattle battle = null)
        {
            Ship = ship;
            Fleet = fleet;

            Battle = battle;
        }

        public double Total =>
            (Base
             + Ship.BaseAccuracy
             + Ship.Equipment.Where(eq => eq != null).Sum(eq => eq.AswAccuracy))
            * ConditionMod
            * FleetMod;

        private double ConditionMod => Ship.Condition switch
        {
            int condition when condition > 52 => 1.2,
            int condition when condition > 32 => 1,
            int condition when condition > 22 => 0.8,
            _ => 0.5
        };


        private int Base => 80;

        private double FleetMod => Fleet.Formation switch
        {
            FormationType.LineAhead => 1,
            FormationType.Diamond => 1,

            FormationType.DoubleLine => 1.2,
            FormationType.Echelon => 1.2,
            FormationType.LineAbreast => 1.2,

            _ => 1
        };
    }
}
