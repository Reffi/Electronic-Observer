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

        /*public static readonly DependencyProperty EquipProperty = DependencyProperty
            .Register(nameof(Equip), typeof(EquipmentDataCustom), typeof(EquipmentSlot),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public EquipmentDataCustom Equip
        {
            get => (EquipmentDataCustom)GetValue(EquipProperty);
            set => SetValue(EquipProperty, value);
        }*/



        private EquipmentSlotViewModel _equipSlotViewModel;

        public EquipmentSlotViewModel EquipSlotViewModel
        {
            get => _equipSlotViewModel;
            set
            {
                _equipSlotViewModel = value;

                // if (EquipSlotViewModel == null) return;

                DataContext = EquipSlotViewModel;
                SetDisplayStats();
            }
        }

        public int SlotIndex
        {
            get => (int) GetValue(SlotIndexProperty);
            set => SetValue(SlotIndexProperty, value);
        }

        public EquipmentSlot()
        {
            InitializeComponent();

            /*EquipSlotViewModel = new EquipmentSlotViewModel();
            EquipSlotViewModel.PropertyChanged += StatChanged;*/
        }

        private void SetDisplayStats()
        {
            switch(EquipSlotViewModel.CategoryType)
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
            SetDisplayStat(StatDisplay0, Resources["IconFirepower"], nameof(EquipSlotViewModel.BaseFirepower));
            SetDisplayStat(StatDisplay1, Resources["IconAccuracy"], nameof(EquipSlotViewModel.BaseAccuracy));
            SetDisplayStat(StatDisplay2, Resources["IconArmor"], nameof(EquipSlotViewModel.BaseArmor));
            SetDisplayStat(StatDisplay3, Resources["IconAA"], nameof(EquipSlotViewModel.BaseAA));
            SetDisplayStat(StatDisplay4, Resources["IconEvasion"], nameof(EquipSlotViewModel.BaseEvasion));

            if (EquipSlotViewModel.BaseLoS > 0)
                SetDisplayStat(StatDisplay5, Resources["IconLoS"], nameof(EquipSlotViewModel.BaseLoS));
            else
                SetDisplayStat(StatDisplay5, Resources["IconASW"], nameof(EquipSlotViewModel.BaseASW));


            // todo some icon to indicate fit bonus
            SetFitDisplayStat(FitDisplay0, null, nameof(EquipSlotViewModel.FitFirepower));
            SetFitDisplayStat(FitDisplay1, null, nameof(EquipSlotViewModel.FitAccuracy));
            SetFitDisplayStat(FitDisplay2, null, nameof(EquipSlotViewModel.FitArmor));
            SetFitDisplayStat(FitDisplay3, null, nameof(EquipSlotViewModel.FitAA));
            SetFitDisplayStat(FitDisplay4, null, nameof(EquipSlotViewModel.FitEvasion));

            if (EquipSlotViewModel.FitLoS > 0)
                SetFitDisplayStat(FitDisplay5, null, nameof(EquipSlotViewModel.FitLoS));
            else
                SetFitDisplayStat(FitDisplay5, null, nameof(EquipSlotViewModel.FitASW));
        }

        private void TorpedoStatDisplay()
        {
            SetDisplayStat(StatDisplay0, Resources["IconTorpedo"], nameof(EquipSlotViewModel.BaseTorpedo));
            SetDisplayStat(StatDisplay1, Resources["IconAccuracy"], nameof(EquipSlotViewModel.BaseAccuracy));
            SetDisplayStat(StatDisplay2, Resources["IconArmor"], nameof(EquipSlotViewModel.BaseArmor));
            SetDisplayStat(StatDisplay3, Resources["IconFirepower"], nameof(EquipSlotViewModel.BaseFirepower));
            SetDisplayStat(StatDisplay4, Resources["IconEvasion"], nameof(EquipSlotViewModel.BaseEvasion));

            if (EquipSlotViewModel.BaseLoS > 0)
                SetDisplayStat(StatDisplay5, Resources["IconLoS"], nameof(EquipSlotViewModel.BaseLoS));
            else
                SetDisplayStat(StatDisplay5, Resources["IconASW"], nameof(EquipSlotViewModel.BaseASW));


            // todo some icon to indicate fit bonus
            SetFitDisplayStat(FitDisplay0, null, nameof(EquipSlotViewModel.FitTorpedo));
            SetFitDisplayStat(FitDisplay1, null, nameof(EquipSlotViewModel.FitAccuracy));
            SetFitDisplayStat(FitDisplay2, null, nameof(EquipSlotViewModel.FitArmor));
            SetFitDisplayStat(FitDisplay3, null, nameof(EquipSlotViewModel.FitFirepower));
            SetFitDisplayStat(FitDisplay4, null, nameof(EquipSlotViewModel.FitEvasion));

            if (EquipSlotViewModel.FitLoS > 0)
                SetFitDisplayStat(FitDisplay5, null, nameof(EquipSlotViewModel.FitLoS));
            else
                SetFitDisplayStat(FitDisplay5, null, nameof(EquipSlotViewModel.FitASW));
        }

        private void SetDisplayStat(Stat display, object icon, string path)
        {
            display.StatIcon = (ImageSource)icon;
            BindingOperations.SetBinding(display, Stat.BaseStatProperty,
                new Binding
                {
                    Source = EquipSlotViewModel,
                    Path = new PropertyPath(path),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
        }

        private void SetFitDisplayStat(Stat display, object icon, string path)
        {
            display.OtherContent = (Canvas)Resources["IconPlusMinus"];



            BindingOperations.SetBinding(display, Stat.BaseStatProperty,
                new Binding
                {
                    Source = EquipSlotViewModel,
                    Path = new PropertyPath(path),
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });

            /*BindingOperations.SetBinding(display, Stat.OtherContentProperty,
                new Binding
                {
                    Source = Resources,
                    Path = new PropertyPath("IconPlusMinus")
                });*/
        }

        private void EquipSelection(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(EquipmentChangeEvent, this));
        }

        private void StatChanged(object sender, PropertyChangedEventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs(DialogShipSimulationWpf.CalculationParametersChangedEvent);
            RaiseEvent(args);
        }
    }
}
