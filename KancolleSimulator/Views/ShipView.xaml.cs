using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Properties;
using ElectronicObserverViewModels;
using EquipmentTypes = ElectronicObserverTypes.EquipmentTypes;

namespace KancolleSimulator.Views
{
    /// <summary>
    /// Interaction logic for ShipView.xaml
    /// </summary>
    public partial class ShipView : UserControl
    {
        public ShipViewModel ViewModel { get; }

        private EquipmentView[] EquipmentDisplays { get; }

        private ShipDataCustom _ship;

        public ShipDataCustom Ship
        {
            get => _ship;
            set
            {
                if (value == null)
                {
                    // ShipName.Visibility = Visibility.Visible;
                    return;
                }

                _ship = value;
                ViewModel.Ship = Ship;

                for (int i = 0; i < 6; i++)
                {
                    bool slotEnabled = i < Ship.EquipmentSlotCount || (i == 5 && Ship.IsExpansionSlotAvailable);
                    if (i < EquipmentDisplays.Length)
                    {
                        // EquipmentDisplays[i].EquipmentSlotViewModel.Equip = ViewModel.EquipmentViewModels[i].Equip;
                        EquipmentDisplays[i].IsEnabled = slotEnabled;
                    }
                }
            }
        }

        public ShipView()
        {
            InitializeComponent();

            EquipmentDisplays = new[] { EquipDisplay0, EquipDisplay1, EquipDisplay2, EquipDisplay3, EquipDisplay4, EquipDisplay5 };

            ViewModel = new ShipViewModel();

            for (int i = 0; i < EquipmentDisplays.Length; i++)
            {
                EquipmentDisplays[i].EquipmentSlotViewModel = ViewModel.EquipmentViewModels[i];
            }

            DataContext = ViewModel;
        }


        private void SetShip(object sender, MouseButtonEventArgs e)
        {
            EquipmentDataCustom eq1 = new EquipmentDataCustom
            {
                Name = "Equip 1",
                Level = 5,
                Proficiency = 4
            };

            Ship = new ShipDataCustom
            {
                Name = "神風",
                EquipmentSlotCount = 5,
                BaseLoS = 20,
                Aircraft = new[] { 20, 20, 46, 12 },
                Equipment = new[] { eq1 }
            };
        }
    }
}
