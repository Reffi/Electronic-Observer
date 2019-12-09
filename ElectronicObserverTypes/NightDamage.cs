using System.Linq;

namespace ElectronicObserverTypes
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
}
