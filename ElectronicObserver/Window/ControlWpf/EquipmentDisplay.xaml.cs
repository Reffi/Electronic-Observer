using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Data;
using ElectronicObserver.Window.ViewModel;

namespace ElectronicObserver.Window.ControlWpf
{
    /// <summary>
    /// Interaction logic for EquipmentDisplay.xaml
    /// </summary>
    public partial class EquipmentDisplay : UserControl
    {
        private Equipment[] equipmentDisplays;

        private ShipViewModel _shipViewModel;
        public ShipViewModel Ship
        {
            get => _shipViewModel;
            set
            {
                if (value == null)
                    return;

                _shipViewModel = value;
                DataContext = Ship;

                for (int i = 0; i < 6; i++)
                {
                    bool slotEnabled = i < _shipViewModel.Ship.EquipmentSlotCount  || (i == 5 && _shipViewModel.IsExpansionSlotAvailable);

                    equipmentDisplays[i].IsEnabled = slotEnabled;
                    equipmentDisplays[i].Equip = _shipViewModel.EquipmentViewModels[i];
                    /*if(i < 5)
                        equipmentDisplays[i].SlotSize = _shipViewModel.Aircraft?[i] ?? 0;*/


                }
            }
        }

        public EquipmentDisplay()
        {
            InitializeComponent();

            equipmentDisplays = new[] { EquipDisplay0, EquipDisplay1, EquipDisplay2, EquipDisplay3, EquipDisplay4, EquipDisplay5 };
        }
    }
}
