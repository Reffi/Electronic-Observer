using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data.HitRate;

namespace ElectronicObserver.Data
{
    public class BattleDataCustom: IDayBattle, INightBattle, INightEvasionBattle
    {
        public EngagementTypes Engagement { get; set; } = EngagementTypes.Parallel;
        public DayAttackKind DayAttack { get; set; } = DayAttackKind.NormalAttack;
        public NightAttackKind NightAttack { get; set; } = NightAttackKind.NormalAttack;
        public NightTorpedoCutinKind TorpedoCutinKind { get; set; } = NightTorpedoCutinKind.None;
        public CvnciKind CvnciKind { get; set; } = CvnciKind.Unknown;
        public HitType HitType { get; set; } = HitType.Hit;
        public bool ActivatedSearchlight { get; set; } = false;
    }
}
