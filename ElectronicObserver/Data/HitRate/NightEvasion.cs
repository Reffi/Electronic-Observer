using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.ControlWpf;

namespace ElectronicObserver.Data.HitRate
{
    public interface INightEvasionShip<out TEquipType> : IEvasionShip<TEquipType> where TEquipType:IEvasionEquipment
    {
        ShipTypes ShipType { get; }
    }

    public interface INightEvasionBattle
    {
        bool ActivatedSearchlight { get; }
    }

    class NightEvasion: EvasionBase
    {
        private INightEvasionShip<IEvasionEquipment> Ship { get; }
        private IEvasionFleet Fleet { get; }
        private INightEvasionBattle Battle { get; }

        public NightEvasion(INightEvasionShip<IEvasionEquipment> ship, IEvasionFleet fleet, 
            INightEvasionBattle battle) : base(ship)
        {
            Ship = ship;
            Fleet = fleet;
            Battle = battle;
        }

        protected override double PrecapMods => FleetMod;
        protected override double PostcapMod => SearchlightMod;
        protected override double PostcapBonus => HeavyCruiserBonus;

        private double FleetMod => Fleet.Formation switch
        {
            FormationType.LineAhead => 1,
            FormationType.DoubleLine => 1,
            FormationType.Diamond => 1,
            FormationType.Echelon => 1.1,
            FormationType.LineAbreast => 1.2,

            _ => 1
        };

        private double SearchlightMod => Battle.ActivatedSearchlight ? 0.2 : 1;

        private double HeavyCruiserBonus => Ship.ShipType switch
        {
            ShipTypes.HeavyCruiser => 5,
            ShipTypes.AviationCruiser => 5,

            _ => 0
        };
    }
}
