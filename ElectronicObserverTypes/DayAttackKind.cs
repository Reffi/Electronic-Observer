using System.ComponentModel.DataAnnotations;

namespace ElectronicObserverTypes
{
    /// <summary>
    /// 昼戦攻撃種別を表します。
    /// </summary>
    public enum DayAttackKind
    {
        /// <summary> 不明 </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "Unknown")]
        Unknown = -1,

        /// <summary> 通常攻撃 (API上でのみ使用されます) </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "NormalAttack")]
        NormalAttack,

        /// <summary> レーザー攻撃 </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "Laser")]
        Laser,

        /// <summary> 連続射撃 </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "DoubleShelling")]
        DoubleShelling,

        /// <summary> カットイン(主砲/副砲) </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "CutinMainSub")]
        CutinMainSub,

        /// <summary> カットイン(主砲/電探) </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "CutinMainRadar")]
        CutinMainRadar,

        /// <summary> カットイン(主砲/徹甲弾) </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "CutinMainAP")]
        CutinMainAP,

        /// <summary> カットイン(主砲/主砲) </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "CutinMainMain")]
        CutinMainMain,

        /// <summary> 空母カットイン </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "CutinAirAttack")]
        CutinAirAttack,

        /// <summary> Nelson Touch </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "SpecialNelson")]
        SpecialNelson = 100,

        /// <summary> 一斉射かッ…胸が熱いな！ </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "SpecialNagato")]
        SpecialNagato = 101,

        /// <summary> 長門、いい？ いくわよ！ 主砲一斉射ッ！ </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "SpecialMutsu")]
        SpecialMutsu = 102,

        /// <summary> 瑞雲立体攻撃 </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "ZuiunMultiAngle")]
        ZuiunMultiAngle = 200,

        /// <summary> 海空立体攻撃 </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "SeaAirMultiAngle")]
        SeaAirMultiAngle = 201,


        /// <summary> 砲撃 </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "Shelling")]
        Shelling = 1000,

        /// <summary> 空撃 </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "AirAttack")]
        AirAttack,

        /// <summary> 爆雷攻撃 </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "DepthCharge")]
        DepthCharge,

        /// <summary> 雷撃 </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "Torpedo")]
        Torpedo,


        /// <summary> ロケット攻撃 </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "Rocket")]
        Rocket = 2000,


        /// <summary> 揚陸攻撃(大発動艇) </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "LandingDaihatsu")]
        LandingDaihatsu = 3000,

        /// <summary> 揚陸攻撃(特大発動艇) </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "LandingTokuDaihatsu")]
        LandingTokuDaihatsu,

        /// <summary> 揚陸攻撃(大発動艇(八九式中戦車&陸戦隊)) </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "LandingDaihatsuTank")]
        LandingDaihatsuTank,

        /// <summary> 揚陸攻撃(特二式内火艇) </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "LandingAmphibious")]
        LandingAmphibious,

        /// <summary> 揚陸攻撃(特大発動艇+戦車第11連隊) </summary>
        [Display(ResourceType = typeof(Properties.DayAttackKind), Name = "LandingTokuDaihatsuTank")]
        LandingTokuDaihatsuTank,

    }
}
