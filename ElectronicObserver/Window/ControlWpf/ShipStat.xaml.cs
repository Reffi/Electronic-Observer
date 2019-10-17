using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility.Helpers;

namespace ElectronicObserver.Window.ControlWpf
{
    /// <summary>
    /// Interaction logic for ShipStat.xaml
    /// </summary>
    public partial class ShipStat : UserControl
    {
        public static readonly DependencyProperty StatIconProperty = DependencyProperty
            .Register(nameof(StatIcon), typeof(ImageSource), typeof(ShipStat),
                new FrameworkPropertyMetadata(default(ImageSource), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty OtherContentProperty = DependencyProperty
            .Register(nameof(OtherContent), typeof(object), typeof(ShipStat),
                new FrameworkPropertyMetadata(default, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty BaseStatProperty = DependencyProperty
            .Register(nameof(BaseStat), typeof(int), typeof(ShipStat), 
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty TotalStatProperty = DependencyProperty
            .Register(nameof(TotalStat), typeof(int), typeof(ShipStat),
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

        public int TotalStat
        {
            get => (int)GetValue(TotalStatProperty);
            set => SetValue(TotalStatProperty, value);
        }

        public ShipStat()
        {
            InitializeComponent();
            // Since data context is a dependency property and dependency properties inherit
            // down the visual tree, we can set the data context of a container and intercept
            // the data context’s inheritance.
            (Content as FrameworkElement).DataContext = this;

            //DataContext = this;
        }
    }
}
