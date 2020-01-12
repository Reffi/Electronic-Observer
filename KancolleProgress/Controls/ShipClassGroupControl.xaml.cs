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
    /// Interaction logic for ShipClassGroupControl.xaml
    /// </summary>
    public partial class ShipClassGroupControl : UserControl
    {
        private IGrouping<ShipClass, ShipDataCustom>? _classGroup;

        public IGrouping<ShipClass, ShipDataCustom> ClassGroup
        {
            get => _classGroup!;
            set
            {
                _classGroup = value;

                ShipClassContainer.Children.Clear();

                foreach (ShipDataCustom ship in ClassGroup)
                {
                    DockPanel dockPanel = new DockPanel();

                    SolidColorBrush brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(ColorFilter.LevelColor(ship.Level)));

                    dockPanel.Children.Add(new Label
                    {
                        Content = $"{ship.Name}",
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        Foreground = brush
                    });

                    dockPanel.Children.Add(new Label
                    {
                        Content = $"{ship.Level}",
                        HorizontalContentAlignment = HorizontalAlignment.Right,
                        Foreground = brush
                    });

                    ShipClassContainer.Children.Add(dockPanel);
                }
            }
        }

        public ShipClassGroupControl()
        {
            InitializeComponent();
        }
    }
}
