﻿using System;
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

namespace ElectronicObserver.Window.ControlWpf
{
    /// <summary>
    /// Interaction logic for ShipSelection.xaml
    /// </summary>
    public partial class ShipSelection : UserControl, INotifyPropertyChanged
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

        private IEnumerable<ShipTypes> _filter = Enumerable.Empty<ShipTypes>();

        private IEnumerable<ShipSelectionItem> _displayedShips;
        public IEnumerable<ShipSelectionItem> DisplayedShips
        {
            get => _displayedShips;
            set => SetField(ref _displayedShips, value);
        }


        private IEnumerable<ShipDataCustom> _ships;
        public IEnumerable<ShipDataCustom> Ships
        {
            get => _ships;
            set
            {
                _ships = value;

                foreach (ShipTypeGroup type in Enum.GetValues(typeof(ShipTypeGroup)))
                {
                    Button button = new Button();

                    button.Content = type.Display();
                    button.Click += (s, e) =>
                    {
                        _filter = Constants.GetShipTypeGroup(type);
                        ReloadList();
                    };

                    Filters.Children.Add(button);
                }

                ReloadList();
            }
        }

        public ShipSelection()
        {
            InitializeComponent();
            List.DataContext = this;
        }

        private void ReloadList()
        {
            DisplayedShips = _ships?
                .Where(ship => _filter.Contains(ship.ShipType))
                .OrderByDescending(ship => ship.Level)
                .Select(ship => new ShipSelectionItem(ship))
                .ToList();
        }
    }
}