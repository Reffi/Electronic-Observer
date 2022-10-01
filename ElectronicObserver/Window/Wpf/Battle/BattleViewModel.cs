﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Data.Battle.Detail;
using ElectronicObserver.Data.Battle.Phase;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Control;
using ElectronicObserver.Window.Dialog.BattleDetail;
using ElectronicObserver.Window.Wpf.Battle.ViewModels;
using ElectronicObserverTypes;
using ElectronicObserverTypes.AntiAir;
using Color = System.Drawing.Color;

namespace ElectronicObserver.Window.Wpf.Battle;

public partial class BattleViewModel : AnchorableViewModel
{
	public FormBattleTranslationViewModel FormBattle { get; }

	private SolidColorBrush WinRankColor_Win { get; } = Utility.Configuration.Config.UI.ForeColor.ToBrush();
	private SolidColorBrush WinRankColor_Lose { get; } = Utility.Configuration.Config.UI.Color_Red.ToBrush();

	public bool ViewVisible { get; set; }

	#region Row 0

	public string? FormationFriendText { get; set; }

	public string? FormationText { get; set; }
	public SolidColorBrush? FormationForeColor { get; set; }

	public string? FormationEnemyText { get; set; }

	#endregion

	#region Row 1

	public string? SearchingFriendText { get; set; }
	public ImageSource? SearchingFriendIcon { get; set; }
	public string? SearchingFriendToolTip { get; set; }

	public string? SearchingText { get; set; }
	public ImageSource? SearchingIcon { get; set; }
	public string? SearchingToolTip { get; set; }

	public string? SearchingEnemyText { get; set; }
	public ImageSource? SearchingEnemyIcon { get; set; }
	public string? SearchingEnemyToolTip { get; set; }

	#endregion

	#region Row 2

	public string? AirStage1FriendText { get; set; }
	public string? AirStage1FriendToolTip { get; set; }
	public SolidColorBrush? AirStage1FriendForeColor { get; set; }
	public ImageSource? AirStage1FriendIcon { get; set; }

	public string? AirSuperiorityText { get; set; }
	public SolidColorBrush? AirSuperiorityForeColor { get; set; }
	public string? AirSuperiorityToolTip { get; set; }

	public string? AirStage1EnemyText { get; set; }
	public string? AirStage1EnemyToolTip { get; set; }
	public SolidColorBrush? AirStage1EnemyForeColor { get; set; }
	public ImageSource? AirStage1EnemyIcon { get; set; }

	#endregion

	#region Row 3

	public string? AirStage2FriendText { get; set; }
	public SolidColorBrush? AirStage2FriendForeColor { get; set; }
	public ImageSource? AirStage2FriendIcon { get; set; }
	public string? AirStage2FriendToolTip { get; set; }

	public string? AACutinText { get; set; }
	public ImageSource? AACutinIcon { get; set; }
	public string? AACutinToolTip { get; set; }

	public string? AirStage2EnemyText { get; set; }
	public SolidColorBrush? AirStage2EnemyForeColor { get; set; }
	public ImageSource? AirStage2EnemyIcon { get; set; }
	public string? AirStage2EnemyToolTip { get; set; }

	#endregion

	#region Row 4

	public string? FleetFriendText { get; set; }
	public string? FleetFriendToolTip { get; set; }
	public ImageSource? FleetFriendIcon { get; set; }

	public bool PlayerFleetVisible { get; set; }

	private bool IsPlayerCombinedFleet { get; set; }
	public bool FleetFriendEscortVisible => IsPlayerCombinedFleet && PlayerFleetVisible;

	public SolidColorBrush? FleetEnemyEscortForeColor { get; set; }
	public SolidColorBrush? FleetEnemyEscortBackColor { get; set; }
	private bool IsEnemyCombinedFleet { get; set; }
	public bool FleetEnemyEscortVisible => IsEnemyCombinedFleet && ViewVisible;

	public SolidColorBrush? FleetEnemyForeColor { get; set; }
	public SolidColorBrush? FleetEnemyBackColor { get; set; }

	#endregion

	#region Row 6

	public string? DamageFriendText { get; set; }

	public string? WinRankText { get; set; }
	public SolidColorBrush? WinRankForeColor { get; set; }

	public string? DamageEnemyText { get; set; }


	#endregion

	public List<HealthBarViewModel> HPBars { get; } = new();
	public ObservableCollection<HealthBarViewModel> PlayerMainHPBars { get; } = new();
	public ObservableCollection<HealthBarViewModel> PlayerEscortHPBars { get; } = new();
	public List<HealthBarViewModel> EnemyMainHPBars { get; } = new();
	public List<HealthBarViewModel> EnemyEscortHPBars { get; } = new();

	public bool CompactMode { get; set; }

	public BattleViewModel() : base("Battle", "Battle",
		ImageSourceIcons.GetIcon(IconContent.FormBattle))
	{
		FormBattle = Ioc.Default.GetService<FormBattleTranslationViewModel>()!;

		Title = FormBattle.Title;
		FormBattle.PropertyChanged += (_, _) => Title = FormBattle.Title;

		for (int i = 0; i < 24; i++)
		{
			HealthBarViewModel vm = new();
			HPBars.Add(vm);
			// HPBars.Add(new ShipStatusHP());
			// HPBars[i].Size = DefaultBarSize;
			// HPBars[i].AutoSize = false;
			// HPBars[i].AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			// HPBars[i].Margin = new Padding(2, 0, 2, 0);
			// HPBars[i].Anchor = AnchorStyles.Left | AnchorStyles.Right;
			// HPBars[i].MainFont = MainFont;
			// HPBars[i].SubFont = SubFont;
			// HPBars[i].UsePrevValue = true;
			// HPBars[i].ShowDifference = true;
			// HPBars[i].MaximumDigit = 9999;

			if (i < 6)
			{
				// TableBottom.Controls.Add(HPBars[i], 0, i + 1);
				PlayerMainHPBars.Add(vm);
			}
			else if (i < 12)
			{
				// TableBottom.Controls.Add(HPBars[i], 1, i - 5);
				PlayerEscortHPBars.Add(vm);
			}
			else if (i < 18)
			{
				// TableBottom.Controls.Add(HPBars[i], 3, i - 11);
				EnemyMainHPBars.Add(vm);
			}
			else
			{
				// TableBottom.Controls.Add(HPBars[i], 2, i - 17);
				EnemyEscortHPBars.Add(vm);
			}
		}

		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += Updated;
		o.ApiReqMap_Start.ResponseReceived += Updated;
		o.ApiReqMap_Next.ResponseReceived += Updated;
		o.ApiReqMap_AirRaid.ResponseReceived += Updated;
		o.ApiReqSortie_Battle.ResponseReceived += Updated;
		o.ApiReqSortie_BattleResult.ResponseReceived += Updated;
		o.ApiReqBattleMidnight_Battle.ResponseReceived += Updated;
		o.ApiReqBattleMidnight_SpMidnight.ResponseReceived += Updated;
		o.ApiReqSortie_AirBattle.ResponseReceived += Updated;
		o.ApiReqSortie_LdAirBattle.ResponseReceived += Updated;
		o.ApiReqSortie_NightToDay.ResponseReceived += Updated;
		o.ApiReqSortie_LdShooting.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_Battle.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_MidnightBattle.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_SpMidnight.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_AirBattle.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_BattleWater.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_LdAirBattle.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_EcBattle.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_EcMidnightBattle.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_EcNightToDay.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_EachBattle.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_EachBattleWater.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_LdShooting.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_BattleResult.ResponseReceived += Updated;
		o.ApiReqPractice_Battle.ResponseReceived += Updated;
		o.ApiReqPractice_MidnightBattle.ResponseReceived += Updated;
		o.ApiReqPractice_BattleResult.ResponseReceived += Updated;

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(CompactMode)) return;

