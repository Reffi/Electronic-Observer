using System.Linq;

namespace ElectronicObserverTypes
{
    public interface ITorpedoEvasionEquipment : IEvasionEquipment
    {
        double TorpedoEvasion { get; }
    }
}
