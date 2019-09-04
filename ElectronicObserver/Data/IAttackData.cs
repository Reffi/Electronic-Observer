using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data
{
    public enum HitType
    {
        Miss,
        Hit,
        Critical
    }
    public interface IDayBattle
    {
        EngagementTypes Engagement { get; }

        DayAttackKind DayAttack { get; }
        HitType HitType { get; }
    }

    public interface INightBattle
    {
        NightAttackKind NightAttack { get; }
        NightTorpedoCutinKind TorpedoCutinKind { get; }
        CvnciKind CvnciKind { get; }
        HitType HitType { get; }
    }
}