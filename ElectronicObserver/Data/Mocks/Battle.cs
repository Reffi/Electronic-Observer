using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.Mocks
{
    public class MockDayBattle : IDayBattle
    {
        public EngagementTypes Engagement { get; set; } = EngagementTypes.Parallel;
        public DayAttackKind DayAttack { get; set; } = DayAttackKind.NormalAttack;
        public HitType HitType { get; set; } = HitType.Hit;
    }

    public class MockNightBattle : INightBattle
    {
        public NightAttackKind NightAttack { get; set; } = NightAttackKind.NormalAttack;
        public NightTorpedoCutinKind TorpedoCutinKind { get; set; } = NightTorpedoCutinKind.None;
        public CvnciKind CvnciKind { get; set; } = CvnciKind.Unknown;
        public HitType HitType { get; set; } = HitType.Hit;
    }
}
