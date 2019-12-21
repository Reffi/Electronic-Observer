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
        private IGrouping<ShipClasses, ShipDataCustom>? _classGroup;

        public IGrouping<ShipClasses, ShipDataCustom> ClassGroup
        {
            get => _classGroup!;
            set
            {
                _classGroup = value;

                ShipClassContainer.Children.Clear();

                foreach (ShipDataCustom ship in ClassGroup)
                {
                    DockPanel s = new DockPanel();

                    SolidColorBrush brush = (SolidColorBrush)(new BrushConverter().ConvertFrom(LevelColor(ship.Level)));

                    s.Children.Add(new Label
                    {
                        Content = $"{ship.Name}",
                        HorizontalContentAlignment = HorizontalAlignment.Left,
                        Foreground = brush
                    });

                    s.Children.Add(new Label
                    {
                        Content = $"{ship.Level}",
                        HorizontalContentAlignment = HorizontalAlignment.Right,
                        Foreground = brush
                    });

                    ShipClassContainer.Children.Add(s);
                }

                string LevelColor(int level) => (level switch
                {
                    175 => Colors.DeepPink,
                    _ when level >= 99 => Colors.DodgerBlue,
                    _ when level >= 90 => Colors.Green,
                    _ when level >= 1 => Colors.Yellow,
                    _ => Colors.Red
                }).ToString();
            }
        }

        public ShipClassGroupControl()
        {
            InitializeComponent();
        }
    }
}
