using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data.HitRate;

namespace ElectronicObserver.Data.Mocks
{
    public class MockAccuracy : IAccuracy
    {
        public double Total { get; set; } = 0;
    }

    public class MockEvasion : IEvasion
    {
        public double Capped { get; set; } = 0;
        public double Postcap { get; set; } = 0;
    }

    public class MockHitRateDefender : IHitRateDefender
    {
        public int Condition { get; set; } = 49;
    }
}
