namespace ElectronicObserverTypes
{
    public interface IAntiInstallationAttacker<out TEquipData> where TEquipData : IAntiInstallationEquipment
    {
        ShipTypes ShipType { get; }
        TEquipData[] Equipment { get; }
    }

    public interface IAntiInstallationEquipment
    {
        int ID { get; }
        int Level { get; }
        bool IsWG { get; }
        bool IsAntiInstallationRocket { get; }
        bool IsMortar { get; }
        bool IsTokuDaihatsu { get; }
        bool IsDaihatsuTank { get; }
        bool IsTokuDaihatsuTank { get; }
        bool IsAmericanDaihatsuTank { get; }
        EquipmentTypes CategoryType { get; }
    }

    public interface IInstallation
    {
        InstallationType InstallationType { get; }
    }
}