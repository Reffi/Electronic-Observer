﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Services;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Tools.AirDefense;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Wpf.Fleet;

public partial class FleetViewModel : AnchorableViewModel
{
	public FormFleetTranslationViewModel FormFleet { get; }
	private ToolService ToolService { get; }

	public FleetStatusViewModel ControlFleet { get; }
	public List<FleetItemViewModel> ControlMember { get; } = new();

	public int FleetId { get; }
	public int AnchorageRepairBound { get; set; }

	public FleetViewModel(int fleetId) : base($"#{fleetId}", $"Fleet{fleetId}",
		ImageSourceIcons.GetIcon(IconContent.FormFleet))
	{
		FormFleet = Ioc.Default.GetService<FormFleetTranslationViewModel>()!;
		ToolService = Ioc.Default.GetService<ToolService>()!;

		Title = $"#{fleetId}";
		FormFleet.PropertyChanged += (_, _) => Title = $"#{fleetId}";

		FleetId = fleetId;

		Utility.SystemEvents.UpdateTimerTick += UpdateTimerTick;

		AnchorageRepairBound = 0;

		ControlFleet = new(FleetId);
		for (int i = 0; i < 7; i++)
		{
			ControlMember.Add(new(this));
		}

		ConfigurationChanged();

		SubscribeToApis();

		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
	}

	private void SubscribeToApis()
	{
		APIObserver o = APIObserver.Instance;

		o.ApiReqNyukyo_Start.RequestReceived += Updated;
		o.ApiReqNyukyo_SpeedChange.RequestReceived += Updated;
		o.ApiReqHensei_Change.RequestReceived += Updated;
		o.ApiReqKousyou_DestroyShip.RequestReceived += Updated;
		o.ApiReqMember_UpdateDeckName.RequestReceived += Updated;
		o.ApiReqKaisou_Remodeling.RequestReceived += Updated;
		o.ApiReqMap_Start.RequestReceived += Updated;
		o.ApiReqHensei_Combined.RequestReceived += Updated;
		o.ApiReqKaisou_OpenExSlot.RequestReceived += Updated;

		o.ApiPort_Port.ResponseReceived += Updated;
		o.ApiGetMember_Ship2.ResponseReceived += Updated;
		o.ApiGetMember_NDock.ResponseReceived += Updated;
		o.ApiReqKousyou_GetShip.ResponseReceived += Updated;
		o.ApiReqHokyu_Charge.ResponseReceived += Updated;
		o.ApiReqKousyou_DestroyShip.ResponseReceived += Updated;
		o.ApiGetMember_Ship3.ResponseReceived += Updated;
		o.ApiReqKaisou_PowerUp.ResponseReceived += Updated;        //requestのほうは面倒なのでこちらでまとめてやる
		o.ApiGetMember_Deck.ResponseReceived += Updated;
		o.ApiGetMember_SlotItem.ResponseReceived += Updated;
		o.ApiReqMap_Start.ResponseReceived += Updated;
		o.ApiReqMap_Next.ResponseReceived += Updated;
		o.ApiGetMember_ShipDeck.ResponseReceived += Updated;
		o.ApiReqHensei_PresetSelect.ResponseReceived += Updated;
		o.ApiReqKaisou_SlotExchangeIndex.ResponseReceived += Updated;
		o.ApiGetMember_RequireInfo.ResponseReceived += Updated;
		o.ApiReqKaisou_SlotDeprive.ResponseReceived += Updated;
		o.ApiReqKaisou_Marriage.ResponseReceived += Updated;
		o.ApiReqMap_AnchorageRepair.ResponseReceived += Updated;
	}

	private void Updated(string apiname, dynamic data)
	{
		KCDatabase db = KCDatabase.Instance;

		if (db.Ships.Count == 0) return;

		FleetData fleet = db.Fleet.Fleets[FleetId];
		if (fleet == null) return;

		ControlFleet.Update(fleet);

		AnchorageRepairBound = fleet.CanAnchorageRepair ? 2 + fleet.MembersInstance[0].SlotInstance.Count(eq => eq != null && eq.MasterEquipment.CategoryType == EquipmentTypes.RepairFacility) : 0;

		for (int i = 0; i < ControlMember.Count; i++)
		{
			ControlMember[i].Update(i < fleet.Members.Count ? fleet.Members[i] : -1);
		}

		int iconIndex = ControlFleet.State.GetIconIndex();
		IconSource = ImageSourceIcons.GetIcon((IconContent)iconIndex);
	}

