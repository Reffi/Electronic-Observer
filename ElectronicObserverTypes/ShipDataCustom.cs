using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ElectronicObserverTypes
{
    public class ShipDataCustom 
    {
        private int _level;

        private int ASWMin { get; }
        private int ASWMax { get; }
        private int ASWMod { get; }
        private int EvasionMin { get; }
        private int EvasionMax { get; }
        private int LoSMin { get; }
        private int LoSMax { get; }

        public int Level
        {
            get => _level;
            set
            {
                _level = value;

                BaseASW = ScaledStat(ASWMin, ASWMax) + ASWMod;
                BaseLoS = ScaledStat(LoSMin, LoSMax);
                BaseEvasion = ScaledStat(EvasionMin, EvasionMax);
            }
        }

        public int HP { get; set; }

        public int BaseArmor { get; set; }

        public int BaseEvasion { get; set; }

        public int[] Aircraft { get; set; } = {0, 0, 0, 0, 0, 0};

        public int BaseSpeed { get; set; }

        public int BaseRange { get; set; }

        public double BaseAccuracy { get; private set; }


        public int Condition { get; set; }

        public int BaseFirepower { get; set; }

        public int BaseTorpedo { get; set; }

        public int BaseAA { get; set; }

        public int BaseASW { get; set; }

        public int BaseLoS { get; set; }

        public int BaseLuck { get; set; }

        public int BaseNightPower { get; private set; }


        public ShipDataCustom() { }

        public ShipDataCustom(IUserShipRecord ship)
        {
            ShipID = (ShipID) ship.ShipId;
            Level = ship.Level;
        }

        public ShipDataCustom(IMasterShipRecord ship)
        {
            Name = ship.ShipName;
            SortID = ship.SortId;
            ShipID = (ShipID)ship.ShipId;
            ShipClass = (ShipClass)ship.ShipClass;
            RemodelBeforeShipId = (ShipID) ship.RemodelBeforeShipId;
            ShipType = (ShipType)ship.ShipType;
        }

        private int ScaledStat(int min, int max)
        {
            if (min == 0 || max == 9999)
                return 0;

            return min + (int) ((max - min) * _level / 99.0);
        }

        public string Name { get; set; }
        public int SortID { get; }
        
        public ShipID ShipID { get; set; }
        public ShipID RemodelBeforeShipId { get; set; }




        public ShipClass ShipClass { get; set; }

        public ShipType ShipType { get; }

        public ShipDataCustom Clone()
        {
            return (ShipDataCustom)MemberwiseClone();
        }
    }
}
