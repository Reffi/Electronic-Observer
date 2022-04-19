﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserverTypes;
using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

[MessagePackObject]
public class ShipTypeConditionModel : ObservableObject, ICondition
{
	[Key(0)] public ObservableCollection<ShipTypes> Types { get; set; } = new() { ShipTypes.Destroyer };
	[Key(1)] public int Count { get; set; }
	[Key(2)] public bool MustBeFlagship { get; set; }
	[Key(3)] public ComparisonType ComparisonType { get; set; } = ComparisonType.GreaterOrEqual;
}
