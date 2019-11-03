using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using ElectronicObserver.Data.Damage;
using ElectronicObserver.Utility.Helpers;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.ControlWpf
{
    public class DamageBonusViewModel : Observable
    {
        public DamageBonus Bonus
        {
            get => _bonus;
            set
            {
                _bonus = value;

                _a1 = _bonus.a1;
                _a2 = _bonus.a2;
                _a3 = _bonus.a3;
                _a4 = _bonus.a4;
                _a5 = _bonus.a5;
                _a6 = _bonus.a6;
                _a7 = _bonus.a7;
                _a8 = _bonus.a8;
                _a9 = _bonus.a9;
                _a10 = _bonus.a10;
                _a11 = _bonus.a11;
                _a12 = _bonus.a12;
                _a13 = _bonus.a13;
                _a14 = _bonus.a14;

                _b1 = _bonus.b1;
                _b2 = _bonus.b2;
                _b3 = _bonus.b3;
                _b4 = _bonus.b4;
                _b5 = _bonus.b5;
                _b6 = _bonus.b6;
                _b7 = _bonus.b7;
                _b8 = _bonus.b8;
                _b9 = _bonus.b9;
                _b10 = _bonus.b10;
                _b11 = _bonus.b11;
                _b12 = _bonus.b12;
                _b13 = _bonus.b13;
                _b14 = _bonus.b14;
            }
        }

        private DamageBonus _bonus;

        private double _a1;
        private double _a2;
        private double _a3;
        private double _a4;
        private double _a5;
        private double _a6;
        private double _a7;
        private double _a8;
        private double _a9;
        private double _a10;
        private double _a11;
        private double _a12;
        private double _a13;
        private double _a14;

        private double _b1;
        private double _b2;
        private double _b3;
        private double _b4;
        private double _b5;
        private double _b6;
        private double _b7;
        private double _b8;
        private double _b9;
        private double _b10;
        private double _b11;
        private double _b12;
        private double _b13;
        private double _b14;

        public double a1
        {
            get => Bonus.a1;
            set
            {
                Bonus.a1 = value;
                SetField(ref _a1, value);
            }
        }
        public double a2
        {
            get => Bonus.a2;
            set
            {
                Bonus.a2 = value;
                SetField(ref _a2, value);
            }
        }
        public double a3
        {
            get => Bonus.a3;
            set
            {
                Bonus.a3 = value;
                SetField(ref _a3, value);
            }
        }
        public double a4
        {
            get => Bonus.a4;
            set
            {
                Bonus.a4 = value;
                SetField(ref _a4, value);
            }
        }
        public double a5
        {
            get => Bonus.a5;
            set
            {
                Bonus.a5 = value;
                SetField(ref _a5, value);
            }
        }
        public double a6
        {
            get => Bonus.a6;
            set
            {
                Bonus.a6 = value;
                SetField(ref _a6, value);
            }
        }
        public double a7
        {
            get => Bonus.a7;
            set
            {
                Bonus.a7 = value;
                SetField(ref _a7, value);
            }
        }
        public double a8
        {
            get => Bonus.a8;
            set
            {
                Bonus.a8 = value;
                SetField(ref _a8, value);
            }
        }
        public double a9
        {
            get => Bonus.a9;
            set
            {
                Bonus.a9 = value;
                SetField(ref _a9, value);
            }
        }
        public double a10
        {
            get => Bonus.a10;
            set
            {
                Bonus.a10 = value;
                SetField(ref _a10, value);
            }
        }
        public double a11
        {
            get => Bonus.a11;
            set
            {
                Bonus.a11 = value;
                SetField(ref _a11, value);
            }
        }
        public double a12
        {
            get => Bonus.a12;
            set
            {
                Bonus.a12 = value;
                SetField(ref _a12, value);
            }
        }
        public double a13
        {
            get => Bonus.a13;
            set
            {
                Bonus.a13 = value;
                SetField(ref _a13, value);
            }
        }
        public double a14
        {
            get => Bonus.a14;
            set
            {
                Bonus.a14 = value;
                SetField(ref _a14, value);
            }
        }


        public double b1
        {
            get => Bonus.b1;
            set
            {
                Bonus.b1 = value;
                SetField(ref _b1, value);
            }
        }
        public double b2
        {
            get => Bonus.b2;
            set
            {
                Bonus.b2 = value;
                SetField(ref _b2, value);
            }
        }
        public double b3
        {
            get => Bonus.b3;
            set
            {
                Bonus.b3 = value;
                SetField(ref _b3, value);
            }
        }
        public double b4
        {
            get => Bonus.b4;
            set
            {
                Bonus.b4 = value;
                SetField(ref _b4, value);
            }
        }
        public double b5
        {
            get => Bonus.b5;
            set
            {
                Bonus.b5 = value;
                SetField(ref _b5, value);
            }
        }
        public double b6
        {
            get => Bonus.b6;
            set
            {
                Bonus.b6 = value;
                SetField(ref _b6, value);
            }
        }
        public double b7
        {
            get => Bonus.b7;
            set
            {
                Bonus.b7 = value;
                SetField(ref _b7, value);
            }
        }
        public double b8
        {
            get => Bonus.b8;
            set
            {
                Bonus.b8 = value;
                SetField(ref _b8, value);
            }
        }
        public double b9
        {
            get => Bonus.b9;
            set
            {
                Bonus.b9 = value;
                SetField(ref _b9, value);
            }
        }
        public double b10
        {
            get => Bonus.b10;
            set
            {
                Bonus.b10 = value;
                SetField(ref _b10, value);
            }
        }
        public double b11
        {
            get => Bonus.b11;
            set
            {
                Bonus.b11 = value;
                SetField(ref _b11, value);
            }
        }
        public double b12
        {
            get => Bonus.b12;
            set
            {
                Bonus.b12 = value;
                SetField(ref _b12, value);
            }
        }
        public double b13
        {
            get => Bonus.b13;
            set
            {
                Bonus.b13 = value;
                SetField(ref _b13, value);
            }
        }
        public double b14
        {
            get => Bonus.b14;
            set
            {
                Bonus.b14 = value;
                SetField(ref _b14, value);
            }
        }

        public DamageBonusViewModel() : this(new DamageBonus()) { }

        public DamageBonusViewModel(DamageBonus bonus) => Bonus = bonus ?? new DamageBonus();
    }

    /// <summary>
    /// Interaction logic for ExtraDamageBonusDisplay.xaml
    /// </summary>
    public partial class DamageBonusDisplay : UserControl
    {
        
        /*public static readonly DependencyProperty DamageBonusProperty = DependencyProperty.Register(
            nameof(DamageBonus), typeof(DamageBonus), typeof(DamageBonusDisplay),
            new PropertyMetadata(ExtraDamageBonusChanged));

        public DamageBonus DamageBonus
        {
            get => (DamageBonus)GetValue(DamageBonusProperty);
            set => SetValue(DamageBonusProperty, value);
        }

        private static void ExtraDamageBonusChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((DamageBonusDisplay) obj).Model.Bonus = (DamageBonus)args.NewValue;
        }*/

        private DamageBonusViewModel Model { get; set; }

        public DamageBonus DamageBonus
        {
            get => Model.Bonus;
            set
            {
                Model = new DamageBonusViewModel(value);
                Model.PropertyChanged += CalculationParametersChanged;

                DataContext = Model;
            }
        }

        public DamageBonusDisplay()
        {
            InitializeComponent();
        }

        private void CalculationParametersChanged(object sender, PropertyChangedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DialogShipSimulationWpf.CalculationParametersChangedEvent, this));
        }
    }
}