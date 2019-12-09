using ElectronicObserverTypes;

namespace ElectronicObserverViewModels
{
    public class SynergyViewModel: Observable
    {
        public FitBonusCustom Synergy
        {
            get => _synergy;
            set
            {
                _synergy = value;

                _firepower = Synergy.Firepower;
                _torpedo = Synergy.Torpedo;
                _aa = Synergy.AA;
                _asw = Synergy.ASW;
                _evasion = Synergy.Evasion;
                _armor = Synergy.Armor;
                _los = Synergy.LoS;
                _accuracy = Synergy.Accuracy;

                OnPropertyChanged(string.Empty);
            }
        }

        private int _firepower;
        private int _torpedo;
        private int _aa;
        private int _asw;
        private int _evasion;
        private int _armor;
        private int _los;
        private int _accuracy;
        private FitBonusCustom _synergy;

        public int Firepower
        {
            get => Synergy.Firepower;
            set
            {
                Synergy.Firepower = value;
                SetField(ref _firepower, value);
            }
        }
        public int Torpedo
        {
            get => Synergy.Torpedo;
            set
            {
                Synergy.Torpedo = value;
                SetField(ref _torpedo, value);
            }
        }
        public int AA
        {
            get => Synergy.AA;
            set
            {
                Synergy.AA = value;
                SetField(ref _aa, value);
            }
        }
        public int ASW
        {
            get => Synergy.ASW;
            set
            {
                Synergy.ASW = value;
                SetField(ref _asw, value);
            }
        }
        public int Evasion
        {
            get => Synergy.Evasion;
            set
            {
                Synergy.Evasion = value;
                SetField(ref _evasion, value);
            }
        }
        public int Armor
        {
            get => Synergy.Armor;
            set
            {
                Synergy.Armor = value;
                SetField(ref _armor, value);
            }
        }
        public int LoS
        {
            get => Synergy.LoS;
            set
            {
                Synergy.LoS = value;
                SetField(ref _los, value);
            }
        }

        /*public int Accuracy
        {
            get => _ship.CurrentSynergies.Accuracy;
            set
            {
                _ship.CurrentSynergies.Accuracy = value;
                SetField(ref _accuracy, value);
            }
        }*/

        public SynergyViewModel() => Synergy = new FitBonusCustom();

        public SynergyViewModel(FitBonusCustom synergy)
        {
            Synergy = synergy;
        }
    }
}
