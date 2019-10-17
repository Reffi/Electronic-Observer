using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.ViewModel;

namespace ElectronicObserver.Window.ControlWpf
{
    /// <summary>
    /// Interaction logic for ShipStatsDisplay.xaml
    /// </summary>
    public partial class ShipStatsDisplay : UserControl
    {
        private ShipViewModel _viewModel;

        public ShipViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                DataContext = ViewModel;
            } 

        }

        public ShipStatsDisplay()
        {
            InitializeComponent();
        }
    }
}
