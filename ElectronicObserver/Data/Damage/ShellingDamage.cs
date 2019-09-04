using System.Linq;
using ElectronicObserver.Data.Mocks;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.Damage
{
    public interface IShellingDamageAttacker<out TEquipType> where TEquipType : IShellingDamageAttackerEquipment
    {
        int Firepower { get; }

        TEquipType[] Equipment { get; }
    }

    public interface IShellingDamageAttackerEquipment
    {
        double Firepower { get; }

        bool IsApShell { get; }
        bool IsMainGun { get; }
        bool IsSecondaryGun { get; }
        bool IsRadar { get; }
    }

    public interface IShellingDamageDefender
    {
        int BaseArmor { get; }

        ShipTypes ShipType { get; }
    }

    public interface IShellingDamageAttackerFleet
    {
        FormationType Formation { get; }
        FleetType Type { get; }
        bool IsMain { get; }
        bool IsVanguardTop { get; }
    }

    public interface IShellingDamageDefenderFleet
    {
        FleetType Type { get; }
        bool IsMain { get; }
    }

    public class ShellingDamage : DamageBase
    {
        private IShellingDamageAttacker<IShellingDamageAttackerEquipment> Attacker { get; }
        private IShellingDamageAttackerFleet AttackerFleet { get; }
        private IShellingDamageDefender Defender { get; }
        private IShellingDamageDefenderFleet DefenderFleet { get; }
        private IDayBattle Battle { get; }

        protected override double PrecapBase =>
            Attacker.Firepower
            + Attacker.Equipment.Where(eq => eq != null).Sum(eq => eq.Firepower)
            + CombinedFleetBonus + 5;

        protected override double PrecapMods =>
            FleetMod
            * EngagementMod;

        protected override double AttackKindPostcapMod => Battle.DayAttack switch
        {
            DayAttackKind.DoubleShelling => 1.2,
            DayAttackKind.CutinMainRadar => 1.2,
            DayAttackKind.CutinMainSub => 1.1,
            DayAttackKind.CutinMainAP => 1.3,
            DayAttackKind.CutinMainMain => 1.5,
            _ => 1
        };

        protected override double CritMod => Battle.HitType switch
        {
            HitType.Critical => 1.5,

            _ => 1
        };

        protected override double BaseArmor => Defender.BaseArmor;

        public ShellingDamage(IShellingDamageAttacker<IShellingDamageAttackerEquipment> attacker,
            IShellingDamageAttackerFleet attackerFleet = null,
            IDayBattle battle = null, IShellingDamageDefender defender = null,
            IShellingDamageDefenderFleet defenderFleet = null,
            ExtraDamageBonus parameters = null) : base(parameters, Constants.Softcap.DayShelling)
        {
            Attacker = attacker;
            AttackerFleet = attackerFleet ?? new MockShellingDamageAttackerFleet();

            Defender = defender ?? new MockShellingDamageDefender();
            DefenderFleet = defenderFleet ?? new MockShellingDamageDefenderFleet();

            Battle = battle ?? new MockDayBattle();
        }

        private int CombinedFleetBonus =>
            (AttackerFleet.Type, AttackerFleet.IsMain, DefenderFleet.Type, DefenderFleet.IsMain) switch
            {
                (FleetType.Single, _, _, _) => 0,

                // against abyssal single fleet
                (FleetType.Carrier, true, FleetType.Single, true) => 2,
                (FleetType.Carrier, false, FleetType.Single, true) => 10,
                (FleetType.Surface, true, FleetType.Single, true) => 10,
                (FleetType.Surface, false, FleetType.Single, true) => -5,
                (FleetType.Transport, true, FleetType.Single, true) => -5,
                (FleetType.Transport, false, FleetType.Single, true) => 10,

                // against abyssal combined fleet
                (FleetType.Transport, true, _, _) => -5,
                (FleetType.Transport, false, _, false) => 10,
                (_, true, _, true) => 2,
                (_, true, _, false) => -5,
                (_, false, _, false) => 10,

                _ => 0
            };

        private double FleetMod => AttackerFleet.Formation switch
        {
            FormationType.FourthPatrolFormation => 1.1,

            FormationType.LineAhead => 1,
            FormationType.SecondPatrolFormation => 1,

            FormationType.DoubleLine => 0.8,
            FormationType.FirstPatrolFormation => 0.8,

            FormationType.Diamond => 0.7,
            FormationType.ThirdPatrolFormation => 0.7,

            FormationType.Echelon => 0.75,

            FormationType.LineAbreast => 0.6,

            FormationType.Vanguard when AttackerFleet.IsVanguardTop => 0.5,
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

        protected override double ApShellMod => CalculateApDamageMod();

        private double CalculateApDamageMod()
        {
            // todo
            if (Defender == null || Defender.ShipType != ShipTypes.Battleship)
                return 1;

            if (Attacker.Equipment.Where(eq => eq != null).Any(eq => eq.IsApShell) ||
                Attacker.Equipment.Where(eq => eq != null).Any(eq => eq.IsMainGun))
                return 1;

            if (Attacker.Equipment.Where(eq => eq != null).Any(eq => eq.IsSecondaryGun))
                return 1.15;

            if (Attacker.Equipment.Where(eq => eq != null).Any(eq => eq.IsRadar))
                return 1.1;

            return 1.08;
        }
    }
}
