using System;
using System.Collections.Generic;
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

namespace ElectronicObserver.Window.ControlWpf
{
    /// <summary>
    /// Interaction logic for ShipSelectionItem.xaml
    /// </summary>
    public partial class ShipSelectionItem : UserControl
    {
        public class ShipSelectRoutedEventArgs : RoutedEventArgs
        {
            public ShipDataCustom Ship { get; set; }

            public ShipSelectRoutedEventArgs(RoutedEvent routedEvent, ShipDataCustom ship) : base(routedEvent)
            {
                Ship = ship;
            }
        }

        public static readonly RoutedEvent ShipSelectionEvent = EventManager.RegisterRoutedEvent(
            "ShipSelection", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ShipSelectionItem));

        public event RoutedEventHandler ShipSelected
        {
            add { AddHandler(ShipSelectionEvent, value); }
            remove { RemoveHandler(ShipSelectionEvent, value); }
        }

        public ShipDataCustom Ship { get; set; }

        public ShipSelectionItem()
        {
            InitializeComponent();
        }

        public ShipSelectionItem(ShipDataCustom ship) : this()
        {
            Ship = ship;

            ShipItem.Content = $"{Ship.ID} {Ship.Name} Lv. {Ship.Level}";
        }

        void ItemOnClick(object sender, RoutedEventArgs e)
        {
            ShipSelectRoutedEventArgs args = new ShipSelectRoutedEventArgs(ShipSelectionEvent, Ship);
            RaiseEvent(args);
        }
    }
}