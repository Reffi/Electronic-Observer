using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{
    public class Evasion
    {
        private IShipDataCustom _ship;
        private FleetDataCustom _fleet;
        private bool _activatedSearchlight;

        private double _precapBase;
        private double _precapMods;
        private double _critMod;
        private double _postcapMods;

        private double Capped => Cap(_precapBase * _precapMods);

        private double Postcap => Math.Floor(SearchlightMod 
                                               * (Math.Floor(Capped) 
                                                  + TorpedoEvasion 
                                                  + HeavyCruiserBonus 
                                                  + FuelBonus));

        public double Shelling
        {
            get
            {
                _precapBase = PrecapBaseShelling;
                _precapMods = PrecapModsShelling;

                return Postcap;
            }
        }

        public double ASW
        {
            get
            {
                _precapBase = PrecapBaseASW;
                _precapMods = PrecapModsASW;

                return Postcap;
            }
        }

        public double Night
        {
            get
            {
                _precapBase = PrecapBaseNight;
                _precapMods = PrecapModsNight;

                return Postcap;
            }
        }

        public Evasion(IShipDataCustom ship, FleetDataCustom fleet = null, bool activatedSearchlight = false)
        {
            _ship = ship;

            _fleet = fleet ?? new FleetDataCustom();

            _activatedSearchlight = activatedSearchlight;
        }

        private double PrecapBaseShelling =>
            _ship.BaseEvasion
            + _ship.Equipment.Where(eq => eq != null).Sum(eq => eq.BaseEvasion)
            + Math.Sqrt(2 * _ship.BaseLuck);

        private double PrecapModsShelling => _fleet.ShellingEvasionMod;



        private double PrecapBaseASW =>
            _ship.BaseEvasion
            + _ship.Equipment.Where(eq => eq != null).Sum(eq => eq.BaseEvasion)
            + Math.Sqrt(2 * _ship.BaseLuck);

        private double PrecapModsASW => _fleet.AswEvasionMod;


        private double PrecapBaseNight =>
            _ship.BaseEvasion
            + _ship.Equipment.Where(eq => eq != null).Sum(eq => eq.BaseEvasion)
            + Math.Sqrt(2 * _ship.BaseLuck);

        private double PrecapModsNight => _fleet.NightEvasionMod;



        private double SearchlightMod => _activatedSearchlight ? 0.2 : 1;
        private double TorpedoEvasion => 0;
        private double HeavyCruiserBonus => 0;
        // potentially misleading since it can only be 0 or negative
        private double FuelBonus => Math.Min(100 - 75, 0);

        private double Cap(double evasion) => evasion switch
        {
            double ev when ev > 64 => 55 + 2 * Math.Sqrt(evasion - 65),
            double ev when ev > 39 => 40 + 3 * Math.Sqrt(evasion - 40),
            double ev => ev
        };
    }
}
