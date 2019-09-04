using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Data.Damage;
using ElectronicObserver.Data.HitRate;
using ElectronicObserver.Utility.Helpers;
using ElectronicObserver.Window.ControlWpf;

namespace ElectronicObserver.Window.Dialog
{
    /// <summary>
    /// Interaction logic for DialogShipSimulationWpf.xaml
    /// </summary>
    public partial class DialogShipSimulationWpf : UserControl
    {
        // todo fix switch expression indents once R# fixes it

        public static readonly RoutedEvent CalculationParametersChangedEvent = EventManager.RegisterRoutedEvent(
            "CalculationParametersChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler),
            typeof(DialogShipSimulationWpf));

        private ShipDataCustom Attacker => AttackerDisplay.Ship;
        private ShipDataCustom Defender => DefenderDisplay.Ship;
        private FleetDataCustom AttackerFleet { get; set; }
        private FleetDataCustom DefenderFleet { get; set; }
        private BattleDataCustom Battle { get; set; }
        private ExtraDamageBonus Bonus => BonusParameters.Bonus;

        int DefaultShipID = -1;


        IEnumerable<ShipDataCustom> ShipList;
        IEnumerable<EquipmentDataCustom> EquipmentList;

        private IEnumerable<ShipDataCustom> EnemyShipList;

        public DialogShipSimulationWpf()
        {
            InitializeComponent();

            AttackerDisplay.AddHandler(EquipmentSelectionItem.EquipmentSelectionEvent,
                new RoutedEventHandler(UpdatePossibleAttacks));
            AttackerDisplay.AddHandler(ShipSelectionItem.ShipSelectionEvent,
                new RoutedEventHandler(UpdatePossibleAttacks));

            DefenderDisplay.AddHandler(EquipmentSelectionItem.EquipmentSelectionEvent,
                new RoutedEventHandler(UpdatePossibleAttacks));
            DefenderDisplay.AddHandler(ShipSelectionItem.ShipSelectionEvent,
                new RoutedEventHandler(UpdatePossibleAttacks));

            AttackerDisplay.AddHandler(CalculationParametersChangedEvent, new RoutedEventHandler(Calculate));
            ExternalParameters.AddHandler(CalculationParametersChangedEvent, new RoutedEventHandler(Calculate));

            DefenderDisplay.AddHandler(CalculationParametersChangedEvent, new RoutedEventHandler(Calculate));
            EnemyExternalParameters.AddHandler(CalculationParametersChangedEvent, new RoutedEventHandler(Calculate));

            BonusParameters.AddHandler(CalculationParametersChangedEvent, new RoutedEventHandler(Calculate));

            ShipList = KCDatabase.Instance.Ships.Values.Select(x => new ShipDataCustom(x));
            EquipmentList = KCDatabase.Instance.Equipments.Values.Select(x => new EquipmentDataCustom(x));

            EnemyShipList = KCDatabase.Instance.MasterShips.Values
                .Where(ship => ship.IsAbyssalShip)
                .Select(x => new ShipDataCustom(x));

            DataContext = this;
        }

        public DialogShipSimulationWpf(int shipID) : this()
        {
            DefaultShipID = shipID;
        }

        private BattleStatDisplay[] powerDisplays;
        private BattleStatDisplay[] accuracyDisplays;

        private BattleStatDisplay[] damageDisplays;
        private BattleStatDisplay[] hitRateDisplays;

        private void DialogShipSimulation_Load(object sender, RoutedEventArgs e)
        {
            if (!ShipList.Any())
            {
                //MessageBox.Show("No ships available.\r\nPlease return to the home port page.", "Ships Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Close();
                return;
            }

            BonusParameters.Bonus = new ExtraDamageBonus();

            AttackerDisplay.Ship = ShipList.First(x => x.MasterID == DefaultShipID);
            AllPossibleAttacks();
            BattlePossibleAttacks();

            DefenderDisplay.Ship = new ShipDataCustom(
                KCDatabase.Instance.MasterShips.Values.First(x => x.ID == 1572));

            AttackerDisplay.Ships = ShipList;
            AttackerDisplay.Equipments = EquipmentList;

            DefenderDisplay.Ships = EnemyShipList;


            UpdatePossibleAttacks(null, null);
        }

