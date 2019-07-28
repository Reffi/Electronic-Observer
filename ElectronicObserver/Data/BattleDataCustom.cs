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
        private Random rng = new Random();

        // figure out better naming so property type and name aren't the same
        public Damage Damage { get; set; }
        public Accuracy Accuracy { get; set; }
        public Evasion Evasion { get; set; }
        public HitRate HitRate { get; set; }

        public EngagementTypes Engagement { get; set; }

        public FleetDataCustom PlayerFleet { get; set; }
        public FleetDataCustom AbyssalFleet { get; set; }

        public int AttackerFleetIndex { get; set; }
        public int DefenderFleetIndex { get; set; }

        public DayAttackKind DayAttack { get; set; }
        public NightAttackKind NightAttack { get; set; }

        public IShipDataCustom Attacker { get; set; }
        public IShipDataCustom Defender { get; set; }

        public BattleDataCustom(FleetDataCustom playerFleet, FleetDataCustom abyssalFleet, 
            EngagementTypes engagement = EngagementTypes.Parallel)
        {
            PlayerFleet = playerFleet;
            AbyssalFleet = abyssalFleet;

            Engagement = engagement;

            MockStuff();
        }

        private void MockStuff()
        {
            IEnumerable<IShipDataCustom> shellingOrder = MakeShellingOrder(PlayerFleet.MainFleet);
            IEnumerable<IShipDataCustom> enemyShellingOrder = MakeShellingOrder(AbyssalFleet.MainFleet);

            Attacker = shellingOrder.First();
            Defender = enemyShellingOrder.FirstOrDefault();

            Damage = new Damage(Attacker, Defender,
                this, PlayerFleet, AbyssalFleet);

            Accuracy = new Accuracy(Attacker, PlayerFleet, this);

            if (Defender == null)
                return;

            Evasion = new Evasion(Defender, AbyssalFleet);
            HitRate = new HitRate(Accuracy, Evasion, Defender.Condition);
        }

        // stupid even if it's just mock
        public IEnumerable<DayAttackKind> PossibleDayAttacks => (Attacker, Defender) switch
        {
            (_, _) when Defender.IsSubmarine => Attacker.AswAttacks,

            _ => Attacker.DayAttacks
        };

        public IEnumerable<DayAttackKind> PossibleAswAttacks => (Attacker, Defender) switch
        {
            (_, _) when Defender.IsSubmarine && Attacker.AswAttacks.Any() => Attacker.AswAttacks,

            _ => Enumerable.Empty<DayAttackKind>()
        };

        public IEnumerable<NightAttackKind> PossibleNightAttacks => (Attacker, Defender) switch
        {
            (_, _) when Defender.IsSubmarine => Enumerable.Empty<NightAttackKind>(),

            _ => Attacker.NightAttacks.Take(1)
        };

        private IEnumerable<IShipDataCustom> MakeShellingOrder(IEnumerable<IShipDataCustom> fleet) =>
            fleet
                .GroupBy(ship => ship.BaseRange)
                .OrderByDescending(group => group.Key) // order range groups from longest to shortest
                .Select(group => group.OrderBy(ship => rng.Next())) // random order within each range group
                .SelectMany(shipList => shipList) // flatten groups
                .ToList();

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
