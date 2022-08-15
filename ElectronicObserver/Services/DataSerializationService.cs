﻿using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Tools.EventLockPlanner;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.AirControlSimulator;
using ElectronicObserverTypes.Serialization.DeckBuilder;
using ElectronicObserverTypes.Serialization.EventLockPlanner;
using ElectronicObserverTypes.Serialization.FleetAnalysis;

namespace ElectronicObserver.Services;

public class DataSerializationService
{
	private static JsonSerializerOptions JsonSerializerOptions => new()
	{
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
	};

	public string DeckBuilderFleet(int fleetId) => fleetId switch
	{
		1 => DeckBuilderFleet1(),
		2 => DeckBuilderFleet2(),
		3 => DeckBuilderFleet3(),
		4 => DeckBuilderFleet4(),
		_ => ""
	};

	private string DeckBuilderFleet1()
	{
		return DeckBuilder
		(
			KCDatabase.Instance.Admiral.Level,
			KCDatabase.Instance.Fleet[1]
		);
	}

	private string DeckBuilderFleet2()
	{
		return DeckBuilder
		(
			KCDatabase.Instance.Admiral.Level,
			fleet2: KCDatabase.Instance.Fleet[2]
		);
	}

	private string DeckBuilderFleet3()
	{
		return DeckBuilder
		(
			KCDatabase.Instance.Admiral.Level,
			fleet3: KCDatabase.Instance.Fleet[3]
		);
	}

	private string DeckBuilderFleet4()
	{
		return DeckBuilder
		(
			KCDatabase.Instance.Admiral.Level,
			fleet4: KCDatabase.Instance.Fleet[4]
		);
	}

	public string FleetAnalysisShips()
	{
		return FleetAnalysisShips(KCDatabase.Instance.Ships.Values);
	}

	public string FleetAnalysisEquipment(bool allEquipment)
	{
		return FleetAnalysisEquipment(KCDatabase.Instance.Equipments.Values
			.Where(eq => allEquipment || eq.IsLocked));
	}

	public string AirControlSimulator
	(
		int hqLevel,
		IFleetData? fleet1 = null,
		IFleetData? fleet2 = null,
		IFleetData? fleet3 = null,
		IFleetData? fleet4 = null,
		IBaseAirCorpsData? airBase1 = null,
		IBaseAirCorpsData? airBase2 = null,
		IBaseAirCorpsData? airBase3 = null,
		IEnumerable<IShipData>? ships = null,
		IEnumerable<IEquipmentData>? equipment = null,
		bool maxAircraftLevelFleet = false,
		bool maxAircraftLevelAirBase = false
	) => JsonSerializer.Serialize
	(
		MakeAirControlSimulatorData
		(
			MakeDeckBuilderData(hqLevel, fleet1, fleet2, fleet3, fleet4, airBase1, airBase2, airBase3, maxAircraftLevelFleet, maxAircraftLevelAirBase),
			ships?.Select(MakeFleetAnalysisShip),
			equipment?.Select(MakeFleetAnalysisEquipment)
		),
		JsonSerializerOptions
	);

	public string DeckBuilder
	(
		int hqLevel,
		IFleetData? fleet1 = null,
		IFleetData? fleet2 = null,
		IFleetData? fleet3 = null,
		IFleetData? fleet4 = null,
		IBaseAirCorpsData? airBase1 = null,
		IBaseAirCorpsData? airBase2 = null,
		IBaseAirCorpsData? airBase3 = null,
		bool maxAircraftLevelFleet = false,
		bool maxAircraftLevelAirBase = false
	) => JsonSerializer.Serialize
	(
		MakeDeckBuilderData(hqLevel, fleet1, fleet2, fleet3, fleet4, airBase1, airBase2, airBase3, maxAircraftLevelFleet, maxAircraftLevelAirBase),
		JsonSerializerOptions
	);

	public static string FleetAnalysisShips(IEnumerable<IShipData> ships) =>
		JsonSerializer.Serialize(MakeFleetAnalysisShips(ships), JsonSerializerOptions);

	public static string FleetAnalysisEquipment(IEnumerable<IEquipmentData> equipment) =>
		JsonSerializer.Serialize(MakeFleetAnalysisEquipment(equipment), JsonSerializerOptions);

