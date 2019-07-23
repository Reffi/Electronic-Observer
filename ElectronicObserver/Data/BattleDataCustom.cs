using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data
{
    public class BattleDataCustom
    {
        public Damage Damage { get; }
        public Accuracy Accuracy { get; }

        public EngagementTypes Engagement { get; set; }

        public FleetDataCustom PlayerFleet { get; set; }
        public FleetDataCustom AbyssalFleet { get; set; }

        public int AttackerFleetIndex { get; set; }
        public int DefenderFleetIndex { get; set; }

        public DayAttackKind DayAttack { get; set; }
        public NightAttackKind NightAttack { get; set; }

        public BattleDataCustom(FleetDataCustom playerFleet, FleetDataCustom abyssalFleet, 
            EngagementTypes engagement = EngagementTypes.Parallel)
        {
            PlayerFleet = playerFleet;
            AbyssalFleet = abyssalFleet;

            Engagement = engagement;

            Random rng = new Random();
            IEnumerable<IShipDataCustom> shellingOrder = PlayerFleet.MainFleet
                .GroupBy(ship => ship.BaseRange)
                .OrderByDescending(group => group.Key) // order range groups from longest to shortest
                .Select(group => group.OrderBy(ship => rng.Next())) // random order within each range group
                .SelectMany(shipList => shipList) // flatten groups
                .ToList();

            Damage = new Damage(shellingOrder.First(), battle: this, attackerFleet: playerFleet, 
                defenderFleet: abyssalFleet);
            Accuracy = new Accuracy(shellingOrder.First(), playerFleet, this);
        }

        public double EngagementMod => Engagement switch
        {
            EngagementTypes.TAdvantage => 1.2,
            EngagementTypes.Parallel => 1,
            EngagementTypes.HeadOn => 0.8,
            EngagementTypes.TDisadvantage => 0.6,
            _ => 1
        };
    }
}
