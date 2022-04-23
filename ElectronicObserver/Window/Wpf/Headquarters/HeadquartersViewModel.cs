﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicObserver.Window.Wpf.Headquarters;

public class HeadquarterItemViewModel : ObservableObject
{
	public string? Text { get; set; }
	public string? ToolTip { get; set; }
	public Visibility Visible { get; set; } = Visibility.Visible;
	public ImageSource? Icon { get; set; }
	public System.Drawing.Color BackColor { get; set; }
	public System.Drawing.Color ForeColor { get; set; }
	public bool Tag { get; set; }

	public SolidColorBrush Foreground => ForeColor.ToBrush();
	public SolidColorBrush Background => BackColor.ToBrush();
}

public class HQLevelViewModel : HeadquarterItemViewModel
{
	public int Value { get; set; }
	public string? TextNext { get; set; }
	public int ValueNext { get; set; }
}

public partial class HeadquartersViewModel : AnchorableViewModel
{
	public FormHeadquartersTranslationViewModel FormHeadquarters { get; }

	public Visibility Visible { get; set; } = Visibility.Collapsed;

	public FontFamily MainFont { get; set; }
	public float MainFontSize { get; set; }
	public SolidColorBrush MainFontColor { get; set; }
	public FontFamily SubFont { get; set; }
	public float SubFontSize { get; set; }
	public SolidColorBrush SubFontColor { get; set; }

	public HeadquarterItemViewModel AdmiralName { get; } = new();
	public HeadquarterItemViewModel AdmiralComment { get; } = new();
	public HQLevelViewModel HQLevel { get; } = new();
	public HeadquarterItemViewModel ShipCount { get; } = new();
	public HeadquarterItemViewModel EquipmentCount { get; } = new();
	public HeadquarterItemViewModel InstantRepair { get; } = new();
	public HeadquarterItemViewModel InstantConstruction { get; } = new();
	public HeadquarterItemViewModel DevelopmentMaterial { get; } = new();
	public HeadquarterItemViewModel ModdingMaterial { get; } = new();
	public HeadquarterItemViewModel FurnitureCoin { get; } = new();
	public HeadquarterItemViewModel DisplayUseItem { get; } = new();
	public HeadquarterItemViewModel Fuel { get; } = new();
	public HeadquarterItemViewModel Ammo { get; } = new();
	public HeadquarterItemViewModel Steel { get; } = new();
	public HeadquarterItemViewModel Bauxite { get; } = new();

	private List<HeadquarterItemViewModel> Items { get; }

