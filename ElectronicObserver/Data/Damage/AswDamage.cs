using System;
using System.Linq;
using ElectronicObserver.Data.Mocks;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.Damage
{
    public interface IAswDamageAttacker<out TEquipType> where TEquipType : IAswDamageAttackerEquipment
    {
        int BaseASW { get; }
        TEquipType[] Equipment { get; }
    }

    public interface IAswDamageAttackerEquipment
    {
        double ASW { get; }

        bool CountsForAswDamage { get; }
        bool IsSonar { get; }
        bool IsSmallSonar { get; }
        bool IsDepthCharge { get; }
        bool IsDepthChargeProjector { get; }
        bool IsSpecialDepthChargeProjector { get; }
    }

    public interface IAswDamageAttackerFleet
    {
        FormationType Formation { get; }
        bool IsVanguardTop { get; }
    }

    public interface IAswDamageDefender
    {
        int BaseArmor { get; }
    }

    public interface IAswDamageDefenderFleet
    {

    }

    public class AswDamage : DamageBase
    {
        private IAswDamageAttacker<IAswDamageAttackerEquipment> Attacker { get; }
        private IAswDamageAttackerFleet AttackerFleet { get; }
        private IAswDamageDefender Defender { get; }
        private IAswDamageDefenderFleet DefenderFleet { get; }
        private IDayBattle Battle { get; }

        public AswDamage(IAswDamageAttacker<IAswDamageAttackerEquipment> attacker,
            IAswDamageAttackerFleet attackerFleet = null,
            IDayBattle battle = null, IAswDamageDefender defender = null, IAswDamageDefenderFleet defenderFleet = null,
            DamageBonus parameters = null) : base(parameters, Constants.Softcap.ASW)
        {
            Attacker = attacker;
            AttackerFleet = attackerFleet ?? new MockAswDamageAttackerFleet();

            Defender = defender ?? new MockAswDamageDefender();
            DefenderFleet = defenderFleet ?? new MockAswDamageDefenderFleet();

            Battle = battle ?? new MockDayBattle();
        }

        protected override double PrecapBase =>
            (2 * Math.Sqrt(Attacker.BaseASW)
             + 1.5 * Attacker.Equipment.Where(eq => eq?.CountsForAswDamage ?? false).Sum(eq => eq.ASW)
             + AswTypeConstant)
            * AswDamageMod;

        protected override double PrecapMods =>
            FleetMod
            * EngagementMod;

        protected override double CritMod => Battle.HitType switch
        {
            HitType.Critical => 1.5,
 
            _ => 1
        };

        protected override double BaseArmor => Defender.BaseArmor;

        // todo
        private int AswTypeConstant => Battle.DayAttack == DayAttackKind.AirAttack ? 8 : 13;

        private double AswDamageMod => CalculateAswDamageMod();

        private double CalculateAswDamageMod()
        {
            // https://twitter.com/KennethWWKK/status/1156195106837286912

            bool sonar = Attacker.Equipment.Where(eq => eq != null).Any(eq => eq.IsSonar);
            bool smallSonar = Attacker.Equipment.Where(eq => eq != null).Any(eq => eq.IsSmallSonar);
            bool depthCharge = Attacker.Equipment.Where(eq => eq != null).Any(eq => eq.IsDepthCharge);
            bool depthChargeProjector = Attacker.Equipment.Where(eq => eq != null).Any(eq => eq.IsDepthChargeProjector);
            bool depthChargeProjectorSpecial =
                Attacker.Equipment.Where(eq => eq != null).Any(eq => eq.IsSpecialDepthChargeProjector);

            bool anyDepthCharge = depthCharge || depthChargeProjector || depthChargeProjectorSpecial;

            double oldSynergy = (sonar, anyDepthCharge) switch
            {
                (true, true) => 1.15,
                _ => 1
            };

            double newSynergy = (smallSonar, depthCharge, depthChargeProjector) switch
            {
                (true, true, true) => 1.25,
                (false, true, true) => 1.15,
                _ => 1
            };

            return oldSynergy * newSynergy;
        }

        private double FleetMod => AttackerFleet.Formation switch
        {
            FormationType.LineAbreast => 1.3,
            FormationType.FirstPatrolFormation => 1.3,

            FormationType.Diamond => 1.2,

            FormationType.Echelon => 1.1,
            FormationType.SecondPatrolFormation => 1.1,

            FormationType.ThirdPatrolFormation => 1,

            FormationType.DoubleLine => 0.8,

            FormationType.FourthPatrolFormation => 0.7,

            FormationType.LineAhead => 0.6,

            FormationType.Vanguard when AttackerFleet.IsVanguardTop => 1,
            FormationType.Vanguard => 0.6,

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