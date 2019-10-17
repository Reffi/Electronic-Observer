using ElectronicObserver.Utility.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data.Damage;
using ElectronicObserver.Data.HitRate;

namespace ElectronicObserver.Data
{
    public class FleetDataCustom : IShellingDamageAttackerFleet, IShellingDamageDefenderFleet, IAswDamageAttackerFleet,
        ICarrierShellingDamageAttackerFleet, ICarrierNightDamageAttackerFleet,
        IAswDamageDefenderFleet, INightDamageAttackerFleet, INightDamageDefenderFleet,
        ICarrierShellingDamageDefenderFleet, ICarrierNightDamageDefenderFleet, IShellingAccuracyFleet,
        IAswAccuracyFleet, INightAccuracyFleet, IEvasionFleet
    {
        public FleetType Type { get; set; } = FleetType.Single;

        public bool IsMain => PositionDetail == FleetPositionDetail.Main ||
                              PositionDetail == FleetPositionDetail.MainFlag;

        public bool IsVanguardTop => PositionDetail == FleetPositionDetail.VanguardTop;
        public FormationType Formation { get; set; } = FormationType.LineAhead;

        public bool NightRecon { get; set; } = true;
        public bool Flare { get; set; } = true;
        public bool Searchlight { get; set; } = true;



        public FleetPositionDetail PositionDetail { get; set; } = FleetPositionDetail.MainFlag;
    }
}
