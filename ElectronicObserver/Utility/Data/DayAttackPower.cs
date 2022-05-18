﻿using System;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Utility.Data;

public static class DayAttackPower
{
	public static int GetDayAttackPower(this IShipData ship, Enum attack, IFleetData fleet, EngagementType engagement = EngagementType.Parallel)
	{
		double basepower = ship.BaseDayAttackPower(fleet);

		basepower *= ship.GetHPDamageBonus() * Damage.EngagementDayAttackMod(engagement);
		basepower += ship.GetLightCruiserDamageBonus() + ship.GetItalianDamageBonus();

		basepower = Math.Floor(Damage.Cap(basepower, Damage.DayAttackCap));

		basepower *= DayAttackMod(attack);

		return (int)basepower;
	}

	private static double BaseDayAttackPower(this IShipData ship, IFleetData fleet) => ship switch
	{
		_ when ship.MasterShip.IsAircraftCarrier => ship.CarrierBasePower(fleet),
		{ MasterShip.ShipId: ShipId.HayasuiKai } when ship.HasAttacker() => ship.CarrierBasePower(fleet),
		{ MasterShip.ShipId: ShipId.YamashioMaruKai } when ship.HasBomber() => ship.CarrierBasePower(fleet),

		_ => ship.SurfaceShipBasePower(fleet)
	};

	private static double SurfaceShipBasePower(this IShipData ship, IFleetData fleet) =>
		ship.FirepowerTotal + ship.GetDayBattleEquipmentLevelBonus() +
		fleet.CombinedFleetDayAttackBonus(ship) + 5;

	private static double CarrierBasePower(this IShipData ship, IFleetData fleet) =>
		Math.Floor((ship.FirepowerTotal + ship.TorpedoBase +
					ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Torpedo ?? 0) +
					Math.Floor(ship.AllSlotInstance.Sum(e => e?.MasterEquipment.Bomber ?? 0) * 1.3) +
					ship.GetDayBattleEquipmentLevelBonus() +
					fleet.CombinedFleetDayAttackBonus(ship)) * 1.5) + 55;

	private static double GetDayBattleEquipmentLevelBonus(this IShipData ship) =>
		ship.AllSlotInstance.Sum(e => e.DayFirepowerBonus());

	private static double DayFirepowerBonus(this IEquipmentData? equip) => equip?.MasterEquipment.CategoryType switch
	{
		EquipmentTypes.MainGunSmall => Math.Sqrt(equip.Level),
		EquipmentTypes.MainGunMedium => Math.Sqrt(equip.Level),
		EquipmentTypes.APShell => Math.Sqrt(equip.Level),
		EquipmentTypes.AADirector => Math.Sqrt(equip.Level),
		EquipmentTypes.Searchlight => Math.Sqrt(equip.Level),
		EquipmentTypes.SearchlightLarge => Math.Sqrt(equip.Level),
		EquipmentTypes.AAGun => Math.Sqrt(equip.Level),
		EquipmentTypes.LandingCraft => Math.Sqrt(equip.Level),
		EquipmentTypes.SpecialAmphibiousTank => Math.Sqrt(equip.Level),

		EquipmentTypes.MainGunLarge => Math.Sqrt(equip.Level) * 1.5,
		EquipmentTypes.MainGunLarge2 => Math.Sqrt(equip.Level) * 1.5,

		EquipmentTypes.Sonar => Math.Sqrt(equip.Level) * 0.75,
		EquipmentTypes.SonarLarge => Math.Sqrt(equip.Level) * 0.75,
		EquipmentTypes.DepthCharge when equip.MasterEquipment.IsDepthChargeProjector => Math.Sqrt(equip.Level) * 0.75,

		EquipmentTypes.SecondaryGun => equip.EquipmentId switch
		{
			EquipmentId.SecondaryGun_12_7cmTwinHighangleGun => 0.2 * equip.Level,
			EquipmentId.SecondaryGun_8cmHighangleGun => 0.2 * equip.Level,
			EquipmentId.SecondaryGun_8cmHighangleGunKai_MachineGun => 0.2 * equip.Level,
			EquipmentId.SecondaryGun_10cmTwinHighangleGunKai_AdditionalMachineGuns => 0.2 * equip.Level,

			EquipmentId.SecondaryGun_15_5cmTripleSecondaryGun => 0.3 * equip.Level,
			EquipmentId.SecondaryGun_15_5cmTripleSecondaryGunKai => 0.3 * equip.Level,

			_ => Math.Sqrt(equip.Level)
		},

		EquipmentTypes.CarrierBasedTorpedo => 0.2 * equip.Level,

		_ => 0
	};

	private static double CombinedFleetDayAttackBonus(this IFleetData fleet, IShipData ship) =>
		(fleet.FleetType, ship.Fleet) switch
		{
			(FleetType.Carrier, 1) => 2,
			(FleetType.Carrier, 2) => 10,

			(FleetType.Surface, 1) => 10,
			(FleetType.Surface, 2) => -5,

			(FleetType.Transport, 1) => -5,
			(FleetType.Transport, 2) => 10,

			_ => 0
		};

	private static double DayAttackMod(Enum attackKind) => attackKind switch
	{
		DayAttackKind.DoubleShelling => 1.2,
		DayAttackKind.CutinMainRadar => 1.2,
		DayAttackKind.CutinMainSub => 1.1,
		DayAttackKind.CutinMainAP => 1.3,
		DayAttackKind.CutinMainMain => 1.5,

		DayAttackKind.ZuiunMultiAngle => 1.35,
		DayAttackKind.SeaAirMultiAngle => 1.3,

		DayAirAttackCutinKind.FighterBomberAttacker => 1.25,
		DayAirAttackCutinKind.BomberBomberAttacker => 1.20,
		DayAirAttackCutinKind.BomberAttacker => 1.15,

		_ => 1
	};
}
