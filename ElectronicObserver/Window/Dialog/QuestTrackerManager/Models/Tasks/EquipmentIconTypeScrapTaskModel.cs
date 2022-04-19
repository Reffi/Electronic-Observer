﻿using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserverTypes;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

[MessagePackObject]
public class EquipmentIconTypeScrapTaskModel : ObservableObject, IQuestTask
{
	[Key(0)] public EquipmentIconType IconType { get; set; } = EquipmentIconType.MainGunSmall;
	[Key(1)] public int Count { get; set; }
	[IgnoreMember] public int Progress { get; set; }

	public EquipmentIconTypeScrapTaskModel()
	{
		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(Progress) or nameof(Count))) return;

			Progress = Math.Clamp(Progress, 0, Count);
			Count = Math.Clamp(Count, 1, int.MaxValue);
		};
	}
}
