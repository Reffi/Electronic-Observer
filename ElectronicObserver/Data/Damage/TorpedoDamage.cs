using System.Linq;
using ElectronicObserver.Data.Mocks;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.Damage
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

    public class TorpedoDamage : DamageBase
    {
        private ITorpedoDamageAttacker<ITorpedoDamageAttackerEquipment> Attacker { get; }
        private ITorpedoAttackerFleetData AttackerFleet { get; }
        private ITorpedoDefenderData Defender { get; }
        private ITorpedoDefenderFleetData DefenderFleet { get; }
        private IDayBattle Battle { get; }

        public TorpedoDamage(ITorpedoDamageAttacker<ITorpedoDamageAttackerEquipment> attacker, ITorpedoAttackerFleetData attackerFleet = null,
            IDayBattle battle = null, ITorpedoDefenderData defender = null,
            ITorpedoDefenderFleetData defenderFleet = null,
            DamageBonus parameters = null) : base(parameters, Constants.Softcap.Torpedo)
        {
            Attacker = attacker;
            AttackerFleet = attackerFleet ?? new MockTorpedoAttackerFleetData();

            Defender = defender ?? new MockTorpedoDefenderData();
            DefenderFleet = defenderFleet ?? new MockTorpedoDefenderFleetData();

            Battle = battle ?? new MockDayBattle();
        }

        protected override double PrecapBase =>
            Attacker.Torpedo
            + Attacker.Equipment.Where(eq => eq != null).Sum(eq => eq.Torpedo)
            + (AttackerFleet.IsCombined ? 0 : 5);

        protected override double PrecapMods =>
            FleetMod
            * EngagementMod;

        protected override double CritMod => Battle.HitType switch
        {
            HitType.Critical => 1.5,

            _ => 1
        };

        protected override double BaseArmor => Defender.Armor;

        private double FleetMod => AttackerFleet.Formation switch
        {
            FormationType.LineAhead => 1,
            FormationType.FourthPatrolFormation => 1,

            FormationType.SecondPatrolFormation => 0.9,

            FormationType.DoubleLine => 0.8,

            FormationType.Diamond => 0.7,
            FormationType.FirstPatrolFormation => 0.7,

            FormationType.Echelon => 0.6,
            FormationType.LineAbreast => 0.6,
            FormationType.ThirdPatrolFormation => 0.6,

            FormationType.Vanguard => 1,

            _ => 1
        };

        private double EngagementMod => Battle.Engagement switch
        {
            EngagementTypes.TAdvantage => 1.2,
            EngagementTypes.Parallel => 1,
            EngagementTypes.HeadOn => 0.8,
            EngagementTypes.TDisadvantage => 0.6,
            _ => 1
        };
    }
}