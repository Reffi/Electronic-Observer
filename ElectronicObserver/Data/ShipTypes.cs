
using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Data
{
	public enum ShipTypes
	{
		/// <summary>海防艦</summary>
		[Display(Name = "DE")]
		Escort = 1,

        /// <summary>駆逐艦</summary>
        [Display(Name = "DD")]
        Destroyer = 2,

        /// <summary>軽巡洋艦</summary>
        [Display(Name = "CL")]
        LightCruiser = 3,

        /// <summary>重雷装巡洋艦</summary>
        [Display(Name = "CLT")]
        TorpedoCruiser = 4,

        /// <summary>重巡洋艦</summary>
        [Display(Name = "CA")]
        HeavyCruiser = 5,

        /// <summary>航空巡洋艦</summary>
        [Display(Name = "CAV")]
        AviationCruiser = 6,

        /// <summary>軽空母</summary>
        [Display(Name = "CVL")]
        LightAircraftCarrier = 7,

        /// <summary>巡洋戦艦</summary>
        [Display(Name = "BC")]
        Battlecruiser = 8,

        /// <summary>戦艦</summary>
        [Display(Name = "BB")]
        Battleship = 9,

        /// <summary>航空戦艦</summary>
        [Display(Name = "BBV")]
        AviationBattleship = 10,

        /// <summary>正規空母</summary>
        [Display(Name = "CV")]
        AircraftCarrier = 11,

		/// <summary>超弩級戦艦</summary>
		SuperDreadnoughts = 12,

        /// <summary>潜水艦</summary>
        [Display(Name = "SS")]
        Submarine = 13,

        /// <summary>潜水空母</summary>
        [Display(Name = "SSV")]
        SubmarineAircraftCarrier = 14,

		/// <summary>輸送艦</summary>
		Transport = 15,

        /// <summary>水上機母艦</summary>
        [Display(Name = "AV")]
        SeaplaneTender = 16,

        /// <summary>揚陸艦</summary>
        [Display(Name = "LHA")]
        AmphibiousAssaultShip = 17,

        /// <summary>装甲空母</summary>
        [Display(Name = "CVB")]
        ArmoredAircraftCarrier = 18,

        /// <summary>工作艦</summary>
        [Display(Name = "AR")]
        RepairShip = 19,

        /// <summary>潜水母艦</summary>
        [Display(Name = "AS")]
        SubmarineTender = 20,

        /// <summary>練習巡洋艦</summary>
        [Display(Name = "CT")]
        TrainingCruiser = 21,

        /// <summary>補給艦</summary>
        [Display(Name = "AO")]
        FleetOiler = 22,

	}
}
