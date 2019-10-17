using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data.HitRate;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.Mocks
{
    public class MockShellingAccuracyShip : IShellingAccuracyShip<MockShellingAccuracyEquipment>
    {
        public double BaseAccuracy { get; set; } = 0;
        public int Condition { get; set; } = 0;
        public double AccuracyFitBonus { get; set; } = 0;

        public MockShellingAccuracyEquipment[] Equipment { get; set; } = { };
    }

    public class MockShellingAccuracyEquipment : IShellingAccuracyEquipment
    {
        public double Accuracy { get; set; } = 0;
        public bool IsApShell { get; set; } = false;
        public bool IsMainGun { get; set; } = false;
        public bool IsSecondaryGun { get; set; } = false;
        public bool IsRadar { get; set; } = false;
    }

    public class MockShellingAccuracyFleet : IShellingAccuracyFleet
    {
        public FormationType Formation { get; set; } = FormationType.LineAhead;
        public FleetType Type { get; set; } = FleetType.Single;
        public bool IsMain { get; set; } = true;
        public bool IsVanguardTop { get; set; } = false;
    }
}