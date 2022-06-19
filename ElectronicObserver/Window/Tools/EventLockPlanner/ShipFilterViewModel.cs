﻿using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Dialog.ShipPicker;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using WanaKanaNet;
namespace ElectronicObserver.Window.Tools.EventLockPlanner;

public partial class ShipFilterViewModel : ObservableObject
{
	public ShipFilterTranslationViewModel ShipFilter { get; }
	public TransliterationService TransliterationService { get; }

	public List<Filter> TypeFilters { get; }

	public int LevelMin { get; set; } = 0;
	public int LevelMax { get; set; } = 175;

	public int AswMin { get; set; } = 0;
	public int AswMax { get; set; } = 200;

	public int LuckMin { get; set; } = 0;
	public int LuckMax { get; set; } = 200;

	public bool CanEquipDaihatsu { get; set; }
	public bool CanEquipTank { get; set; }
	public bool CanEquipFcf { get; set; }
	public bool HasExpansionSlot { get; set; }
	public string? NameFilter { get; set; } = "";

	public ShipFilterViewModel()
	{
		ShipFilter = Ioc.Default.GetService<ShipFilterTranslationViewModel>()!;
		TransliterationService = Ioc.Default.GetService<TransliterationService>()!;

		TypeFilters = Enum.GetValues<ShipTypeGroup>()
			.Select(t => new Filter(t)
			{
				IsChecked = true,
			})
			.ToList();

		foreach (Filter filter in TypeFilters)
		{
			filter.PropertyChanged += (_, _) => OnPropertyChanged(string.Empty);
		}
	}

	public bool MeetsFilterCondition(IShipData ship)
	{
		List<ShipTypes> enabledFilters = TypeFilters
			.Where(f => f.IsChecked)
			.SelectMany(f => f.Value.ToTypes())
			.ToList();

		if (!enabledFilters.Contains(ship.MasterShip.ShipType)) return false;
		if (ship.Level < LevelMin) return false;
		if (ship.Level > LevelMax) return false;
		if (ship.ASWBase < AswMin) return false;
		if (ship.ASWBase > AswMax) return false;
		if (ship.LuckBase < LuckMin) return false;
		if (ship.LuckBase > LuckMax) return false;
		if (CanEquipDaihatsu && !ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.LandingCraft)) return false;
		if (CanEquipTank && !ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.SpecialAmphibiousTank)) return false;
		if (CanEquipFcf && !ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.CommandFacility)) return false;
		if (HasExpansionSlot && !ship.IsExpansionSlotAvailable) return false;
		if (!string.IsNullOrEmpty(NameFilter) && !TransliterationService.Matches(ship.MasterShip, NameFilter, WanaKana.ToRomaji(NameFilter))) return false;
		// other filters

		return true;
	}

	[ICommand]
	private void ToggleShipTypes()
	{
		if (TypeFilters.All(f => f.IsChecked))
		{
			foreach (Filter typeFilter in TypeFilters)
			{
				typeFilter.IsChecked = false;
			}
		}
		else
		{
			foreach (Filter typeFilter in TypeFilters)
			{
				typeFilter.IsChecked = true;
			}
		}
	}
}
