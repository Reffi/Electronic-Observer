namespace ElectronicObserverTypes
{
    public interface IMasterShipRecord
    {
        int ShipId { get; }
        int RemodelBeforeShipId { get; }
        int SortId { get; }
        int ShipType { get; }
        int ShipClass { get; }
        string ShipName { get; }
        int? HpMin { get; }
        int? HpMax { get; }
        int? FirepowerMin { get; }
        int? FirepowerMax { get; }
        int? TorpedoMin { get; }
        int? TorpedoMax { get; }
        int? AaMin { get; }
        int? AaMax { get; }
        int? ArmorMin { get; }
        int? ArmorMax { get; }
        int? AswMinLowerBound { get; }
        int? AswMinUpperBound { get; }
        int? AswMax { get; }
        int? EvasionMinLowerBound { get; }
        int? EvasionMinUpperBound { get; }
        int? EvasionMax { get; }
        int? LosMinLowerBound { get; }
        int? LosMinUpperBound { get; }
        int? LosMax { get; }
        int? LuckMin { get; }
        int? LuckMax { get; }
        int? Range { get; }
        int? Equipment1 { get; }
        int? Equipment2 { get; }
        int? Equipment3 { get; }
        int? Equipment4 { get; }
        int? Equipment5 { get; }
        int? Aircraft1 { get; }
        int? Aircraft2 { get; }
        int? Aircraft3 { get; }
        int? Aircraft4 { get; }
        int? Aircraft5 { get; }
        string MessageGet { get; }
        string MessageAlbum { get; }
        string ResourceName { get; }
        string ResourceGraphicVersion { get; }
        string ResourceVoiceVersion { get; }
        string ResourcePortVoiceVersion { get; }
        int? OriginalCostumeShipId { get; }

    }
}