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

        IShipDataCustom _selectedShip;
        FleetDataCustom fleet;
        BattleDataCustom battle;

        int DefaultShipID = -1;


        IEnumerable<ShipDataCustom> ShipList;
        IEnumerable<IEquipmentDataCustom> EquipmentList;

        public DialogShipSimulationWpf()
        {
            InitializeComponent();

            ShipDisplay.AddHandler(EquipmentSelectionItem.EquipmentSelectionEvent, new RoutedEventHandler(UpdatePossibleAttacks));
            ShipDisplay.AddHandler(ShipSelectionItem.ShipSelectionEvent, new RoutedEventHandler(UpdatePossibleAttacks));

            ShipDisplay.AddHandler(CalculationParametersChangedEvent, new RoutedEventHandler(Calculate));
            ExternalParameters.AddHandler(CalculationParametersChangedEvent, new RoutedEventHandler(Calculate));

            ShipList = KCDatabase.Instance.Ships.Values.Select(x => new ShipDataCustom(x));
            EquipmentList = KCDatabase.Instance.Equipments.Values.Select(x => new EquipmentDataCustom(x));

            DataContext = this;
        }

        public DialogShipSimulationWpf(int shipID) : this()
        {
            DefaultShipID = shipID;
        }

        private BattleStatDisplay[] damageDisplays;
        private BattleStatDisplay[] accuracyDisplays;

        private void DialogShipSimulation_Load(object sender, RoutedEventArgs e)
        {
            if (!ShipList.Any())
            {
                //MessageBox.Show("No ships available.\r\nPlease return to the home port page.", "Ships Unavailable", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Close();
                return;
            }

            _selectedShip = ShipList.First(x => x.MasterID == DefaultShipID);

            GenerateDisplays();

            if (DefaultShipID > 0)
                ShipDisplay.Ship = _selectedShip;

            ShipDisplay.Ships = ShipList;
            ShipDisplay.Equipments = EquipmentList;

            fleet = new FleetDataCustom();
        }

        private void GenerateDisplays()
        {
            dayAttacks = _selectedShip.DayAttacks.ToList();
            aswAttacks = _selectedShip.AswAttacks.ToList();
            nightAttacks = _selectedShip.NightAttacks.ToList();

            int dayAttackCount = dayAttacks.Count;
            int aswAttackCount = aswAttacks.Count;
            int nightAttackCount = nightAttacks.Count;

            int displayCount = 0;

            displayCount += dayAttackCount > 0 ? dayAttackCount + 1 : 0;
            displayCount += aswAttackCount > 0 ? aswAttackCount + 1 : 0;
            displayCount += nightAttackCount > 0 ? nightAttackCount + 1 : 0;

            // +1 for capped
            damageDisplays = new BattleStatDisplay[displayCount + 1];
            accuracyDisplays = new BattleStatDisplay[displayCount];
            for (int i = 0; i < displayCount; i++)
            {
                damageDisplays[i] = new BattleStatDisplay();
                accuracyDisplays[i] = new BattleStatDisplay();
            }
            damageDisplays[damageDisplays.Length - 1] = new BattleStatDisplay();

            DamageDisplay.ItemsSource = damageDisplays;
            AccuracyDisplay.ItemsSource = accuracyDisplays;
        }

        private void UpdatePossibleAttacks(object sender, RoutedEventArgs e)
        {
            _selectedShip = ShipDisplay.Ship;
            GenerateDisplays();

            Calculate(sender, e);
        }

        private List<DayAttackKind> dayAttacks;
        private List<DayAttackKind> aswAttacks;
        private List<NightAttackKind> nightAttacks;

        private void Calculate(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            if (_selectedShip == null)
                return;

            fleet = new FleetDataCustom(ExternalParameters.Parameters.Fleet.Member,
                ExternalParameters.Parameters.Formation.Member);
            fleet.AddToMain(_selectedShip);

            // test data
            FleetDataCustom enemyFleet = new FleetDataCustom();

            battle = new BattleDataCustom(fleet, enemyFleet, ExternalParameters.Parameters.Engagement.Member);

            fleet.NightRecon = ExternalParameters.Parameters.NightRecon;
            fleet.Flare = ExternalParameters.Parameters.Flare;
            fleet.Searchlight = ExternalParameters.Parameters.Searchlight;

            Damage damage = battle.Damage;
            Accuracy accuracy = battle.Accuracy;

            int i = 0;

            if (dayAttacks.Any())
            {
                damageDisplays[i].Separator = "day damage";
                i++;

                damageDisplays[i].AttackName = "capped";
                damageDisplays[i].Value = $"{damage.ShellingCapped:0.##}";
                i++;

                foreach (DayAttackKind dayAttack in dayAttacks)
                {
                    battle.DayAttack = dayAttack;

                    damageDisplays[i].AttackName = $"{dayAttack.Display()} damage";
                    damageDisplays[i].Value = $"{damage.Shelling:0.##} ({damage.ShellingCrit:0.##})";

                    i++;
                }
            }

            if (aswAttacks.Any())
            {
                damageDisplays[i].Separator = "ASW damage";
                i++;

                foreach (DayAttackKind dayAttack in aswAttacks)
                {
                    battle.DayAttack = dayAttack;

                    damageDisplays[i].AttackName = $"{dayAttack.Display()} damage";
                    damageDisplays[i].Value = $"{damage.ASW:0.##} ({damage.AswCrit:0.##})";

                    i++;
                }
            }

            if (nightAttacks.Any())
            {
                damageDisplays[i].Separator = "night damage";
                i++;

                foreach (NightAttackKind nightAttack in nightAttacks)
                {
                    battle.NightAttack = nightAttack;

                    damageDisplays[i].AttackName = $"{nightAttack.Display()} damage";
                    damageDisplays[i].Value = $"{damage.Night:0.##} ({damage.NightCrit:0.##})";

                    i++;
                }
            }

            i = 0;

            if (dayAttacks.Any())
            {
                accuracyDisplays[i].Separator = "day accuracy";
                i++;

                foreach (DayAttackKind dayAttack in dayAttacks)
                {
                    battle.DayAttack = dayAttack;

                    accuracyDisplays[i].AttackName = $"{dayAttack.Display()} acc";
                    accuracyDisplays[i].Value = accuracy.DayShelling.ToString("0.##");

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

                    accuracyDisplays[i].AttackName = $"{dayAttack.Display()} acc";
                    accuracyDisplays[i].Value = accuracy.ASW.ToString("0.##");

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

                    accuracyDisplays[i].AttackName = $"{nightAttack.Display()} acc";
                    accuracyDisplays[i].Value = accuracy.Night.ToString("0.##");

                    i++;
                }
            }

            return;
        }
    }
}
