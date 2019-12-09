using System.ComponentModel.DataAnnotations;

namespace ElectronicObserverTypes
{
    /// <summary>
    /// 夜戦攻撃種別を表します。
    /// </summary>
    public enum NightAttackKind
    {
        /// <summary> 不明 </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "Unknown")]
        Unknown = -1,


        /// <summary> 通常攻撃 (API上でのみ使用されます) </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "NormalAttack")]
        NormalAttack,

        /// <summary> 連続攻撃 </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "DoubleShelling")]
        DoubleShelling,

        /// <summary> カットイン(主砲/魚雷) </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "CutinMainTorpedo")]
        CutinMainTorpedo,

        /// <summary> カットイン(魚雷/魚雷) </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "CutinTorpedoTorpedo")]
        CutinTorpedoTorpedo,

        /// <summary> カットイン(主砲/主砲/副砲) </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "CutinMainSub")]
        CutinMainSub,

        /// <summary> カットイン(主砲/主砲/主砲) </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "CutinMainMain")]
        CutinMainMain,

        /// <summary> 空母カットイン </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "CutinAirAttack")]
        CutinAirAttack,

        /// <summary> 駆逐カットイン(主砲/魚雷/電探) </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "CutinTorpedoRadar")]
        CutinTorpedoRadar,

        /// <summary> 駆逐カットイン(魚雷/見張員/電探) </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "CutinTorpedoPicket")]
        CutinTorpedoPicket,

        /// <summary> Nelson Touch </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "SpecialNelson")]
        SpecialNelson = 100,

        /// <summary> 一斉射かッ…胸が熱いな！ </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "SpecialNagato")]
        SpecialNagato = 101,

        /// <summary> 長門、いい？ いくわよ！ 主砲一斉射ッ！ </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "SpecialMutsu")]
        SpecialMutsu = 102,


        /// <summary> 砲撃 </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "Shelling")]
        Shelling = 1000,

        /// <summary> 空撃 </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "AirAttack")]
        AirAttack,

        /// <summary> 爆雷攻撃 </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "DepthCharge")]
        DepthCharge,

        /// <summary> 雷撃 </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "Torpedo")]
        Torpedo,


        /// <summary> ロケット攻撃 </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "Rocket")]
        Rocket = 2000,


        /// <summary> 揚陸攻撃(大発動艇) </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "LandingDaihatsu")]
        LandingDaihatsu = 3000,

        /// <summary> 揚陸攻撃(特大発動艇) </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "LandingTokuDaihatsu")]
        LandingTokuDaihatsu,

        /// <summary> 揚陸攻撃(大発動艇(八九式中戦車&陸戦隊)) </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "LandingDaihatsuTank")]
        LandingDaihatsuTank,

        /// <summary> 揚陸攻撃(特二式内火艇) </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "LandingAmphibious")]
        LandingAmphibious,

        /// <summary> 揚陸攻撃(特大発動艇+戦車第11連隊) </summary>
        [Display(ResourceType = typeof(Properties.NightAttackKind), Name = "LandingTokuDaihatsuTank")]
        LandingTokuDaihatsuTank,

    }
}