	void UpdateTimerTick()
	{

		FleetData fleet = KCDatabase.Instance.Fleet.Fleets[FleetId];

		// TableFleet.SuspendLayout();
		{
			if (fleet != null)
				ControlFleet.Refresh();

		}
		// TableFleet.ResumeLayout();

		// TableMember.SuspendLayout();
		for (int i = 0; i < ControlMember.Count; i++)
		{
			// ControlMember[i].HP.Refresh();
			// this is for updating the repair timer when a ship is docked
			ControlMember[i].HP.ResumeUpdate();
		}
		// TableMember.ResumeLayout();

		// anchorage repairing
		if (fleet != null && Utility.Configuration.Config.FormFleet.ReflectAnchorageRepairHealing)
		{
			TimeSpan elapsed = DateTime.Now - KCDatabase.Instance.Fleet.AnchorageRepairingTimer;

			if (elapsed.TotalMinutes >= 20 && AnchorageRepairBound > 0)
			{

				for (int i = 0; i < AnchorageRepairBound; i++)
				{
					var hpbar = ControlMember[i].HP;

					double dockingSeconds = hpbar.Tag as double? ?? 0.0;

					if (dockingSeconds <= 0.0)
						continue;

					// hpbar.SuspendUpdate();

					if (!hpbar.UsePrevValue)
					{
						hpbar.UsePrevValue = true;
						hpbar.ShowDifference = true;
					}

					int damage = hpbar.HPBar.MaximumValue - hpbar.PrevValue;
					int healAmount = Math.Min(Calculator.CalculateAnchorageRepairHealAmount(damage, dockingSeconds, elapsed), damage);

					hpbar.RepairTimeShowMode = ShipStatusHPRepairTimeShowMode.MouseOver;
					hpbar.RepairTime = KCDatabase.Instance.Fleet.AnchorageRepairingTimer + Calculator.CalculateAnchorageRepairTime(damage, dockingSeconds, Math.Min(healAmount + 1, damage));
					hpbar.AkashiRepairBar.Value = hpbar.PrevValue + healAmount;

					// todo Akashi repair HP bar changes
					hpbar.ResumeUpdate();
				}
			}
		}
	}

