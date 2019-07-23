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
        private DayAttackKind DayAttack { get; }
        private NightAttackKind NightAttack { get; }

        public AttackKindData(DayAttackKind attackKind) =>
            (DayAttack, NightAttack) = (attackKind, NightAttackKind.Unknown);

        public AttackKindData(NightAttackKind attackKind) =>
            (DayAttack, NightAttack) = (DayAttackKind.Unknown, attackKind);

        public bool CanAttackSubmarines => DayAttack == DayAttackKind.DepthCharge ||
                                           DayAttack == DayAttackKind.AirAttack ||
                                           NightAttack == NightAttackKind.DepthCharge ||
                                           NightAttack == NightAttackKind.AirAttack;

        public bool CanAttackInstallations => InternalCanAttackInstallations();
        private bool InternalCanAttackInstallations()
        {
            if (DayAttack != DayAttackKind.Unknown)
                return DayAttack switch
                {
                    DayAttackKind.Torpedo => false,

                    _ => true
                };

            if (NightAttack != NightAttackKind.Unknown)
                return NightAttack switch
                {
                    NightAttackKind.CutinTorpedoTorpedo => false,
                    NightAttackKind.CutinMainTorpedo => false,
                    NightAttackKind.Torpedo => false,

                    _ => true
                };

            return false;
        }
    }
}