	private static AirControlSimulatorData MakeAirControlSimulatorData
	(
		DeckBuilderData? fleet = null,
		IEnumerable<FleetAnalysisShip>? ships = null,
		IEnumerable<FleetAnalysisEquipment>? equipment = null
	) => new()
	{
		Fleet = fleet,
		Ships = ships,
		Equipment = equipment
	};

	private static DeckBuilderData MakeDeckBuilderData
	(
		int hqLevel,
		IFleetData? fleet1 = null,
		IFleetData? fleet2 = null,
		IFleetData? fleet3 = null,
		IFleetData? fleet4 = null,
		IBaseAirCorpsData? airBase1 = null,
		IBaseAirCorpsData? airBase2 = null,
		IBaseAirCorpsData? airBase3 = null,
		bool maxAircraftLevelFleet = false,
		bool maxAircraftLevelAirBase = false
	) => new()
	{
		HqLevel = hqLevel,
		Fleet1 = MakeDeckBuilderFleet(fleet1, maxAircraftLevelFleet),
		Fleet2 = MakeDeckBuilderFleet(fleet2, maxAircraftLevelFleet),
		Fleet3 = MakeDeckBuilderFleet(fleet3, maxAircraftLevelFleet),
		Fleet4 = MakeDeckBuilderFleet(fleet4, maxAircraftLevelFleet),
		AirBase1 = MakeDeckBuilderAirBase(airBase1, maxAircraftLevelAirBase),
		AirBase2 = MakeDeckBuilderAirBase(airBase2, maxAircraftLevelAirBase),
		AirBase3 = MakeDeckBuilderAirBase(airBase3, maxAircraftLevelAirBase),
	};

	private static DeckBuilderFleet? MakeDeckBuilderFleet
	(
		IFleetData? fleet,
		bool maxAircraftLevel = false
	) => fleet switch
	{
		{ } f => new()
		{
			Name = fleet.Name,
			Ship1 = MakeDeckBuilderShip(f.MembersInstance.Skip(0).FirstOrDefault(), maxAircraftLevel),
			Ship2 = MakeDeckBuilderShip(f.MembersInstance.Skip(1).FirstOrDefault(), maxAircraftLevel),
			Ship3 = MakeDeckBuilderShip(f.MembersInstance.Skip(2).FirstOrDefault(), maxAircraftLevel),
			Ship4 = MakeDeckBuilderShip(f.MembersInstance.Skip(3).FirstOrDefault(), maxAircraftLevel),
			Ship5 = MakeDeckBuilderShip(f.MembersInstance.Skip(4).FirstOrDefault(), maxAircraftLevel),
			Ship6 = MakeDeckBuilderShip(f.MembersInstance.Skip(5).FirstOrDefault(), maxAircraftLevel),
			Ship7 = MakeDeckBuilderShip(f.MembersInstance.Skip(6).FirstOrDefault(), maxAircraftLevel),
		},

		_ => null
	};

	private static DeckBuilderShip? MakeDeckBuilderShip
	(
		IShipData? ship,
		bool maxAircraftLevel = false
	) => ship switch
	{
		{ } s => new()
		{
			Id = s.MasterShip.ShipId,
			Level = s.Level,
			Equipment = new()
			{
				Equipment1 = MakeDeckBuilderEquipment(s.AllSlotInstance.Skip(0).FirstOrDefault(), maxAircraftLevel),
				Equipment2 = MakeDeckBuilderEquipment(s.AllSlotInstance.Skip(1).FirstOrDefault(), maxAircraftLevel),
				Equipment3 = MakeDeckBuilderEquipment(s.AllSlotInstance.Skip(2).FirstOrDefault(), maxAircraftLevel),
				Equipment4 = MakeDeckBuilderEquipment(s.AllSlotInstance.Skip(3).FirstOrDefault(), maxAircraftLevel),
				Equipment5 = MakeDeckBuilderEquipment(s.AllSlotInstance.Skip(4).FirstOrDefault(), maxAircraftLevel),
				EquipmentExpansion = MakeDeckBuilderEquipment(s.ExpansionSlotInstance, maxAircraftLevel),
			},
			Hp = s.HPMax,
			Firepower = s.FirepowerTotal,
			Torpedo = s.TorpedoTotal,
			AntiAir = s.AATotal,
			Armor = s.ArmorTotal,
			AntiSubmarine = s.ASWTotal,
			Evasion = s.EvasionTotal,
			Los = s.LOSTotal,
			Luck = s.LuckTotal,
			Speed = s.Speed,
			Range = s.Range,
		},

		_ => null
	};

