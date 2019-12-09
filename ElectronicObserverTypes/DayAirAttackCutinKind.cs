using System.ComponentModel.DataAnnotations;

namespace ElectronicObserverTypes
{
    /// <summary>
    /// 昼戦空母カットインの種別を表します。
    /// </summary>
    public enum DayAirAttackCutinKind
    {
        None = 0,

        [Display(ResourceType = typeof(Properties.DayAirAttackCutinKind), Name = "FighterBomberAttacker")]
        FighterBomberAttacker,
        [Display(ResourceType = typeof(Properties.DayAirAttackCutinKind), Name = "BomberBomberAttacker")]
        BomberBomberAttacker,
        [Display(ResourceType = typeof(Properties.DayAirAttackCutinKind), Name = "BomberAttacker")]
        BomberAttacker,
    }
}