        private List<DayAttackKind> DayAttacks { get; set; } = new List<DayAttackKind>();
        private List<DayAttackKind> AswAttacks { get; set; } = new List<DayAttackKind>();
        private List<NightAttackKind> NightAttacks { get; set; } = new List<NightAttackKind>();
        private List<CvnciKind> CvnciAttacks { get; set; } = new List<CvnciKind>();

        private List<DayAttackKind> BattleDayAttacks { get; set; } = new List<DayAttackKind>();
        private List<DayAttackKind> BattleAswAttacks { get; set; } = new List<DayAttackKind>();
        private List<NightAttackKind> BattleNightAttacks { get; set; } = new List<NightAttackKind>();
        private List<CvnciKind> BattleCvnciAttacks { get; set; } = new List<CvnciKind>();

        private void AllPossibleAttacks()
        {
            DayAttacks = AttackerDisplay.Ship.DayAttacks.ToList();
            AswAttacks = AttackerDisplay.Ship.AswAttacks.ToList();
            NightAttacks = AttackerDisplay.Ship.NightAttacks.ToList();
            CvnciAttacks = AttackerDisplay.Ship.Cvncis();

            int dayAttackCount = DayAttacks.Count;
            int aswAttackCount = AswAttacks.Count;
            int nightAttackCount = NightAttacks.Count + CvnciAttacks.Count;

            int displayCount = 0;

            displayCount += dayAttackCount > 0 ? dayAttackCount + 1 : 0;
            displayCount += aswAttackCount > 0 ? aswAttackCount + 1 : 0;
            displayCount += nightAttackCount > 0 ? nightAttackCount + 1 : 0;

            powerDisplays = new BattleStatDisplay[displayCount];
            accuracyDisplays = new BattleStatDisplay[displayCount];

            for (int i = 0; i < displayCount; i++)
            {
                powerDisplays[i] = new BattleStatDisplay();
                accuracyDisplays[i] = new BattleStatDisplay();
            }

            PowerDisplay.ItemsSource = powerDisplays;
            AccuracyDisplay.ItemsSource = accuracyDisplays;
        }

        private void BattlePossibleAttacks()
        {
            BattleDayAttacks = AttackerDisplay.Ship.DayAttacks.ToList();
            BattleAswAttacks = AttackerDisplay.Ship.AswAttacks.ToList();
            BattleNightAttacks = AttackerDisplay.Ship.NightAttacks.ToList();

            int dayAttackCount = BattleDayAttacks.Count;
            int aswAttackCount = BattleAswAttacks.Count;
            int nightAttackCount = BattleNightAttacks.Count;

            int displayCount = 0;

            displayCount += dayAttackCount > 0 ? dayAttackCount + 1 : 0;
            displayCount += aswAttackCount > 0 ? aswAttackCount + 1 : 0;
            displayCount += nightAttackCount > 0 ? nightAttackCount + 1 : 0;

            damageDisplays = new BattleStatDisplay[displayCount];
            hitRateDisplays = new BattleStatDisplay[displayCount];

            for (int i = 0; i < displayCount; i++)
            {
                damageDisplays[i] = new BattleStatDisplay();
                hitRateDisplays[i] = new BattleStatDisplay();
            }

            DamageDisplay.ItemsSource = damageDisplays;
            HitRateDisplay.ItemsSource = hitRateDisplays;
        }

        private void UpdatePossibleAttacks(object sender, RoutedEventArgs e)
        {
            if (AttackerDisplay.Ship == null || DefenderDisplay.Ship == null)
                return;

            AllPossibleAttacks();
            BattlePossibleAttacks();

            Calculate(sender, e);
        }

