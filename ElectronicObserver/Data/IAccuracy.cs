namespace ElectronicObserver.Data
{
    public interface IAccuracy
    {
        double DayShelling { get; }
        double ASW { get; }
        double Night { get; }
    }
}