using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Linq;
using ElectronicObserverTypes;

namespace ElectronicObserverViewModels
{
    public class FleetViewModel : Observable
    {
        public ObservableCollection<ShipViewModel> ShipViewModels { get; }

        public FleetViewModel()
        {
            ShipViewModels = new ObservableCollection<ShipViewModel>();
            ShipViewModels.CollectionChanged += ShipViewModelsOnCollectionChanged;
        }

        public double LoS => ShipViewModels.Sum(vm => vm.BaseLoS);

        private void ShipViewModelsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (ShipViewModel item in e.OldItems)
                {
                    //Removed items
                    item.PropertyChanged -= ShipStatChange;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (ShipViewModel item in e.NewItems)
                {
                    //Added items
                    item.PropertyChanged += ShipStatChange;
                }
            }
        }


        private void ShipStatChange(object sender, PropertyChangedEventArgs e)
        {
            // optimization point
            OnPropertyChanged(string.Empty);
        }
    }
}