        private void Calculate(object sender, RoutedEventArgs e)
        {
            if (e != null)
                e.Handled = true;

            if (Attacker == null || Defender == null)
                return;

            Battle = new BattleDataCustom
            {
                Engagement = ExternalParameters.Parameters.Engagement.Member,
            };

            AttackerFleet = new FleetDataCustom
            {
                Type = ExternalParameters.Parameters.Fleet.Member,
                Formation = ExternalParameters.Parameters.Formation.Member,
                PositionDetail = ExternalParameters.Parameters.PositionDetails.Member,

                NightRecon = ExternalParameters.Parameters.NightRecon,
                Flare = ExternalParameters.Parameters.Flare,
                Searchlight = ExternalParameters.Parameters.Searchlight
            };

            DefenderFleet = new FleetDataCustom
            {
                Type = EnemyExternalParameters.Parameters.Fleet.Member,
                Formation = EnemyExternalParameters.Parameters.Formation.Member,
                PositionDetail = EnemyExternalParameters.Parameters.PositionDetails.Member,

                NightRecon = EnemyExternalParameters.Parameters.NightRecon,
                Flare = EnemyExternalParameters.Parameters.Flare,
                Searchlight = EnemyExternalParameters.Parameters.Searchlight
            };

            DamageBase shelling;
            AswDamage asw;
            DamageBase night;

            if (Attacker.IsCarrier)
            {
                shelling = new CarrierShellingDamage(Attacker, AttackerFleet, Battle, Defender, DefenderFleet, Bonus);
                night = new CarrierNightDamage(Attacker, AttackerFleet, Battle, Defender, DefenderFleet, Bonus);
            }
            else
            {
                shelling = new ShellingDamage(Attacker, AttackerFleet, Battle, Defender, DefenderFleet, Bonus);
                night = new NightDamage(Attacker, AttackerFleet, Battle, Defender, DefenderFleet, Bonus);
            }

            asw = new AswDamage(Attacker, AttackerFleet, Battle, Defender, DefenderFleet, Bonus);

            ShellingAccuracy shellingAccuracy = new ShellingAccuracy(Attacker, AttackerFleet, Battle);
            AswAccuracy aswAccuracy = new AswAccuracy(Attacker, AttackerFleet, Battle);
            NightAccuracy nightAccuracy = new NightAccuracy(Attacker, AttackerFleet, Battle);

            PowerDisplays(shelling, asw, night);
            DamageDisplays(shelling, asw, night);
            AccuracyDisplays(shellingAccuracy, aswAccuracy, nightAccuracy);
            HitRateDisplays(shellingAccuracy, aswAccuracy, nightAccuracy);
        }

        private void PowerDisplays(DamageBase shelling, AswDamage asw, DamageBase night)
        {
            int i = 0;

            // don't like this
            double normal;
            double critical;

            if (DayAttacks.Any())
            {
                powerDisplays[i].Separator = Properties.DialogShipSimulationWpf.DayShellingPower;
                i++;

                foreach (DayAttackKind dayAttack in DayAttacks)
                {
                    Battle.DayAttack = dayAttack;

                    Battle.HitType = HitType.Hit;
                    normal = dayAttack == DayAttackKind.Shelling ? shelling.Capped : shelling.Postcap;

                    Battle.HitType = HitType.Critical;
                    critical = shelling.Postcap;

                    powerDisplays[i].AttackName = $"{dayAttack.Display()}";
                    powerDisplays[i].Value = $"{normal:0.##} ({critical:0.##})";

                    i++;
                }
            }

            if (AswAttacks.Any())
            {
                powerDisplays[i].Separator = Properties.DialogShipSimulationWpf.AswPower;
                i++;

                foreach (DayAttackKind dayAttack in AswAttacks)
                {
                    Battle.DayAttack = dayAttack;

                    Battle.HitType = HitType.Hit;
                    normal = asw.Postcap;

                    Battle.HitType = HitType.Critical;
                    critical = asw.Postcap;

                    powerDisplays[i].AttackName = $"{dayAttack.Display()}";
                    powerDisplays[i].Value = $"{normal:0.##} ({critical:0.##})";

                    i++;
                }
            }

            if (NightAttacks.Any() || CvnciAttacks.Any())
            {
                powerDisplays[i].Separator = Properties.DialogShipSimulationWpf.NightShellingPower;
                i++;

                foreach (CvnciKind cvnci in CvnciAttacks)
                {
                    Battle.CvnciKind = cvnci;

                    Battle.HitType = HitType.Hit;
                    normal = night.Postcap;

                    Battle.HitType = HitType.Critical;
                    critical = night.Postcap;

                    powerDisplays[i].AttackName = $"{cvnci.Display()}";
                    powerDisplays[i].Value = $"{normal:0.##} ({critical:0.##})";

                    i++;
                }

                foreach (NightAttackKind nightAttack in NightAttacks)
                {
                    Battle.NightAttack = nightAttack;
                    Battle.CvnciKind = CvnciKind.Unknown;

                    Battle.HitType = HitType.Hit;
                    normal = night.Postcap;

                    Battle.HitType = HitType.Critical;
                    critical = night.Postcap;

                    powerDisplays[i].AttackName = $"{nightAttack.Display()}";
                    powerDisplays[i].Value = $"{normal:0.##} ({critical:0.##})";

                    i++;
                }
            }

        }

