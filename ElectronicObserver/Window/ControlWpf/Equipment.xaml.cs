using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
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

        private EquipmentViewModel _equipViewModel;
        public EquipmentViewModel Equip
        {
            get => _equipViewModel;
            set
            {
                _equipViewModel = value;
                DataContext = Equip;

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
            SetDisplayStat(StatDisplay0, Resources["IconFirepower"], nameof(Equip.BaseFirepower));
            SetDisplayStat(StatDisplay1, Resources["IconAccuracy"], nameof(Equip.BaseAccuracy));
            SetDisplayStat(StatDisplay2, Resources["IconArmor"], nameof(Equip.BaseArmor));
            SetDisplayStat(StatDisplay3, Resources["IconAA"], nameof(Equip.BaseAA));
            SetDisplayStat(StatDisplay4, Resources["IconEvasion"], nameof(Equip.BaseEvasion));

            if (Equip.BaseLoS > 0)
                SetDisplayStat(StatDisplay5, Resources["IconLoS"], nameof(Equip.BaseLoS));
            else
                SetDisplayStat(StatDisplay5, Resources["IconASW"], nameof(Equip.BaseASW));


            // todo some icon to indicate fit bonus
            SetFitDisplayStat(FitDisplay0, null, nameof(Equip.CurrentFitBonus.Firepower));
            SetFitDisplayStat(FitDisplay1, null, nameof(Equip.CurrentFitBonus.Accuracy));
            SetFitDisplayStat(FitDisplay2, null, nameof(Equip.CurrentFitBonus.Armor));
            SetFitDisplayStat(FitDisplay3, null, nameof(Equip.CurrentFitBonus.AA));
            SetFitDisplayStat(FitDisplay4, null, nameof(Equip.CurrentFitBonus.Evasion));

            if (Equip.CurrentFitBonus.LoS > 0)
                SetFitDisplayStat(FitDisplay5, null, nameof(Equip.CurrentFitBonus.LoS));
            else
                SetFitDisplayStat(FitDisplay5, null, nameof(Equip.CurrentFitBonus.ASW));
        }

        private void TorpedoStatDisplay()
        {
            SetDisplayStat(StatDisplay0, Resources["IconTorpedo"], nameof(Equip.BaseTorpedo));
            SetDisplayStat(StatDisplay1, Resources["IconAccuracy"], nameof(Equip.BaseAccuracy));
            SetDisplayStat(StatDisplay2, Resources["IconArmor"], nameof(Equip.BaseArmor));
            SetDisplayStat(StatDisplay3, Resources["IconFirepower"], nameof(Equip.BaseFirepower));
            SetDisplayStat(StatDisplay4, Resources["IconEvasion"], nameof(Equip.BaseEvasion));

            if (Equip.BaseLoS > 0)
                SetDisplayStat(StatDisplay5, Resources["IconLoS"], nameof(Equip.BaseLoS));
            else
                SetDisplayStat(StatDisplay5, Resources["IconASW"], nameof(Equip.BaseASW));


            // todo some icon to indicate fit bonus
            SetFitDisplayStat(FitDisplay0, null, nameof(Equip.CurrentFitBonus.Torpedo));
            SetFitDisplayStat(FitDisplay1, null, nameof(Equip.CurrentFitBonus.Accuracy));
            SetFitDisplayStat(FitDisplay2, null, nameof(Equip.CurrentFitBonus.Armor));
            SetFitDisplayStat(FitDisplay3, null, nameof(Equip.CurrentFitBonus.Firepower));
            SetFitDisplayStat(FitDisplay4, null, nameof(Equip.CurrentFitBonus.Evasion));

            if (Equip.CurrentFitBonus.LoS > 0)
                SetFitDisplayStat(FitDisplay5, null, nameof(Equip.CurrentFitBonus.LoS));
            else
                SetFitDisplayStat(FitDisplay5, null, nameof(Equip.CurrentFitBonus.ASW));
        }

        private void SetDisplayStat(Stat display, object icon, string path)
        {
            display.StatIcon = (ImageSource)icon;
            BindingOperations.SetBinding(display, Stat.BaseStatProperty,
                new Binding
                {
                    Source = Equip,
                    Path = new PropertyPath(path),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
        }

        private void SetFitDisplayStat(Stat display, object icon, string path)
        {
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
