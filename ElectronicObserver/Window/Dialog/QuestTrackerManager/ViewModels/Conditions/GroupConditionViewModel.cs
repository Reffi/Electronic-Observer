﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

public partial class GroupConditionViewModel : ObservableObject, IConditionViewModel
{
	public bool CanBeRemoved { get; set; } = true;
	public Visibility RemoveButtonVisibility => CanBeRemoved.ToVisibility();

	public GroupConditionModel Model { get; }

	public IEnumerable<IConditionViewModel?> Conditions { get; set; }

	public IEnumerable<Operator> Operators { get; }
	public Operator GroupOperator { get; set; }

	public IEnumerable<ConditionType> ConditionTypes { get; }
	public ConditionType SelectedConditionType { get; set; } = ConditionType.ShipType;

	public string Display => string.Join($"{Separator}{GroupOperator.Display()}{Separator}",
		Conditions.Select(ConditionDisplay));

	private string Separator => GroupOperator switch
	{
		// Operator.Or => "\n",
		_ => " "
	};

	private string ConditionDisplay(IConditionViewModel? condition) => condition switch
	{
		null => Properties.Window.Dialog.QuestTrackerManager.UnknownCondition,
		GroupConditionViewModel => $"({condition.Display})",
		_ => condition.Display
	};

	public GroupConditionViewModel(GroupConditionModel model)
	{
		Operators = Enum.GetValues(typeof(Operator)).Cast<Operator>();
		ConditionTypes = Enum.GetValues(typeof(ConditionType)).Cast<ConditionType>();

		Model = model;

		GroupOperator = Model.GroupOperator;
		Conditions = CreateViewModels(Model);

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(GroupOperator)) return;

			Model.GroupOperator = GroupOperator;
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(Display)) return;

			KCDatabase.Instance.Quest.OnQuestUpdated();
		};

		Model.Conditions.CollectionChanged += (_, e) =>
		{
			Conditions = CreateViewModels(Model);
		};
	}

	private IEnumerable<IConditionViewModel?> CreateViewModels(GroupConditionModel model)
	{
		List<IConditionViewModel?> conditions = model.Conditions.Select(c => c switch
		{
			GroupConditionModel g => (IConditionViewModel)new GroupConditionViewModel(g),
			ShipConditionModel s => new ShipConditionViewModel(s),
			ShipTypeConditionModel g => new ShipTypeConditionViewModel(g),
			PartialShipConditionModel p => new PartialShipConditionViewModel(p),
			AllowedShipTypesConditionModel a => new AllowedShipTypesConditionViewModel(a),
			ShipPositionConditionModel p => new ShipPositionConditionViewModel(p),
			ShipNationalityConditionModel n => new ShipNationalityConditionViewModel(n),
			_ => null
		}).ToList();

		foreach (IConditionViewModel? condition in conditions)
		{
			if (condition is null) continue;

			condition.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName is not nameof(IConditionViewModel.Display)) return;

				OnPropertyChanged(nameof(Display));
			};
		}

		return conditions;
	}

	[ICommand]
	private void AddCondition()
	{
		Model.Conditions.Add(SelectedConditionType switch
		{
			ConditionType.Group => new GroupConditionModel(),
			ConditionType.Ship => new ShipConditionModel(),
			ConditionType.ShipType => new ShipTypeConditionModel(),
			ConditionType.PartialShip => new PartialShipConditionModel(),
			ConditionType.AllowedShipTypes => new AllowedShipTypesConditionModel(),
			ConditionType.ShipPosition => new ShipPositionConditionModel(),
			ConditionType.ShipNationality => new ShipNationalityConditionModel(),

			_ => throw new NotImplementedException(),
		});
	}

	[ICommand]
	private void RemoveCondition(ICondition? condition)
	{
		if (condition is null) return;

		Model.Conditions.Remove(condition);
	}

	public bool ConditionMet(IFleetData fleet)
	{
		List<IConditionViewModel> conditions = Conditions.Where(c => c is not null).ToList()!;

		if (!conditions.Any()) return true;

		return GroupOperator switch
		{
			Operator.And => conditions.All(c => c.ConditionMet(fleet)),
			Operator.Or => conditions.Any(c => c.ConditionMet(fleet)),

			_ => throw new NotImplementedException()
		};
	}
}
