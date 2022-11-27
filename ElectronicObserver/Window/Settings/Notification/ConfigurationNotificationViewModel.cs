﻿using System.Collections.Generic;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Notifier;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Settings.Notification.AnchorageRepair;
using ElectronicObserver.Window.Settings.Notification.Base;
using ElectronicObserver.Window.Settings.Notification.BaseAirCorps;
using ElectronicObserver.Window.Settings.Notification.BattleEnd;
using ElectronicObserver.Window.Settings.Notification.Damage;

namespace ElectronicObserver.Window.Settings.Notification
{
	public partial class ConfigurationNotificationViewModel : ConfigurationViewModelBase
	{
		public ConfigurationNotificationTranslationViewModel Translation { get; }

		private Configuration.ConfigurationData Config { get; }

		public ConfigurationNotificationBaseViewModel Expedition { get; }
		public ConfigurationNotificationBaseViewModel Construction { get; }
		public ConfigurationNotificationBaseViewModel Repair { get; }
		public ConfigurationNotificationBaseViewModel Condition { get; }
		public ConfigurationNotificationDamageViewModel Damage { get; }
		public ConfigurationNotificationAnchorageRepairViewModel AnchorageRepair { get; }
		public ConfigurationNotificationBaseAirCorpsViewModel BaseAirCorps { get; }
		public ConfigurationNotificationBattleEndViewModel BattleEnd { get; }
		public ConfigurationNotificationBaseViewModel RemodelLevel { get; }

		public ConfigurationNotificationViewModel(Configuration.ConfigurationData config)
		{
			Translation = Ioc.Default.GetRequiredService<ConfigurationNotificationTranslationViewModel>();

			Config = config;

			Expedition = new(Config.NotifierExpedition, NotifierManager.Instance.Expedition);
			Construction = new(Config.NotifierConstruction, NotifierManager.Instance.Construction);
			Repair = new(Config.NotifierRepair, NotifierManager.Instance.Repair);
			Condition = new(Config.NotifierCondition, NotifierManager.Instance.Condition);
			Damage = new(Config.NotifierDamage, NotifierManager.Instance.Damage);
			AnchorageRepair = new(Config.NotifierAnchorageRepair, NotifierManager.Instance.AnchorageRepair);
			BaseAirCorps = new(Config.NotifierBaseAirCorps, NotifierManager.Instance.BaseAirCorps);
			BattleEnd = new(Config.NotifierBattleEnd, NotifierManager.Instance.BattleEnd);
			RemodelLevel = new(Config.NotifierRemodelLevel, NotifierManager.Instance.RemodelLevel);
		}

		public override void Save()
		{
			// no need to save here because it's already saved in the dialog
		}

		[RelayCommand]
		private void OpenNotificationConfigDialog(Base.ConfigurationNotificationBaseViewModel? config)
		{
			if (config is null) return;

			new ConfigurationNotificationDialog(config).ShowDialog(App.Current!.MainWindow!);
		}
	}
}
