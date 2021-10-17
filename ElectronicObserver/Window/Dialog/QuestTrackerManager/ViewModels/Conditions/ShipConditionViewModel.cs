﻿using ElectronicObserver.Data;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;
using ElectronicObserverTypes;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

public class ShipConditionViewModel : ObservableObject, IConditionViewModel
{
	private IShipDataMaster? _ship;

	public IShipDataMaster Ship
	{
		// bug: Ships don't get loaded till Kancolle loads
		get => _ship ??= Ships.FirstOrDefault(s => s.ShipId == Model.Id)
						 ?? throw new Exception("fix me: accessing this property before Kancolle gets loaded is a bug");
		set => SetProperty(ref _ship, value);
	}

	public IEnumerable<IShipDataMaster> Ships => KCDatabase.Instance.MasterShips.Values
		.Where(s => !s.IsAbyssalShip)
		.OrderBy(s => s.SortID);

	public RemodelComparisonType RemodelComparisonType { get; set; }
	public IEnumerable<RemodelComparisonType> RemodelComparisonTypes { get; }

	public bool MustBeFlagship { get; set; }

	public ShipConditionModel Model { get; }

	public string Display => Ship switch
	{
		null => "",
		_ => $"{Ship.NameEN}({RemodelComparisonType.Display()}){FlagshipConditionDisplay}"
	};

	private string FlagshipConditionDisplay => MustBeFlagship switch
	{
		true => $"({Properties.Window.Dialog.QuestTrackerManager.Flagship})",
		_ => ""
	};

	public ShipConditionViewModel(ShipConditionModel model)
	{
		RemodelComparisonTypes = Enum.GetValues(typeof(RemodelComparisonType))
			.Cast<RemodelComparisonType>();

		Model = model;

		RemodelComparisonType = Model.RemodelComparisonType;
		MustBeFlagship = Model.MustBeFlagship;

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(Ship)) return;

			Model.Id = Ship?.ShipId ?? ShipId.Kamikaze;
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(RemodelComparisonType)) return;

			Model.RemodelComparisonType = RemodelComparisonType;
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(MustBeFlagship)) return;

			Model.MustBeFlagship = MustBeFlagship;
		};
	}

	public bool ConditionMet(IFleetData fleet)
	{
		IEnumerable<IShipData> ships = fleet.MembersInstance.Where(s => s is not null);

		if (MustBeFlagship)
		{
			ships = ships.Take(1);
		}

		return RemodelComparisonType switch
		{
			RemodelComparisonType.Any => ships.Any(AnyRemodelCheck),
			RemodelComparisonType.AtLeast => ships.Any(HigherRemodelCheck),
			RemodelComparisonType.Exact => ships.Any(s => s.MasterShip.ShipId == Model.Id),

			_ => false
		};
	}

	private bool AnyRemodelCheck(IShipData ship)
	{
		IShipDataMaster conditionShip = KCDatabase.Instance.MasterShips.Values
			.First(s => s.ShipId == Model.Id);

		return ship.MasterShip.BaseShip().ShipId == conditionShip.BaseShip().ShipId;
	}

	private bool HigherRemodelCheck(IShipData ship)
	{
		IShipDataMaster? conditionShip = KCDatabase.Instance.MasterShips.Values
			.First(s => s.ShipId == Model.Id);

		while (conditionShip != null)
		{
			if (ship.MasterShip.ShipId == conditionShip.ShipId)
			{
				return true;
			}

			conditionShip = conditionShip.RemodelBeforeShip;
		}

		return false;
	}
}