	public HeadquartersViewModel() : base("HQ", "Headquarters",
		ImageSourceIcons.GetIcon(IconContent.FormHeadQuarters))
	{
		FormHeadquarters = App.Current.Services.GetService<FormHeadquartersTranslationViewModel>()!;

		Title = FormHeadquarters.Title;
		FormHeadquarters.PropertyChanged += (_, _) => Title = FormHeadquarters.Title;

		ShipCount.Icon = ImageSourceIcons.GetIcon(IconContent.HeadQuartersShip);
		EquipmentCount.Icon = ImageSourceIcons.GetIcon(IconContent.HeadQuartersEquipment);
		InstantRepair.Icon = ImageSourceIcons.GetIcon(IconContent.ItemInstantRepair);
		InstantConstruction.Icon = ImageSourceIcons.GetIcon(IconContent.ItemInstantConstruction);
		DevelopmentMaterial.Icon = ImageSourceIcons.GetIcon(IconContent.ItemDevelopmentMaterial);
		ModdingMaterial.Icon = ImageSourceIcons.GetIcon(IconContent.ItemModdingMaterial);
		FurnitureCoin.Icon = ImageSourceIcons.GetIcon(IconContent.ItemFurnitureCoin);
		DisplayUseItem.Icon = ImageSourceIcons.GetIcon(IconContent.ItemPresentBox);
		Fuel.Icon = ImageSourceIcons.GetIcon(IconContent.ResourceFuel);
		Ammo.Icon = ImageSourceIcons.GetIcon(IconContent.ResourceAmmo);
		Steel.Icon = ImageSourceIcons.GetIcon(IconContent.ResourceSteel);
		Bauxite.Icon = ImageSourceIcons.GetIcon(IconContent.ResourceBauxite);

		Items = new()
		{
			AdmiralName,
			AdmiralComment,
			HQLevel,
			ShipCount,
			EquipmentCount,
			InstantRepair,
			InstantConstruction,
			DevelopmentMaterial,
			ModdingMaterial,
			FurnitureCoin,
			DisplayUseItem,
			Fuel,
			Ammo,
			Steel,
			Bauxite,
		};

		APIObserver o = APIObserver.Instance;

		o.ApiReqNyukyo_Start.RequestReceived += Updated;
		o.ApiReqNyukyo_Speedchange.RequestReceived += Updated;
		o.ApiReqKousyou_CreateShip.RequestReceived += Updated;
		o.ApiReqKousyou_CreateShipSpeedChange.RequestReceived += Updated;
		o.ApiReqKousyou_Destroyship.RequestReceived += Updated;
		o.ApiReqKousyou_DestroyItem2.RequestReceived += Updated;
		o.ApiReqMember_UpdateComment.RequestReceived += Updated;

		o.ApiGetMember_Basic.ResponseReceived += Updated;
		o.ApiGetMember_SlotItem.ResponseReceived += Updated;
		o.ApiPort_Port.ResponseReceived += Updated;
		o.ApiGetMember_Ship2.ResponseReceived += Updated;
		o.ApiReqKousyou_Getship.ResponseReceived += Updated;
		o.ApiReqHokyu_Charge.ResponseReceived += Updated;
		o.ApiReqKousyou_Destroyship.ResponseReceived += Updated;
		o.ApiReqKousyou_DestroyItem2.ResponseReceived += Updated;
		o.ApiReqKaisou_Powerup.ResponseReceived += Updated;
		o.ApiReqKousyou_CreateItem.ResponseReceived += Updated;
		o.ApiReqKousyou_RemodelSlot.ResponseReceived += Updated;
		o.ApiGetMember_Material.ResponseReceived += Updated;
		o.ApiGetMember_ShipDeck.ResponseReceived += Updated;
		o.ApiReqAirCorps_SetPlane.ResponseReceived += Updated;
		o.ApiReqAirCorps_Supply.ResponseReceived += Updated;
		o.ApiGetMember_UseItem.ResponseReceived += Updated;


		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		Utility.SystemEvents.UpdateTimerTick += SystemEvents_UpdateTimerTick;

		ConfigurationChanged();
	}

	void ConfigurationChanged()
	{
		MainFont = new(Utility.Configuration.Config.UI.MainFont.FontData.FontFamily.Name);
		MainFontSize = Utility.Configuration.Config.UI.MainFont.FontData.ToSize();
		SubFont = new(Utility.Configuration.Config.UI.SubFont.FontData.FontFamily.Name);
		SubFontSize = Utility.Configuration.Config.UI.SubFont.FontData.ToSize();
		MainFontColor = Utility.Configuration.Config.UI.ForeColor.ToBrush();
		SubFontColor = Utility.Configuration.Config.UI.SubForeColor.ToBrush();

		foreach (HeadquarterItemViewModel item in Items)
		{
			item.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			item.BackColor = Utility.Configuration.Config.UI.BackColor;
		}

		/*
		Font = FlowPanelMaster.Font = Utility.Configuration.Config.UI.MainFont;
		HQLevel.MainFont = Utility.Configuration.Config.UI.MainFont;
		HQLevel.SubFont = Utility.Configuration.Config.UI.SubFont;
		HQLevel.MainFontColor = Utility.Configuration.Config.UI.ForeColor;
		HQLevel.SubFontColor = Utility.Configuration.Config.UI.SubForeColor;

		// 点滅しない設定にしたときに消灯状態で固定されるのを防ぐ
		if (!Utility.Configuration.Config.FormHeadquarters.BlinkAtMaximum)
		{
			if (ShipCount.Tag as bool? ?? false)
			{
				ShipCount.BackColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG;
				ShipCount.ForeColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG;
			}

			if (EquipmentCount.Tag as bool? ?? false)
			{
				EquipmentCount.BackColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG;
				EquipmentCount.ForeColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG;
			}
		}
		*/

		//visibility
		CheckVisibilityConfiguration();
		{
			var visibility = Utility.Configuration.Config.FormHeadquarters.Visibility.List;
			AdmiralName.Visible = visibility[0].ToVisibility();
			AdmiralComment.Visible = visibility[1].ToVisibility();
			HQLevel.Visible = visibility[2].ToVisibility();
			ShipCount.Visible = visibility[3].ToVisibility();
			EquipmentCount.Visible = visibility[4].ToVisibility();
			InstantRepair.Visible = visibility[5].ToVisibility();
			InstantConstruction.Visible = visibility[6].ToVisibility();
			DevelopmentMaterial.Visible = visibility[7].ToVisibility();
			ModdingMaterial.Visible = visibility[8].ToVisibility();
			FurnitureCoin.Visible = visibility[9].ToVisibility();
			Fuel.Visible = visibility[10].ToVisibility();
			Ammo.Visible = visibility[11].ToVisibility();
			Steel.Visible = visibility[12].ToVisibility();
			Bauxite.Visible = visibility[13].ToVisibility();
			DisplayUseItem.Visible = visibility[14].ToVisibility();
		}
		UpdateDisplayUseItem();
	}

