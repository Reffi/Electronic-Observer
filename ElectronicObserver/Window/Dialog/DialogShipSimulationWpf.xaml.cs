using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Utility.Extension_Methods;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.ControlWpf;
using ElectronicObserver.Window.ViewModel;

namespace ElectronicObserver.Window.Dialog
{
    /// <summary>
    /// Interaction logic for DialogShipSimulationWpf.xaml
    /// </summary>
    public partial class DialogShipSimulationWpf : UserControl
    {

        public static readonly RoutedEvent CalculationParametersChangedEvent = EventManager.RegisterRoutedEvent(
            "CalculationParametersChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(DialogShipSimulationWpf));

        FleetDataCustom fleet;
        FleetDataCustom enemyFleet;
        BattleDataCustom battle;

        int DefaultShipID = -1;


        IEnumerable<ShipDataCustom> ShipList;
        IEnumerable<IEquipmentDataCustom> EquipmentList;

        private IEnumerable<ShipDataCustom> EnemyShipList;

        public DialogShipSimulationWpf()
        {
            InitializeComponent();

            ShipDisplay.AddHandler(EquipmentSelectionItem.EquipmentSelectionEvent, new RoutedEventHandler(UpdatePossibleAttacks));
            ShipDisplay.AddHandler(ShipSelectionItem.ShipSelectionEvent, new RoutedEventHandler(UpdatePossibleAttacks));

            EnemyShipDisplay.AddHandler(EquipmentSelectionItem.EquipmentSelectionEvent, new RoutedEventHandler(UpdatePossibleAttacks));
            EnemyShipDisplay.AddHandler(ShipSelectionItem.ShipSelectionEvent, new RoutedEventHandler(UpdatePossibleAttacks));

            ShipDisplay.AddHandler(CalculationParametersChangedEvent, new RoutedEventHandler(Calculate));
            ExternalParameters.AddHandler(CalculationParametersChangedEvent, new RoutedEventHandler(Calculate));

            EnemyShipDisplay.AddHandler(CalculationParametersChangedEvent, new RoutedEventHandler(Calculate));
            EnemyExternalParameters.AddHandler(CalculationParametersChangedEvent, new RoutedEventHandler(Calculate));

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

            ShipDisplay.Ship = ShipList.First(x => x.MasterID == DefaultShipID);

            EnemyShipDisplay.Ship = new ShipDataCustom(
                KCDatabase.Instance.MasterShips.Values.First(x => x.ID == 1572));

            ShipDisplay.Ships = ShipList;
            ShipDisplay.Equipments = EquipmentList;

            EnemyShipDisplay.Ships = EnemyShipList;

            SetBattleAndFleet();
            UpdatePossibleAttacks(null, null);
        }

        private List<DayAttackKind> dayAttacks;
        private List<DayAttackKind> aswAttacks;
        private List<NightAttackKind> nightAttacks;

        private List<DayAttackKind> battleDayAttacks;
        private List<DayAttackKind> battleAswAttacks;
        private List<NightAttackKind> battleNightAttacks;

        private void AllPossibleAttacks()
        {
            dayAttacks = ShipDisplay.Ship.DayAttacks.ToList();
            aswAttacks = ShipDisplay.Ship.AswAttacks.ToList();
            nightAttacks = ShipDisplay.Ship.NightAttacks.ToList();

            int dayAttackCount = dayAttacks.Count;
            int aswAttackCount = aswAttacks.Count;
            int nightAttackCount = nightAttacks.Count;

            int displayCount = 0;

            displayCount += dayAttackCount > 0 ? dayAttackCount + 1 : 0;
            displayCount += aswAttackCount > 0 ? aswAttackCount + 1 : 0;
            displayCount += nightAttackCount > 0 ? nightAttackCount + 1 : 0;

            // +1 for capped
            powerDisplays = new BattleStatDisplay[displayCount + 1];
            accuracyDisplays = new BattleStatDisplay[displayCount];

            for (int i = 0; i < displayCount; i++)
            {
                powerDisplays[i] = new BattleStatDisplay();
                accuracyDisplays[i] = new BattleStatDisplay();
            }
            powerDisplays[powerDisplays.Length - 1] = new BattleStatDisplay();

            PowerDisplay.ItemsSource = powerDisplays;
            AccuracyDisplay.ItemsSource = accuracyDisplays;
        }

        private void BattlePossibleAttacks()
        {
            /*battleDayAttacks = battle.PossibleDayAttacks.ToList();
            battleAswAttacks = battle.PossibleAswAttacks.ToList();
            battleNightAttacks = battle.PossibleNightAttacks.ToList();*/

            battleDayAttacks = ShipDisplay.Ship.DayAttacks.ToList();
            battleAswAttacks = ShipDisplay.Ship.AswAttacks.ToList();
            battleNightAttacks     = ShipDisplay.Ship.NightAttacks.ToList();

             int dayAttackCount = battleDayAttacks.Count;
             int aswAttackCount = battleAswAttacks.Count;
             int nightAttackCount = battleNightAttacks.Count;

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
             if (ShipDisplay.Ship == null || EnemyShipDisplay.Ship == null || battle == null)
                 return;

             AllPossibleAttacks();
             BattlePossibleAttacks();

             Calculate(sender, e);
         }

         private void SetBattleAndFleet()
         {
             EnemyExternalParameters.EngagementSelect.IsEnabled = false;
             EnemyExternalParameters.EngagementSelect.SelectedItem = null;

             fleet = new FleetDataCustom(ExternalParameters.Parameters.Fleet.Member,
                 ExternalParameters.Parameters.Formation.Member)
             {
                 NightRecon = ExternalParameters.Parameters.NightRecon,
                 Flare = ExternalParameters.Parameters.Flare,
                 Searchlight = ExternalParameters.Parameters.Searchlight
             };
             fleet.AddToMain(ShipDisplay.Ship);

             enemyFleet = new FleetDataCustom(EnemyExternalParameters.Parameters.Fleet.Member,
                 EnemyExternalParameters.Parameters.Formation.Member)
             {
                 NightRecon = EnemyExternalParameters.Parameters.NightRecon,
                 Flare = EnemyExternalParameters.Parameters.Flare,
                 Searchlight = EnemyExternalParameters.Parameters.Searchlight
             };
             enemyFleet.AddToMain(EnemyShipDisplay.Ship);

             battle = new BattleDataCustom(fleet, enemyFleet, ExternalParameters.Parameters.Engagement.Member);

         }

         private void Calculate(object sender, RoutedEventArgs e)
         {
             if(e != null)
                 e.Handled = true;

             if (battle == null || ShipDisplay.Ship == null || EnemyShipDisplay.Ship == null)
                 return;

             SetBattleAndFleet();

             Damage damage = battle.Damage;
             Accuracy accuracy = battle.Accuracy;
             HitRate hitRate = battle.HitRate;



             PowerDisplays(damage);
             AccuracyDisplays(accuracy);

             DamageDisplays(damage);
             HitRateDisplays(hitRate);
             /*
             int i = 0;

             if (dayAttacks.Any())
             {
                 powerDisplays[i].Separator = "day power";
                 damageDisplays[i].Separator = "day damage";
                 i++;

                 powerDisplays[i].AttackName = "capped";
                 powerDisplays[i].Value = $"{damage.ShellingCapped:0.##}";
                 i++;

                 foreach (DayAttackKind dayAttack in dayAttacks)
                 {
                     battle.DayAttack = dayAttack;

                     powerDisplays[i].AttackName = $"{dayAttack.Display()}";
                     powerDisplays[i].Value = $"{damage.Shelling:0.##} ({damage.ShellingCrit:0.##})";

                     damageDisplays[i].AttackName = $"{dayAttack.Display()}";
                     damageDisplays[i].Value = $"{damage.ShellingMin:0.##}~{damage.ShellingMax:0.##} ({damage.ShellingCritMin:0.##}~{damage.ShellingCritMax:0.##})";

                     i++;
                 }
             }

             if (aswAttacks.Any())
             {
                 powerDisplays[i].Separator = "ASW";
                 damageDisplays[i].Separator = "ASW";
                 i++;

                 foreach (DayAttackKind dayAttack in aswAttacks)
                 {
                     battle.DayAttack = dayAttack;

                     powerDisplays[i].AttackName = $"{dayAttack.Display()}";
                     powerDisplays[i].Value = $"{damage.ASW:0.##} ({damage.AswCrit:0.##})";

                     damageDisplays[i].AttackName = $"{dayAttack.Display()}";
                     damageDisplays[i].Value = $"{damage.AswMin:0.##}~{damage.AswMax:0.##} ({damage.AswCritMin:0.##}~{damage.AswCritMax:0.##})";

                     i++;
                 }
             }

             if (nightAttacks.Any())
             {
                 powerDisplays[i].Separator = "night";
                 damageDisplays[i].Separator = "night";
                 i++;

                 foreach (NightAttackKind nightAttack in nightAttacks)
                 {
                     battle.NightAttack = nightAttack;

                     powerDisplays[i].AttackName = $"{nightAttack.Display()}";
                     powerDisplays[i].Value = $"{damage.Night:0.##} ({damage.NightCrit:0.##})";

                     damageDisplays[i].AttackName = $"{nightAttack.Display()}";
                     damageDisplays[i].Value = $"{damage.NightMin:0.##}~{damage.NightMax:0.##} ({damage.NightCritMin:0.##}~{damage.NightCritMax:0.##})";

                     i++;
                 }
             }

             i = 0;

             if (dayAttacks.Any())
             {
                 accuracyDisplays[i].Separator = "day accuracy";
                 hitRateDisplays[i].Separator = "day hit rate";
                 i++;

                 foreach (DayAttackKind dayAttack in dayAttacks)
                 {
                     battle.DayAttack = dayAttack;

                     accuracyDisplays[i].AttackName = $"{dayAttack.Display()}";
                     accuracyDisplays[i].Value = $"{accuracy.DayShelling:0.##}";

                     hitRateDisplays[i].AttackName = $"{dayAttack.Display()}";
                     hitRateDisplays[i].Value = $"{hitRate.ShellingCapped:0.##}%";

                     i++;
                 }
             }

             if (aswAttacks.Any())
             {
                 accuracyDisplays[i].Separator = "ASW accuracy";
                 hitRateDisplays[i].Separator = "ASW hit rate";
                 i++;

                 foreach (DayAttackKind dayAttack in aswAttacks)
                 {
                     battle.DayAttack = dayAttack;

                     accuracyDisplays[i].AttackName = $"{dayAttack.Display()}";
                     accuracyDisplays[i].Value = $"{accuracy.ASW:0.##}";

                     hitRateDisplays[i].AttackName = $"{dayAttack.Display()}";
                     hitRateDisplays[i].Value = $"{hitRate.AswCapped:0.##}%";

                     i++;
                 }
             }

             if (nightAttacks.Any())
             {
                 accuracyDisplays[i].Separator = "night accuracy";
                 hitRateDisplays[i].Separator = "night hit rate";
                 i++;

                 foreach (NightAttackKind nightAttack in nightAttacks)
                 {
                     battle.NightAttack = nightAttack;

                     accuracyDisplays[i].AttackName = $"{nightAttack.Display()}";
                     accuracyDisplays[i].Value = $"{accuracy.Night:0.##}";

                     hitRateDisplays[i].AttackName = $"{nightAttack.Display()}";
                     hitRateDisplays[i].Value = $"{hitRate.NightCapped:0.##}";

                     i++;
                 }
             }
             */
            return;
        }

        private void PowerDisplays(Damage damage)
        {
            int i = 0;

            if (dayAttacks.Any())
            {
                powerDisplays[i].Separator = "day power";
                i++;

                powerDisplays[i].AttackName = "capped";
                powerDisplays[i].Value = $"{damage.ShellingCapped:0.##}";
                i++;

                foreach (DayAttackKind dayAttack in dayAttacks)
                {
                    battle.DayAttack = dayAttack;

                    powerDisplays[i].AttackName = $"{dayAttack.Display()}";
                    powerDisplays[i].Value = $"{damage.Shelling:0.##} ({damage.ShellingCrit:0.##})";

                    i++;
                }
            }

            if (aswAttacks.Any())
            {
                powerDisplays[i].Separator = "ASW";
                i++;

                foreach (DayAttackKind dayAttack in aswAttacks)
                {
                    battle.DayAttack = dayAttack;

                    powerDisplays[i].AttackName = $"{dayAttack.Display()}";
                    powerDisplays[i].Value = $"{damage.ASW:0.##} ({damage.AswCrit:0.##})";

                    i++;
                }
            }

            if (nightAttacks.Any())
            {
                powerDisplays[i].Separator = "night";
                i++;

                foreach (NightAttackKind nightAttack in nightAttacks)
                {
                    battle.NightAttack = nightAttack;

                    powerDisplays[i].AttackName = $"{nightAttack.Display()}";
                    powerDisplays[i].Value = $"{damage.Night:0.##} ({damage.NightCrit:0.##})";

                    i++;
                }
            }

        }

        private void AccuracyDisplays(Accuracy accuracy)
        {
            int i = 0;

            if (dayAttacks.Any())
            {
                accuracyDisplays[i].Separator = "day accuracy";
                i++;

                foreach (DayAttackKind dayAttack in dayAttacks)
                {
                    battle.DayAttack = dayAttack;

                    accuracyDisplays[i].AttackName = $"{dayAttack.Display()}";
                    accuracyDisplays[i].Value = $"{accuracy.DayShelling:0.##}";

                    i++;
                }
            }

            if (aswAttacks.Any())
            {
                accuracyDisplays[i].Separator = "ASW accuracy";
                i++;

                foreach (DayAttackKind dayAttack in aswAttacks)
                {
                    battle.DayAttack = dayAttack;

                    accuracyDisplays[i].AttackName = $"{dayAttack.Display()}";
                    accuracyDisplays[i].Value = $"{accuracy.ASW:0.##}";

                    i++;
                }
            }

            if (nightAttacks.Any())
            {
                accuracyDisplays[i].Separator = "night accuracy";
                i++;

                foreach (NightAttackKind nightAttack in nightAttacks)
                {
                    battle.NightAttack = nightAttack;

                    accuracyDisplays[i].AttackName = $"{nightAttack.Display()}";
                    accuracyDisplays[i].Value = $"{accuracy.Night:0.##}";

                    i++;
                }
            }
        }

        private void DamageDisplays(Damage damage)
        {
            int i = 0;

            if (battleDayAttacks.Any())
            {
                damageDisplays[i].Separator = "day damage";
                i++;

                foreach (DayAttackKind dayAttack in battleDayAttacks)
                {
                    battle.DayAttack = dayAttack;

                    damageDisplays[i].AttackName = $"{dayAttack.Display()}";
                    damageDisplays[i].Value = $"{damage.ShellingMin:0.##}~{damage.ShellingMax:0.##} ({damage.ShellingCritMin:0.##}~{damage.ShellingCritMax:0.##})";

                    i++;
                }
            }

            if (battleAswAttacks.Any())
            {
                damageDisplays[i].Separator = "ASW";
                i++;

                foreach (DayAttackKind dayAttack in battleAswAttacks)
                {
                    battle.DayAttack = dayAttack;

                    damageDisplays[i].AttackName = $"{dayAttack.Display()}";
                    damageDisplays[i].Value = $"{damage.AswMin:0.##}~{damage.AswMax:0.##} ({damage.AswCritMin:0.##}~{damage.AswCritMax:0.##})";

                    i++;
                }
            }

            if (battleNightAttacks.Any())
            {
                damageDisplays[i].Separator = "night";
                i++;

                foreach (NightAttackKind nightAttack in battleNightAttacks)
                {
                    battle.NightAttack = nightAttack;

                    damageDisplays[i].AttackName = $"{nightAttack.Display()}";
                    damageDisplays[i].Value = $"{damage.NightMin:0.##}~{damage.NightMax:0.##} ({damage.NightCritMin:0.##}~{damage.NightCritMax:0.##})";

                    i++;
                }
            }
        }

        private void HitRateDisplays(HitRate hitRate)
        {
            int i = 0;

            if (battleDayAttacks.Any())
            {
                hitRateDisplays[i].Separator = "day hit rate";
                i++;

                foreach (DayAttackKind dayAttack in battleDayAttacks)
                {
                    battle.DayAttack = dayAttack;

                    hitRateDisplays[i].AttackName = $"{dayAttack.Display()}";
                    hitRateDisplays[i].Value = $"{hitRate.ShellingCapped:0.##}%";

                    i++;
                }
            }

            if (battleAswAttacks.Any())
            {
                hitRateDisplays[i].Separator = "ASW hit rate";
                i++;

                foreach (DayAttackKind dayAttack in battleAswAttacks)
                {
                    battle.DayAttack = dayAttack;

                    hitRateDisplays[i].AttackName = $"{dayAttack.Display()}";
                    hitRateDisplays[i].Value = $"{hitRate.AswCapped:0.##}%";

                    i++;
                }
            }

            if (battleNightAttacks.Any())
            {
                hitRateDisplays[i].Separator = "night hit rate";
                i++;

                foreach (NightAttackKind nightAttack in battleNightAttacks)
                {
                    battle.NightAttack = nightAttack;

                    hitRateDisplays[i].AttackName = $"{nightAttack.Display()}";
                    hitRateDisplays[i].Value = $"{hitRate.NightCapped:0.##}";

                    i++;
                }
            }
        }
    }
}
