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
using ElectronicObserver.Window.ViewModel;

namespace ElectronicObserver.Window.ControlWpf
{
    /// <summary>
    /// Interaction logic for EquipmentSelectionItem.xaml
    /// </summary>
    public partial class EquipmentSelectionItem : UserControl
    {
        public class EquipmentSelectRoutedEventArgs : RoutedEventArgs
        {
            public EquipmentDataCustom Equip { get; set; }

            public EquipmentSelectRoutedEventArgs(RoutedEvent routedEvent, EquipmentDataCustom equip) : base(routedEvent)
            {
                Equip = equip;
            }
        }

        public static readonly RoutedEvent EquipmentSelectionEvent = EventManager.RegisterRoutedEvent(
            "EquipmentSelection", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EquipmentSelectionItem));

        public event RoutedEventHandler EquipmentSelected
        {
            add { AddHandler(EquipmentSelectionEvent, value); }
            remove { RemoveHandler(EquipmentSelectionEvent, value); }
        }

        public EquipmentDataCustom Equip { get; set; }

        public EquipmentSelectionItem()
        {
            InitializeComponent();
        }

        public EquipmentSelectionItem(EquipmentDataCustom equip) : this()
        {
            Equip = equip;

            EquipmentItem.Content = $" {Equip.Name} +{Equip.Level}";
        }

        void ItemOnClick(object sender, RoutedEventArgs e)
        {
            EquipmentSelectRoutedEventArgs args = new EquipmentSelectRoutedEventArgs(EquipmentSelectionEvent, Equip);
            RaiseEvent(args);
        }
    }
}
