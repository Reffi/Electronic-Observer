using System;

namespace ElectronicObserver.Data.Damage
{
    public abstract class DamageBase
    {
        Random rng = new Random();
        private DamageBonus Parameters { get; }

        private int Cap { get; }

        protected abstract double PrecapBase { get; }
        protected abstract double PrecapMods { get; }
        protected abstract double CritMod { get; }
        protected virtual double AttackKindPostcapMod => 1;
        protected abstract double BaseArmor { get; }

        public double Capped => CapDamage(((PrecapBase * Parameters.a12 + Parameters.b12)
                                           * Parameters.a13 + Parameters.b13)
                                          * PrecapMods * Parameters.a14 + Parameters.b14);

        // no...
        public double Postcap => PostcapInternal1 * AttackKindPostcapMod * Parameters.a11 + Parameters.b11;
        private double PostcapInternal1 => Math.Floor(PostcapInternal2 * Parameters.a10 + Parameters.b10);
        private double PostcapInternal2 => Math.Floor(PostcapInternal3 * CritMod * Parameters.a9 + Parameters.b9);
        private double PostcapInternal3 => Math.Floor(PostcapInternal4 * Parameters.a8 + Parameters.b8);
        private double PostcapInternal4 => Math.Floor(PostcapInternal5 * ApShellMod * Parameters.a7 + Parameters.b7);
        private double PostcapInternal5 => Math.Floor(PostcapInternal6 * Parameters.a6 + Parameters.b6);
        private double PostcapInternal6 => Math.Floor(Capped * Parameters.a5 + Parameters.b5);

        private double MinArmor => 0.7 * (Parameters.a3 * BaseArmor + Parameters.b3);
        private double AddedArmor => Math.Max(0, Parameters.a4 * BaseArmor + Parameters.b4);
        private double MaxArmor => MinArmor + 0.6 * Math.Max(0, AddedArmor - 1);
        private double Armor => Parameters.a2 * (MinArmor + 0.6 * rng.Next((int) AddedArmor)) + Parameters.b2;

        public double Min => (Postcap - MaxArmor) * AmmoMod * Parameters.a1 + Parameters.b1;
        public double Max => (Postcap - MinArmor) * AmmoMod * Parameters.a1 + Parameters.b1;

        protected virtual double ApShellMod => 1;
        protected virtual double AmmoMod => 1;

        private double CapDamage(double damage)
        {
            if (damage > Cap)
                damage = Cap + Math.Sqrt(damage - Cap);

            return damage;
        }

        private DamageBase()
        {
        }

        protected DamageBase(DamageBonus parameters, int cap)
        {
            Parameters = parameters ?? new DamageBonus();
            Cap = cap;
        }
    }
}