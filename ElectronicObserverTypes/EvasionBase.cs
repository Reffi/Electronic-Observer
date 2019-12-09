using System;
using System.Linq;

namespace ElectronicObserverTypes
{
    public interface IEvasionShip<out TEquipType> where TEquipType : IEvasionEquipment
    {
        int BaseEvasion { get; }
        int BaseLuck { get; }
        int Fuel { get; }

        TEquipType[] Equipment { get; }
    }

    public interface IEvasionFleet
    {
        FormationType Formation { get; }
    }

    public interface IEvasionEquipment
    {
        int BaseEvasion { get; }
    }
}
