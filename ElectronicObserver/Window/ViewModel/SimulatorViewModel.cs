using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data.Damage;
using ElectronicObserver.Utility.Helpers;

namespace ElectronicObserver.Window.ViewModel
{
    public class SimulatorViewModel : Observable
    {
        private DamageBonus _bonus;

        public DamageBonus Bonus
        {
            get => _bonus;
            set
            {
                _bonus = value;
                SetField(ref _bonus, value);
            }
        }
    }
}
