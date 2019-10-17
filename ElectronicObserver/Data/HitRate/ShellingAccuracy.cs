using System.Linq;
using ElectronicObserver.Data.Mocks;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.HitRate
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

    class ShellingAccuracy : IAccuracy
    {
        private bool IgnoreFit = false;

        private IShellingAccuracyShip<IShellingAccuracyEquipment> Ship { get; }
        private IShellingAccuracyFleet Fleet { get; }
        private IDayBattle Battle { get; }

        public ShellingAccuracy(IShellingAccuracyShip<IShellingAccuracyEquipment> ship,
            IShellingAccuracyFleet fleet = null,
            IDayBattle battle = null)
        {
            Ship = ship;
            Fleet = fleet ?? new MockShellingAccuracyFleet();

            Battle = battle ?? new MockDayBattle();
        }

        public double Total =>
            ((Base
              + Ship.BaseAccuracy
              + Ship.Equipment.Where(eq => eq != null).Sum(eq => eq.Accuracy))
             * FleetMod
             * ConditionMod
             + (IgnoreFit ? 0 : Ship.AccuracyFitBonus))
            * AttackKindMod
            * ApMod;

        private double ConditionMod => Ship.Condition switch
        {
            int condition when condition > 52 => 1.2,
            int condition when condition > 32 => 1,
            int condition when condition > 22 => 0.8,
            _ => 0.5
        };

        private double ApMod => CalculateApAccuracyMod();

        private double CalculateApAccuracyMod()
        {
            bool ap = Ship.Equipment.Where(eq => eq != null).Any(eq => eq.IsApShell);
            bool main = Ship.Equipment.Where(eq => eq != null).Any(eq => eq.IsMainGun);
            bool sub = Ship.Equipment.Where(eq => eq != null).Any(eq => eq.IsSecondaryGun);
            bool radar = Ship.Equipment.Where(eq => eq != null).Any(eq => eq.IsRadar);

            return (ap, main, sub, radar) switch
            {
                (true, true, false, false) => 1.1,
                (true, true, false, true) => 1.25,

                (true, true, true, false) => 1.2,
                (true, true, true, true) => 1.3,

                _ => 1
            };
        }

        private int Base => (Fleet.Type, Fleet.IsMain) switch
        {
            (FleetType.Surface, true) => 46,
            (FleetType.Surface, false) => 70,

            (FleetType.Carrier, true) => 78,
            (FleetType.Carrier, false) => 43,

            (FleetType.Transport, true) => 51,
            (FleetType.Transport, false) => 45,

            _ => 90
        };

        private double FleetMod => Fleet.Formation switch
        {
            FormationType.Vanguard when Fleet.IsVanguardTop => 0.8,
            FormationType.Vanguard => 1.25,

            FormationType.DoubleLine => 1.2,
            FormationType.Echelon => 1.2,
            FormationType.LineAbreast => 1.2,

            FormationType.LineAhead => 1,
            FormationType.Diamond => 1,

            _ => 1
        };

        private double AttackKindMod => Battle.DayAttack switch
        {
            DayAttackKind.CutinMainRadar => 1.5,

            DayAttackKind.CutinMainSub => 1.3,
            DayAttackKind.CutinMainAP => 1.3,

            DayAttackKind.CutinMainMain => 1.2,

            DayAttackKind.DoubleShelling => 1.1,

            _ => 1
        };
    }
}