	/// <summary>
	/// VisibleFlags 設定をチェックし、不正な値だった場合は初期値に戻します。
	/// </summary>
	public static void CheckVisibilityConfiguration()
	{
		const int count = 15;
		var config = Utility.Configuration.Config.FormHeadquarters;

		if (config.Visibility == null)
			config.Visibility = new Utility.Storage.SerializableList<bool>(Enumerable.Repeat(true, count).ToList());

		for (int i = config.Visibility.List.Count; i < count; i++)
		{
			config.Visibility.List.Add(true);
		}

	}

	/// <summary>
	/// 各表示項目の名称を返します。
	/// </summary>
	public static IEnumerable<string> GetItemNames()
	{
		var formHeadquarters = App.Current.Services.GetService<FormHeadquartersTranslationViewModel>()!;

		yield return formHeadquarters.ItemNameName;
		yield return formHeadquarters.ItemNameComment;
		yield return formHeadquarters.ItemNameHQLevel;
		yield return formHeadquarters.ItemNameShipSlots;
		yield return formHeadquarters.ItemNameEquipmentSlots;
		yield return formHeadquarters.ItemNameInstantRepair;
		yield return formHeadquarters.ItemNameInstantConstruction;
		yield return formHeadquarters.ItemNameDevelopmentMaterial;
		yield return formHeadquarters.ItemNameImproveMaterial;
		yield return formHeadquarters.ItemNameFurnitureCoin;
		yield return formHeadquarters.ItemNameFuel;
		yield return formHeadquarters.ItemNameAmmo;
		yield return formHeadquarters.ItemNameSteel;
		yield return formHeadquarters.ItemNameBauxite;
		yield return formHeadquarters.ItemNameOtherItem;
	}


