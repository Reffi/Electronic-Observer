using System;

namespace ElectronicObserverTypes
{
    public interface IAccuracy
    {
        double Total { get; }
    }

    public interface IEvasion
    {
        double Capped { get; }
        double Postcap { get; }
    }

    public interface IHitRateDefender
    {
        int Condition { get; }
    }
}
