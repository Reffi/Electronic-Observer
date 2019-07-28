using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{
    /*
     *
       public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
       {
           T[] elements = source.ToArray();
           for (int i = elements.Length - 1; i >= 0; i--)
           {
               // Swap element "i" with a random earlier element it (or itself)
               // ... except we don't really need to swap it fully, as we can
               // return it immediately, and afterwards it's irrelevant.
               int swapIndex = rng.Next(i + 1);
               yield return elements[swapIndex];
               elements[swapIndex] = elements[i];
           }
       }
     *
     *
     */

    public class FleetDataCustom
    {
        private List<IShipDataCustom> _mainFleet;
        private List<IShipDataCustom> _escortFleet;

        public List<IShipDataCustom> MainFleet => _mainFleet;

        public FleetType Type { get; }
        public FormationType Formation { get; }

        public bool NightRecon { get; set; }
        public bool Flare { get; set; }
        public bool Searchlight { get; set; }

        public FleetDataCustom(FleetType fleetType = FleetType.Single, FormationType formation = FormationType.LineAhead)
        {
            _mainFleet = new List<IShipDataCustom>();
            _escortFleet = new List<IShipDataCustom>();

            Type = fleetType;
            Formation = formation;
        }

        public void AddToMain(IShipDataCustom ship)
        {
            _mainFleet.Add(ship);
        }

        public void AddToEscort(IShipDataCustom ship)
        {
            _escortFleet.Add(ship);
        }

        private bool IsVanguardTop(IShipDataCustom ship)
        {
            // top: 0~2 bottom: 3~5
            // top: 0~2 bottom: 3~6

            int index = _mainFleet.IndexOf(ship);

            // for testing vanguard without a fleet
            if (index == 0)
                return true;

            return index * 2 < _mainFleet.Count;
        }

        public int BaseAccuracy(IShipDataCustom ship)
        {
            int baseAcc;

            if (!IsCombinedFleet || (Type == FleetType.Carrier && IsInMainFleet(ship)))
                baseAcc = 90;
            else if (Type == FleetType.Surface && IsInMainFleet(ship))
                baseAcc = 57;
            else
                baseAcc = 74;

            if (IsCombinedFleet && IsInEscortFleet(ship) && _escortFleet.IndexOf(ship) == 0)
                baseAcc -= 10;

            return baseAcc;
        }

        public int BaseAswAccuracy(IShipDataCustom ship) => 80;

        public int BaseNightAccuracy(IShipDataCustom ship) => 69;

        public bool IsCombinedFleet =>
            Type == FleetType.Carrier || Type == FleetType.Surface || Type == FleetType.Transport;

        private bool IsInMainFleet(IShipDataCustom ship) => _mainFleet.Contains(ship);
        private bool IsInEscortFleet(IShipDataCustom ship) => _escortFleet.Contains(ship);

        public double ShellingDamageMod(IShipDataCustom ship) => Formation switch
        {
            FormationType.FourthPatrolFormation => 1.1,

            FormationType.LineAhead => 1,
            FormationType.SecondPatrolFormation => 1,

            FormationType.DoubleLine => 0.8,
            FormationType.FirstPatrolFormation => 0.8,

            FormationType.Diamond => 0.7,
            FormationType.ThirdPatrolFormation => 0.7,

            FormationType.Echelon => 0.75,

            FormationType.LineAbreast => 0.6,

            FormationType.Vanguard when IsVanguardTop(ship) => 0.5,
            FormationType.Vanguard => 1,

            _ => 1
        };

        public double TorpedoDamageMod(IShipDataCustom ship) => Formation switch
        {
            FormationType.LineAhead => 1,
            FormationType.FourthPatrolFormation => 1,

            FormationType.SecondPatrolFormation => 0.9,

            FormationType.DoubleLine => 0.8,

            FormationType.Diamond => 0.7,
            FormationType.FirstPatrolFormation => 0.7,

            FormationType.Echelon => 0.6,
            FormationType.LineAbreast => 0.6,
            FormationType.ThirdPatrolFormation => 0.6,

            FormationType.Vanguard => 1,

            _ => 1
        };
        

        public double AswDamageMod(IShipDataCustom ship) => Formation switch
        {
            FormationType.LineAbreast => 1.3,
            FormationType.FirstPatrolFormation => 1.3,

            FormationType.Diamond => 1.2,

            FormationType.Echelon => 1.1,
            FormationType.SecondPatrolFormation => 1.1,

            FormationType.ThirdPatrolFormation => 1,

            FormationType.DoubleLine => 0.8,

            FormationType.FourthPatrolFormation => 0.7,

            FormationType.LineAhead => 0.6,

            FormationType.Vanguard when IsVanguardTop(ship) => 1,
            FormationType.Vanguard => 0.6,

            _ => 1
        };

        public double NightDamageMod(IShipDataCustom ship) => Formation switch
        {
            FormationType.Vanguard when IsVanguardTop(ship) => 0.5,
            _ => 1
        };

        public double AaMod()
        {
            switch (Formation)
            {
                case FormationType.LineAhead:
                case FormationType.Echelon:
                case FormationType.LineAbreast:
                default:
                    return 1;

                case FormationType.DoubleLine:
                    return 1.2;

                case FormationType.Diamond:
                    return 1.6;
            }
        }

        public double AirstrikeDamageMod()
        {
            switch (Formation)
            {
                // they should all be 1
                default:
                    return 1;
            }
        }

        public double ShellingAccuracyMod(IShipDataCustom ship) => Formation switch
        {
            FormationType.Vanguard when IsVanguardTop(ship) => 0.8,
            FormationType.Vanguard => 1.25,

            FormationType.DoubleLine => 1.2,
            FormationType.Echelon => 1.2,
            FormationType.LineAbreast => 1.2,

            FormationType.LineAhead => 1,
            FormationType.Diamond => 1,

            _ => 1
        };

        public double TorpedoAccuracyMod()
        {
            switch (Formation)
            {
                case FormationType.LineAhead:
                default:
                    return 1;

                case FormationType.DoubleLine:
                    return 0.8;

                case FormationType.Diamond:
                    return 0.4;

                case FormationType.Echelon:
                    return 0.6;

                case FormationType.LineAbreast:
                    return 0.3;
            }
        }

        public double AswAccuracyMod(IShipDataCustom ship) => Formation switch
        {
            FormationType.LineAhead => 1,
            FormationType.Diamond => 1,

            FormationType.DoubleLine => 1.2,
            FormationType.Echelon => 1.2,
            FormationType.LineAbreast => 1.2,

            _ => 1 
        };

        public double NightAccuracyMod(IShipDataCustom ship) => Formation switch
        {
            FormationType.LineAhead => 1,

            FormationType.DoubleLine => 0.9,

            FormationType.Echelon => 0.8,
            FormationType.LineAbreast => 0.8,

            FormationType.Diamond => 0.7,

            _ => 1
        };

        public double AirstrikeAccuracyMod()
        {
            switch (Formation)
            {
                // should be 1 for all
                default:
                    return 1;
            }
        }





        public double ShellingEvasionMod => Formation switch
        {
            FormationType.LineAhead => 1,
            FormationType.DoubleLine => 1,
            FormationType.Diamond => 1.1,
            FormationType.Echelon => 1.2,
            FormationType.LineAbreast => 1.3,

            _ => 1
        };

        public double TorpedoEvasionMod()
        {
            switch (Formation)
            {
                case FormationType.LineAhead:
                case FormationType.DoubleLine:
                default:
                    return 1;

                case FormationType.Diamond:
                    return 1.1;

                case FormationType.Echelon:
                    return 1.3;

                case FormationType.LineAbreast:
                    return 1.4;
            }
        }

        public double AswEvasionMod => Formation switch
        {
            FormationType.LineAhead => 1,
            FormationType.DoubleLine => 1,
            FormationType.Diamond => 1,
            FormationType.LineAbreast => 1.1,
            FormationType.Echelon => 1.3,

            _ => 1
        };

        public double NightEvasionMod => Formation switch
        {
            FormationType.LineAhead => 1,
            FormationType.DoubleLine => 1,
            FormationType.Diamond => 1,
            FormationType.Echelon => 1.1,
            FormationType.LineAbreast => 1.2,

            _ => 1
        };

        public double AirstrikeEvasionMod => 1;
    }
}
