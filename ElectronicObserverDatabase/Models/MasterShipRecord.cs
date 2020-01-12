using ElectronicObserverTypes;

namespace ElectronicObserverDatabase.Models
{
    public partial class MasterShipRecord : IMasterShipRecord
    {
        public int ShipId { get; set; }
        public int RemodelBeforeShipId { get; set; }
        public int SortId { get; set; }
        public int ShipType { get; set; }
        public int ShipClass { get; set; }
        public string ShipName { get; set; }
        public int? HpMin { get; set; }
        public int? HpMax { get; set; }
        public int? FirepowerMin { get; set; }
        public int? FirepowerMax { get; set; }
        public int? TorpedoMin { get; set; }
        public int? TorpedoMax { get; set; }
        public int? AaMin { get; set; }
        public int? AaMax { get; set; }
        public int? ArmorMin { get; set; }
        public int? ArmorMax { get; set; }
        public int? AswMinLowerBound { get; set; }
        public int? AswMinUpperBound { get; set; }
        public int? AswMax { get; set; }
        public int? EvasionMinLowerBound { get; set; }
        public int? EvasionMinUpperBound { get; set; }
        public int? EvasionMax { get; set; }
        public int? LosMinLowerBound { get; set; }
        public int? LosMinUpperBound { get; set; }
        public int? LosMax { get; set; }
        public int? LuckMin { get; set; }
        public int? LuckMax { get; set; }
        public int? Range { get; set; }
        public int? Equipment1 { get; set; }
        public int? Equipment2 { get; set; }
        public int? Equipment3 { get; set; }
        public int? Equipment4 { get; set; }
        public int? Equipment5 { get; set; }
        public int? Aircraft1 { get; set; }
        public int? Aircraft2 { get; set; }
        public int? Aircraft3 { get; set; }
        public int? Aircraft4 { get; set; }
        public int? Aircraft5 { get; set; }
        public string? MessageGet { get; set; }
        public string? MessageAlbum { get; set; }
        public string? ResourceName { get; set; }
        public string? ResourceGraphicVersion { get; set; }
        public string? ResourceVoiceVersion { get; set; }
        public string? ResourcePortVoiceVersion { get; set; }
        public int? OriginalCostumeShipId { get; set; }
    }
}
