using ElectronicObserver.Data;
using Xunit;

namespace ElectronicObserverTests.Data
{
    public class MockAccuracy : IAccuracy
    {
        public double DayShelling { get; }
        public double Night { get; }
        public double ASW { get; }

        public MockAccuracy(double shelling, double night, double asw)
        {
            DayShelling = shelling;
            Night = night;
            ASW = asw;
        }
    }

    public class MockEvasion : IEvasion
    {
        public double Shelling { get; }
        public double Night { get; }
        public double ASW { get; }

        public MockEvasion(double shelling, double night, double asw)
        {
            Shelling = shelling;
            Night = night;
            ASW = asw;
        }
    }

    public class HitRateTests
    {
        [Fact]
        public void HitRateTest()
        {
            MockAccuracy Accuracy = new MockAccuracy(200, 90, 80);
            MockEvasion Evasion = new MockEvasion(50, 30, 10);
            int defenderEvasion = 49;

            HitRate hitRate = new HitRate(Accuracy, Evasion, defenderEvasion);

            Assert.Equal(151, hitRate.ShellingCapped);
            Assert.Equal(61, hitRate.NightCapped);
            Assert.Equal(71, hitRate.AswCapped);

            Assert.Equal(97, hitRate.Shelling);
            Assert.Equal(61, hitRate.Night);
            Assert.Equal(71, hitRate.ASW);
        }

        [Fact]
        public void MinHitRateTest()
        {
            MockAccuracy Accuracy = new MockAccuracy(0, 0, 0);
            MockEvasion Evasion = new MockEvasion(9999, 9999, 9999);
            int defenderEvasion = 49;

            HitRate hitRate = new HitRate(Accuracy, Evasion, defenderEvasion);

            Assert.Equal(11, hitRate.ShellingCapped);
            Assert.Equal(11, hitRate.NightCapped);
            Assert.Equal(11, hitRate.AswCapped);

            Assert.Equal(11, hitRate.Shelling);
            Assert.Equal(11, hitRate.Night);
            Assert.Equal(11, hitRate.ASW);
        }

        [Fact]
        public void MaxHitRateTest()
        {
            MockAccuracy Accuracy = new MockAccuracy(9999, 9999, 9999);
            MockEvasion Evasion = new MockEvasion(0, 0, 0);
            int defenderEvasion = 49;

            HitRate hitRate = new HitRate(Accuracy, Evasion, defenderEvasion);

            Assert.Equal(97, hitRate.Shelling);
            Assert.Equal(97, hitRate.Night);
            Assert.Equal(97, hitRate.ASW);
        }
    }
}