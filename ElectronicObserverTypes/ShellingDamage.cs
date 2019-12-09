namespace ElectronicObserverTypes
{
    public interface IShellingDamageAttacker<out TEquipType> where TEquipType : IShellingDamageAttackerEquipment
    {
        int Firepower { get; }

        TEquipType[] Equipment { get; }
    }

    public interface IShellingDamageAttackerEquipment
    {
        double Firepower { get; }

        bool IsApShell { get; }
        bool IsMainGun { get; }
        bool IsSecondaryGun { get; }
        bool IsRadar { get; }
    }

    public interface IShellingDamageDefender
    {
        int BaseArmor { get; }

        ShipTypes ShipType { get; }
    }

    public interface IShellingDamageAttackerFleet
    {
        FormationType Formation { get; }
        FleetType Type { get; }
        bool IsMain { get; }
        bool IsVanguardTop { get; }
    }

    public interface IShellingDamageDefenderFleet
    {
        FleetType Type { get; }
        bool IsMain { get; }
    }
}