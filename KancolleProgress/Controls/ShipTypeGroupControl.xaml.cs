using System;
using System.Collections.Generic;
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

namespace KancolleProgress.Controls
{
    /// <summary>
    /// Interaction logic for ShipTypeGroupControl.xaml
    /// </summary>
    public partial class ShipTypeGroupControl : UserControl
    {
        private IGrouping<ShipTypeGroup, ShipDataCustom>? _group;

        public IGrouping<ShipTypeGroup, ShipDataCustom> Group
        {
            get => _group!;
            set
            {
                _group = value;

                ShipTypeGroupLabel.Content = Group.Key.Display();

                ShipClassContainer.Children.Clear();

                var shipClassGroups = Group
                    .GroupBy(s => s.ShipClass);

                foreach (var shipClassGroup in shipClassGroups)
                {
                    /*StackPanel s = new StackPanel();

                    foreach (ShipDataCustom ship in shipClassGroup)
                    {
                        s.Children.Add(new Label
                        {
                            Content = $"{ship.Name} {ship.Level}"
                        });
                    }*/

                    ShipClassContainer.Children.Add(new ShipClassGroupControl{ClassGroup = shipClassGroup });
                }
            }
        }

        public ShipTypeGroupControl()
        {
            InitializeComponent();
        }
    }
}
