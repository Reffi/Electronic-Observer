using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Helpers;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.ViewModel;

namespace ElectronicObserver.Window.ControlWpf
{
    /// <summary>
    /// Interaction logic for Equipment.xaml
    /// </summary>
    public partial class Equipment : UserControl
    {
        public static readonly RoutedEvent EquipmentChangeEvent = EventManager.RegisterRoutedEvent(
            "EquipmentChange", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Equipment));

        public event RoutedEventHandler EquipmentChange
        {
            add { AddHandler(EquipmentChangeEvent, value); }
            remove { RemoveHandler(EquipmentChangeEvent, value); }
        }

        public static readonly DependencyProperty SlotIndexProperty = DependencyProperty
            .Register(nameof(SlotIndex), typeof(int), typeof(Equipment),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty SlotSizeProperty = DependencyProperty
            .Register(nameof(SlotSize), typeof(int), typeof(Equipment),
                new FrameworkPropertyMetadata(0));

        private EquipmentViewModel _equipViewModel;
        public EquipmentViewModel Equip
        {
            get => _equipViewModel;
            set
            {
                _equipViewModel = value;
                DataContext = _equipViewModel;

                if (_equipViewModel == null)
                {
                    //DataContext = null;
                    EmptyEquip();
                    EquipName.Visibility = Visibility.Visible;
                    return;
                }

                IsEnabled = true;

                // DataContext must be set for this method
                SetDisplayStats();

                /*string link = KCResourceHelper
                    .GetEquipmentImagePath(_equipViewModel.ID, KCResourceHelper.ResourceTypeEquipmentCard);
                    */
                //if (link == null)
                {
                    //EquipName.Content = _equip.Name;
                    EquipName.Visibility = Visibility.Visible;
                    // EquipImage.Source = null;
                    return;
                }

                EquipName.Visibility = Visibility.Hidden;

                // using FileStream stream = new FileStream(link, FileMode.Open, FileAccess.Read);

                // there should be a better way to find the image path
                // Uri test = new Uri(stream.Name);
                // EquipImage.Source = new BitmapImage(test);
            }
        }

        public int SlotIndex
        {
            get => (int) GetValue(SlotIndexProperty);
            set => SetValue(SlotIndexProperty, value);
        }

        public int SlotSize
        {
            get => (int)GetValue(SlotSizeProperty);
            set => SetValue(SlotSizeProperty, value);
        }

        public Equipment()
        {
            InitializeComponent();
        }

        private void SetDisplayStats()
        {
            switch(Equip.CategoryType)
            {
                case EquipmentTypes.Torpedo:
                case EquipmentTypes.MidgetSubmarine:
                    TorpedoStatDisplay();
                    break;

                default:
                    MainGunStatDisplay();
                    break;
            }
        }

        private void MainGunStatDisplay()
        {
            SetDisplayStat(StatDisplay0, "火力", nameof(Equip.BaseFirepower));
            SetDisplayStat(StatDisplay1, "命中", nameof(Equip.BaseAccuracy));
            SetDisplayStat(StatDisplay2, "装甲", nameof(Equip.BaseArmor));
            SetDisplayStat(StatDisplay3, "対空", nameof(Equip.BaseAA));
            SetDisplayStat(StatDisplay4, "回避", nameof(Equip.BaseEvasion));

            if (Equip.BaseLoS > 0)
                SetDisplayStat(StatDisplay5, "索敵", nameof(Equip.BaseLoS));
            else
                SetDisplayStat(StatDisplay5, "対潜", nameof(Equip.BaseASW));



            SetFitDisplayStat(FitDisplay0, "+", nameof(Equip.CurrentFitBonus.Firepower));
            SetFitDisplayStat(FitDisplay1, "+", nameof(Equip.CurrentFitBonus.Accuracy));
            SetFitDisplayStat(FitDisplay2, "+", nameof(Equip.CurrentFitBonus.Armor));
            SetFitDisplayStat(FitDisplay3, "+", nameof(Equip.CurrentFitBonus.AA));
            SetFitDisplayStat(FitDisplay4, "+", nameof(Equip.CurrentFitBonus.Evasion));

            if (Equip.CurrentFitBonus.LoS > 0)
                SetFitDisplayStat(FitDisplay5, "+", nameof(Equip.CurrentFitBonus.LoS));
            else
                SetFitDisplayStat(FitDisplay5, "+", nameof(Equip.CurrentFitBonus.ASW));
        }

        private void TorpedoStatDisplay()
        {
            SetDisplayStat(StatDisplay0, "雷装", nameof(Equip.BaseTorpedo));
            SetDisplayStat(StatDisplay1, "命中", nameof(Equip.BaseAccuracy));
            SetDisplayStat(StatDisplay2, "装甲", nameof(Equip.BaseArmor));
            SetDisplayStat(StatDisplay3, "火力", nameof(Equip.BaseFirepower));
            SetDisplayStat(StatDisplay4, "回避", nameof(Equip.BaseEvasion));

            if (Equip.BaseLoS > 0)
                SetDisplayStat(StatDisplay5, "索敵", nameof(Equip.BaseLoS));
            else
                SetDisplayStat(StatDisplay5, "対潜", nameof(Equip.BaseASW));



            SetFitDisplayStat(FitDisplay0, "+", nameof(Equip.CurrentFitBonus.Torpedo));
            SetFitDisplayStat(FitDisplay1, "+", nameof(Equip.CurrentFitBonus.Accuracy));
            SetFitDisplayStat(FitDisplay2, "+", nameof(Equip.CurrentFitBonus.Armor));
            SetFitDisplayStat(FitDisplay3, "+", nameof(Equip.CurrentFitBonus.Firepower));
            SetFitDisplayStat(FitDisplay4, "+", nameof(Equip.CurrentFitBonus.Evasion));

            if (Equip.CurrentFitBonus.LoS > 0)
                SetFitDisplayStat(FitDisplay5, "+", nameof(Equip.CurrentFitBonus.LoS));
            else
                SetFitDisplayStat(FitDisplay5, "+", nameof(Equip.CurrentFitBonus.ASW));
        }

        private void SetDisplayStat(Stat display, string name, string path)
        {
            display.StatName = name;
            BindingOperations.SetBinding(display, Stat.BaseStatProperty,
                new Binding
                {
                    Source = Equip,
                    Path = new PropertyPath(path),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
        }

        private void SetFitDisplayStat(Stat display, string name, string path)
        {
            display.StatName = name;
            BindingOperations.SetBinding(display, Stat.BaseStatProperty,
                new Binding
                {
                    Source = Equip.CurrentFitBonus,
                    Path = new PropertyPath(path),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
        }

        private void EmptyEquip()
        {
            // EquipImage.Source = null;

            BindingOperations.ClearAllBindings(StatDisplay0);
            BindingOperations.ClearAllBindings(StatDisplay1);
            BindingOperations.ClearAllBindings(StatDisplay2);
            BindingOperations.ClearAllBindings(StatDisplay3);
            BindingOperations.ClearAllBindings(StatDisplay4);
            BindingOperations.ClearAllBindings(StatDisplay5);
        }

        private void EquipSelection(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(EquipmentChangeEvent, this));
        }

        private void StatChanged(object sender, DataTransferEventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs(DialogShipSimulationWpf.CalculationParametersChangedEvent);
            RaiseEvent(args);
        }
    }
}
