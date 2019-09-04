using ElectronicObserver.Data;
using ElectronicObserver.Data.HitRate;
using ElectronicObserver.Data.Mocks;
using Xunit;

namespace ElectronicObserverTests.Data
{
    public class HitRateTests
    {
        MockHitRateDefender NormalMoraleShip = new MockHitRateDefender
        {
            Condition = 49
        };

        [Fact]
        public void HitRateTest()
        {
            MockAccuracy accuracy = new MockAccuracy
            {
                Total = 200
            };
            MockEvasion evasion = new MockEvasion
            {
                Postcap = 50
            };

            HitRate hitRate = new HitRate(accuracy, evasion, NormalMoraleShip);

            Assert.Equal(151, hitRate.Capped);
            Assert.Equal(97, hitRate.Postcap);
        }

        [Fact]
        public void MinHitRateTest()
        {
            MockAccuracy accuracy = new MockAccuracy
            {
                Total = 0
            };
            MockEvasion evasion = new MockEvasion
            {
                Postcap = 9999
            };

            HitRate hitRate = new HitRate(accuracy, evasion, NormalMoraleShip);

            Assert.Equal(11, hitRate.Capped);
            Assert.Equal(11, hitRate.Postcap);
        }

        [Fact]
        public void MaxHitRateTest()
        {
            MockAccuracy accuracy = new MockAccuracy
            {
                Total = 9999
            };
            MockEvasion evasion = new MockEvasion
            {
                Postcap = 0
            };

            HitRate hitRate = new HitRate(accuracy, evasion, NormalMoraleShip);

            Assert.Equal(97, hitRate.Postcap);
        }
    }
}