	void Updated(string apiname, dynamic data)
	{

		KCDatabase db = KCDatabase.Instance;

		var configUI = Utility.Configuration.Config.UI;

		if (!db.Admiral.IsAvailable)
			return;

		//FlowPanelMaster.SuspendLayout();

		//Admiral
		//FlowPanelAdmiral.SuspendLayout();
		AdmiralName.Text = string.Format("{0} {1}", db.Admiral.AdmiralName, Constants.GetAdmiralRank(db.Admiral.Rank));
		{
			StringBuilder tooltip = new();

			var sortieCount = db.Admiral.SortieWin + db.Admiral.SortieLose;
			tooltip.AppendFormat(FormHeadquarters.AdmiralNameToolTipSortie + "\r\n",
				sortieCount, db.Admiral.SortieWin, db.Admiral.SortieWin / Math.Max(sortieCount, 1.0), db.Admiral.SortieLose);

			tooltip.AppendFormat(FormHeadquarters.AdmiralNameToolTipSortieExp + "\r\n",
				db.Admiral.Exp / Math.Max(sortieCount, 1.0),
				db.Admiral.Exp / Math.Max(db.Admiral.SortieWin, 1.0));

			tooltip.AppendFormat(FormHeadquarters.AdmiralNameToolTipExpedition + "\r\n",
				db.Admiral.MissionCount, db.Admiral.MissionSuccess, db.Admiral.MissionSuccess / Math.Max(db.Admiral.MissionCount, 1.0), db.Admiral.MissionCount - db.Admiral.MissionSuccess);

			var practiceCount = db.Admiral.PracticeWin + db.Admiral.PracticeLose;
			tooltip.AppendFormat(FormHeadquarters.AdmiralNameToolTipPractice + "\r\n",
				practiceCount, db.Admiral.PracticeWin, db.Admiral.PracticeWin / Math.Max(practiceCount, 1.0), db.Admiral.PracticeLose);

			tooltip.AppendFormat(FormHeadquarters.AdmiralNameToolTipFirstClassMedals + "\r\n", db.Admiral.Medals);

			AdmiralName.ToolTip = tooltip.ToString();
		}

		AdmiralComment.Text = db.Admiral.Comment;
		//FlowPanelAdmiral.ResumeLayout();

		//HQ Level
		HQLevel.Value = db.Admiral.Level;
		{
			StringBuilder tooltip = new StringBuilder();
			if (db.Admiral.Level < ExpTable.AdmiralExp.Max(e => e.Key))
			{
				HQLevel.TextNext = "next:";
				HQLevel.ValueNext = ExpTable.GetNextExpAdmiral(db.Admiral.Exp);
				tooltip.AppendFormat("{0} / {1}\r\n", db.Admiral.Exp, ExpTable.AdmiralExp[db.Admiral.Level + 1].Total);
			}
			else
			{
				HQLevel.TextNext = "exp:";
				HQLevel.ValueNext = db.Admiral.Exp;
			}

			//戦果ツールチップ
			//fixme: もっとましな書き方はなかっただろうか
			{
				var res = RecordManager.Instance.Resource.GetRecordPrevious();
				if (res != null)
				{
					int diff = db.Admiral.Exp - res.HQExp;
					tooltip.AppendFormat(FormHeadquarters.HQLevelToolTipSenkaSession + "\r\n", diff, diff * 7 / 10000.0);
				}
			}
			{
				var res = RecordManager.Instance.Resource.GetRecordDay();
				if (res != null)
				{
					int diff = db.Admiral.Exp - res.HQExp;
					tooltip.AppendFormat(FormHeadquarters.HQLevelToolTipSenkaDay + "\r\n", diff, diff * 7 / 10000.0);
				}
			}
			{
				var res = RecordManager.Instance.Resource.GetRecordMonth();
				if (res != null)
				{
					int diff = db.Admiral.Exp - res.HQExp;
					tooltip.AppendFormat(FormHeadquarters.HQLevelToolTipSenkaMonth + "\r\n", diff, diff * 7 / 10000.0);
				}
			}

			HQLevel.ToolTip = tooltip.ToString();
		}

		//Fleet
		// FlowPanelFleet.SuspendLayout();
		{

			ShipCount.Text = string.Format("{0}/{1}", RealShipCount, db.Admiral.MaxShipCount);
			if (RealShipCount > db.Admiral.MaxShipCount - 5)
			{
				ShipCount.BackColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG;
				ShipCount.ForeColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG;
			}
			else
			{
				ShipCount.BackColor = System.Drawing.Color.Transparent;
				ShipCount.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			}
			ShipCount.Tag = RealShipCount >= db.Admiral.MaxShipCount;

			EquipmentCount.Text = string.Format("{0}/{1}", RealEquipmentCount, db.Admiral.MaxEquipmentCount);
			if (RealEquipmentCount > db.Admiral.MaxEquipmentCount + 3 - 20)
			{
				EquipmentCount.BackColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG;
				EquipmentCount.ForeColor = Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG;
			}
			else
			{
				EquipmentCount.BackColor = System.Drawing.Color.Transparent;
				EquipmentCount.ForeColor = Utility.Configuration.Config.UI.ForeColor;
			}
			EquipmentCount.Tag = RealEquipmentCount >= db.Admiral.MaxEquipmentCount;

		}
		// FlowPanelFleet.ResumeLayout();



		var resday = RecordManager.Instance.Resource.GetRecord(DateTime.Now.AddHours(-5).Date.AddHours(5));
		var resweek = RecordManager.Instance.Resource.GetRecord(DateTime.Now.AddHours(-5).Date.AddDays(-(((int)DateTime.Now.AddHours(-5).DayOfWeek + 6) % 7)).AddHours(5)); //月曜日起点
		var resmonth = RecordManager.Instance.Resource.GetRecord(new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddHours(5));


		//UseItems
		// FlowPanelUseItem.SuspendLayout();

		InstantRepair.Text = db.Material.InstantRepair.ToString();
		if (db.Material.InstantRepair >= 3000)
		{
			InstantRepair.ForeColor = configUI.Headquarters_MaterialMaxFG;
			InstantRepair.BackColor = configUI.Headquarters_MaterialMaxBG;
		}
		else if (db.Material.InstantRepair < (configUI.HqResLowAlertBucket == -1 ? db.Admiral.MaxResourceRegenerationAmount : configUI.HqResLowAlertBucket))
		{
			InstantRepair.ForeColor = configUI.Headquarters_ResourceLowFG;
			InstantRepair.BackColor = configUI.Headquarters_ResourceLowBG;
		}
		else
		{
			InstantRepair.ForeColor = configUI.ForeColor;
			InstantRepair.BackColor = System.Drawing.Color.Transparent;
		}
		InstantRepair.ToolTip = string.Format(FormHeadquarters.ResourceToolTip,
			resday == null ? 0 : (db.Material.InstantRepair - resday.InstantRepair),
			resweek == null ? 0 : (db.Material.InstantRepair - resweek.InstantRepair),
			resmonth == null ? 0 : (db.Material.InstantRepair - resmonth.InstantRepair));

		InstantConstruction.Text = db.Material.InstantConstruction.ToString();
		if (db.Material.InstantConstruction >= 3000)
		{
			InstantConstruction.ForeColor = configUI.Headquarters_MaterialMaxFG;
			InstantConstruction.BackColor = configUI.Headquarters_MaterialMaxBG;
		}
		else
		{
			InstantConstruction.ForeColor = configUI.ForeColor;
			InstantConstruction.BackColor = System.Drawing.Color.Transparent;
		}
		InstantConstruction.ToolTip = string.Format(FormHeadquarters.ResourceToolTip,
			resday == null ? 0 : (db.Material.InstantConstruction - resday.InstantConstruction),
			resweek == null ? 0 : (db.Material.InstantConstruction - resweek.InstantConstruction),
			resmonth == null ? 0 : (db.Material.InstantConstruction - resmonth.InstantConstruction));

		DevelopmentMaterial.Text = db.Material.DevelopmentMaterial.ToString();
		if (db.Material.DevelopmentMaterial >= 3000)
		{
			DevelopmentMaterial.ForeColor = configUI.Headquarters_MaterialMaxFG;
			DevelopmentMaterial.BackColor = configUI.Headquarters_MaterialMaxBG;
		}
		else
		{
			DevelopmentMaterial.ForeColor = configUI.ForeColor;
			DevelopmentMaterial.BackColor = System.Drawing.Color.Transparent;
		}
		DevelopmentMaterial.ToolTip = string.Format(FormHeadquarters.ResourceToolTip,
			resday == null ? 0 : (db.Material.DevelopmentMaterial - resday.DevelopmentMaterial),
			resweek == null ? 0 : (db.Material.DevelopmentMaterial - resweek.DevelopmentMaterial),
			resmonth == null ? 0 : (db.Material.DevelopmentMaterial - resmonth.DevelopmentMaterial));

		ModdingMaterial.Text = db.Material.ModdingMaterial.ToString();
		if (db.Material.ModdingMaterial >= 3000)
		{
			ModdingMaterial.ForeColor = configUI.Headquarters_MaterialMaxFG;
			ModdingMaterial.BackColor = configUI.Headquarters_MaterialMaxBG;
		}
		else
		{
			ModdingMaterial.ForeColor = configUI.ForeColor;
			ModdingMaterial.BackColor = System.Drawing.Color.Transparent;
		}
		ModdingMaterial.ToolTip = string.Format(FormHeadquarters.ResourceToolTip,
			resday == null ? 0 : (db.Material.ModdingMaterial - resday.ModdingMaterial),
			resweek == null ? 0 : (db.Material.ModdingMaterial - resweek.ModdingMaterial),
			resmonth == null ? 0 : (db.Material.ModdingMaterial - resmonth.ModdingMaterial));

		const int furnitureCoinCap = 350000;

		FurnitureCoin.Text = db.Admiral.FurnitureCoin.ToString();
		if (db.Admiral.FurnitureCoin >= furnitureCoinCap)
		{
			FurnitureCoin.ForeColor = configUI.Headquarters_CoinMaxFG;
			FurnitureCoin.BackColor = configUI.Headquarters_CoinMaxBG;
		}
		else
		{
			FurnitureCoin.ForeColor = configUI.ForeColor;
			FurnitureCoin.BackColor = System.Drawing.Color.Transparent;
		}
		{
			int small = db.UseItems[10]?.Count ?? 0;
			int medium = db.UseItems[11]?.Count ?? 0;
			int large = db.UseItems[12]?.Count ?? 0;

			FurnitureCoin.ToolTip = string.Format(FormHeadquarters.FurnitureCoinToolTip,
				small, small * 200,
				medium, medium * 400,
				large, large * 700);
		}
		UpdateDisplayUseItem();
		// FlowPanelUseItem.ResumeLayout();


		//Resources
		// FlowPanelResource.SuspendLayout();
		{
			const int resourceHardcap = 350000;

			Fuel.Text = db.Material.Fuel.ToString();

			if (db.Material.Fuel >= resourceHardcap)
			{
				Fuel.ForeColor = configUI.Headquarters_ResourceMaxFG;
				Fuel.BackColor = configUI.Headquarters_ResourceMaxBG;
			}
			else if (db.Material.Fuel < (configUI.HqResLowAlertFuel == -1 ? db.Admiral.MaxResourceRegenerationAmount : configUI.HqResLowAlertFuel))
			{
				Fuel.ForeColor = configUI.Headquarters_ResourceLowFG;
				Fuel.BackColor = configUI.Headquarters_ResourceLowBG;
			}
			else if (db.Material.Fuel > db.Admiral.MaxResourceRegenerationAmount)
			{
				Fuel.ForeColor = configUI.Headquarters_ResourceOverFG;
				Fuel.BackColor = configUI.Headquarters_ResourceOverBG;
			}
			else
			{
				Fuel.ForeColor = configUI.ForeColor;
				Fuel.BackColor = System.Drawing.Color.Transparent;
			}
			Fuel.ToolTip = string.Format(FormHeadquarters.ResourceToolTip,
				resday == null ? 0 : (db.Material.Fuel - resday.Fuel),
				resweek == null ? 0 : (db.Material.Fuel - resweek.Fuel),
				resmonth == null ? 0 : (db.Material.Fuel - resmonth.Fuel));

			Ammo.Text = db.Material.Ammo.ToString();
			if (db.Material.Ammo >= resourceHardcap)
			{
				Ammo.ForeColor = configUI.Headquarters_ResourceMaxFG;
				Ammo.BackColor = configUI.Headquarters_ResourceMaxBG;
			}
			else if (db.Material.Ammo < (configUI.HqResLowAlertAmmo == -1 ? db.Admiral.MaxResourceRegenerationAmount : configUI.HqResLowAlertAmmo))
			{
				Ammo.ForeColor = configUI.Headquarters_ResourceLowFG;
				Ammo.BackColor = configUI.Headquarters_ResourceLowBG;
			}
			else if (db.Material.Ammo > db.Admiral.MaxResourceRegenerationAmount)
			{
				Ammo.ForeColor = configUI.Headquarters_ResourceOverFG;
				Ammo.BackColor = configUI.Headquarters_ResourceOverBG;
			}
			else
			{
				Ammo.ForeColor = configUI.ForeColor;
				Ammo.BackColor = System.Drawing.Color.Transparent;
			}
			Ammo.ToolTip = string.Format(FormHeadquarters.ResourceToolTip,
				resday == null ? 0 : (db.Material.Ammo - resday.Ammo),
				resweek == null ? 0 : (db.Material.Ammo - resweek.Ammo),
				resmonth == null ? 0 : (db.Material.Ammo - resmonth.Ammo));

			Steel.Text = db.Material.Steel.ToString();
			if (db.Material.Steel >= resourceHardcap)
			{
				Steel.ForeColor = configUI.Headquarters_ResourceMaxFG;
				Steel.BackColor = configUI.Headquarters_ResourceMaxBG;
			}
			else if (db.Material.Steel < (configUI.HqResLowAlertSteel == -1 ? db.Admiral.MaxResourceRegenerationAmount : configUI.HqResLowAlertSteel))
			{
				Steel.ForeColor = configUI.Headquarters_ResourceLowFG;
				Steel.BackColor = configUI.Headquarters_ResourceLowBG;
			}
			else if (db.Material.Steel > db.Admiral.MaxResourceRegenerationAmount)
			{
				Steel.ForeColor = configUI.Headquarters_ResourceOverFG;
				Steel.BackColor = configUI.Headquarters_ResourceOverBG;
			}
			else
			{
				Steel.ForeColor = configUI.ForeColor;
				Steel.BackColor = System.Drawing.Color.Transparent;
			}
			Steel.ToolTip = string.Format(FormHeadquarters.ResourceToolTip,
				resday == null ? 0 : (db.Material.Steel - resday.Steel),
				resweek == null ? 0 : (db.Material.Steel - resweek.Steel),
				resmonth == null ? 0 : (db.Material.Steel - resmonth.Steel));

			Bauxite.Text = db.Material.Bauxite.ToString();
			if (db.Material.Bauxite >= resourceHardcap)
			{
				Bauxite.ForeColor = configUI.Headquarters_ResourceMaxFG;
				Bauxite.BackColor = configUI.Headquarters_ResourceMaxBG;
			}
			else if (db.Material.Bauxite < (configUI.HqResLowAlertBauxite == -1 ? db.Admiral.MaxResourceRegenerationAmount : configUI.HqResLowAlertBauxite))
			{
				Bauxite.ForeColor = configUI.Headquarters_ResourceLowFG;
				Bauxite.BackColor = configUI.Headquarters_ResourceLowBG;
			}
			else if (db.Material.Bauxite > db.Admiral.MaxResourceRegenerationAmount)
			{
				Bauxite.ForeColor = configUI.Headquarters_ResourceOverFG;
				Bauxite.BackColor = configUI.Headquarters_ResourceOverBG;
			}
			else
			{
				Bauxite.ForeColor = configUI.ForeColor;
				Bauxite.BackColor = System.Drawing.Color.Transparent;
			}
			Bauxite.ToolTip = string.Format(FormHeadquarters.ResourceToolTip,
				resday == null ? 0 : (db.Material.Bauxite - resday.Bauxite),
				resweek == null ? 0 : (db.Material.Bauxite - resweek.Bauxite),
				resmonth == null ? 0 : (db.Material.Bauxite - resmonth.Bauxite));

		}
		// FlowPanelResource.ResumeLayout();

		// FlowPanelMaster.ResumeLayout();
		// if (!FlowPanelMaster.Visible)
		// FlowPanelMaster.Visible = true;
		// AdmiralName.Refresh();
		// AdmiralComment.Refresh();

		Visible = Visibility.Visible;
	}

