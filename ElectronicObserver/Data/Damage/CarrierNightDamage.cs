using System;
using System.Linq;
using ElectronicObserver.Data.Mocks;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.Damage
{
    public interface ICarrierNightDamageAttacker<out TEquipType> where TEquipType : ICarrierNightDamageEquipment
    {
        int BaseFirepower { get; }
        int[] Aircraft { get; }
        TEquipType[] Equipment { get; }
    }

    public interface ICarrierNightDamageEquipment
    {
        int BaseFirepower { get; }
        int BaseTorpedo { get; }
        int BaseASW { get; }
        int BaseBombing { get; }
        double UpgradeNightPower { get; }

        bool IsNightAircraft { get; }
        bool IsNightCapableAircraft { get; }
    }

    public interface ICarrierNightDamageDefender
    {
        int BaseArmor { get; }
        ShipTypes ShipType { get; }
    }

    public interface ICarrierNightDamageAttackerFleet
    {
        FormationType Formation { get; }
        FleetType Type { get; }
        bool IsMain { get; }
        bool IsVanguardTop { get; }
    }

    public interface ICarrierNightDamageDefenderFleet
    {
        FleetType Type { get; }
        bool IsMain { get; }
    }

    public class CarrierNightDamage : DamageBase
    {
        private ICarrierNightDamageAttacker<ICarrierNightDamageEquipment> Attacker { get; }
        private ICarrierNightDamageAttackerFleet AttackerFleet { get; }
        private ICarrierNightDamageDefender Defender { get; }
        private ICarrierNightDamageDefenderFleet DefenderFleet { get; }
        private INightBattle Battle { get; }

        protected override double PrecapBase => Attacker.BaseFirepower + AircraftNightPower;

        private double AircraftNightPower => Attacker.Equipment
            .Zip(Attacker.Aircraft, (eq, count) => (eq, count))
            .Where(x => x.eq != null && (x.eq.IsNightAircraft || x.eq.IsNightCapableAircraft))
            .Sum(x => x.eq.BaseFirepower + x.eq.BaseTorpedo + x.eq.UpgradeNightPower
                      + x.count * (x.eq.IsNightAircraft ? 3 : 0)
                      + Math.Sqrt(x.count)
                      * (x.eq.IsNightAircraft ? 0.45 : 0.3)
                      * (x.eq.BaseFirepower + x.eq.BaseTorpedo + x.eq.BaseASW + x.eq.BaseBombing));

        protected override double PrecapMods => FleetMod * AttackKindPrecapMod;

        private double AttackKindPrecapMod => Battle.CvnciKind switch
        {
            CvnciKind.FFA => 1.25,
            CvnciKind.Pair => 1.2,
            CvnciKind.Other => 1.18,

            _ => 1
        };

        protected override double CritMod => Battle.HitType switch
        {
            HitType.Critical => 1.5,

            _ => 1
        };

        protected override double AttackKindPostcapMod => 1;

        protected override double BaseArmor => Defender.BaseArmor;

        public CarrierNightDamage(ICarrierNightDamageAttacker<ICarrierNightDamageEquipment> attacker,
            ICarrierNightDamageAttackerFleet attackerFleet = null,
            INightBattle battle = null, ICarrierNightDamageDefender defender = null,
            ICarrierNightDamageDefenderFleet defenderFleet = null,
            DamageBonus parameters = null) : base(parameters, Constants.Softcap.NightBattle)
        {
            Attacker = attacker;
            AttackerFleet = attackerFleet ?? new MockCarrierNightDamageAttackerFleet();

            Defender = defender ?? new MockCarrierNightDamageDefender();
            DefenderFleet = defenderFleet ?? new MockCarrierNightDamageDefenderFleet();

            Battle = battle ?? new MockNightBattle();
        }

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
    }
}