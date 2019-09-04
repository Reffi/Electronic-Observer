using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.HitRate
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

    public class HitRate
    {
        private const int MinHitRate = 10;
        private const int MaxHitRate = 97;

        private IAccuracy Accuracy { get; }
        private IEvasion Evasion { get; }
        private IHitRateDefender Ship { get; }

        public double Capped =>
            Math.Max(MinHitRate, Accuracy.Total - Evasion.Postcap) * MoraleMod + 1;
        public double Postcap => Math.Floor(Math.Min(MaxHitRate, Capped)) + ProficiencyBonus;

        public HitRate(IAccuracy accuracy, IEvasion evasion, IHitRateDefender ship)
        {
            Accuracy = accuracy;
            Evasion = evasion;
            Ship = ship;
        }

        private double MoraleMod => Ship.Condition switch
        {
            int condition when condition > 49 => 0.7,
            int condition when condition > 29 => 1,
            int condition when condition > 19 => 1.2,
            _ => 1.4
        };

        private double ProficiencyBonus => 0;
    }
}
