using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.ControlWpf
{
    /// <summary>
    /// Interaction logic for Stat.xaml
    /// </summary>
    public partial class Stat : UserControl
    {
        public static readonly DependencyProperty StatNameProperty = DependencyProperty
            .Register(nameof(StatName), typeof(string), typeof(Stat),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public static readonly DependencyProperty BaseStatProperty = DependencyProperty
            .Register(nameof(BaseStat), typeof(int), typeof(Stat),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string StatName
        {
            get => (string)GetValue(StatNameProperty);
            set => SetValue(StatNameProperty, value);
        }

        public int BaseStat
        {
            get => (int)GetValue(BaseStatProperty);
            set => SetValue(BaseStatProperty, value);
        }

        public Stat()
        {
            InitializeComponent();
            // Since data context is a dependency property and dependency properties inherit
            // down the visual tree, we can set the data context of a container and intercept
            // the data context’s inheritance.
            (Content as FrameworkElement).DataContext = this;
        }


        private void StatChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void StatChanged(object sender, DataTransferEventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs(DialogShipSimulationWpf.CalculationParametersChangedEvent);
            RaiseEvent(args);
        }
    }
}