	void SystemEvents_UpdateTimerTick()
	{
		KCDatabase db = KCDatabase.Instance;

		if (db.Ships.Count <= 0) return;

		if (Utility.Configuration.Config.FormHeadquarters.BlinkAtMaximum)
		{
			if (ShipCount.Tag as bool? ?? false)
			{
				ShipCount.BackColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG : System.Drawing.Color.Transparent;
				ShipCount.ForeColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG : Utility.Configuration.Config.UI.ForeColor;
			}

			if (EquipmentCount.Tag as bool? ?? false)
			{
				EquipmentCount.BackColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Headquarters_ShipCountOverBG : System.Drawing.Color.Transparent;
				EquipmentCount.ForeColor = DateTime.Now.Second % 2 == 0 ? Utility.Configuration.Config.UI.Headquarters_ShipCountOverFG : Utility.Configuration.Config.UI.ForeColor;
			}
		}
	}

	[ICommand]
	private void ShowResourceChart()
	{
		new Dialog.DialogResourceChart().Show(App.Current.MainWindow);
	}

	[ICommand]
	private void CopyResources()
	{
		try
		{
			var mat = KCDatabase.Instance.Material;
			Clipboard.SetDataObject
			(
				$"{mat.Fuel}/{mat.Ammo}/{mat.Steel}/{mat.Bauxite}/" +
				$"{mat.InstantRepair}{FormHeadquarters.CopyToClipboardBuckets}/" +
				$"{mat.DevelopmentMaterial}{FormHeadquarters.CopyToClipboardDevelopmentMaterials}/" +
				$"{mat.InstantConstruction}{FormHeadquarters.CopyToClipboardInstantConstruction}/" +
				$"{mat.ModdingMaterial}{FormHeadquarters.CopyToClipboardImproveMaterial}"
			);
		}
		catch (Exception ex)
		{
			Utility.Logger.Add(3, FormHeadquarters.FailedToCopyToClipboard + ex.Message);
		}
	}


