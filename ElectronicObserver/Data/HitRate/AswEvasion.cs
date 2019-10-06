using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.HitRate
{
    class AswEvasion: EvasionBase
    {
        private IEvasionShip<IEvasionEquipment> Ship { get; }
        private IEvasionFleet Fleet { get; }

        public AswEvasion(IEvasionShip<IEvasionEquipment> ship, IEvasionFleet fleet) : base(ship)
        {
            Fleet = fleet;
        }

        protected override double PrecapMods => FleetMod;
        protected override double PostcapMod => 1;
        protected override double PostcapBonus => 0;

        public double FleetMod => Fleet.Formation switch
        {
            FormationType.LineAhead => 1,
            FormationType.DoubleLine => 1,
            FormationType.Diamond => 1,
            FormationType.LineAbreast => 1.1,
            FormationType.Echelon => 1.3,

            _ => 1
        };
    }
}