        private void AccuracyDisplays(ShellingAccuracy shellingAccuracy, AswAccuracy aswAccuracy,
            NightAccuracy nightAccuracy)
        {
            int i = 0;

            if (DayAttacks.Any())
            {
                accuracyDisplays[i].Separator = Properties.DialogShipSimulationWpf.DayShellingAccuracy;
                i++;

                foreach (DayAttackKind dayAttack in DayAttacks)
                {
                    Battle.DayAttack = dayAttack;

                    accuracyDisplays[i].AttackName = $"{dayAttack.Display()}";
                    accuracyDisplays[i].Value = $"{shellingAccuracy.Total:0.##}";

                    i++;
                }
            }

            if (AswAttacks.Any())
            {
                accuracyDisplays[i].Separator = Properties.DialogShipSimulationWpf.AswAccuracy;
                i++;

                foreach (DayAttackKind dayAttack in AswAttacks)
                {
                    Battle.DayAttack = dayAttack;

                    accuracyDisplays[i].AttackName = $"{dayAttack.Display()}";
                    accuracyDisplays[i].Value = $"{aswAccuracy.Total:0.##}";

                    i++;
                }
            }

            if (NightAttacks.Any())
            {
                accuracyDisplays[i].Separator = Properties.DialogShipSimulationWpf.NightShellingAccuracy;
                i++;

                foreach (NightAttackKind nightAttack in NightAttacks)
                {
                    Battle.NightAttack = nightAttack;

                    accuracyDisplays[i].AttackName = $"{nightAttack.Display()}";
                    accuracyDisplays[i].Value = $"{nightAccuracy.Total:0.##}";

                    i++;
                }
            }
        }

