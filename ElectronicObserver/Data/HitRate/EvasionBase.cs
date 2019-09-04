using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data.HitRate
{
    public interface IEvasionShip<out TEquipType> where TEquipType : IEvasionEquipment
    {
        int BaseEvasion { get; }
        int BaseLuck { get; }
        int Fuel { get; }

        TEquipType[] Equipment { get; }
    }

    public interface IEvasionEquipment
    {
        int BaseEvasion { get; }
    }

    public abstract class EvasionBase : IEvasion
    {
        private IEvasionShip<IEvasionEquipment> Ship;

        public double Capped => Cap(PrecapBase * PrecapMods);

        public double Postcap => Math.Floor(PostcapMod * (Math.Floor(Capped) + PostcapBonus + FuelBonus));

        protected EvasionBase(IEvasionShip<IEvasionEquipment> ship)
        {
            Ship = ship;
        }

        private double PrecapBase =>
            Ship.BaseEvasion
            + Ship.Equipment.Where(eq => eq != null).Sum(eq => eq.BaseEvasion)
            + Math.Sqrt(2 * Ship.BaseLuck);

        protected abstract double PrecapMods { get; }

        protected abstract double PostcapMod { get; }

        protected abstract double PostcapBonus { get; }


        // protected double PostcapBonus => TorpedoEvasion + HeavyCruiserBonus

        // private double SearchlightMod => Battle.ActivatedSearchlight ? 0.2 : 1;
        private double TorpedoEvasion => 0;
        private double HeavyCruiserBonus => 0;

        // potentially misleading since it can only be 0 or negative
        private double FuelBonus => Math.Min(Ship.Fuel - 75, 0);

        private double Cap(double evasion) => evasion switch
        {
            double ev when ev > 64 => 55 + 2 * Math.Sqrt(evasion - 65),
            double ev when ev > 39 => 40 + 3 * Math.Sqrt(evasion - 40),
            double ev => ev
        };
    }
}
