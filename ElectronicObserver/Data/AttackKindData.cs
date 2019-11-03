using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data
{
    public class AttackKindData
    {
        public Enum Value { get; }

        public AttackKindData(DayAttackKind attackKind) => Value = attackKind;

        public AttackKindData(DayAirAttackCutinKind attackKind) => Value = attackKind;

        public AttackKindData(NightAttackKind attackKind) => Value = attackKind;

        public AttackKindData(CvnciKind attackKind) => Value = attackKind;

        public bool CanHitSurface => Value switch
        {
            DayAttackKind.Unknown => false,
            DayAttackKind.NormalAttack => true,
            DayAttackKind.Laser => true,
            DayAttackKind.DoubleShelling => true,
            DayAttackKind.CutinMainSub => true,
            DayAttackKind.CutinMainRadar => true,
            DayAttackKind.CutinMainAP => true,
            DayAttackKind.CutinMainMain => true,
            DayAttackKind.CutinAirAttack => true,
            DayAttackKind.SpecialNelson => true,
            DayAttackKind.SpecialNagato => true,
            DayAttackKind.SpecialMutsu => true,
            DayAttackKind.ZuiunMultiAngle => true,
            DayAttackKind.SeaAirMultiAngle => true,
            DayAttackKind.Shelling => true,
            DayAttackKind.AirAttack => true,
            DayAttackKind.DepthCharge => false,
            DayAttackKind.Torpedo => true,
            DayAttackKind.Rocket => false,
            DayAttackKind.LandingDaihatsu => false,
            DayAttackKind.LandingTokuDaihatsu => false,
            DayAttackKind.LandingDaihatsuTank => false,
            DayAttackKind.LandingAmphibious => false,
            DayAttackKind.LandingTokuDaihatsuTank => false,

            NightAttackKind.Unknown => false,
            NightAttackKind.NormalAttack => true,
            NightAttackKind.DoubleShelling => true,
            NightAttackKind.CutinMainTorpedo => true,
            NightAttackKind.CutinTorpedoTorpedo => true,
            NightAttackKind.CutinMainSub => true,
            NightAttackKind.CutinMainMain => true,
            NightAttackKind.CutinAirAttack => true,
            NightAttackKind.CutinTorpedoRadar => true,
            NightAttackKind.CutinTorpedoPicket => true,
            NightAttackKind.SpecialNelson => true,
            NightAttackKind.SpecialNagato => true,
            NightAttackKind.SpecialMutsu => true,
            NightAttackKind.Shelling => true,
            NightAttackKind.AirAttack => true,
            NightAttackKind.DepthCharge => false,
            NightAttackKind.Torpedo => true,
            NightAttackKind.Rocket => false,
            NightAttackKind.LandingDaihatsu => false,
            NightAttackKind.LandingTokuDaihatsu => false,
            NightAttackKind.LandingDaihatsuTank => false,
            NightAttackKind.LandingAmphibious => false,
            NightAttackKind.LandingTokuDaihatsuTank => false,

            DayAirAttackCutinKind.None => false,
            DayAirAttackCutinKind.FighterBomberAttacker => true,
            DayAirAttackCutinKind.BomberBomberAttacker => true,
            DayAirAttackCutinKind.BomberAttacker => true,

            CvnciKind.Unknown => false,
            CvnciKind.FFA => true,
            CvnciKind.Pair => true,
            CvnciKind.Other => true,

            _ => false
        };

        public bool CanHitSubmarine => Value switch
        {
            DayAttackKind.DepthCharge => true,
            DayAttackKind.AirAttack => true,
            NightAttackKind.DepthCharge => true,
            NightAttackKind.AirAttack => true,

            _ => false
        };

        public bool CanHitInstallation => Value switch
        {
            DayAttackKind.Torpedo => false,

            NightAttackKind.CutinTorpedoTorpedo => false,
            NightAttackKind.CutinMainTorpedo => false,
            NightAttackKind.Torpedo => false,

            _ => true
        };
    }
}