        private void DamageDisplays(DamageBase shelling, AswDamage asw, DamageBase night)
        {
            int i = 0;

            double normalMin;
            double normalMax;
            double criticalMin;
            double criticalMax;

            if (BattleDayAttacks.Any())
            {
                damageDisplays[i].Separator = Properties.DialogShipSimulationWpf.DayShellingDamage;
                i++;

                foreach (DayAttackKind dayAttack in BattleDayAttacks)
                {
                    Battle.DayAttack = dayAttack;

                    Battle.HitType = HitType.Hit;
                    normalMin = shelling.Min;
                    normalMax = shelling.Max;

                    Battle.HitType = HitType.Critical;
                    criticalMin = shelling.Min;
                    criticalMax = shelling.Max;

                    damageDisplays[i].AttackName = $"{dayAttack.Display()}";
                    damageDisplays[i].Value =
                        $"{normalMin:0.##}~{normalMax:0.##} ({criticalMin:0.##}~{criticalMax:0.##})";

                    i++;
                }
            }

            if (BattleAswAttacks.Any())
            {
                damageDisplays[i].Separator = Properties.DialogShipSimulationWpf.AswDamage;
                i++;

                foreach (DayAttackKind dayAttack in BattleAswAttacks)
                {
                    Battle.DayAttack = dayAttack;

                    Battle.HitType = HitType.Hit;
                    normalMin = asw.Min;
                    normalMax = asw.Max;

                    Battle.HitType = HitType.Critical;
                    criticalMin = asw.Min;
                    criticalMax = asw.Max;

                    damageDisplays[i].AttackName = $"{dayAttack.Display()}";
                    damageDisplays[i].Value =
                        $"{normalMin:0.##}~{normalMax:0.##} ({criticalMin:0.##}~{criticalMax:0.##})";

                    i++;
                }
            }

            if (BattleNightAttacks.Any())
            {
                damageDisplays[i].Separator = Properties.DialogShipSimulationWpf.NightShellingDamage;
                i++;

                foreach (NightAttackKind nightAttack in BattleNightAttacks)
                {
                    Battle.NightAttack = nightAttack;

                    Battle.HitType = HitType.Hit;
                    normalMin = night.Min;
                    normalMax = night.Max;

                    Battle.HitType = HitType.Critical;
                    criticalMin = night.Min;
                    criticalMax = night.Max;

                    damageDisplays[i].AttackName = $"{nightAttack.Display()}";
                    damageDisplays[i].Value =
                        $"{normalMin:0.##}~{normalMax:0.##} ({criticalMin:0.##}~{criticalMax:0.##})";

                    i++;
                }
            }
        }

        private void HitRateDisplays(ShellingAccuracy shellingAccuracy, AswAccuracy aswAccuracy,
            NightAccuracy nightAccuracy)
        {
            int i = 0;

            EvasionBase shellingEvasion = new ShellingEvasion(Defender, DefenderFleet);
            EvasionBase aswEvasion = new AswEvasion(Defender, DefenderFleet);
            EvasionBase nightEvasion = new NightEvasion(Defender, DefenderFleet, Battle);

            HitRate shellingHitRate = new HitRate(shellingAccuracy, shellingEvasion, Defender);
            HitRate aswHitRate = new HitRate(aswAccuracy, aswEvasion, Defender);
            HitRate nightHitRate = new HitRate(nightAccuracy, nightEvasion, Defender);

            if (BattleDayAttacks.Any())
            {
                hitRateDisplays[i].Separator = Properties.DialogShipSimulationWpf.DayShellingHitRate;
                i++;

                foreach (DayAttackKind dayAttack in BattleDayAttacks)
                {
                    Battle.DayAttack = dayAttack;

                    hitRateDisplays[i].AttackName = $"{dayAttack.Display()}";
                    hitRateDisplays[i].Value = $"{shellingHitRate.Capped:0.##}%";

                    i++;
                }
            }

            if (BattleAswAttacks.Any())
            {
                hitRateDisplays[i].Separator = Properties.DialogShipSimulationWpf.AswHitRate;
                i++;

                foreach (DayAttackKind dayAttack in BattleAswAttacks)
                {
                    Battle.DayAttack = dayAttack;

                    hitRateDisplays[i].AttackName = $"{dayAttack.Display()}";
                    hitRateDisplays[i].Value = $"{aswHitRate.Capped:0.##}%";

                    i++;
                }
            }

            if (BattleNightAttacks.Any())
            {
                hitRateDisplays[i].Separator = Properties.DialogShipSimulationWpf.NightShellingHitRate;
                i++;

                foreach (NightAttackKind nightAttack in BattleNightAttacks)
                {
                    Battle.NightAttack = nightAttack;

                    hitRateDisplays[i].AttackName = $"{nightAttack.Display()}";
                    hitRateDisplays[i].Value = $"{nightHitRate.Capped:0.##}%";

                    i++;
                }
            }
        }

    }
}