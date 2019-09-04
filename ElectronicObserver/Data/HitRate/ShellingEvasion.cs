using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.HitRate
{
    public interface IShellingEvasionFleet
    {
        FormationType Formation { get; }
    }

    class ShellingEvasion : EvasionBase
    {
        private IEvasionShip<IEvasionEquipment> Ship { get; }
        private IShellingEvasionFleet Fleet { get; }

        public ShellingEvasion(IEvasionShip<IEvasionEquipment> ship,
            IShellingEvasionFleet fleet):base(ship)
        {
            Ship = ship;
            Fleet = fleet;
        }

        protected override double PrecapMods => FleetMod;
        protected override double PostcapMod => 1;
        protected override double PostcapBonus => 0;

        private double FleetMod => Fleet.Formation switch
        {
            FormationType.LineAhead => 1,
            FormationType.DoubleLine => 1,
            FormationType.Diamond => 1.1,
            FormationType.Echelon => 1.2,
            FormationType.LineAbreast => 1.3,

            _ => 1
        };
    }
}
