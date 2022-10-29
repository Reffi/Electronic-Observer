﻿using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.SubWindow.Compass;

public class ConfigurationCompassViewModel : ConfigurationViewModelBase
{
	public ConfigurationCompassTranslationViewModel Translation { get; }
	private Configuration.ConfigurationData.ConfigFormCompass Config { get; }

	public int CandidateDisplayCount { get; set; }

	public bool IsScrollable { get; set; }

	public int MaxShipNameWidth { get; set; }

	public ConfigurationCompassViewModel(Configuration.ConfigurationData.ConfigFormCompass config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationCompassTranslationViewModel>();

		Config = config;
		Load();
	}

	private void Load()
	{
		CandidateDisplayCount = Config.CandidateDisplayCount;
		IsScrollable = Config.IsScrollable;
		MaxShipNameWidth = Config.MaxShipNameWidth;
	}

	public override void Save()
	{
		Config.CandidateDisplayCount = CandidateDisplayCount;
		Config.IsScrollable = IsScrollable;
		Config.MaxShipNameWidth = MaxShipNameWidth;
	}
}
