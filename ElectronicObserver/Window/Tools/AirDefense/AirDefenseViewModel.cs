﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.AirDefense;

public class AirDefenseViewModel : WindowViewModelBase
{
	public AirDefenseTranslationViewModel AirDefense { get; }

	public bool ShowAll { get; set; }
	private bool IsCombined => SelectedFleet is FleetId.CombinedFleet;

	public ObservableCollection<AirDefenseRowViewModel> Rows { get; } = new();
	public string AdjustedFleetAAText { get; set; } = "";
	public string AnnihilationProbabilityText { get; set; } = "";

	public List<FleetId> Fleets { get; }
	public List<FormationType> Formations { get; set; } = new();
	public List<int> AaciApis { get; set; } = new();

	public FleetId SelectedFleet { get; set; } = FleetId.FirstFleet;
	public FormationType SelectedFormation { get; set; } = FormationType.LineAhead;
	public int SelectedAACI { get; set; }

	public int EnemyAircraftCount { get; set; } = 36;

	public List<FormationType> RegularFleetFormations { get; } = new()
	{
		FormationType.LineAhead,
		FormationType.DoubleLine,
		FormationType.Diamond,
		FormationType.Echelon,
		FormationType.LineAbreast,
		FormationType.Vanguard,
	};

	public List<FormationType> CombinedFleetFormations { get; } = new()
	{
		FormationType.FirstPatrolFormation,
		FormationType.SecondPatrolFormation,
		FormationType.ThirdPatrolFormation,
		FormationType.FourthPatrolFormation,
	};

	public AirDefenseViewModel()
	{
		AirDefense = Ioc.Default.GetRequiredService<AirDefenseTranslationViewModel>();

		Fleets = Enum.GetValues<FleetId>().ToList();

		UpdateFormation();
		UpdateAACutinKind(ShowAll);
		Updated();

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SelectedFleet)) return;

			Updated();
			UpdateAACutinKind(ShowAll);
			UpdateFormation();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SelectedFormation)) return;

			Updated();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SelectedAACI)) return;

			Updated();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(EnemyAircraftCount)) return;

			Updated();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ShowAll)) return;

			UpdateAACutinKind(ShowAll);
		};
	}

	private void Updated()
	{
		IShipData?[] ships = GetShips().ToArray();
		int formation = (int)SelectedFormation;
		int aaCutinKind = SelectedAACI;
		int enemyAircraftCount = EnemyAircraftCount;


		// 加重対空値
		double[] adjustedAAs = ships.Select(s => s == null ? 0.0 : Calculator.GetAdjustedAAValue(s)).ToArray();

		// 艦隊防空値
		double adjustedFleetAA = Calculator.GetAdjustedFleetAAValue(ships, formation);

		// 割合撃墜
		double[] proportionalAAs = adjustedAAs.Select((val, i) => Calculator.GetProportionalAirDefense(val, IsCombined ? (i < 6 ? 1 : 2) : -1)).ToArray();

		// 固定撃墜
		int[] fixedAAs = adjustedAAs.Select((val, i) => Calculator.GetFixedAirDefense(val, adjustedFleetAA, aaCutinKind, IsCombined ? (i < 6 ? 1 : 2) : -1)).ToArray();



		int[] shootDownBoth = adjustedAAs.Select((val, i) => ships[i] == null ? 0 :
			Calculator.GetShootDownCount(enemyAircraftCount, proportionalAAs[i], fixedAAs[i], aaCutinKind)).ToArray();

		int[] shootDownProportional = adjustedAAs.Select((val, i) => ships[i] == null ? 0 :
			Calculator.GetShootDownCount(enemyAircraftCount, proportionalAAs[i], 0, aaCutinKind)).ToArray();

		int[] shootDownFixed = adjustedAAs.Select((val, i) => ships[i] == null ? 0 :
			Calculator.GetShootDownCount(enemyAircraftCount, 0, fixedAAs[i], aaCutinKind)).ToArray();

		int[] shootDownFailed = adjustedAAs.Select((val, i) => ships[i] == null ? 0 :
			Calculator.GetShootDownCount(enemyAircraftCount, 0, 0, aaCutinKind)).ToArray();

		double[] aaRocketBarrageProbability = ships.Select(ship => Calculator.GetAARocketBarrageProbability(ship)).ToArray();


		Rows.Clear();

		for (int i = 0; i < ships.Length; i++)
		{
			if (ships[i] == null) continue;

			Rows.Add(new
			(
				this,
				ships[i].Name,
				ships[i].AABase,
				adjustedAAs[i],
				proportionalAAs[i],
				fixedAAs[i],
				shootDownBoth[i],
				shootDownProportional[i],
				shootDownFixed[i],
				shootDownFailed[i],
				aaRocketBarrageProbability[i]
			));
		}

		AdjustedFleetAAText = adjustedFleetAA.ToString("0.0");
		{
			IEnumerable<int> allShootDown = shootDownBoth.Concat(shootDownProportional).Concat(shootDownFixed).Concat(shootDownFailed);
			AnnihilationProbabilityText = (allShootDown.Count(i => i >= enemyAircraftCount) / Math.Max(ships.Count(s => s != null) * 4, 1.0)).ToString("p1");
		}
	}


	private IEnumerable<IShipData?> GetShips() => SelectedFleet switch
	{
		FleetId.CombinedFleet => KCDatabase.Instance.Fleet[1].MembersWithoutEscaped!
			.Concat(KCDatabase.Instance.Fleet[2].MembersWithoutEscaped!),

		_ => KCDatabase.Instance.Fleet[(int)SelectedFleet].MembersWithoutEscaped!
	};

	private void UpdateAACutinKind(bool showAll) => AaciApis = showAll switch
	{
		true => Enumerable.Range(0, Calculator.AACutinFixedBonus.Keys.Max() + 1)
			.Select(kind => kind)
			.ToList(),

		_ => GetShips()
			.Where(s => s != null)
			.Select(s => Calculator.GetAACutinKind(s.ShipID, s.AllSlotMaster.ToArray()))
			.Concat(Enumerable.Repeat(0, 1))
			.Distinct()
			.OrderBy(i => i)
			.Select(kind => kind)
			.ToList()
	};

	private void UpdateFormation()
	{
		Formations = IsCombined switch
		{
			true => CombinedFleetFormations,
			_ => RegularFleetFormations
		};

		SelectedFormation = Formations.First();
	}
}