	void ConfigurationChanged()
	{
		var c = Utility.Configuration.Config;

		// MainFont = Font = c.UI.MainFont;
		// SubFont = c.UI.SubFont;

		// AutoScroll = c.FormFleet.IsScrollable;

		var fleet = KCDatabase.Instance.Fleet[FleetId];

		// TableFleet.SuspendLayout();
		if (ControlFleet != null && fleet != null)
		{
			ControlFleet.ConfigurationChanged();
			ControlFleet.Update(fleet);
		}
		// TableFleet.ResumeLayout();

		// TableMember.SuspendLayout();
		if (ControlMember != null)
		{
			bool showAircraft = c.FormFleet.ShowAircraft;
			bool fixShipNameWidth = c.FormFleet.FixShipNameWidth;
			bool shortHPBar = c.FormFleet.ShortenHPBar;
			bool colorMorphing = c.UI.BarColorMorphing;
			System.Drawing.Color[] colorScheme = c.UI.BarColorScheme.Select(col => col.ColorData).ToArray();
			bool showNext = c.FormFleet.ShowNextExp;
			bool showConditionIcon = c.FormFleet.ShowConditionIcon;
			var levelVisibility = c.FormFleet.EquipmentLevelVisibility;
			bool showAircraftLevelByNumber = c.FormFleet.ShowAircraftLevelByNumber;
			int fixedShipNameWidth = c.FormFleet.FixedShipNameWidth;
			bool isLayoutFixed = c.UI.IsLayoutFixed;

			for (int i = 0; i < ControlMember.Count; i++)
			{
				var member = ControlMember[i];

				member.Equipments.ShowAircraft = showAircraft;
				if (fixShipNameWidth)
				{
					member.Name.MaxWidth = fixedShipNameWidth;
				}
				else
				{
					member.Name.MaxWidth = int.MaxValue;
				}

				// member.HP.SuspendUpdate();
				member.HP.Text = shortHPBar ? "" : "HP:";
				member.HP.HPBar.ColorMorphing = colorMorphing;
				member.HP.HPBar.SetBarColorScheme(colorScheme);
				// member.HP.MaximumSize = isLayoutFixed ? new Size(int.MaxValue, (int)ControlHelper.GetDefaultRowStyle().Height - member.HP.Margin.Vertical) : Size.Empty;
				// member.HP.ResumeUpdate();

				member.Level.NextVisible = showNext;
				member.Level.TextNext = showNext ? "next:" : null;

				member.Condition.ImageAlign = showConditionIcon ? System.Drawing.ContentAlignment.MiddleLeft : System.Drawing.ContentAlignment.MiddleCenter;
				member.Equipments.LevelVisibility = levelVisibility;
				member.Equipments.ShowAircraftLevelByNumber = showAircraftLevelByNumber;
				// member.Equipments.MaximumSize = isLayoutFixed ? new Size(int.MaxValue, (int)ControlHelper.GetDefaultRowStyle().Height - member.Equipments.Margin.Vertical) : Size.Empty;
				member.ShipResource.BarFuel.ColorMorphing =
					member.ShipResource.BarAmmo.ColorMorphing = colorMorphing;
				member.ShipResource.BarFuel.SetBarColorScheme(colorScheme);
				member.ShipResource.BarAmmo.SetBarColorScheme(colorScheme);

				member.ConfigurationChanged();
				if (fleet != null)
					member.Update(i < fleet.Members.Count ? fleet.Members[i] : -1);
			}
		}

		// ControlHelper.SetTableRowStyles(TableMember, ControlHelper.GetDefaultRowStyle());
		// TableMember.ResumeLayout();

		// TableMember.Location = new Point(TableMember.Location.X, TableFleet.Bottom /*+ Math.Max( TableFleet.Margin.Bottom, TableMember.Margin.Top )*/ );

		// TableMember.PerformLayout();        //fixme:サイズ変更に親パネルが追随しない

	}


