using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{
    public class HitRate
    {
        private const int MinHitRate = 10;
        private const int MaxHitRate = 97;

        private Accuracy _accuracy;
        private Evasion _evasion;

        private double _accuracyValue;
        private double _evasionValue;

        private int _defenderCondition;

        public double Capped => 
            Math.Max(MinHitRate, _accuracyValue - _evasionValue) * MoraleMod + 1;
        public double Postcap => Math.Floor(Math.Min(MaxHitRate, Capped)) + ProficiencyBonus;

        public double ShellingCapped
        {
            get
            {
                _accuracyValue = _accuracy.DayShelling;
                _evasionValue = _evasion?.Shelling ?? 0;

                return Capped;
            }
        }

        public double AswCapped
        {
            get
            {
                _accuracyValue = _accuracy.ASW;
                _evasionValue = _evasion?.ASW ?? 0;

                return Capped;
            }
        }

        public double NightCapped
        {
            get
            {
                _accuracyValue = _accuracy.Night;
                _evasionValue = _evasion?.Night ?? 0;

                return Capped;
            }
        }

        public HitRate(Accuracy accuracy, Evasion evasion, int defenderCondition)
            => (_accuracy, _evasion, _defenderCondition) = (accuracy, evasion, defenderCondition);

        private double MoraleMod => _defenderCondition switch
            {
            int condition when condition > 49 => 0.7,
            int condition when condition > 29 => 1,
            int condition when condition > 19 => 1.2,
            _ => 1.4
            };

        private double ProficiencyBonus => 0;
    }
}
