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
    public partial class EquipmentSlot : UserControl
    {
        public static readonly RoutedEvent EquipmentChangeEvent = EventManager.RegisterRoutedEvent(
            "EquipmentChange", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EquipmentSlot));

        public event RoutedEventHandler EquipmentChange
        {
            add => AddHandler(EquipmentChangeEvent, value);
            remove => RemoveHandler(EquipmentChangeEvent, value);
        }

        public static readonly DependencyProperty SlotIndexProperty = DependencyProperty
            .Register(nameof(SlotIndex), typeof(int), typeof(EquipmentSlot),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty EquipProperty = DependencyProperty
            .Register(nameof(Equip), typeof(EquipmentDataCustom), typeof(EquipmentSlot),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        private EquipmentViewModel _equipViewModel;
        public EquipmentViewModel EquipViewModel
        {
            get => _equipViewModel;
            set
            {
                _equipViewModel = value;
                DataContext = EquipViewModel;

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

        public EquipmentDataCustom Equip
        {
            get => (EquipmentDataCustom)GetValue(EquipProperty);
            set => SetValue(EquipProperty, value);
        }

        public EquipmentSlot()
        {
            InitializeComponent();
        }

        private void SetDisplayStats()
        {
            switch(EquipViewModel.CategoryType)
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
            SetDisplayStat(StatDisplay0, Resources["IconFirepower"], nameof(EquipViewModel.BaseFirepower));
            SetDisplayStat(StatDisplay1, Resources["IconAccuracy"], nameof(EquipViewModel.BaseAccuracy));
            SetDisplayStat(StatDisplay2, Resources["IconArmor"], nameof(EquipViewModel.BaseArmor));
            SetDisplayStat(StatDisplay3, Resources["IconAA"], nameof(EquipViewModel.BaseAA));
            SetDisplayStat(StatDisplay4, Resources["IconEvasion"], nameof(EquipViewModel.BaseEvasion));

            if (EquipViewModel.BaseLoS > 0)
                SetDisplayStat(StatDisplay5, Resources["IconLoS"], nameof(EquipViewModel.BaseLoS));
            else
                SetDisplayStat(StatDisplay5, Resources["IconASW"], nameof(EquipViewModel.BaseASW));


            // todo some icon to indicate fit bonus
            SetFitDisplayStat(FitDisplay0, null, nameof(EquipViewModel.CurrentFitBonus.Firepower));
            SetFitDisplayStat(FitDisplay1, null, nameof(EquipViewModel.CurrentFitBonus.Accuracy));
            SetFitDisplayStat(FitDisplay2, null, nameof(EquipViewModel.CurrentFitBonus.Armor));
            SetFitDisplayStat(FitDisplay3, null, nameof(EquipViewModel.CurrentFitBonus.AA));
            SetFitDisplayStat(FitDisplay4, null, nameof(EquipViewModel.CurrentFitBonus.Evasion));

            if (EquipViewModel.CurrentFitBonus.LoS > 0)
                SetFitDisplayStat(FitDisplay5, null, nameof(EquipViewModel.CurrentFitBonus.LoS));
            else
                SetFitDisplayStat(FitDisplay5, null, nameof(EquipViewModel.CurrentFitBonus.ASW));
        }

        private void TorpedoStatDisplay()
        {
            SetDisplayStat(StatDisplay0, Resources["IconTorpedo"], nameof(EquipViewModel.BaseTorpedo));
            SetDisplayStat(StatDisplay1, Resources["IconAccuracy"], nameof(EquipViewModel.BaseAccuracy));
            SetDisplayStat(StatDisplay2, Resources["IconArmor"], nameof(EquipViewModel.BaseArmor));
            SetDisplayStat(StatDisplay3, Resources["IconFirepower"], nameof(EquipViewModel.BaseFirepower));
            SetDisplayStat(StatDisplay4, Resources["IconEvasion"], nameof(EquipViewModel.BaseEvasion));

            if (EquipViewModel.BaseLoS > 0)
                SetDisplayStat(StatDisplay5, Resources["IconLoS"], nameof(EquipViewModel.BaseLoS));
            else
                SetDisplayStat(StatDisplay5, Resources["IconASW"], nameof(EquipViewModel.BaseASW));


            // todo some icon to indicate fit bonus
            SetFitDisplayStat(FitDisplay0, null, nameof(EquipViewModel.CurrentFitBonus.Torpedo));
            SetFitDisplayStat(FitDisplay1, null, nameof(EquipViewModel.CurrentFitBonus.Accuracy));
            SetFitDisplayStat(FitDisplay2, null, nameof(EquipViewModel.CurrentFitBonus.Armor));
            SetFitDisplayStat(FitDisplay3, null, nameof(EquipViewModel.CurrentFitBonus.Firepower));
            SetFitDisplayStat(FitDisplay4, null, nameof(EquipViewModel.CurrentFitBonus.Evasion));

            if (EquipViewModel.CurrentFitBonus.LoS > 0)
                SetFitDisplayStat(FitDisplay5, null, nameof(EquipViewModel.CurrentFitBonus.LoS));
            else
                SetFitDisplayStat(FitDisplay5, null, nameof(EquipViewModel.CurrentFitBonus.ASW));
        }

        private void SetDisplayStat(Stat display, object icon, string path)
        {
            display.StatIcon = (ImageSource)icon;
            BindingOperations.SetBinding(display, Stat.BaseStatProperty,
                new Binding
                {
                    Source = EquipViewModel,
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
                    Source = EquipViewModel.CurrentFitBonus,
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
