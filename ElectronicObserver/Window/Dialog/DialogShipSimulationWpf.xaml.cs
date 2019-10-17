using System;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ElectronicObserver.Data.Damage;
using ElectronicObserver.Data.HitRate;
using ElectronicObserver.Utility.Helpers;
using ElectronicObserver.Window.ControlWpf;

namespace ElectronicObserver.Window.Dialog
{
    /// <summary>
    /// Interaction logic for DialogShipSimulationWpf.xaml
    /// </summary>
    public partial class DialogShipSimulationWpf
    {
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
        }

        public DialogShipSimulationWpf(int shipID) : this()
        {
            DefaultShipID = shipID;
        }

        private BattleStatDisplay[] powerDisplays;
        private BattleStatDisplay[] accuracyDisplays;

        private BattleStatDisplay[] damageDisplays;
        private BattleStatDisplay[] hitRateDisplays;

        private void DialogShipSimulation_Loaded(object sender, RoutedEventArgs e)
        {
            // todo make a language select setting
            Properties.Resources.Culture = new System.Globalization.CultureInfo("en");

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

            ShipList = KCDatabase.Instance.Ships.Values.Select(x => new ShipDataCustom(x)).ToList();
            EquipmentList = KCDatabase.Instance.Equipments.Values.Select(x => new EquipmentDataCustom(x));

            EnemyShipList = KCDatabase.Instance.MasterShips.Values
                .Where(ship => ship.IsAbyssalShip)
                .Select(x => new ShipDataCustom(x));

            DataContext = this;

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
                KCDatabase.Instance.MasterShips.Values.First(x => x.ID == 1501));

            AttackerDisplay.Ships = ShipList;
            AttackerDisplay.Equipments = EquipmentList;

            DefenderDisplay.Ships = EnemyShipList;


