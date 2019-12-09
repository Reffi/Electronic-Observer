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
    /// Interaction logic for FleetView.xaml
    /// </summary>
    public partial class FleetView : UserControl
    {
        public FleetViewModel FleetViewModel { get; }

        private ShipView[] MainFleet { get; }

        public FleetView()
        {
            InitializeComponent();

            MainFleet = new[] {Ship1, Ship2, Ship3, Ship4, Ship5, Ship6};

            FleetViewModel = new FleetViewModel();

            DataContext = FleetViewModel;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (ShipView shipView in MainFleet)
            {
                FleetViewModel.ShipViewModels.Add(shipView.ViewModel);
            }
        }
    }
}
