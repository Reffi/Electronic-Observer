using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.ViewModel;

namespace ElectronicObserver.Window.ControlWpf
{
    /// <summary>
    /// Interaction logic for ShipDisplay.xaml
    /// </summary>
    public partial class ShipDisplay : UserControl
    {
        private ShipViewModel _viewModel;
        public ShipViewModel ViewModel => _viewModel;

        void ShipSelected(object sender, RoutedEventArgs e)
        {
            ShipSelectionItem.ShipSelectRoutedEventArgs args = (ShipSelectionItem.ShipSelectRoutedEventArgs) e;

            CloseShipSelection(sender, null);

            if (args.Ship == null)
            {
                e.Handled = true;
                return;
            }

            Ship = args.Ship;

        }

        void EquipSelected(object sender, RoutedEventArgs e)
        {
            EquipmentSelectionItem.EquipmentSelectRoutedEventArgs args =
                (EquipmentSelectionItem.EquipmentSelectRoutedEventArgs) e;

            if (args.Equip == null)
            {
                e.Handled = true;
                CloseEquipmentSelection(sender, null);
                return;
            }

            //_ship.Equipment[_currentEquipmentSlot.SlotIndex] = args.Equip;

            EquipmentDataCustom[] newEquip = new EquipmentDataCustom[6];

            for (int i = 0; i < 6; i++)
            {
                if (i == _currentEquipmentSlot.SlotIndex)
                {
                    newEquip[i] = args.Equip;
                }
                else
                {
                    newEquip[i] = ViewModel.Equipment[i];
                }
            }

            ViewModel.Equipment = newEquip;
            _currentEquipmentSlot.EquipSlotViewModel = ViewModel.EquipmentViewModels[_currentEquipmentSlot.SlotIndex];

            DataContext = ViewModel;
            CloseEquipmentSelection(sender, null);
            RaiseEvent(new RoutedEventArgs(DialogShipSimulationWpf.CalculationParametersChangedEvent));
            //EquipmentDisplay.EquipDisplay0.Equip = args.Equip;
        }

        void EquipChange(object sender, RoutedEventArgs e)
        {
            ShowEquipmentSelection(e.OriginalSource, null);
            e.Handled = true;
        }

        private ShipDataCustom _ship;

        public ShipDataCustom Ship
        {
            get => _ship;
            set
            {
                if (value == null)
                {
                    ShipName.Visibility = Visibility.Visible;
                    return;
                }

                _viewModel = new ShipViewModel();
                _viewModel.PropertyChanged += CalculationParametersChanged;

                _ship = value;
                ViewModel.Ship = _ship;
                SynergyDisplay.Synergy = ViewModel.SynergyViewModel;
                DataContext = ViewModel;


                // EquipmentDisplay.Ship = _viewModel;
                EquipmentSelect.EquippableCategories = _ship.EquippableCategories.Cast<EquipmentTypes>();

                StatDisplay.ViewModel = ViewModel;

                for (int i = 0; i < 6; i++)
                {
                    bool slotEnabled = i < Ship.EquipmentSlotCount || (i == 5 && Ship.IsExpansionSlotAvailable);
                    if (i < EquipmentDisplays.Length)
                    {
                        EquipmentDisplays[i].EquipSlotViewModel = ViewModel.EquipmentViewModels[i];
                        EquipmentDisplays[i].IsEnabled = slotEnabled;
                    }
                }

                RaiseEvent(new RoutedEventArgs(DialogShipSimulationWpf.CalculationParametersChangedEvent));

                string resourceType = _ship.IsAbyssal
                    ? KCResourceHelper.ResourceTypeShipFull
                    : KCResourceHelper.ResourceTypeShipAlbumZoom;

                string link = KCResourceHelper.GetShipImagePath(value.ID, false,
                    resourceType);

                if (link == null)
                {
                    ShipName.Visibility = Visibility.Visible;
                    ShipImage.Source = null;
                    return;
                }

                ShipName.Visibility = Visibility.Hidden;

                using FileStream stream = new FileStream(link, FileMode.Open, FileAccess.Read);

                // there should be a better way to find the image path
                Uri test = new Uri(stream.Name);
                ShipImage.Source = new BitmapImage(test);
            }
        }

        private IEnumerable<ShipDataCustom> _ships;

        public IEnumerable<ShipDataCustom> Ships
        {
            get => _ships;
            set
            {
                _ships = value;
                ShipSelect.Ships = _ships;
            }
        }

        private IEnumerable<EquipmentDataCustom> _equipment;

        public IEnumerable<EquipmentDataCustom> Equipment
        {
            get => _equipment;
            set
            {
                _equipment = value;
                EquipmentSelect.Equips = _equipment;
            }
        }

        private EquipmentSlot[] EquipmentDisplays { get; }

        private EquipmentSlot _currentEquipmentSlot;

        public ShipDisplay()
        {
            InitializeComponent();

            EquipmentDisplays = new[] { EquipDisplay0, EquipDisplay1, EquipDisplay2, EquipDisplay3, EquipDisplay4, EquipDisplay5 };
        }

        private void ShipDisplay_Loaded(object sender, RoutedEventArgs e)
        {
            ShipSelect.AddHandler(ShipSelectionItem.ShipSelectionEvent, new RoutedEventHandler(ShipSelected));
            EquipmentSelect.AddHandler(EquipmentSelectionItem.EquipmentSelectionEvent,
                new RoutedEventHandler(EquipSelected));

            foreach (EquipmentSlot equipmentDisplay in EquipmentDisplays)
            {
                equipmentDisplay.AddHandler(EquipmentSlot.EquipmentChangeEvent, new RoutedEventHandler(EquipChange));
            }
        }

        private void ShowShipSelection(object sender, MouseButtonEventArgs e)
        {
            ShipSelectionOverlay.Visibility = Visibility.Visible;
        }

        private void CloseShipSelection(object sender, MouseButtonEventArgs e)
        {
            ShipSelectionOverlay.Visibility = Visibility.Hidden;
        }

        private void ShowEquipmentSelection(object sender, MouseButtonEventArgs e)
        {
            _currentEquipmentSlot = (EquipmentSlot) sender;
            EquipmentSelectionOverlay.Visibility = Visibility.Visible;
        }

        private void CloseEquipmentSelection(object sender, MouseButtonEventArgs e)
        {
            _currentEquipmentSlot = null;
            EquipmentSelectionOverlay.Visibility = Visibility.Hidden;
        }

        private void CalculationParametersChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ShipViewModel.DisplayFirepower):
                case nameof(ShipViewModel.DisplayTorpedo):
                case nameof(ShipViewModel.DisplayAA):
                case nameof(ShipViewModel.DisplayASW):
                case nameof(ShipViewModel.DisplayLoS):
                case nameof(ShipViewModel.DisplayAccuracy):
                case nameof(ShipViewModel.DisplayArmor):
                case nameof(ShipViewModel.DisplayEvasion):
                case nameof(ShipViewModel.DisplayNightPower):
                    return;
            }

            RaiseEvent(new RoutedEventArgs(DialogShipSimulationWpf.CalculationParametersChangedEvent));
        }
    }
}
