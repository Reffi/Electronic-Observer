﻿using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;
using ElectronicObserver.Window.Dialog.ShipPicker;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

public partial class ShipConditionViewModelV2 : ObservableObject, IConditionViewModel
{
	private IKCDatabase Db { get; }

	private ShipPickerViewModel ShipPickerViewModel { get; }

	private IShipDataMaster? _ship;

	public IShipDataMaster? Ship
	{
		// bug: Ships don't get loaded till Kancolle loads
		get => Model.Id switch
		{
			ShipId.Unknown => null,
			_ => _ship ??= Ships.FirstOrDefault(s => s.ShipId == Model.Id)
				?? throw new Exception("fix me: accessing this property before Kancolle gets loaded is a bug"),
		};
		set => SetProperty(ref _ship, value);
	}

	private IEnumerable<IShipDataMaster> Ships => Db.MasterShips.Values
		.Where(s => !s.IsAbyssalShip)
		.OrderBy(s => s.SortID);

	public ShipClass ShipClass { get; set; }

	public IEnumerable<ShipClass> ShipClasses => Db.MasterShips.Values
		.Where(s => !s.IsAbyssalShip)
		.DistinctBy(s => s.ShipClass)
		.OrderBy(s => s.ShipType)
		.ThenBy(s => s.SortID)
		.Select(s => (ShipClass)s.ShipClass)
		.Prepend(ShipClass.Unknown);

	public RemodelComparisonType RemodelComparisonType { get; set; }
	public IEnumerable<RemodelComparisonType> RemodelComparisonTypes { get; }

	public bool MustBeFlagship { get; set; }

	public ShipConditionModelV2 Model { get; }

	public string Display => (ShipClass, Ship, RemodelComparisonType, MustBeFlagship) switch
	{
		(ShipClass.Unknown, null, _, _) => "",
		_ => string.Join(" ", ConditionList.Where(s => !string.IsNullOrEmpty(s)))
	};

	private string ShipClassConditionDisplay => ShipClass switch
	{
		ShipClass.Unknown => "",
		ShipClass id => Constants.GetShipClass(id),
	};

	private string ShipConditionDisplay => Ship switch
	{
		null => "",
		_ => Ship.NameEN
	};

	private string RemodelComparisonDisplay => Ship switch
	{
		null => "",
		_ => $"({RemodelComparisonType.Display()})"
	};


	private string FlagshipConditionDisplay => MustBeFlagship switch
	{
		true => $"({Properties.Window.Dialog.QuestTrackerManager.Flagship})",
		_ => ""
	};

	public List<string> ConditionList => new()
	{
		ShipClassConditionDisplay,
		ShipConditionDisplay,
		RemodelComparisonDisplay,
		FlagshipConditionDisplay,
	};

	public ShipConditionViewModelV2(ShipConditionModelV2 model)
	{
		Db = Ioc.Default.GetService<IKCDatabase>()!;
		ShipPickerViewModel = Ioc.Default.GetService<ShipPickerViewModel>()!;

		RemodelComparisonTypes = Enum.GetValues(typeof(RemodelComparisonType))
			.Cast<RemodelComparisonType>();

		Model = model;

		RemodelComparisonType = Model.RemodelComparisonType;
		MustBeFlagship = Model.MustBeFlagship;
		ShipClass = Model.ShipClass;

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(Ship)) return;

			Model.Id = _ship?.ShipId ?? ShipId.Unknown;

			if (Ship?.ShipId is not null && (ShipClass?)Ship?.ShipClass != ShipClass)
			{
				ShipClass = 0;
			}
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

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ShipClass)) return;

			Model.ShipClass = ShipClass;

			if (ShipClass is not 0 && (ShipClass?)Ship?.ShipClass != ShipClass)
			{
				Ship = null;
			}
		};
	}

	[RelayCommand]
	private void OpenShipPicker()
	{
		ShipPickerView shipPicker = new(ShipPickerViewModel);
		if (shipPicker.ShowDialog(App.Current.MainWindow) is true)
		{
			Model.Id = shipPicker.PickedShip!.ShipId;
			Ship = shipPicker.PickedShip!;
		}
	}

	public bool ConditionMet(IFleetData fleet)
	{
		List<IShipData> ships = fleet.MembersInstance.Where(s => s is not null).ToList();

		if (MustBeFlagship)
		{
			ships = ships.Take(1).ToList();
		}

		if (Model.ShipClass is not 0)
		{
			if (!ships.Select(s => (ShipClass)s.MasterShip.ShipClass).Contains(Model.ShipClass))
			{
				return false;
			}
		}

		if (Ship is null) return true;

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
		IShipDataMaster conditionShip = Db.MasterShips.Values
			.First(s => s.ShipId == Model.Id);

		return ship.MasterShip.BaseShip().ShipId == conditionShip.BaseShip().ShipId;
	}

	private bool HigherRemodelCheck(IShipData ship)
	{
		IShipDataMaster? conditionShip = Db.MasterShips.Values
			.First(s => s.ShipId == Model.Id);

		IShipDataMaster? masterShip = ship.MasterShip;

		while (masterShip is not null)
		{
			if (masterShip.ShipId == conditionShip.ShipId)
			{
				return true;
			}

			masterShip = masterShip.RemodelBeforeShip;
		}

		return false;
	}
}
