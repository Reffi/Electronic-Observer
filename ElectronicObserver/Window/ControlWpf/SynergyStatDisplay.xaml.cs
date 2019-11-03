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
        private SynergyViewModel _synergy;

        public SynergyViewModel Synergy
        {
            get => _synergy;
            set
            {
                _synergy = value;
                DataContext = Synergy;
            }
        }

        public SynergyStatDisplay()
        {
            InitializeComponent();

            /*FirepowerIcon.StatIcon = IconContent.ParameterFirepower;
            TorpedoIcon.StatIcon = IconContent.ParameterTorpedo;
            AaIcon.StatIcon = IconContent.ParameterAA;
            ArmorIcon.StatIcon = IconContent.ParameterArmor;
            AswIcon.StatIcon = IconContent.ParameterASW;
            EvasionIcon.StatIcon = IconContent.ParameterEvasion;
            LoSIcon.StatIcon = IconContent.ParameterLOS;*/
        }
    }
}
