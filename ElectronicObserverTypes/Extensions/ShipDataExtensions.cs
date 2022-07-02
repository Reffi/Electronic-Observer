﻿using System;
using System.Linq;
using ElectronicObserverTypes.Evasion;

namespace ElectronicObserverTypes.Extensions;

public static class ShipDataExtensions
{
	public static IShipDataMaster BaseShip(this IShipDataMaster ship)
	{
		IShipDataMaster temp = ship;

		while (temp.RemodelBeforeShip != null)
		{
			temp = temp.RemodelBeforeShip;
		}

		return temp;
	}

	/// <summary>
	/// 深海棲艦かどうか
	/// </summary>
	public static bool IsAbyssalShip(this IShipDataMaster ship) => ship.ShipID > 1500;

	public static double Accuracy(this IShipData ship) =>
		2 * Math.Sqrt(ship.Level) + 1.5 * Math.Sqrt(ship.LuckTotal);

	public static double ShellingEvasion(this IShipData ship) =>
		new ShellingEvasion(ship).PostcapValue;

	public static double TorpedoEvasion(this IShipData ship) =>
		new TorpedoEvasion(ship).PostcapValue;

	public static double AirstrikeEvasion(this IShipData ship) =>
		new AirstrikeEvasion(ship).PostcapValue;

	public static double AswEvasion(this IShipData ship) =>
		new AswEvasion(ship).PostcapValue;

	public static double NightEvasion(this IShipData ship) =>
		new NightEvasion(ship).PostcapValue;

