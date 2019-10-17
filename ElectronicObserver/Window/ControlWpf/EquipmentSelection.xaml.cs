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
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Helpers;
using ElectronicObserver.Window.ViewModel;

namespace ElectronicObserver.Window.ControlWpf
{
    /// <summary>
    /// Interaction logic for EquipmentSelection.xaml
    /// </summary>
    public partial class EquipmentSelection : UserControl, INotifyPropertyChanged
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

        private IEnumerable<EquipmentSelectionItem> _displayedEquipment;

        public IEnumerable<EquipmentSelectionItem> DisplayedEquipment
        {
            get => _displayedEquipment;
            set => SetField(ref _displayedEquipment, value);
        }

        private IEnumerable<EquipmentTypes> _filter = Enumerable.Empty<EquipmentTypes>();

        private IEnumerable<EquipmentTypes> _equippableCategories;
        public IEnumerable<EquipmentTypes> EquippableCategories
        {
            get => _equippableCategories;
            set
            {
                _equippableCategories = value;

                Filters.Children.Clear();
                

                foreach (EquipmentTypeGroup type in Enum.GetValues(typeof(EquipmentTypeGroup)))
                {
                    Button button = new Button();

                    button.Content = type.Display();
                    button.Click += (s, e) =>
                    {
                        _filter = Constants.GetEquipmentTypeGroup(type);
                        ReloadList();
                    };

                    Filters.Children.Add(button);
                }

            }
        }

        private IEnumerable<EquipmentDataCustom> _equips;
        public IEnumerable<EquipmentDataCustom> Equips
        {
            get => _equips;
            set => _equips = value;
        }

        public EquipmentSelection()
        {
            InitializeComponent();
            List.DataContext = this;
        }

        private void ReloadList()
        {
            DisplayedEquipment = _equips?
                .Where(eq => _filter
                    .Where(f => _equippableCategories.Contains(f))
                    .Contains(eq.CategoryType))
                .OrderBy(eq => eq.ID)
                .ThenByDescending(eq => eq.Level)
                .Select(eq => new EquipmentSelectionItem(eq))
                .ToList();
        }
    }
}
