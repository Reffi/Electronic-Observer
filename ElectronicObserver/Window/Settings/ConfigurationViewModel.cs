﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Timers;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Properties.Window.Dialog;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Settings.Connection;
using ElectronicObserver.Window.Settings.Log;
using ElectronicObserver.Window.Settings.UI;

namespace ElectronicObserver.Window.Settings;

public partial class ConfigurationViewModel : WindowViewModelBase
{
	public ConfigurationTranslationViewModel Translation { get; }

	public ConfigurationConnectionViewModel Connection { get; }
	public ConfigurationUIViewModel UI { get; }
	public ConfigurationLogViewModel Log { get; }

	private IEnumerable<ConfigurationViewModelBase> Configurations()
	{
		yield return Connection;
		yield return UI;
		yield return Log;
	}

	private Timer Timer { get; } = new();
	public string Log_PlayTime { get; set; } = "";
	private DateTime ShownTime { get; set; }
	private double PlayTimeCache { get; set; }

	public bool? DialogResult { get; set; }

	public ConfigurationViewModel()
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationTranslationViewModel>();

		Connection = new(Configuration.Config.Connection);
		UI = new(Configuration.Config.UI);
		Log = new(Configuration.Config.Log);

		ShownTime = DateTime.Now;
		PlayTimeCache = Configuration.Config.Log.PlayTime;
		UpdatePlayTime();

		Timer.Interval = 1000;
		Timer.Elapsed += (sender, args) =>
		{
			UpdatePlayTime();
		};

		Timer.Start();
	}

	private void UpdatePlayTime()
	{
		double elapsed = (DateTime.Now - ShownTime).TotalSeconds;
		Log_PlayTime = Utility.Mathematics.DateTimeHelper.ToTimeElapsedString(TimeSpan.FromSeconds(PlayTimeCache + elapsed));
	}

	private bool TrySaveConfigurations()
	{
		List<ValidationResult> errors = Configurations()
			.SelectMany(c => c.GetErrors())
			.DistinctBy(v => v.ErrorMessage)
			.ToList();

		if (errors.Any())
		{
			string caption = DialogConfiguration.DialogCaptionErrorTitle;
			string errorMessage = string.Join("\n", errors);

			MessageBox.Show(App.Current!.MainWindow!, errorMessage, caption, MessageBoxButton.OK, MessageBoxImage.Error);
			
			return false;
		}

		foreach (ConfigurationViewModelBase configuration in Configurations())
		{
			configuration.Save();
		}

		return true;
	}

	[ICommand]
	private void Confirm()
	{
		if (!TrySaveConfigurations()) return;

		Configuration.Instance.OnConfigurationChanged();
		DialogResult = true;
	}

	[ICommand]
	private void Cancel()
	{
		DialogResult = false;
	}
}
