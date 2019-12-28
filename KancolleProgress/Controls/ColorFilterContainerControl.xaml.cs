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
    /// Interaction logic for ColorFilterContainerControl.xaml
    /// </summary>
    public partial class ColorFilterContainerControl : UserControl
    {
        private List<ShipDataCustom> _ships;

        List<ColorFilter> ColorFilters => new List<ColorFilter>
        {
            new ColorFilter{Level = 175, Name = "Max", Ships = Ships},
            new ColorFilter{Level = 99, Name = "99+", Ships = Ships},
            new ColorFilter{Level = 90, Name = "90+", Ships = Ships},
            new ColorFilter{Level = 1, Name = "Collection", Ships = Ships},
            new ColorFilter{Level = 0, Name = "Missing", Ships = Ships},
        };

        public List<ShipDataCustom> Ships
        {
            get => _ships;
            set
            {
                _ships = value;

                ColorFilterContainer.Children.Clear();

                foreach (ColorFilter colorFilter in ColorFilters)
                {
                    ColorFilterContainer.Children.Add(new ColorFilterControl { ColorFilter = colorFilter });
                }
            } 

        }

        public ColorFilterContainerControl()
        {
            InitializeComponent();
        }
    }
}
