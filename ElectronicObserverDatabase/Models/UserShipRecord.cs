using ElectronicObserverTypes;

namespace ElectronicObserverDatabase.Models
{
    public partial class UserShipRecord : IUserShipRecord
    {
        public int DropId { get; set; }
        public int ShipId { get; set; }
        public int Level { get; set; }
    }
}
