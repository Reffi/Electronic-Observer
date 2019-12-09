using System;
using System.Collections.Generic;
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
using ElectronicObserverViewModels;

namespace KancolleSimulator.Views
{
    /// <summary>
    /// Interaction logic for EquipmentView.xaml
    /// </summary>
    public partial class EquipmentView : UserControl
    {
        private EquipmentSlotViewModel _equipmentSlotViewModel;

        public EquipmentSlotViewModel EquipmentSlotViewModel
        {
            get => _equipmentSlotViewModel;
            set
            {
                _equipmentSlotViewModel = value;

                DataContext = EquipmentSlotViewModel;
            }
        }

        public EquipmentView()
        {
            InitializeComponent();
        }
    }
}
