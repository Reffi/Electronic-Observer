using System;
using System.Linq;
using ElectronicObserver.Data.Mocks;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.Damage
{
    public interface ICarrierShellingDamageAttacker<out TEquipType> where TEquipType : ICarrierShellingDamageEquipment
    {
        int Firepower { get; }
        int Torpedo { get; }
        TEquipType[] Equipment { get; }
    }

    public interface ICarrierShellingDamageEquipment
    {
        double Firepower { get; }
        double Torpedo { get; }
        double Bombing { get; }
    }

    public interface ICarrierShellingDamageDefender
    {
        int BaseArmor { get; }
        ShipTypes ShipType { get; }
    }

    public interface ICarrierShellingDamageAttackerFleet
    {
        FormationType Formation { get; }
        FleetType Type { get; }
        bool IsMain { get; }
        bool IsVanguardTop { get; }
    }

    public interface ICarrierShellingDamageDefenderFleet
    {
        FleetType Type { get; }
        bool IsMain { get; }
    }

    public class CarrierShellingDamage : DamageBase
    {
        private ICarrierShellingDamageAttacker<ICarrierShellingDamageEquipment> Attacker { get; }
        private ICarrierShellingDamageAttackerFleet AttackerFleet { get; }
        private ICarrierShellingDamageDefender Defender { get; }
        private ICarrierShellingDamageDefenderFleet DefenderFleet { get; }
        private IDayBattle Battle { get; }

        protected override double PrecapBase =>
            55 + 1.5 * (Attacker.Firepower
                        + Attacker.Torpedo
                        + Attacker.Equipment.Where(eq => eq != null)
                            .Sum(eq => eq.Firepower + eq.Torpedo + Math.Floor(1.3*eq.Bombing))
                        + CombinedFleetBonus);

        protected override double PrecapMods =>
            FleetMod
            * EngagementMod;

        protected override double CritMod => Battle.HitType switch
        {
            HitType.Critical => 1.5,
 
            _ => 1
        };

        // todo CVCI mods
        protected override double AttackKindPostcapMod => Battle.CvciKind switch
        {
            DayAirAttackCutinKind.FighterBomberAttacker => 1.25,
            DayAirAttackCutinKind.BomberBomberAttacker => 1.2,
            DayAirAttackCutinKind.BomberAttacker => 1.15,

            _ => 1
        };

        protected override double BaseArmor => Defender.BaseArmor;

        public CarrierShellingDamage(ICarrierShellingDamageAttacker<ICarrierShellingDamageEquipment> attacker, ICarrierShellingDamageAttackerFleet attackerFleet = null, 
            IDayBattle battle = null, ICarrierShellingDamageDefender defender = null, ICarrierShellingDamageDefenderFleet defenderFleet = null,
            ExtraDamageBonus parameters = null) : base(parameters, Constants.Softcap.DayShelling)
        {
            Attacker = attacker;
            AttackerFleet = attackerFleet ?? new MockCarrierShellingDamageAttackerFleet();

            Defender = defender ?? new MockCarrierShellingDamageDefender();
            DefenderFleet = defenderFleet ?? new MockCarrierShellingDamageDefenderFleet();

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
    }
}