using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data.Mocks;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.Damage
{
    public interface INightDamageAttacker<out TEquipType> where TEquipType : INightDamageAttackerEquipment
    {
        int Firepower { get; }
        int Torpedo { get; }

        TEquipType[] Equipment { get; }
    }

    public interface INightDamageAttackerEquipment
    {
        int BaseFirepower { get; }
        int BaseTorpedo { get; }
        double UpgradeNightPower { get; }
    }

    public interface INightDamageDefender
    {
        int BaseArmor { get; }
        bool IsInstallation { get; }
    }

    public interface INightDamageAttackerFleet
    {
        bool NightRecon { get; }
        FormationType Formation { get; }
        bool IsVanguardTop { get; }
    }

    public interface INightDamageDefenderFleet
    {
    }

    public class NightDamage : DamageBase
    {
        private INightDamageAttacker<INightDamageAttackerEquipment> Attacker { get; }
        private INightDamageAttackerFleet AttackerFleet { get; }
        private INightDamageDefender Defender { get; }
        private INightDamageDefenderFleet DefenderFleet { get; }
        private INightBattle Battle { get; }

        public NightDamage(INightDamageAttacker<INightDamageAttackerEquipment> attacker,
            INightDamageAttackerFleet attackerFleet = null,
            INightBattle battle = null, INightDamageDefender defender = null,
            INightDamageDefenderFleet defenderFleet = null,
            ExtraDamageBonus parameters = null) : base(parameters, Constants.Softcap.NightBattle)
        {
            Attacker = attacker;
            AttackerFleet = attackerFleet ?? new MockNightDamageAttackerFleet();

            Defender = defender ?? new MockNightDamageDefender();
            DefenderFleet = defenderFleet ?? new MockNightDamageDefenderFleet();

            Battle = battle ?? new MockNightBattle();
        }

        protected override double PrecapBase => NightPower + (AttackerFleet.NightRecon ? 5 : 0);

        protected override double PrecapMods =>
            FleetMod
            * AttackKindMod;

        protected override double CritMod => Battle.HitType switch
        {
            HitType.Critical => 1.5,

            _ => 1
        };

        protected override double BaseArmor => Defender.BaseArmor;



        private double NightPower => Defender.IsInstallation switch
        {
            true => Attacker.Firepower + Attacker.Equipment.Where(eq => eq != null)
                        .Sum(eq => eq.BaseFirepower + eq.UpgradeNightPower),

            false => Attacker.Firepower + Attacker.Torpedo + Attacker.Equipment.Where(eq => eq != null)
                         .Sum(eq => eq.BaseFirepower + eq.BaseTorpedo + eq.UpgradeNightPower),
        };

        private double AttackKindMod => Battle.NightAttack switch
        {
            NightAttackKind.DoubleShelling => 1.2,
            NightAttackKind.CutinMainSub => 1.75,
            NightAttackKind.CutinMainMain => 2,
            NightAttackKind.CutinMainTorpedo => 1.3,

            NightAttackKind.CutinTorpedoTorpedo
            when Battle.TorpedoCutinKind == NightTorpedoCutinKind.LateModelTorpedoSubmarineEquipment => 1.75,

            NightAttackKind.CutinTorpedoTorpedo
            when Battle.TorpedoCutinKind == NightTorpedoCutinKind.LateModelTorpedo2 => 1.6,

            NightAttackKind.CutinTorpedoTorpedo => 1.5,

            _ => 1
        };

        private double FleetMod => AttackerFleet.Formation switch
        {
            FormationType.Vanguard when AttackerFleet.IsVanguardTop => 0.5,

            _ => 1
        };
    }
}
