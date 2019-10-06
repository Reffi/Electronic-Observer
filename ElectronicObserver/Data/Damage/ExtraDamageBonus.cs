namespace ElectronicObserver.Data.Damage
{
    public class ExtraDamageBonus
    {
        public double a1 { get; set; } = 1;
        public double a2 { get; set; } = 1;
        public double a3 { get; set; } = 1;
        public double a4 { get; set; } = 1;
        public double a5 { get; set; } = 1;
        public double a6 { get; set; } = 1;
        public double a7 { get; set; } = 1;
        public double a8 { get; set; } = 1;
        public double a9 { get; set; } = 1;
        public double a10 { get; set; } = 1;
        public double a11 { get; set; } = 1;
        /// <summary>
        /// DD/CL anti installation mod
        /// </summary>
        public double a12 { get; set; } = 1;
        /// <summary>
        /// land plane anti installation mod
        /// </summary>
        public double a13 { get; set; } = 1;
        /// <summary>
        /// precap mods
        /// </summary>
        public double a14 { get; set; } = 1;

        public double b1 { get; set; }
        public double b2 { get; set; }
        public double b3 { get; set; }
        public double b4 { get; set; }
        public double b5 { get; set; }
        public double b6 { get; set; }
        public double b7 { get; set; }
        public double b8 { get; set; }
        public double b9 { get; set; }
        public double b10 { get; set; }
        public double b11 { get; set; }

        /// <summary>
        /// sub anti installation bonus
        /// </summary>
        public double b12 { get; set; }

        /// <summary>
        /// WG42
        /// </summary>
        public double b13 { get; set; }

        public double b14 { get; set; }

        public static ExtraDamageBonus operator +(ExtraDamageBonus a, ExtraDamageBonus b) => new ExtraDamageBonus
        {
            a1 = a.a1 * b.a1,
            a2 = a.a2 * b.a2,
            a3 = a.a3 * b.a3,
            a4 = a.a4 * b.a4,
            a5 = a.a5 * b.a5,
            a6 = a.a6 * b.a6,
            a7 = a.a7 * b.a7,
            a8 = a.a8 * b.a8,
            a9 = a.a9 * b.a9,
            a10 = a.a10 * b.a10,
            a11 = a.a11 * b.a11,
            a12 = a.a12 * b.a12,
            a13 = a.a13 * b.a13,
            a14 = a.a14 * b.a14,

            b1 = a.b1 + b.b1,
            b2 = a.b2 + b.b2,
            b3 = a.b3 + b.b3,
            b4 = a.b4 + b.b4,
            b5 = a.b5 + b.b5,
            b6 = a.b6 + b.b6,
            b7 = a.b7 + b.b7,
            b8 = a.b8 + b.b8,
            b9 = a.b9 + b.b9,
            b10 = a.b10 + b.b10,
            b11 = a.b11 + b.b11,
            b12 = a.b12 + b.b12,
            b13 = a.b13 + b.b13,
            b14 = a.b14 + b.b14
        };
    }
}
