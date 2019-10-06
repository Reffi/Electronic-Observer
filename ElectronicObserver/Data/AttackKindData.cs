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

        public AttackKindData(NightAttackKind attackKind) => Value = attackKind;

        public AttackKindData(CvnciKind attackKind) => Value = attackKind;

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