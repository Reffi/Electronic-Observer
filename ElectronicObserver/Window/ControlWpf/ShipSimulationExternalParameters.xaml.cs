using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Helpers;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.ControlWpf
{
    /// <summary>
    /// Interaction logic for ShipSimulationExternalParameters.xaml
    /// </summary>
    public partial class ShipSimulationExternalParameters : UserControl
    {
        public class ComboboxEnumItem<TEnumType> where TEnumType : System.Enum
        {
            public TEnumType Member { get; }
            public string Display => Member.Display();

            public ComboboxEnumItem(TEnumType member)
            {
                Member = member;
            }
        }

        public class ExternalParameters : Observable
        {
            private ComboboxEnumItem<EngagementTypes> _engagement;
            private ComboboxEnumItem<FormationType> _formation;
            private ComboboxEnumItem<FleetType> _fleet;
            private ComboboxEnumItem<FleetPositionDetail> _positionDetails;
            private bool _nightRecon;
            private bool _flare;
            private bool _searchlight;

            public ComboboxEnumItem<EngagementTypes> Engagement
            {
                get => _engagement;
                set => SetField(ref _engagement, value);
            }

            public ComboboxEnumItem<FormationType> Formation
            {
                get => _formation;
                set => SetField(ref _formation, value);
            }

            public ComboboxEnumItem<FleetType> Fleet
            {
                get => _fleet;
                set => SetField(ref _fleet, value);
            }

            public ComboboxEnumItem<FleetPositionDetail> PositionDetails
            {
                get => _positionDetails;
                set => SetField(ref _positionDetails, value);
            }

            public bool NightRecon
            {
                get => _nightRecon;
                set => SetField(ref _nightRecon, value);
            }

            public bool Flare
            {
                get => _flare;
                set => SetField(ref _flare, value);
            }

            public bool Searchlight
            {
                get => _searchlight;
                set => SetField(ref _searchlight, value);
            }

            /*public IEnumerable<FormationType> Formations =>
                Enum.GetValues(typeof(FormationType)).Cast<FormationType>();*/

            public ExternalParameters(EngagementTypes engagement = EngagementTypes.Parallel,
                FormationType formation = FormationType.LineAhead, FleetType fleet = FleetType.Single, 
                FleetPositionDetail positiionDetail = FleetPositionDetail.MainFlag, bool nightRecon = false, 
                bool flare = false, bool searchlight = false)
            {
                NightRecon = nightRecon;
                Flare = flare;
                Searchlight = searchlight;
            }
        }

        private ExternalParameters _externalParameters;
        public ExternalParameters Parameters => _externalParameters;

        public ShipSimulationExternalParameters()
        {
            // bind to fleet and battle?
            InitializeComponent();
            _externalParameters = new ExternalParameters();

            List<ComboboxEnumItem<FormationType>> formations = Enum.GetValues(typeof(FormationType))
                .Cast<FormationType>()
                .Select(x => new ComboboxEnumItem<FormationType>(x))
                .ToList();

            List<ComboboxEnumItem<EngagementTypes>> engagements = Enum.GetValues(typeof(EngagementTypes))
                .Cast<EngagementTypes>()
                .Select(x => new ComboboxEnumItem<EngagementTypes>(x))
                .ToList();

            List<ComboboxEnumItem<FleetType>> fleets = Enum.GetValues(typeof(FleetType))
                .Cast<FleetType>()
                .Select(x => new ComboboxEnumItem<FleetType>(x))
                .ToList();

            List<ComboboxEnumItem<FleetPositionDetail>> positionDetails = Enum.GetValues(typeof(FleetPositionDetail))
                .Cast<FleetPositionDetail>()
                .Select(x => new ComboboxEnumItem<FleetPositionDetail>(x))
                .ToList();

            FormationSelect.Items.Clear();
            FormationSelect.ItemsSource = formations;

            EngagementSelect.Items.Clear();
            EngagementSelect.ItemsSource = engagements;

            FleetTypeSelect.Items.Clear();
            FleetTypeSelect.ItemsSource = fleets;

            FleetPositionDetails.Items.Clear();
            FleetPositionDetails.ItemsSource = positionDetails;

            _externalParameters.Formation = formations.Single(x => x.Member == FormationType.LineAhead);
            _externalParameters.Fleet = fleets.Single(x => x.Member == FleetType.Single);
            _externalParameters.Engagement = engagements.Single(x => x.Member == EngagementTypes.Parallel);
            _externalParameters.PositionDetails = positionDetails.Single(x => x.Member == FleetPositionDetail.MainFlag);



            DataContext = _externalParameters;
        }

        public ShipSimulationExternalParameters(EngagementTypes engagement = EngagementTypes.Parallel,
            FormationType formation = FormationType.LineAhead, FleetType fleet = FleetType.Single,
            FleetPositionDetail positiionDetail = FleetPositionDetail.MainFlag, bool nightRecon = false, 
            bool flare = false, bool searchlight = false)
        {
            _externalParameters = new ExternalParameters(engagement, formation, fleet, positiionDetail,
                nightRecon, flare, searchlight);
            DataContext = _externalParameters;
        }

        public void ParameterChange(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(DialogShipSimulationWpf.CalculationParametersChangedEvent, this));
        }
    }
}