	public static int MainGunCount(this IShipData ship) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsMainGun == true);

	public static bool HasMainGun(this IShipData ship, int count = 1) => ship.MainGunCount() >= count;

	public static bool HasMainGunLarge(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.CategoryType is
			EquipmentTypes.MainGunLarge or
			EquipmentTypes.MainGunLarge2)
		 >= count;

	public static bool HasMainGunLargeFcr(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.MainGunLarge_16inchMk_ITripleGunKai_FCRType284)
		>= count;

	public static int SecondaryGunCount(this IShipData ship) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.CategoryType == EquipmentTypes.SecondaryGun);

	public static bool HasSecondaryGun(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.SecondaryGun);

	public static bool HasApShell(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.APShell);

	public static bool HasAaShell(this IShipData ship, int count) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.CategoryType == EquipmentTypes.AAShell) >= count;

	public static bool HasSeaplane(this IShipData ship) => ship.AllSlotInstance
		.Zip(ship.Aircraft, (e, size) => (e, size))
		.Any(s => s.size > 0 && s.e?.MasterEquipment.IsSeaplane() == true);

	public static bool HasRadar(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsRadar is true) >= count;

	public static bool HasSurfaceRadar(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.IsSurfaceRadar is true);

	public static bool HasAirRadar(this IShipData ship, int count) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsAirRadar is true) >= count;

	public static bool HasRadarGfcs(this IShipData ship, int count) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.RadarSmall_GFCSMk_37)
		>= count;

	public static bool HasSuisei634(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e.IsSuisei634()) >= count;

	public static bool HasZuiun(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e.IsZuiun()) >= count;

	public static bool HasFighter(this IShipData ship) => ship.AllSlotInstance
		.Zip(ship.Aircraft, (e, size) => (e, size))
		.Any(s => s.size > 0 && s.e?.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedFighter);

	public static bool HasBomber(this IShipData ship, int count = 1) => ship.AllSlotInstance
																			.Zip(ship.Aircraft, (e, size) => (e, size))
																			.Count(s => s.size > 0 && s.e?.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedBomber)
																		>= count;

	public static bool HasAttacker(this IShipData ship) => ship.AllSlotInstance
		.Zip(ship.Aircraft, (e, size) => (e, size))
		.Any(s => s.size > 0 && s.e?.MasterEquipment.CategoryType == EquipmentTypes.CarrierBasedTorpedo);

	public static bool HasJetBomber(this IShipData ship, int count = 1) =>
		ship.AllSlotInstance
			.Zip(ship.Aircraft, (e, size) => (e, size))
			.Count(s => s.size > 0 && s.e?.MasterEquipment.CategoryType is EquipmentTypes.JetBomber)
		>= count;

	public static bool HasTorpedo(this IShipData ship, int count = 1) => ship.AllSlotInstance
																			 .Count(e => e?.MasterEquipment.IsTorpedo == true)
																		 >= count;

	public static bool HasSkilledLookouts(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.SurfaceShipPersonnel);

	public static bool HasDestroyerSkilledLookouts(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.EquipmentId == EquipmentId.SurfaceShipPersonnel_TorpedoSquadronSkilledLookouts);

	public static bool HasDrum(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.TransportContainer);

	/// <summary>
	/// 空母系か (軽空母/正規空母/装甲空母)
	/// </summary>
	public static bool IsAircraftCarrier(this IShipData ship) => ship.MasterShip.IsAircraftCarrier();

	/// <summary>
	/// 空母系か (軽空母/正規空母/装甲空母)
	/// </summary>
	public static bool IsAircraftCarrier(this IShipDataMaster ship) => ship.ShipType is
		ShipTypes.LightAircraftCarrier or
		ShipTypes.AircraftCarrier or
		ShipTypes.ArmoredAircraftCarrier;

	public static bool IsNightCarrier(this IShipData ship) =>
		ship.HasNightAviationPersonnel() ||
		ship.MasterShip.ShipId is
			ShipId.SaratogaMkII or
			ShipId.AkagiKaiNiE or
			ShipId.KagaKaiNiE or
			ShipId.RyuuhouKaiNiE;

	private static bool HasNightAviationPersonnel(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.IsNightAviationPersonnel == true);

	public static bool HasNightFighter(this IShipData ship, int count = 1) => ship.AllSlotInstance
																				  .Count(e => e?.MasterEquipment.IsNightFighter == true)
																			  >= count;

	public static bool HasNightAttacker(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.IsNightAttacker == true);

	public static bool HasNightAircraft(this IShipData ship, int count = 1) => ship.AllSlotInstance
																				   .Count(e => e?.MasterEquipment.IsNightAircraft == true || e?.IsNightCapableAircraft() == true)
																			   >= count;

	public static bool HasNightPhototoubePlane(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.EquipmentId == EquipmentId.CarrierBasedBomber_SuiseiModel12_wType31PhotoelectricFuzeBombs);

	public static bool HasSwordfish(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.IsSwordfish ?? false);

	public static bool HasLateModelTorp(this IShipData ship, int count = 1) => ship.AllSlotInstance
																				   .Count(e => e?.MasterEquipment.IsLateModelTorpedo() == true)
																			   >= count;

	public static bool HasSubmarineEquipment(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.SubmarineEquipment);

	public static bool HasStarShell(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.CategoryType == EquipmentTypes.StarShell);

	public static bool HasSearchlight(this IShipData ship) => ship.AllSlotInstance
		.Any(e => e?.MasterEquipment.CategoryType switch
		{
			EquipmentTypes.Searchlight => true,
			EquipmentTypes.SearchlightLarge => true,
			_ => false
		});

	public static int HighAngleGunCount(this IShipData ship) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsHighAngleGun is true);

	public static bool HasHighAngleGun(this IShipData ship, int count = 1) =>
		ship.HighAngleGunCount() >= count;

	public static int HighAngleDirectorGunCount(this IShipData ship) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsHighAngleGunWithAADirector is true);

	public static bool HasHighAngleDirectorGun(this IShipData ship, int count = 1) =>
		ship.HighAngleDirectorGunCount() >= count;

	public static int HighAngleDirectorWithoutGunCount(this IShipData ship) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.IsHighAngleGun is true &&
			e.MasterEquipment.AA < 8);

	public static bool HasHighAngleWithoutDirectorGun(this IShipData ship, int count = 1) =>
		ship.HighAngleDirectorWithoutGunCount() >= count;

	public static bool HasDirector(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.CategoryType is EquipmentTypes.AADirector) >= count;

	/// <summary>
	/// Both min and max are inclusive
	/// </summary>
	public static bool HasAaGun(this IShipData ship, int count = 1, int min = 0, int max = 9999) =>
		ship.AllSlotInstance
			.Count(e => e?.MasterEquipment.CategoryType is EquipmentTypes.AAGun &&
				e.MasterEquipment.AA >= min &&
				e.MasterEquipment.AA <= max)
			>= count;

	public static bool HasPompom(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.AAGun_QF2pounderOctuplePompomGun)
		>= count;

	public static bool HasAaRocketBritish(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.AAGun_20tube7inchUPRocketLaunchers)
		>= count;

	public static bool HasAaRocketMod(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.AAGun_12cm30tubeRocketLauncherKai2)
		>= count;

	public static bool HasHighAngleMusashi(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.SecondaryGun_10cmTwinHighangleGunKai_AdditionalMachineGuns)
		>= count;

	public static bool HasHighAngleAmerican(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.MainGunSmall_5inchSingleGunMk_30)
		>= count;

	public static bool HasHighAngleAmericanKai(this IShipData ship, int count = 1) => ship.AllSlotInstance
			.Count(e => e?.MasterEquipment.EquipmentId is
				EquipmentId.MainGunSmall_5inchSingleGunMk_30Kai)
		>= count;

	public static bool HasHighAngleAmericanGfcs(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.MainGunSmall_5inchSingleGunMk_30Kai_GFCSMk_37)
		>= count;

	public static int HighAngleAtlantaCount(this IShipData ship) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.MainGunMedium_5inchTwinDualpurposeGunMount_ConcentratedDeployment);

	public static bool HasHighAngleAtlanta(this IShipData ship, int count = 1) =>
		ship.HighAngleAtlantaCount() >= count;

	public static int HighAngleAtlantaGfcsCount(this IShipData ship) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.MainGunMedium_GFCSMk_37_5inchTwinDualpurposeGunMount_ConcentratedDeployment);

	public static bool HasHighAngleAtlantaGfcs(this IShipData ship, int count = 1) =>
		ship.HighAngleAtlantaGfcsCount() >= count;

	public static bool HasHighAngleConcentrated(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			(EquipmentId)464)
		>= count;

	public static bool HasYamatoRadar(this IShipData ship, int count = 1) => ship.AllSlotInstance
		.Count(e => e?.MasterEquipment.EquipmentId is
			EquipmentId.RadarLarge_15mDuplexRangefinder_Type21AirRADARKai2 or
			(EquipmentId)460)
		>= count;

	public static bool IsIseClassK2(this IShipData ship) => ship.MasterShip.ShipId switch
	{
		ShipId.IseKaiNi => true,
		ShipId.HyuugaKaiNi => true,
		_ => false
	};

	public static bool IsDestroyer(this IShipData ship) => ship.MasterShip.ShipType switch
	{
		ShipTypes.Destroyer => true,
		_ => false
	};

	public static bool IsSurfaceShip(this IShipData ship) => ship.MasterShip.ShipType switch
	{
		ShipTypes.Escort => true,
		ShipTypes.Destroyer => true,
		ShipTypes.LightCruiser => true,
		ShipTypes.TorpedoCruiser => true,
		ShipTypes.HeavyCruiser => true,
		ShipTypes.AviationCruiser => true,
		ShipTypes.Battlecruiser => true,
		ShipTypes.Battleship => true,
		ShipTypes.AviationBattleship => true,
		ShipTypes.SuperDreadnoughts => true,
		ShipTypes.Transport => true,
		ShipTypes.SeaplaneTender => true,
		ShipTypes.AmphibiousAssaultShip => true,
		ShipTypes.RepairShip => true,
		ShipTypes.SubmarineTender => true,
		ShipTypes.TrainingCruiser => true,
		ShipTypes.FleetOiler => true,

		_ => false
	};

	public static bool IsSpecialNightCarrier(this IShipData ship) => ship.MasterShip.ShipId is
		ShipId.GrafZeppelin or
		ShipId.GrafZeppelinKai or
		ShipId.Saratoga or
		ShipId.TaiyouKaiNi or
		ShipId.ShinyouKaiNi or
		ShipId.UnyouKaiNi;

	public static bool IsArkRoyal(this IShipData ship) => ship.MasterShip.ShipId switch
	{
		ShipId.ArkRoyal => true,
		ShipId.ArkRoyalKai => true,

		_ => false
	};

	public static double GetHPDamageBonus(this IShipData ship) => ship.HPRate switch
	{
		_ when ship.HPRate <= 0.25 => 0.4,
		_ when ship.HPRate <= 0.5 => 0.7,
		_ => 1,
	};

	public static double GetLightCruiserDamageBonus(this IShipData ship)
	{
		if (ship.MasterShip.ShipType != ShipTypes.LightCruiser &&
			ship.MasterShip.ShipType != ShipTypes.TorpedoCruiser &&
			ship.MasterShip.ShipType != ShipTypes.TrainingCruiser)
		{
			return 0;
		}

		int single = ship.AllSlotInstance.Count(e => e?.EquipmentId switch
		{
			EquipmentId.MainGunMedium_14cmSingleGun => true,
			EquipmentId.SecondaryGun_15_2cmSingleGun => true,
			_ => false
		});

		int twin = ship.AllSlotInstance.Count(e => e?.EquipmentId switch
		{
			EquipmentId.MainGunMedium_14cmTwinGun => true,
			EquipmentId.MainGunMedium_15_2cmTwinGun => true,
			EquipmentId.MainGunMedium_15_2cmTwinGunKai => true,
			_ => false
		});

		return Math.Sqrt(twin) * 2.0 + Math.Sqrt(single);
	}

	public static double GetItalianDamageBonus(this IShipData ship)
	{
		// todo turn to switch expression once we get or patterns
		switch (ship.MasterShip.ShipId)
		{
			case ShipId.Zara:
			case ShipId.ZaraKai:
			case ShipId.ZaraDue:
			case ShipId.Pola:
			case ShipId.PolaKai:
				return Math.Sqrt(ship.AllSlotInstance.Count(e => e?.EquipmentId == EquipmentId.MainGunMedium_203mm53TwinGun));

			default:
				return 0;
		}
	}

	public static ShipNationality Nationality(this IShipDataMaster ship)
	{
		if (ship.IsAbyssalShip) return ShipNationality.Unknown;

		return ship.SortID switch
		{
			< 1000 => ShipNationality.Unknown,
			< 30000 => ShipNationality.Japanese,
			< 31000 => ShipNationality.German,
			< 32000 => ShipNationality.Italian,
			< 33000 => ShipNationality.American,
			< 34000 => ShipNationality.British,
			< 35000 => ShipNationality.French,
			< 36000 => ShipNationality.Russian,
			< 37000 => ShipNationality.Swedish,
			< 38000 => ShipNationality.Dutch,
			< 39000 => ShipNationality.Australian,

			_ => ShipNationality.Unknown
		};
	}
}
