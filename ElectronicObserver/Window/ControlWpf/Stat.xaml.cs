using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Helpers;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.ViewModel;

namespace ElectronicObserver.Window.ControlWpf
{
    /// <summary>
    /// Interaction logic for Stat.xaml
    /// </summary>
    public partial class Stat : UserControl
    {
        public static readonly DependencyProperty StatIconProperty = DependencyProperty
            .Register(nameof(StatIcon), typeof(ImageSource), typeof(Stat),
                new FrameworkPropertyMetadata(default(ImageSource), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty OtherContentProperty = DependencyProperty
            .Register(nameof(OtherContent), typeof(object), typeof(Stat),
                new FrameworkPropertyMetadata(default, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty BaseStatProperty = DependencyProperty
            .Register(nameof(BaseStat), typeof(int), typeof(Stat),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public ImageSource StatIcon
        {
            get => (ImageSource)GetValue(StatIconProperty);
            set => SetValue(StatIconProperty, value);
        }

        public object OtherContent
        {
            get => GetValue(OtherContentProperty);
            set => SetValue(OtherContentProperty, value);
        }

        public int BaseStat
        {
            get => (int)GetValue(BaseStatProperty);
            set => SetValue(BaseStatProperty, value);
        }

        public Stat()
        {
            InitializeComponent();

            (Content as FrameworkElement).DataContext = this;
        }

        private void StatChanged(object sender, DataTransferEventArgs e)
        {
            RoutedEventArgs args = new RoutedEventArgs(DialogShipSimulationWpf.CalculationParametersChangedEvent);
            RaiseEvent(args);
        }
    }
}
