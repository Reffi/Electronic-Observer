using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.HitRate
{
    public interface ITorpedoEvasionEquipment : IEvasionEquipment
    {
        double TorpedoEvasion { get; }
    }

    class TorpedoEvasion : EvasionBase
    {
        private IEvasionShip<ITorpedoEvasionEquipment> Ship { get; }
        private IEvasionFleet Fleet { get; }

        public TorpedoEvasion(IEvasionShip<ITorpedoEvasionEquipment> ship, IEvasionFleet fleet)
            : base(ship)
        {
            Ship = ship;
            Fleet = fleet;
        }

        protected override double PrecapMods => FleetMod;
        protected override double PostcapMod => 1;

        protected override double PostcapBonus =>
            Ship.Equipment.Where(eq => eq != null)
                .Sum(eq => eq.TorpedoEvasion);

        private double FleetMod => Fleet.Formation switch
        {
            FormationType.LineAhead => 1,
            FormationType.DoubleLine => 1,
            FormationType.Diamond => 1.1,
            FormationType.Echelon => 1.3,
            FormationType.LineAbreast => 1.4,

            _ => 1
        };
    }
}
