﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Tasks;

public class ExpeditionTaskViewModel : ObservableObject, IQuestTaskViewModel
{
	public IEnumerable<ExpeditionModel> Expeditions { get; }

	public ExpeditionTaskModel Model { get; }

	public string Display => $"{Model.Progress}/{Model.Count} {GetClearCondition()}";
	public string Progress => (Model.Progress >= Model.Count) switch
	{
		true => "",
		_ => Display
	};
	public string ClearCondition => GetClearCondition();

	private string GetClearCondition()
	{
		StringBuilder sb = new();

		sb.Append($"{Model.Expedition.DisplayId}: {Model.Expedition.NameEN} x ");
		sb.Append(Model.Count);

		return sb.ToString();
	}

	public ExpeditionTaskViewModel(ExpeditionTaskModel model)
	{
		Model = model;

		Expeditions = KCDatabase.Instance.Mission.Values
			.Select(m => new ExpeditionModel(m.MissionID));

		Model.PropertyChanged += (sender, args) =>
		{
			OnPropertyChanged(nameof(Display));
		};

		PropertyChanged += (_, e) =>
		{
			if (e.PropertyName is not nameof(Display)) return;

			KCDatabase.Instance.Quest.OnQuestUpdated();
		};
	}

	public void Increment(int areaId)
	{
		if (areaId == Model.Expedition.MissionId)
		{
			Model.Progress++;
		}
	}
}