			Utility.Configuration.Config.FormBattle.CompactMode = CompactMode;

			foreach (HealthBarViewModel hpBar in HPBars)
			{
				hpBar.CompactMode = CompactMode;
			}
		};

		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		ConfigurationChanged();
	}

	[ICommand]
	private void ShowBattleDetail()
	{
		var bm = KCDatabase.Instance.Battle;

		if (bm == null || bm.BattleMode == BattleManager.BattleModes.Undefined) return;

		var dialog = new BattleDetailView(new BattleDetailViewModel()
		{
			BattleDetailText = BattleDetailDescriptor.GetBattleDetail(bm)
		});

		dialog.Show(App.Current.MainWindow);
	}

	[ICommand]
	private void ShowBattleResult()
	{
		ViewVisible = true;
		PlayerFleetVisible = true;
	}

	private void Updated(string apiname, dynamic data)
	{
		KCDatabase db = KCDatabase.Instance;
		BattleManager bm = db.Battle;
		bool hideDuringBattle = Utility.Configuration.Config.FormBattle.HideDuringBattle;

		/*
		BaseLayoutPanel.SuspendLayout();
		TableTop.SuspendLayout();
		TableBottom.SuspendLayout();
		*/

		switch (apiname)
		{

			case "api_port/port":
				// BaseLayoutPanel.Visible = false;
				// ToolTipInfo.RemoveAll();
				ViewVisible = false;
				PlayerFleetVisible = false;
				break;

			case "api_req_map/start":
			case "api_req_map/next":
				if (!bm.Compass.HasAirRaid)
				{
					ViewVisible = false;
					break;
				}

				SetFormation(bm);
				ClearSearchingResult();
				ClearBaseAirAttack();
				SetAerialWarfare(null, ((BattleBaseAirRaid)bm.BattleDay).BaseAirRaid);
				SetHPBar(bm.BattleDay);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = true;
				break;

			case "api_req_map/air_raid":
				if (!bm.HeavyBaseAirRaids.Any())
				{
					ViewVisible = false;
					break;
				}

				SetFormation(bm);
				ClearSearchingResult();
				ClearBaseAirAttack();
				SetAerialWarfare(null, bm.HeavyBaseAirRaids.Last().BaseAirRaid);
				SetHPBar(bm.HeavyBaseAirRaids.Last());
				SetDamageRate(bm);

				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = true;
				break;

			case "api_req_sortie/battle":
			case "api_req_practice/battle":
			case "api_req_sortie/ld_airbattle":
			case "api_req_sortie/ld_shooting":
			{

				SetFormation(bm);
				SetSearchingResult(bm.BattleDay);
				SetBaseAirAttack(bm.BattleDay.BaseAirAttack);
				SetAerialWarfare(bm.BattleDay.JetAirBattle, bm.BattleDay.AirBattle);
				SetHPBar(bm.BattleDay);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_battle_midnight/battle":
			case "api_req_practice/midnight_battle":
			{

				SetNightBattleEvent(bm.BattleNight.NightInitial);
				SetHPBar(bm.BattleNight);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_battle_midnight/sp_midnight":
			{

				SetFormation(bm);
				ClearBaseAirAttack();
				ClearAerialWarfare();
				ClearSearchingResult();
				SetNightBattleEvent(bm.BattleNight.NightInitial);
				SetHPBar(bm.BattleNight);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_sortie/airbattle":
			{

				SetFormation(bm);
				SetSearchingResult(bm.BattleDay);
				SetBaseAirAttack(bm.BattleDay.BaseAirAttack);
				SetAerialWarfare(bm.BattleDay.JetAirBattle, bm.BattleDay.AirBattle, ((BattleAirBattle)bm.BattleDay).AirBattle2);
				SetHPBar(bm.BattleDay);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_sortie/night_to_day":
			{
				// 暫定
				var battle = bm.BattleNight as BattleDayFromNight;

				SetFormation(bm);
				ClearAerialWarfare();
				ClearSearchingResult();
				ClearBaseAirAttack();
				SetNightBattleEvent(battle.NightInitial);

				if (battle.NextToDay)
				{
					SetSearchingResult(battle);
					SetBaseAirAttack(battle.BaseAirAttack);
					SetAerialWarfare(battle.JetAirBattle, battle.AirBattle);
				}

				SetHPBar(bm.BattleDay);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_combined_battle/battle":
			case "api_req_combined_battle/battle_water":
			case "api_req_combined_battle/ld_airbattle":
			case "api_req_combined_battle/ec_battle":
			case "api_req_combined_battle/each_battle":
			case "api_req_combined_battle/each_battle_water":
			case "api_req_combined_battle/ld_shooting":
			{

				SetFormation(bm);
				SetSearchingResult(bm.BattleDay);
				SetBaseAirAttack(bm.BattleDay.BaseAirAttack);
				SetAerialWarfare(bm.BattleDay.JetAirBattle, bm.BattleDay.AirBattle);
				SetHPBar(bm.BattleDay);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_combined_battle/airbattle":
			{

				SetFormation(bm);
				SetSearchingResult(bm.BattleDay);
				SetBaseAirAttack(bm.BattleDay.BaseAirAttack);
				SetAerialWarfare(bm.BattleDay.JetAirBattle, bm.BattleDay.AirBattle, ((BattleCombinedAirBattle)bm.BattleDay).AirBattle2);
				SetHPBar(bm.BattleDay);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_combined_battle/midnight_battle":
			case "api_req_combined_battle/ec_midnight_battle":
			{

				SetNightBattleEvent(bm.BattleNight.NightInitial);
				SetHPBar(bm.BattleNight);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_combined_battle/sp_midnight":
			{

				SetFormation(bm);
				ClearAerialWarfare();
				ClearSearchingResult();
				ClearBaseAirAttack();
				SetNightBattleEvent(bm.BattleNight.NightInitial);
				SetHPBar(bm.BattleNight);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_combined_battle/ec_night_to_day":
			{
				var battle = bm.BattleNight as BattleDayFromNight;

				SetFormation(bm);
				ClearAerialWarfare();
				ClearSearchingResult();
				ClearBaseAirAttack();
				SetNightBattleEvent(battle.NightInitial);

				if (battle.NextToDay)
				{
					SetSearchingResult(battle);
					SetBaseAirAttack(battle.BaseAirAttack);
					SetAerialWarfare(battle.JetAirBattle, battle.AirBattle);
				}

				SetHPBar(battle);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_sortie/battleresult":
			case "api_req_combined_battle/battleresult":
			case "api_req_practice/battle_result":
			{

				SetMVPShip(bm);

				// BaseLayoutPanel.Visible = true;
				ViewVisible = true;
				PlayerFleetVisible = true;
			}
			break;

		}
		/*
		TableTop.ResumeLayout();
		TableBottom.ResumeLayout();

		BaseLayoutPanel.ResumeLayout();


		if (Utility.Configuration.Config.UI.IsLayoutFixed)
			TableTop.Width = TableTop.GetPreferredSize(BaseLayoutPanel.Size).Width;
		else
			TableTop.Width = TableBottom.ClientSize.Width;
		TableTop.Height = TableTop.GetPreferredSize(BaseLayoutPanel.Size).Height;
		*/
	}

	/// <summary>
	/// 陣形・交戦形態を設定します。
	/// </summary>
	private void SetFormation(BattleManager bm)
	{
		FormationFriendText = Constants.GetFormationShort(bm.FirstBattle.Searching.FormationFriend);
		FormationEnemyText = Constants.GetFormationShort(bm.FirstBattle.Searching.FormationEnemy);
		FormationText = Constants.GetEngagementForm(bm.FirstBattle.Searching.EngagementForm);



		if (bm.IsEnemyCombined && bm.StartsFromDayBattle)
		{
			// highlights for the fleet you'll fight in night battle
			// todo: this should probably go to config
			Color highlightForeColor = Color.Black;
			Color highlightBackColor = Color.LightSteelBlue;

			bool willMain = bm.WillNightBattleWithMainFleet();

			FleetEnemyForeColor = bm.Compass?.EventID switch
			{
				5 => Utility.Configuration.Config.UI.Color_Red.ToBrush(),
				_ when willMain => highlightForeColor.ToBrush(),
				_ => Utility.Configuration.Config.UI.ForeColor.ToBrush()
			};

			FleetEnemyBackColor = willMain switch
			{
				true => highlightBackColor.ToBrush(),
				_ => Color.Transparent.ToBrush()
			};

			FleetEnemyEscortForeColor = willMain switch
			{
				true => Utility.Configuration.Config.UI.ForeColor.ToBrush(),
				_ => highlightForeColor.ToBrush()
			};

			FleetEnemyEscortBackColor = willMain switch
			{
				true => Color.Transparent.ToBrush(),
				_ => highlightBackColor.ToBrush()
			};
		}
		else
		{
			FleetEnemyForeColor = bm.Compass?.EventID switch
			{
				5 => Utility.Configuration.Config.UI.Color_Red.ToBrush(),
				_ => Utility.Configuration.Config.UI.ForeColor.ToBrush()
			};
			FleetEnemyEscortForeColor = Utility.Configuration.Config.UI.ForeColor.ToBrush();
			FleetEnemyBackColor = FleetEnemyEscortBackColor = Color.Transparent.ToBrush();
		}

		FormationForeColor = (bm.FirstBattle.Searching.EngagementForm switch
		{
			3 => Utility.Configuration.Config.UI.Color_Green,
			4 => Utility.Configuration.Config.UI.Color_Red,
			_ => Utility.Configuration.Config.UI.ForeColor
		}).ToBrush();
	}

	/// <summary>
	/// 索敵結果を設定します。
	/// </summary>
	private void SetSearchingResult(BattleData bd)
	{
		/*
		void SetResult(ImageLabel label, int search)
		{
			label.Text = Constants.GetSearchingResultShort(search);
			label.ImageAlign = search > 0 ? ContentAlignment.MiddleLeft : ContentAlignment.MiddleCenter;
			label.ImageIndex = search > 0 ? (int)(search < 4 ? ResourceManager.EquipmentContent.Seaplane : ResourceManager.EquipmentContent.Radar) : -1;
			ToolTipInfo.SetToolTip(label, null);
		}

		SetResult(SearchingFriend, bd.Searching.SearchingFriend);
		SetResult(SearchingEnemy, bd.Searching.SearchingEnemy);
		*/

		int search = bd.Searching.SearchingFriend;

		SearchingFriendText = Constants.GetSearchingResultShort(search);
		SearchingFriendIcon = search switch
		{
			<= 0 => null,
			< 4 => ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.Seaplane),
			_ => ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.Radar)
		};
		SearchingFriendToolTip = null;

		search = bd.Searching.SearchingEnemy;

		SearchingEnemyText = Constants.GetSearchingResultShort(search);
		SearchingEnemyIcon = search switch
		{
			<= 0 => null,
			< 4 => ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.Seaplane),
			_ => ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.Radar)
		};
		SearchingEnemyToolTip = null;
	}

	/// <summary>
	/// 索敵結果をクリアします。
	/// 索敵フェーズが発生しなかった場合にこれを設定します。
	/// </summary>
	private void ClearSearchingResult()
	{
		/*
		void ClearResult(ImageLabel label)
		{
			label.Text = "-";
			label.ImageAlign = ContentAlignment.MiddleCenter;
			label.ImageIndex = -1;
			ToolTipInfo.SetToolTip(label, null);
		}

		ClearResult(SearchingFriend);
		ClearResult(SearchingEnemy);
		*/

		SearchingFriendText = "-";
		SearchingFriendIcon = null;
		SearchingFriendToolTip = null;

		SearchingEnemyText = "-";
		SearchingEnemyIcon = null;
		SearchingEnemyToolTip = null;
	}

	/// <summary>
	/// 基地航空隊フェーズの結果を設定します。
	/// </summary>
	private void SetBaseAirAttack(PhaseBaseAirAttack pd)
	{
		if (pd?.IsAvailable == true)
		{

			SearchingText = FormBattle.ABText;
			// Searching.ImageAlign = ContentAlignment.MiddleLeft;
			// Searching.ImageIndex = (int)ResourceManager.EquipmentContent.LandAttacker;
			SearchingIcon = ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.LandBasedAttacker);

			var sb = new StringBuilder();
			int index = 1;

			foreach (var phase in pd.AirAttackUnits)
			{

				sb.AppendFormat(GeneralRes.BaseWave + " - " + GeneralRes.BaseAirCorps + " :\r\n",
					index, phase.AirUnitID);

				if (phase.IsStage1Available)
				{
					sb.AppendFormat("　St1: " + GeneralRes.FriendlyAir + " -{0}/{1} | " + GeneralRes.EnemyAir + " -{2}/{3} | {4}\r\n",
						phase.AircraftLostStage1Friend, phase.AircraftTotalStage1Friend,
						phase.AircraftLostStage1Enemy, phase.AircraftTotalStage1Enemy,
						Constants.GetAirSuperiority(phase.AirSuperiority));
				}
				if (phase.IsStage2Available)
				{
					sb.AppendFormat("　St2: " + GeneralRes.FriendlyAir + " -{0}/{1} | " + GeneralRes.EnemyAir + " -{2}/{3}\r\n",
						phase.AircraftLostStage2Friend, phase.AircraftTotalStage2Friend,
						phase.AircraftLostStage2Enemy, phase.AircraftTotalStage2Enemy);
				}

				index++;
			}

			SearchingToolTip = sb.ToString();
		}
		else
		{
			ClearBaseAirAttack();
		}
	}

	/// <summary>
	/// 基地航空隊フェーズの結果をクリアします。
	/// </summary>
	private void ClearBaseAirAttack()
	{
		/*
		Searching.Text = GeneralRes.ClearBaseAirAttack;
		Searching.ImageAlign = ContentAlignment.MiddleCenter;
		Searching.ImageIndex = -1;
		ToolTipInfo.SetToolTip(Searching, null);
		*/

		SearchingText = GeneralRes.ClearBaseAirAttack;
		SearchingIcon = null;
		SearchingToolTip = null;
	}


	/// <summary>
	/// 航空戦表示用ヘルパー
	/// </summary>
	private class AerialWarfareFormatter
	{
		public readonly PhaseAirBattleBase Air;
		public string PhaseName;

		public AerialWarfareFormatter(PhaseAirBattleBase air, string phaseName)
		{
			Air = air;
			PhaseName = phaseName;
		}

		public bool Enabled => Air != null && Air.IsAvailable;
		public bool Stage1Enabled => Enabled && Air.IsStage1Available;
		public bool Stage2Enabled => Enabled && Air.IsStage2Available;

		public bool GetEnabled(int stage)
		{
			if (stage == 1)
				return Stage1Enabled;
			else if (stage == 2)
				return Stage2Enabled;
			else
				throw new ArgumentOutOfRangeException();
		}

		public int GetAircraftLost(int stage, bool isFriend)
		{
			if (stage == 1)
				return isFriend ? Air.AircraftLostStage1Friend : Air.AircraftLostStage1Enemy;
			else if (stage == 2)
				return isFriend ? Air.AircraftLostStage2Friend : Air.AircraftLostStage2Enemy;
			else
				throw new ArgumentOutOfRangeException();
		}

		public int GetAircraftTotal(int stage, bool isFriend)
		{
			if (stage == 1)
				return isFriend ? Air.AircraftTotalStage1Friend : Air.AircraftTotalStage1Enemy;
			else if (stage == 2)
				return isFriend ? Air.AircraftTotalStage2Friend : Air.AircraftTotalStage2Enemy;
			else
				throw new ArgumentOutOfRangeException();
		}

		public int GetTouchAircraft(bool isFriend) => isFriend ? Air.TouchAircraftFriend : Air.TouchAircraftEnemy;

	}

	void ClearAircraftLabel(ImageLabel label)
	{
		/*
		label.Text = "-";
		label.ForeColor = SystemColors.ControlText;
		label.ImageAlign = ContentAlignment.MiddleCenter;
		label.ImageIndex = -1;
		ToolTipInfo.SetToolTip(label, null);
		*/
	}



	private void SetAerialWarfare(PhaseAirBattleBase phaseJet, PhaseAirBattleBase phase1) => SetAerialWarfare(phaseJet, phase1, null);

	/// <summary>
	/// 航空戦情報を設定します。
	/// </summary>
	/// <param name="phaseJet">噴式航空戦のデータ。発生していなければ null</param>
	/// <param name="phase1">第一次航空戦（通常航空戦）のデータ。</param>
	/// <param name="phase2">第二次航空戦のデータ。発生していなければ null</param>
	private void SetAerialWarfare(PhaseAirBattleBase phaseJet, PhaseAirBattleBase phase1, PhaseAirBattleBase phase2)
	{
		var phases = new[]
		{
			new AerialWarfareFormatter(phaseJet, FormBattle.AerialPhaseJet),
			new AerialWarfareFormatter(phase1, FormBattle.AerialPhase1),
			new AerialWarfareFormatter(phase2, FormBattle.AerialPhase2),
		};

		if (!phases[0].Enabled && !phases[2].Enabled)
			phases[1].PhaseName = "";

		(string?, string?, SolidColorBrush?, ImageSource?) SetShootdown(int stage, bool isFriend, bool needAppendInfo)
		{
			string? labelText;
			string? labelToolTip;
			SolidColorBrush? labelForeColor;
			ImageSource? labelIcon;

			var phasesEnabled = phases.Where(p => p.GetEnabled(stage));

			if (needAppendInfo)
			{
				labelText = string.Join(",", phasesEnabled.Select(p => "-" + p.GetAircraftLost(stage, isFriend)));
				labelToolTip = string.Join("", phasesEnabled.Select(p => $"{p.PhaseName}-{p.GetAircraftLost(stage, isFriend)}/{p.GetAircraftTotal(stage, isFriend)}\r\n"));
			}
			else
			{
				labelText = $"-{phases[1].GetAircraftLost(stage, isFriend)}/{phases[1].GetAircraftTotal(stage, isFriend)}";
				labelToolTip = null;
			}

			if (phasesEnabled.Any(p =>
				p.GetAircraftTotal(stage, isFriend) > 0 &&
				p.GetAircraftLost(stage, isFriend) == p.GetAircraftTotal(stage, isFriend)))
				labelForeColor = Utility.Configuration.Config.UI.Color_Red.ToBrush();
			else
				labelForeColor = Utility.Configuration.Config.UI.ForeColor.ToBrush();

			// label.ImageAlign = ContentAlignment.MiddleCenter;
			// label.ImageIndex = -1;
			labelIcon = null;

			return (labelText, labelToolTip, labelForeColor, labelIcon);
		}

		void ClearAACutinLabel()
		{
			AACutinText = FormBattle.AirDefense;
			AACutinIcon = null;
			AACutinToolTip = null;
		}

		if (phases[1].Stage1Enabled)
		{
			bool needAppendInfo = phases[0].Stage1Enabled || phases[2].Stage1Enabled;
			var phases1 = phases.Where(p => p.Stage1Enabled);

			AirSuperiorityText = Constants.GetAirSuperiority(phases[1].Air.AirSuperiority);
			AirSuperiorityForeColor = (phases[1].Air.AirSuperiority switch
			{
				// AS+ or AS
				1 or 2 => Utility.Configuration.Config.UI.Color_Green,
				// AI or AI-
				3 or 4 => Utility.Configuration.Config.UI.Color_Red,

				_ => Utility.Configuration.Config.UI.ForeColor
			}).ToBrush();

			AirSuperiorityToolTip = needAppendInfo ?
				string.Join("", phases1.Select(p => $"{p.PhaseName}{Constants.GetAirSuperiority(p.Air.AirSuperiority)}\r\n"))
				: null;

			// SetShootdown(AirStage1Friend, 1, true, needAppendInfo);
			// SetShootdown(AirStage1Enemy, 1, false, needAppendInfo);


			(AirStage1FriendText, AirStage1FriendToolTip, AirStage1FriendForeColor, AirStage1FriendIcon) =
				SetShootdown(1, true, needAppendInfo);

			(AirStage1EnemyText, AirStage1EnemyToolTip, AirStage1EnemyForeColor, AirStage1EnemyIcon) =
				SetShootdown(1, false, needAppendInfo);

			(string?, ImageSource?) SetTouch(bool isFriend, string? currentToolTip)
			{
				string? toolTip = currentToolTip;
				ImageSource? icon;

				if (phases1.Any(p => p.GetTouchAircraft(isFriend) > 0))
				{
					// label.ImageAlign = ContentAlignment.MiddleLeft;
					// label.ImageIndex = (int)ResourceManager.EquipmentContent.Seaplane;

					icon = ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.Seaplane);
					toolTip += FormBattle.Contact + "\r\n" +
							   string.Join("\r\n", phases1.Select(p => $"{p.PhaseName}{(KCDatabase.Instance.MasterEquipments[p.GetTouchAircraft(isFriend)]?.NameEN ?? FormBattle.None)}"));
				}
				else
				{
					// label.ImageAlign = ContentAlignment.MiddleCenter;
					// label.ImageIndex = -1;
					icon = null;
				}

				return (toolTip, icon);
			}

			// SetTouch(AirStage1Friend, true);
			// SetTouch(AirStage1Enemy, false);

			(AirStage1FriendToolTip, AirStage1FriendIcon) = SetTouch(true, AirStage1FriendToolTip);
			(AirStage1EnemyToolTip, AirStage1EnemyIcon) = SetTouch(false, AirStage1EnemyToolTip);
		}
		else
		{

			AirSuperiorityText = Constants.GetAirSuperiority(-1);
			AirSuperiorityToolTip = null;

			// ClearAircraftLabel(AirStage1Friend);
			// ClearAircraftLabel(AirStage1Enemy);

			AirStage1FriendText = "-";
			AirStage1FriendForeColor = Utility.Configuration.Config.UI.ForeColor.ToBrush();
			AirStage1FriendIcon = null;
			AirStage1FriendToolTip = null;

			AirStage1EnemyText = "-";
			AirStage1EnemyForeColor = Utility.Configuration.Config.UI.ForeColor.ToBrush();
			AirStage1EnemyIcon = null;
			AirStage1EnemyToolTip = null;
		}

		if (phases[1].Stage2Enabled)
		{
			bool needAppendInfo = phases[0].Stage2Enabled || phases[2].Stage2Enabled;
			var phases2 = phases.Where(p => p.Stage2Enabled);


			// SetShootdown(AirStage2Friend, 2, true, needAppendInfo);
			// SetShootdown(AirStage2Enemy, 2, false, needAppendInfo);

			(AirStage2FriendText, AirStage2FriendToolTip, AirStage2FriendForeColor, AirStage2FriendIcon) =
				SetShootdown(2, true, needAppendInfo);

			(AirStage2EnemyText, AirStage2EnemyToolTip, AirStage2EnemyForeColor, AirStage2EnemyIcon) =
				SetShootdown(2, false, needAppendInfo);

			if (phases2.Any(p => p.Air.IsAACutinAvailable))
			{
				AACutinText = "#" + string.Join("/", phases2.Select(p => p.Air.IsAACutinAvailable ? (p.Air.AACutInIndex + 1).ToString() : "-"));
				// AACutin.ImageAlign = ContentAlignment.MiddleLeft;
				// AACutin.ImageIndex = (int)ResourceManager.EquipmentContent.HighAngleGun;
				AACutinIcon = ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.HighAngleGun);

				string ConditionDisplay(int id) => AntiAirCutIn.FromId(id).EquipmentConditionsMultiLineDisplay();
				
				AACutinToolTip = FormBattle.AACI + "\r\n" + string.Join("\r\n", phases2
					.Select(p => p.PhaseName + (p.Air.IsAACutinAvailable ?
						$"{p.Air.AACutInShipName}\r\n{FormBattle.AACIType}{p.Air.AACutInKind}\n{ConditionDisplay(p.Air.AACutInKind)}"
						: FormBattle.DidNotActivate)));
			}
			else
			{
				ClearAACutinLabel();
			}
		}
		else
		{
			// ClearAircraftLabel(AirStage2Friend);
			// ClearAircraftLabel(AirStage2Enemy);
			AirStage2FriendText = "-";
			AirStage2FriendForeColor = Utility.Configuration.Config.UI.ForeColor.ToBrush();
			AirStage2FriendIcon = null;
			AirStage2FriendToolTip = null;

			AirStage2EnemyText = "-";
			AirStage2EnemyForeColor = Utility.Configuration.Config.UI.ForeColor.ToBrush();
			AirStage2EnemyIcon = null;
			AirStage2EnemyToolTip = null;

			ClearAACutinLabel();
		}
	}

	private void ClearAerialWarfare()
	{
		AirSuperiorityText = "-";
		AirSuperiorityToolTip = null;

		// ClearAircraftLabel(AirStage1Friend);
		// ClearAircraftLabel(AirStage1Enemy);
		// ClearAircraftLabel(AirStage2Friend);
		// ClearAircraftLabel(AirStage2Enemy);

		AirStage1FriendText = "-";
		AirStage1FriendForeColor = Utility.Configuration.Config.UI.ForeColor.ToBrush();
		AirStage1FriendIcon = null;
		AirStage1FriendToolTip = null;

		AirStage1EnemyText = "-";
		AirStage1EnemyForeColor = Utility.Configuration.Config.UI.ForeColor.ToBrush();
		AirStage1EnemyIcon = null;
		AirStage1EnemyToolTip = null;

		AirStage2FriendText = "-";
		AirStage2FriendForeColor = Utility.Configuration.Config.UI.ForeColor.ToBrush();
		AirStage2FriendIcon = null;
		AirStage2FriendToolTip = null;

		AirStage2EnemyText = "-";
		AirStage2EnemyForeColor = Utility.Configuration.Config.UI.ForeColor.ToBrush();
		AirStage2EnemyIcon = null;
		AirStage2EnemyToolTip = null;

		AACutinText = "-";
		AACutinIcon = null;
		AACutinToolTip = null;
	}

	/// <summary>
	/// 両軍のHPゲージを設定します。
	/// </summary>
	private void SetHPBar(BattleData bd)
	{
		KCDatabase db = KCDatabase.Instance;
		bool isPractice = bd.IsPractice;
		bool isFriendCombined = bd.IsFriendCombined;
		bool isEnemyCombined = bd.IsEnemyCombined;
		bool isBaseAirRaid = bd.IsBaseAirRaid;
		bool hasFriend7thShip = bd.Initial.FriendMaxHPs.Count(hp => hp > 0) == 7;

		IsPlayerCombinedFleet = isFriendCombined;
		IsEnemyCombinedFleet = isEnemyCombined;

		var initial = bd.Initial;
		var resultHPs = bd.ResultHPs;
		var attackDamages = bd.AttackDamages;

		/*
		foreach (var bar in HPBars)
			bar.SuspendUpdate();
		*/

		void EnableHPBar(int index, int initialHP, int resultHP, int maxHP)
		{
			HPBars[index].Value = resultHP;
			HPBars[index].PrevValue = initialHP;
			HPBars[index].MaximumValue = maxHP;
			HPBars[index].BackColor = Color.Transparent;
			HPBars[index].Visible = true;
		}

		void DisableHPBar(int index)
		{
			HPBars[index].Visible = false;
		}

		void SetEnemyBackground(int index)
		{
			HPBars[index].BackColor = HPBars[index].Value switch
			{
				< 1 => Color.FromArgb(0x40, 0x4D, 0xA6, 0xDF),
				_ => Color.Transparent
			};
		}

		// friend main
		for (int i = 0; i < initial.FriendInitialHPs.Length; i++)
		{
			int refindex = BattleIndex.Get(BattleSides.FriendMain, i);

			if (initial.FriendInitialHPs[i] != -1)
			{
				EnableHPBar(refindex, initial.FriendInitialHPs[i], resultHPs[refindex], initial.FriendMaxHPs[i]);

				string name;
				bool isEscaped;
				bool isLandBase;

				var bar = HPBars[refindex];

				if (isBaseAirRaid)
				{
					name = string.Format(FormBattle.AirBase, i + 1);
					isEscaped = false;
					isLandBase = true;
					// it's air base, not land base
					bar.Text = "AB";        //note: Land Base (Landing Boat もあるらしいが考えつかなかったので)

				}
				else
				{
					IShipData ship = bd.Initial.FriendFleet.MembersInstance[i];
					name = ship.NameWithLevel;
					isEscaped = bd.Initial.FriendFleet.EscapedShipList.Contains(ship.MasterID);
					isLandBase = ship.MasterShip.IsLandBase;
					bar.Text = KCDatabase.Instance.Translation.Ship.TypeNameShort(ship.MasterShip.ShipType);
				}

				bar.ToolTip = string.Format
				("{0}\r\nHP: ({1} → {2})/{3} ({4}) [{5}]\r\n" + GeneralRes.DamageDone + ": {6}\r\n\r\n{7}",
					name,
					Math.Max(bar.PrevValue, 0),
					Math.Max(bar.Value, 0),
					bar.MaximumValue,
					bar.Value - bar.PrevValue,
					Constants.GetDamageState((double)bar.Value / bar.MaximumValue, isPractice, isLandBase, isEscaped),
					attackDamages[refindex],
					bd.GetBattleDetail(refindex)
				);

				bar.BackColor = isEscaped switch
				{
					true => Utility.Configuration.Config.UI.Battle_ColorHPBarsEscaped,
					_ => Color.Transparent
				};
				// bar.RepaintHPtext();
			}
			else
			{
				DisableHPBar(refindex);
			}
		}


		// enemy main
		for (int i = 0; i < initial.EnemyInitialHPs.Length; i++)
		{
			int refindex = BattleIndex.Get(BattleSides.EnemyMain, i);

			if (initial.EnemyInitialHPs[i] != -1)
			{
				EnableHPBar(refindex, initial.EnemyInitialHPs[i], resultHPs[refindex], initial.EnemyMaxHPs[i]);
				IShipDataMaster ship = bd.Initial.EnemyMembersInstance[i];

				var bar = HPBars[refindex];
				bar.Text = KCDatabase.Instance.Translation.Ship.TypeNameShort(ship.ShipType);
				SetEnemyBackground(refindex);

				bar.ToolTip =
					string.Format("{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n\r\n{7}",
						ship.NameWithClass,
						initial.EnemyLevels[i],
						Math.Max(bar.PrevValue, 0),
						Math.Max(bar.Value, 0),
						bar.MaximumValue,
						bar.Value - bar.PrevValue,
						Constants.GetDamageState((double)bar.Value / bar.MaximumValue, isPractice, ship.IsLandBase),
						bd.GetBattleDetail(refindex)
					);
			}
			else
			{
				DisableHPBar(refindex);
			}
		}


		// friend escort
		if (isFriendCombined)
		{
			// FleetFriendEscort.Visible = true;

			for (int i = 0; i < initial.FriendInitialHPsEscort.Length; i++)
			{
				int refindex = BattleIndex.Get(BattleSides.FriendEscort, i);

				if (initial.FriendInitialHPsEscort[i] != -1)
				{
					EnableHPBar(refindex, initial.FriendInitialHPsEscort[i], resultHPs[refindex], initial.FriendMaxHPsEscort[i]);

					IShipData ship = bd.Initial.FriendFleetEscort.MembersInstance[i];
					bool isEscaped = bd.Initial.FriendFleetEscort.EscapedShipList.Contains(ship.MasterID);

					var bar = HPBars[refindex];
					bar.Text = KCDatabase.Instance.Translation.Ship.TypeNameShort(ship.MasterShip.ShipType);

					bar.ToolTip = string.Format(
						"{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n" + GeneralRes.DamageDone + ": {7}\r\n\r\n{8}",
						ship.MasterShip.NameWithClass,
						ship.Level,
						Math.Max(bar.PrevValue, 0),
						Math.Max(bar.Value, 0),
						bar.MaximumValue,
						bar.Value - bar.PrevValue,
						Constants.GetDamageState((double)bar.Value / bar.MaximumValue, isPractice, ship.MasterShip.IsLandBase, isEscaped),
						attackDamages[refindex],
						bd.GetBattleDetail(refindex)
					);

					bar.BackColor = isEscaped switch
					{
						true => Utility.Configuration.Config.UI.Battle_ColorHPBarsEscaped,
						_ => Color.Transparent
					};
					// bar.RepaintHPtext();
				}
				else
				{
					DisableHPBar(refindex);
				}
			}

		}
		else
		{
			// FleetFriendEscort.Visible = false;
			/*
			foreach (var i in BattleIndex.FriendEscort.Skip(Math.Max(bd.Initial.FriendFleet.Members.Count - 6, 0)))
				DisableHPBar(i);
			*/
		}

		MoveHPBar(hasFriend7thShip);



		// enemy escort
		if (isEnemyCombined)
		{
			// FleetEnemyEscort.Visible = true;

			for (int i = 0; i < 6; i++)
			{
				int refindex = BattleIndex.Get(BattleSides.EnemyEscort, i);

				if (initial.EnemyInitialHPsEscort[i] != -1)
				{
					EnableHPBar(refindex, initial.EnemyInitialHPsEscort[i], resultHPs[refindex], initial.EnemyMaxHPsEscort[i]);

					IShipDataMaster ship = bd.Initial.EnemyMembersEscortInstance[i];

					var bar = HPBars[refindex];
					bar.Text = KCDatabase.Instance.Translation.Ship.TypeNameShort(ship.ShipType);
					SetEnemyBackground(refindex);

					bar.ToolTip =
						string.Format("{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n\r\n{7}",
							ship.NameWithClass,
							bd.Initial.EnemyLevelsEscort[i],
							Math.Max(bar.PrevValue, 0),
							Math.Max(bar.Value, 0),
							bar.MaximumValue,
							bar.Value - bar.PrevValue,
							Constants.GetDamageState((double)bar.Value / bar.MaximumValue, isPractice, ship.IsLandBase),
							bd.GetBattleDetail(refindex)
						);
				}
				else
				{
					DisableHPBar(refindex);
				}
			}

		}
		else
		{
			// FleetEnemyEscort.Visible = false;
			/*
			foreach (var i in BattleIndex.EnemyEscort)
				DisableHPBar(i);
			*/
		}

		{   // support
			PhaseSupport support = null;

			if (bd is BattleDayFromNight bddn)
			{
				if (bddn.NightSupport?.IsAvailable ?? false)
					support = bddn.NightSupport;
			}
			if (support == null)
				support = bd.Support;

			if (support?.IsAvailable ?? false)
			{
				FleetFriendIcon = ImageSourceIcons.GetEquipmentIcon(support.SupportFlag switch
				{
					1 => EquipmentIconType.CarrierBasedTorpedo,
					2 => EquipmentIconType.MainGunLarge,
					3 => EquipmentIconType.Torpedo,
					4 => EquipmentIconType.DepthCharge,
					_ => EquipmentIconType.Unknown
				});

				// FleetFriend.ImageAlign = ContentAlignment.MiddleLeft;
				FleetFriendToolTip = FormBattle.SupportExpedition + "\r\n" + support.GetBattleDetail();

				if ((isFriendCombined || hasFriend7thShip) && isEnemyCombined)
					FleetFriendText = FormBattle.FleetFriendShort;
				else
					FleetFriendText = FormBattle.FleetFriend;

			}
			else
			{
				FleetFriendIcon = null;
				FleetFriendText = FormBattle.FleetFriend;
				FleetFriendToolTip = null;

			}
		}

		// sunk background should be prioritized
		if (HPBars[BattleIndex.EnemyMain1].Value > 0)
		{
			HPBars[BattleIndex.EnemyMain1].BackColor = bd.Initial.IsBossDamaged switch
			{
				true => Utility.Configuration.Config.UI.Battle_ColorHPBarsBossDamaged,
				_ => Color.Transparent
			};
		}

		if (!isBaseAirRaid)
		{
			foreach (int i in bd.MVPShipIndexes)
			{
				HPBars[BattleIndex.Get(BattleSides.FriendMain, i)].BackColor = Utility.Configuration.Config.UI.Battle_ColorHPBarsMVP;
				// HPBars[BattleIndex.Get(BattleSides.FriendMain, i)].RepaintHPtext();
			}

			if (isFriendCombined)
			{
				foreach (int i in bd.MVPShipCombinedIndexes)
				{
					HPBars[BattleIndex.Get(BattleSides.FriendEscort, i)].BackColor = Utility.Configuration.Config.UI.Battle_ColorHPBarsMVP;
					// HPBars[BattleIndex.Get(BattleSides.FriendEscort, i)].RepaintHPtext();
				}
			}
		}
		/*
		foreach (var bar in HPBars)
			bar.ResumeUpdate();
		*/
	}

	private bool _hpBarMoved = false;
	/// <summary>
	/// 味方遊撃部隊７人目のHPゲージ（通常時は連合艦隊第二艦隊旗艦のHPゲージ）を移動します。
	/// </summary>
	private void MoveHPBar(bool hasFriend7thShip)
	{

		if (Utility.Configuration.Config.FormBattle.Display7thAsSingleLine && hasFriend7thShip)
		{
			if (_hpBarMoved) return;

			PlayerMainHPBars.Add(PlayerEscortHPBars[0]);
			PlayerEscortHPBars.RemoveAt(0);

			// TableBottom.SetCellPosition(HPBars[BattleIndex.FriendEscort1], new TableLayoutPanelCellPosition(0, 7));
			// bool fixSize = Utility.Configuration.Config.UI.IsLayoutFixed;
			// bool showHPBar = Utility.Configuration.Config.FormBattle.ShowHPBar;
			// ControlHelper.SetTableRowStyle(TableBottom, 7, fixSize ? new RowStyle(SizeType.Absolute, showHPBar ? 21 : 16) : new RowStyle(SizeType.AutoSize));
			_hpBarMoved = true;
		}
		else
		{
			if (!_hpBarMoved) return;

			PlayerEscortHPBars.Insert(0, PlayerMainHPBars[6]);
			PlayerMainHPBars.RemoveAt(6);

			// TableBottom.SetCellPosition(HPBars[BattleIndex.FriendEscort1], new TableLayoutPanelCellPosition(1, 1));
			// ControlHelper.SetTableRowStyle(TableBottom, 7, new RowStyle(SizeType.Absolute, 0));
			_hpBarMoved = false;
		}

	}


	/// <summary>
	/// 損害率と戦績予測を設定します。
	/// </summary>
	private void SetDamageRate(BattleManager bm)
	{

		int rank = bm.PredictWinRank(out double friendrate, out double enemyrate);

		DamageFriendText = friendrate.ToString("p1");
		DamageEnemyText = enemyrate.ToString("p1");

		if (bm.IsBaseAirRaid)
		{
			int kind = bm.Compass.AirRaidDamageKind;
			WinRankText = Constants.GetAirRaidDamageShort(kind);
			WinRankForeColor = (1 <= kind && kind <= 3) ? WinRankColor_Lose : WinRankColor_Win;
		}
		else
		{
			WinRankText = Constants.GetWinRank(rank);
			WinRankForeColor = rank >= 4 ? WinRankColor_Win : WinRankColor_Lose;
		}

		// WinRank.MinimumSize = Utility.Configuration.Config.UI.IsLayoutFixed ? new Size(DefaultBarSize.Width, 0) : new Size(HPBars[0].Width, 0);

	}

	/// <summary>
	/// 夜戦における各種表示を設定します。
	/// </summary>
	private void SetNightBattleEvent(PhaseNightInitial pd)
	{
		FleetData fleet = pd.FriendFleet;

		//味方探照灯判定
		{
			int index = pd.SearchlightIndexFriend;

			if (index != -1)
			{
				IShipData ship = fleet.MembersInstance[index];

				AirStage1FriendText = "#" + (index + (pd.IsFriendEscort ? 6 : 0) + 1);
				AirStage1FriendForeColor = Utility.Configuration.Config.UI.ForeColor.ToBrush();
				AirStage1FriendIcon = ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.Searchlight);
				AirStage1FriendToolTip = GeneralRes.SearchlightUsed + ": " + ship.NameWithLevel;
			}
			else
			{
				AirStage1FriendToolTip = null;
			}
		}

		//敵探照灯判定
		{
			int index = pd.SearchlightIndexEnemy;
			if (index != -1)
			{
				AirStage1EnemyText = "#" + (index + (pd.IsEnemyEscort ? 6 : 0) + 1);
				AirStage1EnemyForeColor = Utility.Configuration.Config.UI.ForeColor.ToBrush();
				AirStage1EnemyIcon = ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.Searchlight);
				AirStage1EnemyToolTip = GeneralRes.SearchlightUsed + ": " + pd.SearchlightEnemyInstance.NameWithClass;
			}
			else
			{
				AirStage1EnemyToolTip = null;
			}
		}


		//夜間触接判定
		if (pd.TouchAircraftFriend != -1)
		{
			SearchingFriendText = GeneralRes.NightContact;
			SearchingFriendIcon = ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.Seaplane);
			SearchingFriendToolTip = GeneralRes.NightContacting + ": " + KCDatabase.Instance.MasterEquipments[pd.TouchAircraftFriend].NameEN;
		}
		else
		{
			SearchingFriendToolTip = null;
		}

		if (pd.TouchAircraftEnemy != -1)
		{
			SearchingEnemyText = GeneralRes.NightContact;
			SearchingEnemyIcon = ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.Seaplane);
			SearchingEnemyToolTip = GeneralRes.NightContacting + ": " + KCDatabase.Instance.MasterEquipments[pd.TouchAircraftEnemy].NameEN;
		}
		else
		{
			SearchingEnemyToolTip = null;
		}

		//照明弾投射判定
		{
			int index = pd.FlareIndexFriend;

			if (index != -1)
			{
				AirStage2FriendText = "#" + (index + 1);
				AirStage2FriendForeColor = Utility.Configuration.Config.UI.ForeColor.ToBrush();
				AirStage2FriendIcon = ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.StarShell);
				AirStage2FriendToolTip = GeneralRes.StarShellUsed + ": " + pd.FlareFriendInstance.NameWithLevel;

			}
			else
			{
				AirStage2FriendToolTip = null;
			}
		}

		{
			int index = pd.FlareIndexEnemy;

			if (index != -1)
			{
				AirStage2EnemyText = "#" + (index + 1);
				AirStage2EnemyForeColor = Utility.Configuration.Config.UI.ForeColor.ToBrush();
				AirStage2EnemyIcon = ImageSourceIcons.GetEquipmentIcon(EquipmentIconType.StarShell);
				AirStage2EnemyToolTip = GeneralRes.StarShellUsed + ": " + pd.FlareEnemyInstance.NameWithClass;
			}
			else
			{
				AirStage2EnemyToolTip = null;
			}
		}
	}


	/// <summary>
	/// 戦闘終了後に、MVP艦の表示を更新します。
	/// </summary>
	/// <param name="bm">戦闘データ。</param>
	private void SetMVPShip(BattleManager bm)
	{

		bool isCombined = bm.IsCombinedBattle;

		var bd = bm.StartsFromDayBattle ? (BattleData)bm.BattleDay : (BattleData)bm.BattleNight;
		var br = bm.Result;

		var friend = bd.Initial.FriendFleet;
		var escort = !isCombined ? null : bd.Initial.FriendFleetEscort;


		/*// DEBUG
		{
			BattleData lastbattle = bm.StartsFromDayBattle ? (BattleData)bm.BattleNight ?? bm.BattleDay : (BattleData)bm.BattleDay ?? bm.BattleNight;
			if ( lastbattle.MVPShipIndexes.Count() > 1 || !lastbattle.MVPShipIndexes.Contains( br.MVPIndex - 1 ) ) {
				Utility.Logger.Add( 1, "MVP is wrong : [" + string.Join( ",", lastbattle.MVPShipIndexes ) + "] => " + ( br.MVPIndex - 1 ) );
			}
			if ( isCombined && ( lastbattle.MVPShipCombinedIndexes.Count() > 1 || !lastbattle.MVPShipCombinedIndexes.Contains( br.MVPIndexCombined - 1 ) ) ) {
				Utility.Logger.Add( 1, "MVP is wrong (escort) : [" + string.Join( ",", lastbattle.MVPShipCombinedIndexes ) + "] => " + ( br.MVPIndexCombined - 1 ) );
			}
		}
		//*/


		for (int i = 0; i < friend.Members.Count; i++)
		{
			if (friend.EscapedShipList.Contains(friend.Members[i]))
			{
				HPBars[i].BackColor = Utility.Configuration.Config.UI.Battle_ColorHPBarsEscaped;
			}
			else if (br.MVPIndex == i + 1)
			{
				HPBars[i].BackColor = Utility.Configuration.Config.UI.Battle_ColorHPBarsMVP;
			}
			else
			{
				HPBars[i].BackColor = Color.Transparent;
			}

			// HPBars[i].RepaintHPtext();
		}

		if (escort != null)
		{
			for (int i = 0; i < escort.Members.Count; i++)
			{
				if (escort.EscapedShipList.Contains(escort.Members[i]))
				{
					HPBars[i + 6].BackColor = Utility.Configuration.Config.UI.Battle_ColorHPBarsEscaped;
				}
				else if (br.MVPIndexCombined == i + 1)
				{
					HPBars[i + 6].BackColor = Utility.Configuration.Config.UI.Battle_ColorHPBarsMVP;
				}
				else
				{
					HPBars[i + 6].BackColor = Color.Transparent;
				}

				// HPBars[i + 6].RepaintHPtext();
			}
		}

		/*// debug
		if ( WinRank.Text.First().ToString() != bm.Result.Rank ) {
			Utility.Logger.Add( 1, string.Format( "戦闘評価予測が誤っています。(予測: {0}, 実際: {1})", WinRank.Text.First().ToString(), bm.Result.Rank ) );
		}
		//*/

	}


	void ConfigurationChanged()
	{
		var config = Utility.Configuration.Config;

		System.Drawing.Color[] colorScheme = config.UI.BarColorScheme.Select(col => col.ColorData).ToArray();

		foreach (HealthBarViewModel hpBar in HPBars)
		{
			hpBar.SetBarColorScheme(colorScheme);
			hpBar.ColorMorphing = config.UI.BarColorMorphing;
		}

		CompactMode = Utility.Configuration.Config.FormBattle.CompactMode;

		/*
		MainFont = TableTop.Font = TableBottom.Font = Font = config.UI.MainFont;
		SubFont = config.UI.SubFont;

		BaseLayoutPanel.AutoScroll = config.FormBattle.IsScrollable;


		bool fixSize = config.UI.IsLayoutFixed;
		bool showHPBar = config.FormBattle.ShowHPBar;

		TableBottom.SuspendLayout();
		if (fixSize)
		{
			ControlHelper.SetTableColumnStyles(TableBottom, new ColumnStyle(SizeType.AutoSize));
			ControlHelper.SetTableRowStyle(TableBottom, 0, new RowStyle(SizeType.Absolute, 21));
			for (int i = 1; i <= 6; i++)
				ControlHelper.SetTableRowStyle(TableBottom, i, new RowStyle(SizeType.Absolute, showHPBar ? 21 : 16));
			ControlHelper.SetTableRowStyle(TableBottom, 8, new RowStyle(SizeType.Absolute, 21));
		}
		else
		{
			ControlHelper.SetTableColumnStyles(TableBottom, new ColumnStyle(SizeType.AutoSize));
			ControlHelper.SetTableRowStyles(TableBottom, new RowStyle(SizeType.AutoSize));
		}
		if (HPBars != null)
		{
			foreach (var b in HPBars)
			{
				b.MainFont = MainFont;
				b.SubFont = SubFont;
				b.AutoSize = !fixSize;
				if (!b.AutoSize)
				{
					b.Size = (HPBars[12].Visible && HPBars[18].Visible) ? SmallBarSize : DefaultBarSize;
				}
				b.HPBar.ColorMorphing = config.UI.BarColorMorphing;
				b.HPBar.SetBarColorScheme(config.UI.BarColorScheme.Select(col => col.ColorData).ToArray());
				b.ShowHPBar = showHPBar;
			}
		}
		FleetFriend.MaximumSize =
		FleetFriendEscort.MaximumSize =
		FleetEnemy.MaximumSize =
		FleetEnemyEscort.MaximumSize =
		DamageFriend.MaximumSize =
		DamageEnemy.MaximumSize =
			fixSize ? DefaultBarSize : Size.Empty;

		WinRank.MinimumSize = fixSize ? new Size(80, 0) : new Size(HPBars[0].Width, 0);

		TableBottom.ResumeLayout();

		TableTop.SuspendLayout();
		if (fixSize)
		{
			ControlHelper.SetTableColumnStyles(TableTop, new ColumnStyle(SizeType.Absolute, 21 * 4));
			ControlHelper.SetTableRowStyles(TableTop, new RowStyle(SizeType.Absolute, 21));
			TableTop.Width = TableTop.GetPreferredSize(BaseLayoutPanel.Size).Width;
		}
		else
		{
			ControlHelper.SetTableColumnStyles(TableTop, new ColumnStyle(SizeType.Percent, 100));
			ControlHelper.SetTableRowStyles(TableTop, new RowStyle(SizeType.AutoSize));
			TableTop.Width = TableBottom.ClientSize.Width;
		}
		TableTop.Height = TableTop.GetPreferredSize(BaseLayoutPanel.Size).Height;
		TableTop.ResumeLayout();
		*/
	}
}