	private string GetNameString(IFleetData? fleet)
	{
		if (fleet == null) return "";

		var members = fleet.MembersInstance.Where(s => s != null);

		int levelSum = members.Sum(s => s.Level);

		int fueltotal = members.Sum(s => Math.Max((int)Math.Floor(s.FuelMax * (s.IsMarried ? 0.85 : 1.00)), 1));
		int ammototal = members.Sum(s => Math.Max((int)Math.Floor(s.AmmoMax * (s.IsMarried ? 0.85 : 1.00)), 1));

		int fuelunit = members.Sum(s => Math.Max((int)Math.Floor(s.FuelMax * 0.2 * (s.IsMarried ? 0.85 : 1.00)), 1));
		int ammounit = members.Sum(s => Math.Max((int)Math.Floor(s.AmmoMax * 0.2 * (s.IsMarried ? 0.85 : 1.00)), 1));

		int speed = members.Select(s => s.Speed).DefaultIfEmpty(20).Min();

		string supporttype;
		switch (fleet.SupportType)
		{
			case 0:
			default:
				supporttype = "n/a"; break;
			case 1:
				supporttype = "Aerial Support"; break;
			case 2:
				supporttype = "Support Shelling"; break;
			case 3:
				supporttype = "Long-range Torpedo Attack"; break;
		}

		double expeditionBonus = Calculator.GetExpeditionBonus(fleet);
		int tp = Calculator.GetTPDamage(fleet);

		// 各艦ごとの ドラム缶 or 大発系 を搭載している個数
		var transport = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.TransportContainer));
		var landing = members.Select(s => s.AllSlotInstanceMaster.Count(eq => eq?.CategoryType == EquipmentTypes.LandingCraft || eq?.CategoryType == EquipmentTypes.SpecialAmphibiousTank));


		return string.Format(
			"Lv sum: {0} / avg: {1:0.00}\r\n" +
			"{2} fleet\r\n" +
			"Support Expedition: {3}\r\n" +
			"Total FP {4} / Torp {5} / AA {6} / ASW {7} / LOS {8}\r\n" +
			"Drum: {9} ({10} ships)\r\n" +
			"Daihatsu: {11} ({12} ships, +{13:p1})\r\n" +
			"TP: S {14} / A {15}\r\n" +
			"Consumption: {16} fuel / {17} ammo\r\n" +
			"({18} fuel / {19} ammo per battle)",
			levelSum,
			(double)levelSum / Math.Max(fleet.Members.Count(id => id != -1), 1),
			Constants.GetSpeed(speed),
			supporttype,
			members.Sum(s => s.FirepowerTotal),
			members.Sum(s => s.TorpedoTotal),
			members.Sum(s => s.AATotal),
			members.Sum(s => s.ASWTotal),
			members.Sum(s => s.LOSTotal),
			transport.Sum(),
			transport.Count(i => i > 0),
			landing.Sum(),
			landing.Count(i => i > 0),
			expeditionBonus,
			tp,
			(int)(tp * 0.7),
			fueltotal,
			ammototal,
			fuelunit,
			ammounit
		);
	}

	private string GetFleetAirString(IFleetData? fleet)
	{
		if (fleet == null) return "";

		int airSuperiority = fleet.GetAirSuperiority();
		bool includeLevel = Utility.Configuration.Config.FormFleet.AirSuperiorityMethod == 1;

		return string.Format(GeneralRes.ASTooltip,
			(int)(airSuperiority / 3.0),
			(int)(airSuperiority / 1.5),
			Math.Max((int)(airSuperiority * 1.5 - 1), 0),
			Math.Max((int)(airSuperiority * 3.0 - 1), 0),
			includeLevel ? "w/o Proficiency" : "w/ Proficiency",
			includeLevel ? Calculator.GetAirSuperiorityIgnoreLevel(fleet) : Calculator.GetAirSuperiority(fleet));
	}

	private string GetFleetLosString(IFleetData? fleet, int branchWeight)
	{
		if (fleet == null) return "";

		StringBuilder sb = new StringBuilder();
		double probStart = fleet.GetContactProbability();
		var probSelect = fleet.GetContactSelectionProbability();

		sb.AppendFormat("Formula 33 (n={0})\r\n　(Click to switch between weighting)\r\n\r\nContact:\r\n　AS+ {1:p1} / AS {2:p1}\r\n",
			branchWeight,
			probStart,
			probStart * 0.6);

		if (probSelect.Count > 0)
		{
			sb.AppendLine("Selection:");

			foreach (var p in probSelect.OrderBy(p => p.Key))
			{
				sb.AppendFormat("　Acc+{0}: {1:p1}\r\n", p.Key, p.Value);
			}
		}

		return sb.ToString();
	}

	private string GetFleetAaString(IFleetData? fleet)
	{
		if (fleet == null) return "";

		var sb = new StringBuilder();
		double lineahead = Calculator.GetAdjustedFleetAAValue(fleet, 1);

		sb.AppendFormat(GeneralRes.AntiAirPower,
			lineahead,
			Calculator.GetAdjustedFleetAAValue(fleet, 2),
			Calculator.GetAdjustedFleetAAValue(fleet, 3));

		return sb.ToString();
	}

	#region Commands

	[ICommand]
	private void Copy()
	{

		StringBuilder sb = new StringBuilder();
		KCDatabase db = KCDatabase.Instance;
		FleetData fleet = db.Fleet[FleetId];
		if (fleet == null) return;

		sb.AppendFormat(FormFleet.CopyFleetText + "\r\n", fleet.Name, fleet.GetAirSuperiority(), fleet.GetSearchingAbilityString(ControlFleet.BranchWeight), Calculator.GetTPDamage(fleet));
		for (int i = 0; i < fleet.Members.Count; i++)
		{
			if (fleet[i] == -1)
				continue;

			ShipData ship = db.Ships[fleet[i]];

			sb.AppendFormat("{0}/Lv{1}\t", ship.MasterShip.NameEN, ship.Level);

			var eq = ship.AllSlotInstance;


			if (eq != null)
			{
				for (int j = 0; j < eq.Count; j++)
				{

					if (eq[j] == null) continue;

					int count = 1;
					for (int k = j + 1; k < eq.Count; k++)
					{
						if (eq[k] != null && eq[k].EquipmentID == eq[j].EquipmentID && eq[k].Level == eq[j].Level && eq[k].AircraftLevel == eq[j].AircraftLevel)
						{
							count++;
						}
						else
						{
							break;
						}
					}

					if (count == 1)
					{
						sb.AppendFormat("{0}{1}", j == 0 ? "" : ", ", eq[j].NameWithLevel);
					}
					else
					{
						sb.AppendFormat("{0}{1}x{2}", j == 0 ? "" : ", ", eq[j].NameWithLevel, count);
					}

					j += count - 1;
				}
			}

			sb.AppendLine();
		}


		Clipboard.SetDataObject(sb.ToString());
	}

	/*
	private void ContextMenuFleet_Opening(object sender, CancelEventArgs e)
	{

		ContextMenuFleet_Capture.Visible = Utility.Configuration.Config.Debug.EnableDebugMenu;

	}
	*/

	/// <summary>
	/// 「艦隊デッキビルダー」用編成コピー
	/// <see cref="http://www.kancolle-calc.net/deckbuilder.html"/>
	/// </summary>
	[ICommand]
	private void CopyDeckBuilder()
	{

		StringBuilder sb = new StringBuilder();
		KCDatabase db = KCDatabase.Instance;

		// 手書き json の悲しみ

		sb.Append(@"{""version"":4,");

		foreach (var fleet in db.Fleet.Fleets.Values)
		{
			if (fleet == null || fleet.MembersInstance.All(m => m == null)) continue;

			sb.AppendFormat(@"""f{0}"":{{", fleet.FleetID);

			int shipcount = 1;
			foreach (var ship in fleet.MembersInstance)
			{
				if (ship == null) break;

				sb.AppendFormat(@"""s{0}"":{{""id"":{1},""lv"":{2},""luck"":{3},""items"":{{",
					shipcount,
					ship.ShipID,
					ship.Level,
					ship.LuckBase);

				int eqcount = 1;
				foreach (var eq in ship.AllSlotInstance.Where(eq => eq != null))
				{
					if (eq == null) break;

					sb.AppendFormat(@"""i{0}"":{{""id"":{1},""rf"":{2},""mas"":{3}}},", eqcount >= 6 ? "x" : eqcount.ToString(), eq.EquipmentID, eq.Level, eq.AircraftLevel);

					eqcount++;
				}

				if (eqcount > 1)
					sb.Remove(sb.Length - 1, 1);        // remove ","
				sb.Append(@"}},");

				shipcount++;
			}

			if (shipcount > 0)
				sb.Remove(sb.Length - 1, 1);        // remove ","
			sb.Append(@"},");

		}

		sb.Remove(sb.Length - 1, 1);        // remove ","
		sb.Append(@"}");

		Clipboard.SetDataObject(sb.ToString());
	}

	/// <summary>
	/// 「艦隊晒しページ」用編成コピー
	/// <see cref="http://kancolle-calc.net/kanmusu_list.html"/>
	/// </summary>
	[ICommand]
	private void CopyKanmusuList()
	{

		StringBuilder sb = new StringBuilder();
		KCDatabase db = KCDatabase.Instance;

		// version
		sb.Append(".2");

		// <たね艦娘(完全未改造時)のID, 艦娘リスト>　に分類
		Dictionary<int, List<ShipData>> shiplist = new Dictionary<int, List<ShipData>>();

		foreach (var ship in db.Ships.Values.Where(s => s.IsLocked))
		{
			var master = ship.MasterShip;
			while (master.RemodelBeforeShip != null)
				master = master.RemodelBeforeShip;

			if (!shiplist.ContainsKey(master.ShipID))
			{
				shiplist.Add(master.ShipID, new List<ShipData>() { ship });
			}
			else
			{
				shiplist[master.ShipID].Add(ship);
			}
		}

		// 上で作った分類の各項を文字列化
		foreach (var sl in shiplist)
		{
			sb.Append("|").Append(sl.Key).Append(":");

			foreach (var ship in sl.Value.OrderByDescending(s => s.Level))
			{
				sb.Append(ship.Level);

				// 改造レベルに達しているのに未改造の艦は ".<たね=1, 改=2, 改二=3, ...>" を付加
				if (ship.MasterShip.RemodelAfterShipID != 0 && ship.ExpNextRemodel == 0)
				{
					sb.Append(".");
					int count = 1;
					var master = ship.MasterShip;
					while (master.RemodelBeforeShip != null)
					{
						master = master.RemodelBeforeShip;
						count++;
					}
					sb.Append(count);
				}
				sb.Append(",");
			}

			// 余った "," を削除
			sb.Remove(sb.Length - 1, 1);
		}

		Clipboard.SetDataObject(sb.ToString());
	}

	/// <summary>
	/// 
	/// <see cref="https://kancolle-fleetanalysis.firebaseapp.com"/>
	/// </summary>
	[ICommand]
	private void CopyFleetAnalysis()
	{
		KCDatabase db = KCDatabase.Instance;
		List<string> ships = new List<string>();

		foreach (ShipData ship in db.Ships.Values.Where(s => s.IsLocked))
		{
			int[] apiKyouka =
			{
				ship.FirepowerModernized,
				ship.TorpedoModernized,
				ship.AAModernized,
				ship.ArmorModernized,
				ship.LuckModernized,
				ship.HPMaxModernized,
				ship.ASWModernized
			};

			int expProgress = 0;
			if (ExpTable.ShipExp.ContainsKey(ship.Level + 1) && ship.Level != 99)
			{
				expProgress = (ExpTable.ShipExp[ship.Level].Next - ship.ExpNext)
							  / ExpTable.ShipExp[ship.Level].Next;
			}

			int[] apiExp = { ship.ExpTotal, ship.ExpNext, expProgress };

			string shipId = $"\"api_ship_id\":{ship.ShipID}";
			string level = $"\"api_lv\":{ship.Level}";
			string kyouka = $"\"api_kyouka\":[{string.Join(",", apiKyouka)}]";
			string exp = $"\"api_exp\":[{string.Join(",", apiExp)}]";
			string slotEx = $"\"api_slot_ex\":{ship.ExpansionSlot}";
			string sallyArea = $"\"api_sally_area\":{(ship.SallyArea)}";

			string[] analysisData = { shipId, level, kyouka, exp, slotEx, sallyArea };

			ships.Add($"{{{string.Join(",", analysisData)}}}");
		}

		string json = $"[{string.Join(",", ships)}]";

		Clipboard.SetDataObject(json);
	}

	/// <summary>
	/// <see cref="https://kancolle-fleetanalysis.firebaseapp.com"/>
	/// </summary>
	[ICommand]
	private void CopyFleetAnalysisEquip()
	{
		Clipboard.SetDataObject(GenerateEquipList(false));
	}

	[ICommand]
	private void CopyFleetAnalysisAllEquip()
	{
		Clipboard.SetDataObject(GenerateEquipList(true));
	}

	public static string GenerateEquipList(bool allEquipment)
	{
		StringBuilder sb = new StringBuilder();
		KCDatabase db = KCDatabase.Instance;

		// 手書き json の悲しみ
		// pain and suffering

		sb.Append("[");

		foreach (EquipmentData equip in db.Equipments.Values.Where(eq => allEquipment || eq.IsLocked))
		{
			sb.Append($"{{\"api_slotitem_id\":{equip.EquipmentID},\"api_level\":{equip.Level}}},");
		}

		sb.Remove(sb.Length - 1, 1);        // remove ","
		sb.Append("]");

		return sb.ToString();
	}

	/// <summary>
	/// Short versions are for the fleet analysis spreadsheet
	/// <see cref="https://docs.google.com/spreadsheets/d/1NuLlff6EXM0XQ_qNHP9lEOosbwHXamaVNJb72M7ZLoY"/>
	/// </summary>
	[ICommand]
	private void CopyFleetAnalysisShipsShort()
	{
		KCDatabase db = KCDatabase.Instance;
		List<string> ships = new List<string>();

		foreach (ShipData ship in db.Ships.Values.Where(s => s.IsLocked))
		{
			int[] apiKyouka =
			{
				ship.FirepowerModernized,
				ship.TorpedoModernized,
				ship.AAModernized,
				ship.ArmorModernized,
				ship.LuckModernized,
				ship.HPMaxModernized,
				ship.ASWModernized
			};

			int expProgress = 0;
			if (ExpTable.ShipExp.ContainsKey(ship.Level + 1) && ship.Level != 99)
			{
				expProgress = (ExpTable.ShipExp[ship.Level].Next - ship.ExpNext) / ExpTable.ShipExp[ship.Level].Next;
			}

			int[] apiExp = { ship.ExpTotal, ship.ExpNext, expProgress };

			string shipId = $"\"id\":{ship.ShipID}";
			string level = $"\"lv\":{ship.Level}";
			string kyouka = $"\"st\":[{string.Join(",", apiKyouka)}]";
			string exp = $"\"exp\":[{string.Join(",", apiExp)}]";

			string[] analysisData = { shipId, level, kyouka, exp };

			ships.Add($"{{{string.Join(",", analysisData)}}}");
		}

		string json = $"[{string.Join(",", ships)}]";

		Clipboard.SetDataObject(json);
	}

	[ICommand]
	private void CopyFleetAnalysisLockedEquipShort()
	{
		GenerateEquipListShort(false);
	}

	[ICommand]
	private void CopyFleetAnalysisAllEquipShort()
	{
		GenerateEquipListShort(true);
	}

	/// <summary>
	/// <see cref="https://docs.google.com/spreadsheets/d/1ppbOl9MR_8g_CPDpgMVDdRnhEMRTjg4x78bCg8uLzdg"/>
	/// </summary>
	/// <param name="allEquipment"></param>
	private void GenerateEquipListShort(bool allEquipment)
	{
		StringBuilder sb = new StringBuilder();
		KCDatabase db = KCDatabase.Instance;

		// 手書き json の悲しみ
		// pain and suffering

		sb.Append("[");

		foreach (EquipmentData equip in db.Equipments.Values.Where(eq => allEquipment || eq.IsLocked))
		{
			sb.Append($"{{\"id\":{equip.EquipmentID},\"lv\":{equip.Level}}},");
		}

		sb.Remove(sb.Length - 1, 1);        // remove ","
		sb.Append("]");

		Clipboard.SetDataObject(sb.ToString());
	}

	[ICommand]
	private void AntiAirDetails()
	{
		AirDefenseViewModel viewModel = new()
		{
			SelectedFleet = (KCDatabase.Instance.Fleet.CombinedFlag != 0 && FleetId is 1 or 2) switch
			{
				true => Tools.AirDefense.FleetId.CombinedFleet,
				_ => (FleetId)FleetId
			}
		};

		new AirDefenseWindow(viewModel).Show(App.Current.MainWindow);
	}

	/*
	private void ContextMenuFleet_Capture_Click(object sender, EventArgs e)
	{

		using (Bitmap bitmap = new Bitmap(this.ClientSize.Width, this.ClientSize.Height))
		{
			this.DrawToBitmap(bitmap, this.ClientRectangle);

			Clipboard.SetData(DataFormats.Bitmap, bitmap);
		}
	}
	*/

	[ICommand]
	private void OutputFleetImage()
	{
		using (var dialog = new DialogFleetImageGenerator(FleetId))
		{
			dialog.ShowDialog(App.Current.MainWindow);
		}
	}

	[ICommand]
	private void OpenAirControlSimulator()
	{
		ToolService.AirControlSimulator(new()
		{
			Fleet1 = FleetId is 1,
			Fleet2 = FleetId is 2 || (FleetId is 1 && KCDatabase.Instance.Fleet.CombinedFlag > 0),
			Fleet3 = FleetId is 3,
			Fleet4 = FleetId is 4,
		});
	}
	#endregion
}
