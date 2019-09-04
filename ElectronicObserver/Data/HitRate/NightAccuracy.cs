using System.Linq;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.HitRate
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

    public class NightAccuracy : IAccuracy
    {
        private INightAccuracyShip<INightAccuracyEquipment> Ship { get; }
        private INightAccuracyFleet Fleet { get; }
        private INightBattle Battle { get; }

        public NightAccuracy(INightAccuracyShip<INightAccuracyEquipment> ship, INightAccuracyFleet fleet = null,
            INightBattle battle = null)
        {
            Ship = ship;
            Fleet = fleet;

            Battle = battle;
        }

        public double Total =>
            ((Fleet.NightRecon ? 1.1 : 1)
             * (Base + (Fleet.Flare ? 5 : 0))
             + Ship.BaseAccuracy
             + Ship.Equipment.Where(eq => eq != null).Sum(eq => eq.Accuracy))
            * FleetMod
            * ConditionMod
            * AttackKindMod
            + (Fleet.Searchlight ? 7 : 0)
            + Ship.NightAccuracyFitBonus;

        private double ConditionMod => Ship.Condition switch
        {
            int condition when condition > 52 => 1.2,
            int condition when condition > 32 => 1,
            int condition when condition > 22 => 0.8,
            _ => 0.5
        };

        private double AttackKindMod => Battle.NightAttack switch
        {
            NightAttackKind.CutinMainMain => 2,

            NightAttackKind.CutinTorpedoTorpedo => 1.65,

            NightAttackKind.CutinMainTorpedo => 1.5,
            NightAttackKind.CutinMainSub => 1.5,

            NightAttackKind.DoubleShelling => 1.1,

            NightAttackKind.NormalAttack => 1,

            _ => 1
        };

        private int Base => 69;

        private double FleetMod => Fleet.Formation switch
        {
            FormationType.LineAhead => 1,

            FormationType.DoubleLine => 0.9,

            FormationType.Echelon => 0.8,
            FormationType.LineAbreast => 0.8,

            FormationType.Diamond => 0.7,

            _ => 1
        };
    }
}
