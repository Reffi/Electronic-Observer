namespace ElectronicObserverTypes
{
    public interface INightEvasionShip<out TEquipType> : IEvasionShip<TEquipType> where TEquipType:IEvasionEquipment
    {
        ShipTypes ShipType { get; }
    }

    public interface INightEvasionBattle
    {
        bool ActivatedSearchlight { get; }
    }
}
