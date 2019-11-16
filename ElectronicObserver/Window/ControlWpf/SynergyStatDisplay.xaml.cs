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
using ElectronicObserver.Resource;
using ElectronicObserver.Window.ViewModel;

namespace ElectronicObserver.Window.ControlWpf
{
    /// <summary>
    /// Interaction logic for SynergyStatDisplay.xaml
    /// </summary>
    public partial class SynergyStatDisplay : UserControl
    {
        private SynergyViewModel _synergyViewModel;

        public SynergyViewModel SynergyViewModel
        {
            get => _synergyViewModel;
            set
            {
                _synergyViewModel = value;
                DataContext = SynergyViewModel;
            }
        }

        public SynergyStatDisplay()
        {
            InitializeComponent();

            SynergyViewModel = new SynergyViewModel();
            DataContext = SynergyViewModel;
        }
    }
}