	private static DeckBuilderEquipment? MakeDeckBuilderEquipment
	(
		IEquipmentData? equipment,
		bool maxAircraftLevel
	) => equipment switch
	{
		{ } eq => new()
		{
			Id = eq.MasterEquipment.EquipmentId,
			Level = eq.Level,
			AircraftLevel = GetAircraftLevel(eq, maxAircraftLevel),
		},

		_ => null
	};

	private static int? GetAircraftLevel(IEquipmentData equipment, bool maxAircraftLevel) =>
		(equipment, maxAircraftLevel) switch
		{
			({ MasterEquipment.IsAircraft: true }, true) => 7,
			({ MasterEquipment.IsAircraft: true }, false) => equipment.AircraftLevel,
			_ => null
		};

	private static DeckBuilderAirBase? MakeDeckBuilderAirBase
	(
		IBaseAirCorpsData? airBase,
		bool maxAircraftLevel
	) => airBase switch
	{
		{ } ab => new()
		{
			Name = airBase.Name,
			Equipment = new()
			{
				Equipment1 = MakeDeckBuilderEquipment(ab.Squadrons.Values.Skip(0).FirstOrDefault()?.EquipmentInstance, maxAircraftLevel),
				Equipment2 = MakeDeckBuilderEquipment(ab.Squadrons.Values.Skip(1).FirstOrDefault()?.EquipmentInstance, maxAircraftLevel),
				Equipment3 = MakeDeckBuilderEquipment(ab.Squadrons.Values.Skip(2).FirstOrDefault()?.EquipmentInstance, maxAircraftLevel),
				Equipment4 = MakeDeckBuilderEquipment(ab.Squadrons.Values.Skip(3).FirstOrDefault()?.EquipmentInstance, maxAircraftLevel),
			},
			Mode = airBase.ActionKind,
			Distance = airBase.Distance,
		},

		_ => null,
	};

	private static IEnumerable<FleetAnalysisShip> MakeFleetAnalysisShips(IEnumerable<IShipData> ships)
		=> ships.Select(MakeFleetAnalysisShip);

	private static FleetAnalysisShip MakeFleetAnalysisShip(IShipData ship) => new()
	{
		ShipId = ship.MasterShip.ShipId,
		Level = ship.Level,
		Modernization = new List<int>
		{
			ship.FirepowerModernized,
			ship.TorpedoModernized,
			ship.AAModernized,
			ship.ArmorModernized,
			ship.LuckModernized,
			ship.HPMaxModernized,
			ship.ASWModernized,
		},
		Experience = new List<double>
		{
			ship.ExpTotal,
			ship.ExpNext,
			ship.ExpNextPercentage
		},
		ExpansionSlot = ship.ExpansionSlot,
		SallyArea = ship.SallyArea,
	};

	private static IEnumerable<FleetAnalysisEquipment> MakeFleetAnalysisEquipment(IEnumerable<IEquipmentData> equipment)
		=> equipment.Select(MakeFleetAnalysisEquipment);

	private static FleetAnalysisEquipment MakeFleetAnalysisEquipment(IEquipmentData equipment) => new()
	{
		Id = equipment.MasterEquipment.EquipmentId,
		Level = equipment.Level,
	};

	public string EventLockPlanner(EventLockPlannerViewModel viewModel)
	{
		EventLockPlannerData data = new()
		{
			Locks = viewModel.LockGroups.Select(g => new EventLockPlannerLock
			{
				Id = g.Id,
				A = g.Color.A,
				R = g.Color.R,
				G = g.Color.G,
				B = g.Color.B,
				Name = g.Name,
			}).ToList(),
			Phases = viewModel.EventPhases.Select(p => new EventLockPlannerPhase
			{
				LockGroups = p.PhaseLockGroups.Select(g => g.Id).ToList(),
				Name = p.Name,
			}).ToList()
		};

		return JsonSerializer.Serialize(data, JsonSerializerOptions);
	}
}
