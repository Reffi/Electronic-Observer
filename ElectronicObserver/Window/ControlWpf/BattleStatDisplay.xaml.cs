using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace ElectronicObserver.Window.ControlWpf
{
    /// <summary>
    /// Interaction logic for BattleStatDisplay.xaml
    /// </summary>
    public partial class BattleStatDisplay : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        private string _attackName;
        private string _value;

        public string AttackName
        {
            get => _attackName;
            set
            {
                if (value == null)
                    Visibility = Visibility.Collapsed;

                Visibility = Visibility.Visible;
                SeparatorDisplay.Visibility = Visibility.Collapsed;
                AttackNameDisplay.Visibility = Visibility.Visible;
                ValueDisplay.Visibility = Visibility.Visible;

                SetField(ref _attackName, value);
            }
        }

        public string Value
        {
            get => _value;
            set
            {
                SetField(ref _value, value);
            }
        }

        public string Separator
        {
            get => _value;
            set
            {
                SeparatorDisplay.Visibility = Visibility.Visible;
                AttackNameDisplay.Visibility = Visibility.Collapsed;
                ValueDisplay.Visibility = Visibility.Collapsed;

                SetField(ref _value, value);
            }
        }


        public BattleStatDisplay()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}