	private void UpdateDisplayUseItem()
	{

		var db = KCDatabase.Instance;
		var itemID = Utility.Configuration.Config.FormHeadquarters.DisplayUseItemID;
		var item = db.UseItems[itemID];
		var itemMaster = db.MasterUseItems[itemID];
		string tail = "\r\n" + FormHeadquarters.DisplayUseItemToolTipHint;



		switch (itemMaster?.Name)
		{
			case null:
				DisplayUseItem.Text = "???";
				DisplayUseItem.ToolTip = string.Format(FormHeadquarters.UnknownItem,
					Utility.Configuration.Config.FormHeadquarters.DisplayUseItemID) + tail;
				break;

			// '18 spring event special mode
			case "お米":
			case "梅干":
			case "海苔":
			case "お茶":
				DisplayUseItem.Text = (item?.Count ?? 0).ToString();
				DisplayUseItem.ToolTip =
					$"{FormHeadquarters.Rice}: {db.UseItems[85]?.Count ?? 0}\r\n" +
					$"{FormHeadquarters.Umeboshi}: {db.UseItems[86]?.Count ?? 0}\r\n" +
					$"{FormHeadquarters.Nori}: {db.UseItems[87]?.Count ?? 0}\r\n" +
					$"{FormHeadquarters.Tea}: {db.UseItems[88]?.Count ?? 0}\r\n" +
					$"{tail}";
				break;

			// '19 autumn event special mode
			case "秋刀魚":
			case "鰯":
				DisplayUseItem.Text = (item?.Count ?? 0).ToString();
				DisplayUseItem.ToolTip =
					$"{FormHeadquarters.Sanma}: {db.UseItems[68]?.Count ?? 0}\r\n" +
					$"{FormHeadquarters.Iwashi}: {db.UseItems[93]?.Count ?? 0}\r\n" +
					$"{tail}";
				break;

			default:
				DisplayUseItem.Text = (item?.Count ?? 0).ToString();
				DisplayUseItem.ToolTip = itemMaster.Name + tail;
				break;
		}
	}

	[ICommand]
	private void ViewUseItems()
	{
		var db = KCDatabase.Instance;
		var sb = new StringBuilder();
		foreach (var item in db.UseItems.Values)
		{
			sb.Append(item.MasterUseItem.Name).Append(" x ").Append(item.Count).AppendLine();
		}

		MessageBox.Show(sb.ToString(), FormHeadquarters.ListOfOwnedItems, MessageBoxButton.OK, MessageBoxImage.Information);
	}

	private int RealShipCount
	{
		get
		{
			if (KCDatabase.Instance.Battle != null)
				return KCDatabase.Instance.Ships.Count + KCDatabase.Instance.Battle.DroppedShipCount;

			return KCDatabase.Instance.Ships.Count;
		}
	}

	private int RealEquipmentCount
	{
		get
		{
			int equipmentCount = KCDatabase.Instance.Equipments.Values
				.Count(e => e.MasterEquipment.UsesSlotSpace());

			if (KCDatabase.Instance.Battle != null)
				return equipmentCount + KCDatabase.Instance.Battle.DroppedEquipmentCount;

			return equipmentCount;
		}
	}

}