            UpdatePossibleAttacks(null, null);
        }

        private List<DayAttackKind> DayAttacks { get; set; } = new List<DayAttackKind>();
        private List<DayAirAttackCutinKind> CvciAttacks { get; set; } = new List<DayAirAttackCutinKind>();
        private List<DayAttackKind> AswAttacks { get; set; } = new List<DayAttackKind>();
        private List<NightAttackKind> NightAttacks { get; set; } = new List<NightAttackKind>();
        private List<CvnciKind> CvnciAttacks { get; set; } = new List<CvnciKind>();

        private IEnumerable<AttackKindData> BattleDayAttacks { get; set; } = Enumerable.Empty<AttackKindData>();
        private IEnumerable<AttackKindData> BattleAswAttacks { get; set; } = Enumerable.Empty<AttackKindData>();
        private IEnumerable<AttackKindData> BattleNightAttacks { get; set; } = Enumerable.Empty<AttackKindData>();
        private IEnumerable<AttackKindData> BattleCvnciAttacks { get; set; } = Enumerable.Empty<AttackKindData>();

        private void AllPossibleAttacks()
        {
            DayAttacks = Attacker.DayAttacks.ToList();
            CvciAttacks = Attacker.Cvcis();
            AswAttacks = Attacker.AswAttacks.ToList();
            NightAttacks = Attacker.NightAttacks.ToList();
            CvnciAttacks = Attacker.Cvncis();

            int dayAttackCount = DayAttacks.Count + CvciAttacks.Count;
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
            BattleDayAttacks = AttackFilter(Attacker, Defender, Attacker.DayAttacks.Select(a => new AttackKindData(a)));
            BattleAswAttacks = AttackFilter(Attacker, Defender, Attacker.AswAttacks.Select(a => new AttackKindData(a)));
            BattleNightAttacks =
                AttackFilter(Attacker, Defender, Attacker.NightAttacks.Select(a => new AttackKindData(a)));
            BattleCvnciAttacks = AttackFilter(Attacker, Defender, Attacker.Cvncis().Select(a => new AttackKindData(a)));

            // todo possible attack filter

            int dayAttackCount = BattleDayAttacks.Count();
            int aswAttackCount = BattleAswAttacks.Count();
            int nightAttackCount = BattleNightAttacks.Count() + BattleCvnciAttacks.Count();

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
            if (Attacker == null || Defender == null) return;

            AllPossibleAttacks();
            BattlePossibleAttacks();

            BonusParameters.Bonus = new AntiInstallationDamage(Attacker, Defender).ShellingBonus();

            Calculate(sender, e);
        }

        private void Calculate(object sender, RoutedEventArgs e)
        {
            if (e != null)
            {
                e.Handled = true;
            }

            if (Attacker == null || Defender == null) return;

            if (ExternalParameters.Parameters == null) return;

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

            if (DayAttacks.Any() || CvciAttacks.Any())
            {
                powerDisplays[i].Separator = Properties.DialogShipSimulationWpf.LabelDayShellingPower;
                i++;

                foreach (DayAirAttackCutinKind cvci in CvciAttacks)
                {
                    Battle.CvciKind = cvci;

                    Battle.HitType = HitType.Hit;
                    normal = shelling.Postcap;

                    Battle.HitType = HitType.Critical;
                    critical = shelling.Postcap;

                    powerDisplays[i].AttackName = $"{cvci.Display()}";
                    powerDisplays[i].Value = $"{normal:0.##} ({critical:0.##})";

                    i++;
                }

                foreach (DayAttackKind dayAttack in DayAttacks)
                {
                    Battle.DayAttack = dayAttack;

                    Battle.HitType = HitType.Hit;
                    normal = shelling.Postcap;

                    Battle.HitType = HitType.Critical;
                    critical = shelling.Postcap;

                    powerDisplays[i].AttackName = $"{dayAttack.Display()}";
                    powerDisplays[i].Value = $"{normal:0.##} ({critical:0.##})";

                    i++;
                }
            }

            if (AswAttacks.Any())
            {
                powerDisplays[i].Separator = Properties.DialogShipSimulationWpf.LabelAswPower;
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
                powerDisplays[i].Separator = Properties.DialogShipSimulationWpf.LabelNightShellingPower;
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
                accuracyDisplays[i].Separator = Properties.DialogShipSimulationWpf.LabelDayShellingAccuracy;
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
                accuracyDisplays[i].Separator = Properties.DialogShipSimulationWpf.LabelAswAccuracy;
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
                accuracyDisplays[i].Separator = Properties.DialogShipSimulationWpf.LabelNightShellingAccuracy;
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
                damageDisplays[i].Separator = Properties.DialogShipSimulationWpf.LabelDayShellingDamage;
                i++;

                foreach (AttackKindData attack in BattleDayAttacks)
                {
                    DayAttackKind dayAttack = (DayAttackKind) attack.Value;

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
                damageDisplays[i].Separator = Properties.DialogShipSimulationWpf.LabelAswDamage;
                i++;

                foreach (AttackKindData attack in BattleAswAttacks)
                {
                    DayAttackKind dayAttack = (DayAttackKind) attack.Value;

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
                damageDisplays[i].Separator = Properties.DialogShipSimulationWpf.LabelNightShellingDamage;
                i++;

                foreach (AttackKindData attack in BattleNightAttacks)
                {
                    NightAttackKind nightAttack = (NightAttackKind) attack.Value;

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
                hitRateDisplays[i].Separator = Properties.DialogShipSimulationWpf.LabelDayShellingHitRate;
                i++;

                foreach (AttackKindData attack in BattleDayAttacks)
                {
                    DayAttackKind dayAttack = (DayAttackKind) attack.Value;

                    Battle.DayAttack = dayAttack;

                    hitRateDisplays[i].AttackName = $"{dayAttack.Display()}";
                    hitRateDisplays[i].Value = $"{shellingHitRate.Capped:0.##}%";

                    i++;
                }
            }

            if (BattleAswAttacks.Any())
            {
                hitRateDisplays[i].Separator = Properties.DialogShipSimulationWpf.LabelAswHitRate;
                i++;

                foreach (AttackKindData attack in BattleAswAttacks)
                {
                    DayAttackKind dayAttack = (DayAttackKind) attack.Value;

                    Battle.DayAttack = dayAttack;

                    hitRateDisplays[i].AttackName = $"{dayAttack.Display()}";
                    hitRateDisplays[i].Value = $"{aswHitRate.Capped:0.##}%";

                    i++;
                }
            }

            if (BattleNightAttacks.Any())
            {
                hitRateDisplays[i].Separator = Properties.DialogShipSimulationWpf.LabelNightShellingHitRate;
                i++;

                foreach (AttackKindData attack in BattleNightAttacks)
                {
                    NightAttackKind nightAttack = (NightAttackKind) attack.Value;

                    Battle.NightAttack = nightAttack;

                    hitRateDisplays[i].AttackName = $"{nightAttack.Display()}";
                    hitRateDisplays[i].Value = $"{nightHitRate.Capped:0.##}%";

                    i++;
                }
            }
        }

        private IEnumerable<AttackKindData> AttackFilter(ShipDataCustom attacker, ShipDataCustom defender,
            IEnumerable<AttackKindData> attacks)
        {
            if (defender == null) return attacks;

            IEnumerable<AttackKindData> attackList = attacks;

            if (defender.IsSubmarine)
            {
                attackList = attackList.Where(a => a.CanHitSubmarine);
            }
            else
            {
                attackList = attackList.Where(a => !a.CanHitSubmarine);
            }

            if (defender.IsInstallation)
            {
                if (attacker.Equipment.Any(eq => eq?.CategoryType == EquipmentTypes.CarrierBasedBomber))
                    return Enumerable.Empty<AttackKindData>();

                attackList = attackList.Where(a => a.CanHitInstallation);
            }

            return attackList;
        }


    }
}
