﻿using System.ComponentModel.DataAnnotations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.SubWindow.Quest;

public enum AutoSavingTiming
{
	[Display(ResourceType = typeof(ConfigRes), Name = "ProgressAutoSaving_Disable")]
	None,

	[Display(ResourceType = typeof(ConfigRes), Name = "ProgressAutoSaving_Hourly")]
	Hourly,

	[Display(ResourceType = typeof(ConfigRes), Name = "ProgressAutoSaving_Daily")]
	Daily,
}
