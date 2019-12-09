using System.ComponentModel.DataAnnotations;

namespace ElectronicObserverTypes
{
    public enum FleetPositionDetail
    {
        [Display(ResourceType = typeof(Properties.FleetPositionDetail), Name = "MainFlag")]
        MainFlag,
        [Display(ResourceType = typeof(Properties.FleetPositionDetail), Name = "Main")]
        Main,
        [Display(ResourceType = typeof(Properties.FleetPositionDetail), Name = "EscortFlag")]
        EscortFlag,
        [Display(ResourceType = typeof(Properties.FleetPositionDetail), Name = "Escort")]
        Escort,
        [Display(ResourceType = typeof(Properties.FleetPositionDetail), Name = "VanguardFlag")]
        VanguardFlag,
        [Display(ResourceType = typeof(Properties.FleetPositionDetail), Name = "VanguardTop")]
        VanguardTop,
        [Display(ResourceType = typeof(Properties.FleetPositionDetail), Name = "VanguardBottom")]
        VanguardBottom,
    }
}