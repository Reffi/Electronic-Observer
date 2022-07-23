﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using AvalonDock;
using AvalonDock.Layout;
using AvalonDock.Layout.Serialization;
using AvalonDock.Themes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DynaJson;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Notifier;
using ElectronicObserver.Observer;
using ElectronicObserver.Properties;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Dialog.QuestTrackerManager;
using ElectronicObserver.Window.Dialog.VersionInformation;
using ElectronicObserver.Window.Dialog.ResourceChartWPF;
using ElectronicObserver.Window.Integrate;
using ElectronicObserver.Window.Tools.AirDefense;
using ElectronicObserver.Window.Tools.AutoRefresh;
using ElectronicObserver.Window.Tools.ConstructionRecordViewer;
using ElectronicObserver.Window.Tools.DevelopmentRecordViewer;
using ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment;
using ElectronicObserver.Window.Tools.DialogAlbumMasterShip;
using ElectronicObserver.Window.Tools.DropRecordViewer;
using ElectronicObserver.Window.Tools.EquipmentList;
using ElectronicObserver.Window.Tools.EventLockPlanner;
using ElectronicObserver.Window.Wpf;
using ElectronicObserver.Window.Wpf.Arsenal;
using ElectronicObserver.Window.Wpf.BaseAirCorps;
using ElectronicObserver.Window.Wpf.Battle;
using ElectronicObserver.Window.Wpf.Compass;
using ElectronicObserver.Window.Wpf.Dock;
using ElectronicObserver.Window.Wpf.Fleet;
using ElectronicObserver.Window.Wpf.FleetOverview;
using ElectronicObserver.Window.Wpf.FleetPreset;
using ElectronicObserver.Window.Wpf.Headquarters;
using ElectronicObserver.Window.Wpf.Quest;
using ElectronicObserver.Window.Wpf.ShipGroup.ViewModels;
using ElectronicObserver.Window.Wpf.ShipGroupWinforms;
using ElectronicObserver.Window.Wpf.WinformsWrappers;
using ElectronicObserver.Window.Wpf.InformationView;
using ElectronicObserver.Window.Wpf.Log;
using MessagePack;
using Microsoft.EntityFrameworkCore;
using ModernWpf;
using MessageBox = System.Windows.MessageBox;
using Timer = System.Windows.Forms.Timer;
#if DEBUG
using System.Text.Encodings.Web;
using ElectronicObserverTypes;
#endif

namespace ElectronicObserver.ViewModels;

public partial class FormMainViewModel : ObservableObject
{
	private FormMainWpf Window { get; }
	private DockingManager DockingManager { get; }
	private Configuration.ConfigurationData Config { get; }
	public FormMainTranslationViewModel FormMain { get; }
	private ToolService ToolService { get; }
	private System.Windows.Forms.Timer UIUpdateTimer { get; }
	public bool Topmost { get; set; }
	public int GridSplitterSize { get; set; } = 1;
	public bool CanChangeGridSplitterSize { get; set; }
	public bool LockLayout { get; set; }
	public bool CanAutoHide => !LockLayout;

	private string LayoutFolder => @"Settings\Layout";
	private string DefaultLayoutPath => Path.Combine(LayoutFolder, "Default.xml");
	private string LayoutPath => Config.Life.LayoutFilePath;
	private string PositionPath => Path.ChangeExtension(LayoutPath, ".Position.json");
	private string IntegratePath => Path.ChangeExtension(LayoutPath, ".Integrate.json");

	public bool NotificationsSilenced { get; set; }
	private DateTime PrevPlayTimeRecorded { get; set; } = DateTime.MinValue;
	public FontFamily Font { get; set; }
	public double FontSize { get; set; }
	public SolidColorBrush FontBrush { get; set; }
	public FontFamily SubFont { get; set; }
	public double SubFontSize { get; set; }
	public SolidColorBrush SubFontBrush { get; set; }

	public List<Theme> Themes { get; } = new()
	{
		new Vs2013LightTheme(),
		new Vs2013BlueTheme(),
		new Vs2013DarkTheme(),
	};
	public Theme CurrentTheme { get; set; }
	public Color BackgroundColor { get; set; }

	private WindowPosition Position { get; set; } = new();
	// single instance hack
	private EventLockPlannerWindow? EventLockPlannerWindow { get; set; }
	private AutoRefreshWindow? AutoRefreshWindow { get; set; }

	#region Icons

	public ImageSource? ConfigurationImageSource { get; }

	public ImageSource? FleetsImageSource { get; }
	public ImageSource? FleetOverviewImageSource { get; }
	public ImageSource? ShipGroupImageSource { get; }
	public ImageSource? FleetPresetImageSource { get; }
	public ImageSource? DockImageSource { get; }
	public ImageSource? ArsenalImageSource { get; }
	public ImageSource? BaseAirCorpsImageSource { get; }
	public ImageSource? HeadquartersImageSource { get; }
	public ImageSource? QuestImageSource { get; }
	public ImageSource? InformationImageSource { get; }
	public ImageSource? CompassImageSource { get; }
	public ImageSource? BattleImageSource { get; }
	public ImageSource? BrowserHostImageSource { get; }
	public ImageSource? LogImageSource { get; }
	public ImageSource? JsonImageSource { get; }
	public ImageSource? WindowCaptureImageSource { get; }

	public ImageSource? EquipmentListImageSource { get; }
	public ImageSource? DropRecordImageSource { get; }
	public ImageSource? DevelopmentRecordImageSource { get; }
	public ImageSource? ConstructionRecordImageSource { get; }
	public ImageSource? ResourceChartImageSource { get; }
	public ImageSource? AlbumMasterShipImageSource { get; }
	public ImageSource? AlbumMasterEquipmentImageSource { get; }
	public ImageSource? AntiAirDefenseImageSource { get; }
	public ImageSource? FleetImageGeneratorImageSource { get; }
	public ImageSource? BaseAirCorpsSimulationImageSource { get; }
	public ImageSource? ExpCheckerImageSource { get; }
	public ImageSource? ExpeditionCheckImageSource { get; }
	public ImageSource? KancolleProgressImageSource { get; }
	public ImageSource? ExtraBrowserImageSource { get; }
	public ImageSource? AutoRefreshImageSource { get; }

	public ImageSource? ViewHelpImageSource { get; }
	public ImageSource? ViewVersionImageSource { get; }

	#endregion

	public ObservableCollection<AnchorableViewModel> Views { get; } = new();

	public List<FleetViewModel> Fleets { get; }
	public FleetOverviewViewModel FleetOverview { get; }
	public ShipGroupWinformsViewModel FormShipGroup { get; }
	public ShipGroupViewModel ShipGroup { get; }
	public FleetPresetViewModel FleetPreset { get; }

	public DockViewModel Dock { get; }
	public ArsenalViewModel Arsenal { get; }
	public BaseAirCorpsViewModel BaseAirCorps { get; }

	public HeadquartersViewModel Headquarters { get; }
	public QuestViewModel Quest { get; }
	public InformationViewModel FormInformation { get; }

	public CompassViewModel Compass { get; }
	public BattleViewModel Battle { get; }

	public FormBrowserHostViewModel FormBrowserHost { get; }
	public LogViewModel FormLog { get; }
	public FormJsonViewModel FormJson { get; }
	public FormWindowCaptureViewModel WindowCapture { get; }

	public StripStatusViewModel StripStatus { get; } = new();
	public int ClockFormat { get; set; }

	private bool DebugEnabled { get; set; }
	public Visibility DebugVisibility => DebugEnabled.ToVisibility();

	private string GeneratedDataFolder => "Generated";
	public bool GenerateMasterDataVisible =>
#if DEBUG
		true;
#else
		false;
#endif

	public FormMainViewModel(DockingManager dockingManager, FormMainWpf window)
	{
		Window = window;
		DockingManager = dockingManager;

		Config = Configuration.Config;
		FormMain = Ioc.Default.GetService<FormMainTranslationViewModel>()!;
		ToolService = Ioc.Default.GetService<ToolService>()!;

		CultureInfo cultureInfo = new(Configuration.Config.UI.Culture);

		Thread.CurrentThread.CurrentCulture = cultureInfo;
		Thread.CurrentThread.CurrentUICulture = cultureInfo;

		Utility.Logger.Instance.LogAdded += data =>
		{
			if (Window.CheckAccess())
			{
				// Invokeはメッセージキューにジョブを投げて待つので、別のBeginInvokeされたジョブが既にキューにあると、
				// それを実行してしまい、BeginInvokeされたジョブの順番が保てなくなる
				// GUIスレッドによる処理は、順番が重要なことがあるので、GUIスレッドからInvokeを呼び出してはいけない
				Window.Dispatcher.Invoke(new Utility.LogAddedEventHandler(Logger_LogAdded), data);
			}
			else
			{
				Logger_LogAdded(data);
			}
		};

		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		string softwareName = CultureInfo.CurrentCulture.Name switch
		{
			"en-US" => SoftwareInformation.SoftwareNameEnglish,
			_ => SoftwareInformation.SoftwareNameJapanese
		};

		Utility.Logger.Add(2, softwareName + Properties.Window.FormMain.Starting);

		ResourceManager.Instance.Load();
		RecordManager.Instance.Load();
		KCDatabase.Instance.Load();
		NotifierManager.Instance.Initialize(Window);
		SyncBGMPlayer.Instance.ConfigurationChanged();

		using ElectronicObserverContext db = new();
		db.Database.Migrate();

		UIUpdateTimer = new Timer() { Interval = 1000 };
		UIUpdateTimer.Tick += UIUpdateTimer_Tick;
		UIUpdateTimer.Start();

		#region Icon settings

		ConfigurationImageSource = ImageSourceIcons.GetIcon(IconContent.FormConfiguration);

		FleetsImageSource = ImageSourceIcons.GetIcon(IconContent.FormFleet);
		FleetOverviewImageSource = ImageSourceIcons.GetIcon(IconContent.FormFleet);
		ShipGroupImageSource = ImageSourceIcons.GetIcon(IconContent.FormShipGroup);
		FleetPresetImageSource = ImageSourceIcons.GetIcon(IconContent.FormFleetPreset);
		DockImageSource = ImageSourceIcons.GetIcon(IconContent.FormDock);
		ArsenalImageSource = ImageSourceIcons.GetIcon(IconContent.FormArsenal);
		BaseAirCorpsImageSource = ImageSourceIcons.GetIcon(IconContent.FormBaseAirCorps);
		HeadquartersImageSource = ImageSourceIcons.GetIcon(IconContent.FormHeadQuarters);
		QuestImageSource = ImageSourceIcons.GetIcon(IconContent.FormQuest);
		InformationImageSource = ImageSourceIcons.GetIcon(IconContent.FormInformation);
		CompassImageSource = ImageSourceIcons.GetIcon(IconContent.FormCompass);
		BattleImageSource = ImageSourceIcons.GetIcon(IconContent.FormBattle);
		BrowserHostImageSource = ImageSourceIcons.GetIcon(IconContent.FormBrowser);
		LogImageSource = ImageSourceIcons.GetIcon(IconContent.FormLog);
		JsonImageSource = ImageSourceIcons.GetIcon(IconContent.FormJson);
		WindowCaptureImageSource = ImageSourceIcons.GetIcon(IconContent.FormWindowCapture);

		EquipmentListImageSource = ImageSourceIcons.GetIcon(IconContent.FormEquipmentList);
		DropRecordImageSource = ImageSourceIcons.GetIcon(IconContent.FormDropRecord);
		DevelopmentRecordImageSource = ImageSourceIcons.GetIcon(IconContent.FormDevelopmentRecord);
		ConstructionRecordImageSource = ImageSourceIcons.GetIcon(IconContent.FormConstructionRecord);
		ResourceChartImageSource = ImageSourceIcons.GetIcon(IconContent.FormResourceChart);
		AlbumMasterShipImageSource = ImageSourceIcons.GetIcon(IconContent.FormAlbumShip);
		AlbumMasterEquipmentImageSource = ImageSourceIcons.GetIcon(IconContent.FormAlbumEquipment);
		AntiAirDefenseImageSource = ImageSourceIcons.GetIcon(IconContent.FormAntiAirDefense);
		FleetImageGeneratorImageSource = ImageSourceIcons.GetIcon(IconContent.FormFleetImageGenerator);
		BaseAirCorpsSimulationImageSource = ImageSourceIcons.GetIcon(IconContent.FormBaseAirCorps);
		ExpCheckerImageSource = ImageSourceIcons.GetIcon(IconContent.FormExpChecker);
		ExpeditionCheckImageSource = ImageSourceIcons.GetIcon(IconContent.FormExpeditionCheck);
		KancolleProgressImageSource = ImageSourceIcons.GetIcon(IconContent.FormEquipmentList);
		ExtraBrowserImageSource = ImageSourceIcons.GetIcon(IconContent.FormBrowser);
		AutoRefreshImageSource = ImageSourceIcons.GetIcon(IconContent.BrowserRefresh);

		ViewHelpImageSource = ImageSourceIcons.GetIcon(IconContent.FormInformation);
		ViewVersionImageSource = ImageSourceIcons.GetIcon(IconContent.AppIcon);

		/*
		Icon = ResourceManager.Instance.AppIcon;

		StripMenu_File_Configuration.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormConfiguration];

		StripMenu_View_Fleet.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet];
		StripMenu_View_FleetOverview.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleet];
		StripMenu_View_ShipGroup.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormShipGroup];
		StripMenu_View_Dock.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormDock];
		StripMenu_View_Arsenal.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormArsenal];
		StripMenu_View_Headquarters.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormHeadQuarters];
		StripMenu_View_Quest.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormQuest];
		StripMenu_View_Information.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormInformation];
		StripMenu_View_Compass.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormCompass];
		StripMenu_View_Battle.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBattle];
		StripMenu_View_Browser.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBrowser];
		StripMenu_View_Log.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormLog];
		StripMenu_WindowCapture.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormWindowCapture];
		StripMenu_View_BaseAirCorps.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBaseAirCorps];
		StripMenu_View_Json.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormJson];
		StripMenu_View_FleetPreset.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleetPreset];

		StripMenu_Tool_EquipmentList.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormEquipmentList];
		StripMenu_Tool_DropRecord.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormDropRecord];
		StripMenu_Tool_DevelopmentRecord.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormDevelopmentRecord];
		StripMenu_Tool_ConstructionRecord.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormConstructionRecord];
		StripMenu_Tool_ResourceChart.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormResourceChart];
		StripMenu_Tool_AlbumMasterShip.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormAlbumShip];
		StripMenu_Tool_AlbumMasterEquipment.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormAlbumEquipment];
		StripMenu_Tool_AntiAirDefense.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormAntiAirDefense];
		StripMenu_Tool_FleetImageGenerator.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormFleetImageGenerator];
		StripMenu_Tool_BaseAirCorpsSimulation.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormBaseAirCorps];
		StripMenu_Tool_ExpChecker.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormExpChecker];
		StripMenu_Tool_ExpeditionCheck.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormExpeditionCheck];
		StripMenu_Tool_KancolleProgress.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormEquipmentList];


		StripMenu_Help_Help.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.FormInformation];
		StripMenu_Help_Version.Image = ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.AppIcon];
		*/

		#endregion

		APIObserver.Instance.Start(Configuration.Config.Connection.Port, Window);

		Fleets = new List<FleetViewModel>()
		{
			new(1),
			new(2),
			new(3),
			new(4),
		};
		foreach (FleetViewModel fleet in Fleets)
		{
			Views.Add(fleet);
		}
		Views.Add(FleetOverview = new FleetOverviewViewModel(Fleets));
		Views.Add(FormShipGroup = new ShipGroupWinformsViewModel());
		// Views.Add(ShipGroup = new());
		Views.Add(FleetPreset = new FleetPresetViewModel());

		Views.Add(Dock = new DockViewModel());
		Views.Add(Arsenal = new ArsenalViewModel());
		Views.Add(BaseAirCorps = new BaseAirCorpsViewModel());

		Views.Add(Headquarters = new HeadquartersViewModel());
		Views.Add(Quest = new QuestViewModel());
		Views.Add(FormInformation = new InformationViewModel());

		Views.Add(Compass = new CompassViewModel());
		Views.Add(Battle = new BattleViewModel());

		Views.Add(FormBrowserHost = new FormBrowserHostViewModel() { Visibility = Visibility.Visible });
		Views.Add(FormLog = new LogViewModel());
		Views.Add(FormJson = new FormJsonViewModel());
		Views.Add(WindowCapture = new FormWindowCaptureViewModel(this));

		ConfigurationChanged(); //設定から初期化

		// LoadLayout();

		SoftwareInformation.CheckUpdate();
		Task.Run(async () => await SoftwareUpdater.CheckUpdateAsync());
		CancellationTokenSource cts = new();
		Task.Run(async () => await SoftwareUpdater.PeriodicUpdateCheckAsync(cts.Token));

		/*
		// デバッグ: 開始時にAPIリストを読み込む
		if (Configuration.Config.Debug.LoadAPIListOnLoad)
		{
			try
			{
				await Task.Factory.StartNew(() => LoadAPIList(Configuration.Config.Debug.APIListPath));

				Activate();     // 上記ロードに時間がかかるとウィンドウが表示されなくなることがあるので
			}
			catch (Exception ex)
			{

				Utility.Logger.Add(3, LoggerRes.FailedLoadAPI + ex.Message);
			}
		}
		*/

		APIObserver.Instance.ResponseReceived += (a, b) => UpdatePlayTime();


		// 🎃
		if (DateTime.Now.Month == 10 && DateTime.Now.Day == 31)
		{
			APIObserver.Instance.ApiPort_Port.ResponseReceived += CallPumpkinHead;
		}

		// 完了通知（ログインページを開く）
		// fBrowser.InitializeApiCompleted();
		if (FormBrowserHost.WinformsControl is FormBrowserHost host)
		{
			host.InitializeApiCompleted();
		}

		NotificationsSilenced = NotifierManager.Instance.GetNotifiers().All(n => n.IsSilenced);

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(Topmost)) return;

			Configuration.Config.Life.TopMost = Topmost;
			ConfigurationChanged();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(GridSplitterSize)) return;

			LoadLayout(Window);
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(LockLayout)) return;

			Config.Life.LockLayout = LockLayout;
			ConfigurationChanged();
		};

		Logger.Add(3, Resources.StartupComplete);
	}

	// Toggle TopMost of Main Form back and forth to workaround a .Net Bug: KB2756203 (~win7) / KB2769674 (win8~)
	private void RefreshTopMost()
	{
		Topmost = !Topmost;
		Topmost = !Topmost;
	}

	[ICommand]
	private void AutoHide(LayoutAnchorable anchorable)
	{
		anchorable.ToggleAutoHide();
	}

	#region File

	[ICommand]
	private void SaveData()
	{
		RecordManager.Instance.SaveAll();
	}

	[ICommand]
	private void LoadData()
	{
		if (MessageBox.Show(Resources.AskLoad, Properties.Window.FormMain.ConfirmatonCaption,
				MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
			== MessageBoxResult.Yes)
		{
			RecordManager.Instance.Load();
		}
	}

	[ICommand]
	public void SaveLayout(object? sender)
	{
		if (sender is not FormMainWpf window) return;

		foreach (FormIntegrateViewModel integrate in Views.OfType<FormIntegrateViewModel>())
		{
			integrate.RaiseContentIdChanged();
		}

		XmlLayoutSerializer serializer = new(DockingManager);
		serializer.Serialize(LayoutPath);

		Position.Top = window.Top;
		Position.Left = window.Left;
		Position.Height = window.Height;
		Position.Width = window.Width;
		Position.WindowState = window.WindowState;

		File.WriteAllText(PositionPath, JsonSerializer.Serialize(Position, new JsonSerializerOptions()
		{
			WriteIndented = true
		}));

		IEnumerable<FormIntegrate.WindowInfo> integrateWindows = Views
			.OfType<FormIntegrateViewModel>()
			.Select(i => (i.WinformsControl! as FormIntegrate)!.WindowData);

		byte[]? data = MessagePackSerializer.Serialize(integrateWindows);

		File.WriteAllText(IntegratePath, MessagePackSerializer.ConvertToJson(data));
	}

	[ICommand]
	public void LoadLayout(object? sender)
	{
		if (sender is not FormMainWpf window) return;
		if (Path.GetExtension(LayoutPath) is ".zip")
		{
			Config.Life.LayoutFilePath = DefaultLayoutPath;
		}
		if (!File.Exists(LayoutPath)) return;

		DockingManager.Layout = new LayoutRoot();

		if (File.Exists(IntegratePath) && WindowCapture.WinformsControl is FormWindowCapture capture)
		{
			capture.CloseAll();

			string integrateString = File.ReadAllText(IntegratePath);
			byte[]? data = MessagePackSerializer.ConvertFromJson(integrateString);

			IEnumerable<FormIntegrate.WindowInfo> integrateWindows = MessagePackSerializer
				.Deserialize<IEnumerable<FormIntegrate.WindowInfo>>(data);

			foreach (FormIntegrate.WindowInfo info in integrateWindows)
			{
				// the constructor captures it so no need to call AddCapturedWindow
				FormIntegrate integrate = new(this, info);
				// capture.AddCapturedWindow(integrate);
			}

			capture.AttachAll();
		}

		try
		{
			XmlLayoutSerializer serializer = new(DockingManager);
			serializer.Deserialize(LayoutPath);
		}
		catch
		{
			if (MessageBox.Show(FormMain.LayoutLoadFailed, FormMain.LayoutLoadFailedTitle,
					MessageBoxButton.YesNo, MessageBoxImage.Error, MessageBoxResult.Yes)
				== MessageBoxResult.Yes)
			{
				ProcessStartInfo psi = new()
				{
					FileName = @"https://github.com/ElectronicObserverEN/ElectronicObserver/issues/71",
					UseShellExecute = true
				};
				Process.Start(psi);
			}

			throw;
		}

		if (File.Exists(PositionPath))
		{
			try
			{
				Position = JsonSerializer.Deserialize<WindowPosition>(File.ReadAllText(PositionPath)) ?? new WindowPosition();
			}
			catch
			{
				// couldn't get position, keep the default
			}
		}

		window.Top = Position.Top;
		window.Left = Position.Left;
		window.Width = Position.Width;
		window.Height = Position.Height;
		window.WindowState = Position.WindowState;

		SetAnchorableProperties();
	}

	private string LayoutFilter => "Layout File|*.xml";

	[ICommand]
	private void OpenLayout()
	{
		using OpenFileDialog dialog = new()
		{
			Filter = LayoutFilter,
			Title = Properties.Window.FormMain.OpenLayoutCaption
		};

		PathHelper.InitOpenFileDialog(Configuration.Config.Life.LayoutFilePath, dialog);

		if (dialog.ShowDialog(App.Current.MainWindow) != System.Windows.Forms.DialogResult.OK) return;

		string oldLayoutPath = Configuration.Config.Life.LayoutFilePath;
		Configuration.Config.Life.LayoutFilePath = PathHelper.GetPathFromOpenFileDialog(dialog);

		try
		{
			LoadLayout(Window);
		}
		catch
		{
			try
			{
				Configuration.Config.Life.LayoutFilePath = oldLayoutPath;
				LoadLayout(Window);
			}
			catch
			{
				// can't really do anything if the old layout is broken for some reason
			}
		}
	}

	[ICommand]
	private void SaveLayoutAs()
	{
		using SaveFileDialog dialog = new()
		{
			Filter = LayoutFilter,
			Title = Properties.Window.FormMain.SaveLayoutCaption
		};

		PathHelper.InitSaveFileDialog(Configuration.Config.Life.LayoutFilePath, dialog);

		if (dialog.ShowDialog(App.Current.MainWindow) != System.Windows.Forms.DialogResult.OK) return;

		Configuration.Config.Life.LayoutFilePath = PathHelper.GetPathFromSaveFileDialog(dialog);
		SaveLayout(Window);
	}

	[ICommand]
	private void SilenceNotifications()
	{
		foreach (var n in NotifierManager.Instance.GetNotifiers())
			n.IsSilenced = NotificationsSilenced;
	}

	[ICommand]
	private void OpenConfiguration()
	{
		UpdatePlayTime();

		using DialogConfiguration dialog = new(Configuration.Config);
		if (dialog.ShowDialog(App.Current.MainWindow) != System.Windows.Forms.DialogResult.OK) return;

		dialog.ToConfiguration(Configuration.Config);
		Configuration.Instance.OnConfigurationChanged();
	}

	#endregion

	#region View

	[ICommand]
	private void OpenView(AnchorableViewModel view)
	{
		view.Visibility = Visibility.Visible;
		view.IsSelected = true;
		view.IsActive = true;
	}

	[ICommand]
	public void CloseIntegrate(FormIntegrateViewModel integrate)
	{
		if (integrate.WinformsControl is FormIntegrate i)
		{
			i.Detach();
			if (WindowCapture.WinformsControl is FormWindowCapture capture)
			{
				capture.CapturedWindows.Remove(i);
			}
		}

		Views.Remove(integrate);

		// AvalonDock always hides anchorables, but integrate anchorables should always be closed
		List<LayoutAnchorable> integrateAnchorables = DockingManager.Layout.Hidden
			.Where(a => a.ContentId.StartsWith(FormIntegrate.Prefix))
			.ToList();

		foreach (LayoutAnchorable anchorable in integrateAnchorables)
		{
			DockingManager.Layout.Hidden.Remove(anchorable);
		}
	}

	[ICommand]
	private void StripMenu_WindowCapture_AttachAll_Click()
	{
		if (WindowCapture is not { WinformsControl: FormWindowCapture fwc }) return;

		fwc.AttachAll();
	}

	[ICommand]
	private void StripMenu_WindowCapture_DetachAll_Click()
	{
		if (WindowCapture is not { WinformsControl: FormWindowCapture fwc }) return;

		fwc.DetachAll();
	}

	#endregion

	#region Tools

	[ICommand]
	private void OpenEquipmentList(bool useNewVersion)
	{
		if (useNewVersion)
		{
			new EquipmentListWindow().Show(Window);
		}
		else
		{
			DialogEquipmentList equipmentList = new DialogEquipmentList();
			RefreshTopMost();
			equipmentList.Show(Window);
		}
	}

	[ICommand]
	private void OpenDropRecord(bool useNewVersion)
	{
		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButton.OK,
				MessageBoxImage.Error);
			return;
		}

		if (RecordManager.Instance.ShipDrop.Record.Count == 0)
		{
			MessageBox.Show(GeneralRes.NoDevData, Properties.Window.FormMain.ErrorCaption, MessageBoxButton.OK,
				MessageBoxImage.Error);
			return;
		}

		if (useNewVersion)
		{
			new DropRecordViewerWindow().Show(Window);
		}
		else
		{
			new DialogDropRecordViewer().Show(Window);
		}
	}

	[ICommand]
	private void OpenDevelopmentRecord(bool useNewVersion)
	{
		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButton.OK,
				MessageBoxImage.Error);
			return;
		}

		if (RecordManager.Instance.Development.Record.Count == 0)
		{
			MessageBox.Show(GeneralRes.NoDevData, Properties.Window.FormMain.ErrorCaption, MessageBoxButton.OK,
				MessageBoxImage.Error);
			return;
		}

		if (useNewVersion)
		{
			new DevelopmentRecordViewerWindow().Show(Window);
		}
		else
		{
			new DialogDevelopmentRecordViewer().Show(Window);
		}
	}

	[ICommand]
	private void OpenConstructionRecord(bool useNewVersion)
	{
		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show(GeneralRes.KancolleMustBeLoaded, GeneralRes.NoMasterData, MessageBoxButton.OK,
				MessageBoxImage.Error);
			return;
		}

		if (RecordManager.Instance.Construction.Record.Count == 0)
		{
			MessageBox.Show(GeneralRes.NoBuildData, Properties.Window.FormMain.ErrorCaption, MessageBoxButton.OK,
				MessageBoxImage.Error);
			return;
		}

		if (useNewVersion)
		{
			new ConstructionRecordViewerWindow().Show(Window);
		}
		else
		{
			new DialogConstructionRecordViewer().Show(Window);
		}
	}

	[ICommand]
	private void OpenResourceChart(bool useNewVersion)
	{
		if (useNewVersion)
		{
			new ResourceChartWPF().Show(Window);
		}
		else
		{
			new DialogResourceChart().Show(Window);
		}
	}

	[ICommand]
	private void OpenAlbumMasterShip()
	{

		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show(Properties.Window.FormMain.ShipDataNotLoaded, Properties.Window.FormMain.ErrorCaption,
				MessageBoxButton.OK, MessageBoxImage.Error);
			return;
		}

		DialogAlbumMasterShipWpf albumMasterShip = new();
		RefreshTopMost();
		albumMasterShip.Show(Window);
	}

	[ICommand]
	private void OpenAlbumMasterEquipment()
	{

		if (KCDatabase.Instance.MasterEquipments.Count == 0)
		{
			MessageBox.Show(Properties.Window.FormMain.EquipmentDataNotLoaded, Properties.Window.FormMain.ErrorCaption,
				MessageBoxButton.OK, MessageBoxImage.Error);
			return;
		}

		DialogAlbumMasterEquipmentWpf dialogAlbumMasterEquipment = new();
		RefreshTopMost();
		dialogAlbumMasterEquipment.Show(Window);
	}

	[ICommand]
	private void OpenAntiAirDefense(bool useNewVersion)
	{
		if (useNewVersion)
		{
			if (!KCDatabase.Instance.Fleet.IsAvailable)
			{
				MessageBox.Show
				(
					Properties.Window.Dialog.DialogAntiAirDefense.DataNotLoaded,
					Properties.Window.Dialog.DialogAntiAirDefense.Error,
					MessageBoxButton.OK, MessageBoxImage.Error
				);

				return;
			}

			new AirDefenseWindow(new()).Show(Window);
		}
		else
		{
			new DialogAntiAirDefense().Show(Window);
		}
	}

	[ICommand]
	private void OpenFleetImageGenerator()
	{
		new DialogFleetImageGenerator(1).Show(Window);
	}

	[ICommand]
	private void OpenBaseAirCorpsSimulation(bool useNewVersion)
	{
		if (useNewVersion)
		{
			ToolService.AirControlSimulator();
		}
		else
		{
			new DialogBaseAirCorpsSimulation().Show(Window);
		}
	}

	[ICommand]
	private void OpenExpChecker()
	{
		new DialogExpChecker().Show(Window);
	}

	[ICommand]
	private void OpenExpeditionCheck()
	{
		new DialogExpeditionCheck().Show(Window);
	}

	[ICommand]
	private void OpenKancolleProgress()
	{
		new DialogKancolleProgressWpf().Show(Window);
	}

	[ICommand]
	private void OpenExtraBrowser()
	{
		ElectronicObserver.Window.FormBrowserHost.Instance.Browser.OpenExtraBrowser();
	}

	[ICommand]
	private void OpenQuestTrackerManager()
	{
		if (!KCDatabase.Instance.Quest.IsLoaded)
		{
			MessageBox.Show(Properties.Window.FormMain.QuestDataNotLoaded, Properties.Window.FormMain.ErrorCaption,
				MessageBoxButton.OK, MessageBoxImage.Error);
			return;
		}

		new QuestTrackerManagerWindow().Show(Window);
	}

	[ICommand]
	private void OpenEventLockPlanner()
	{
		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show(Properties.Window.FormMain.ShipDataNotLoaded, Properties.Window.FormMain.ErrorCaption,
				MessageBoxButton.OK, MessageBoxImage.Error);
			return;
		}

		if (EventLockPlannerWindow is not null) return;

		EventLockPlannerViewModel viewModel = new(KCDatabase.Instance.Ships.Values, KCDatabase.Instance.Translation.Lock);
		EventLockPlannerWindow = new(viewModel);

		EventLockPlannerWindow.Closed += (sender, args) =>
		{
			EventLockPlannerWindow = null;
		};

		EventLockPlannerWindow.Show(Window);
	}

	[ICommand]
	private void OpenAutoRefresh()
	{
		if (AutoRefreshWindow is not null) return;

		AutoRefreshViewModel viewModel = Ioc.Default.GetRequiredService<AutoRefreshViewModel>();
		viewModel.Areas = KCDatabase.Instance.MapArea.Values.ToList();
		viewModel.Infos = KCDatabase.Instance.MapInfo.Values.ToList();

		AutoRefreshWindow = new(viewModel);

		AutoRefreshWindow.Closed += (sender, args) =>
		{
			AutoRefreshWindow = null;
		};

		AutoRefreshWindow.Show(Window);
	}

	#endregion

	#region Debug

	[ICommand]
	private void LoadAPIFromFile()
	{

		/*/
		using ( var dialog = new DialogLocalAPILoader() ) {

			if ( dialog.ShowDialog( this ) == System.Windows.Forms.DialogResult.OK ) {
				if ( APIObserver.Instance.APIList.ContainsKey( dialog.APIName ) ) {

					if ( dialog.IsResponse ) {
						APIObserver.Instance.LoadResponse( dialog.APIPath, dialog.FileData );
					}
					if ( dialog.IsRequest ) {
						APIObserver.Instance.LoadRequest( dialog.APIPath, dialog.FileData );
					}

				}
			}
		}
		/*/
		new DialogLocalAPILoader2().Show(Window);
		//*/
	}

	[ICommand]
	private async void LoadInitialAPI()
	{
		using OpenFileDialog ofd = new();

		ofd.Title = "Load API List";
		ofd.Filter = "API List|*.txt|File|*";
		ofd.InitialDirectory = Utility.Configuration.Config.Connection.SaveDataPath;
		if (!string.IsNullOrWhiteSpace(Utility.Configuration.Config.Debug.APIListPath))
			ofd.FileName = Utility.Configuration.Config.Debug.APIListPath;

		if (ofd.ShowDialog(App.Current.MainWindow) == System.Windows.Forms.DialogResult.OK)
		{

			try
			{

				await Task.Factory.StartNew(() => LoadAPIList(ofd.FileName));

			}
			catch (Exception ex)
			{

				MessageBox.Show("Failed to load API List.\r\n" + ex.Message, Properties.Window.FormMain.ErrorCaption,
					MessageBoxButton.OK, MessageBoxImage.Error);

			}

		}
	}

	[ICommand]
	private void LoadAPIList(string path)
	{

		string parent = Path.GetDirectoryName(path);

		using StreamReader sr = new(path);
		string line;
		while ((line = sr.ReadLine()) != null)
		{

			bool isRequest = false;
			{
				int slashindex = line.IndexOf('/');
				if (slashindex != -1)
				{

					switch (line.Substring(0, slashindex).ToLower())
					{
						case "q":
						case "request":
							isRequest = true;
							goto case "s";
						case "":
						case "s":
						case "response":
							line = line.Substring(Math.Min(slashindex + 1, line.Length));
							break;
					}

				}
			}

			if (APIObserver.Instance.APIList.ContainsKey(line))
			{
				APIBase api = APIObserver.Instance.APIList[line];

				if (isRequest ? api.IsRequestSupported : api.IsResponseSupported)
				{

					string[] files = Directory.GetFiles(parent,
						string.Format("*{0}@{1}.json", isRequest ? "Q" : "S", line.Replace('/', '@')),
						SearchOption.TopDirectoryOnly);

					if (files.Length == 0)
						continue;

					Array.Sort(files);

					using StreamReader sr2 = new(files[files.Length - 1]);
					if (isRequest)
					{
						Window.Dispatcher.Invoke((Action)(() =>
						{
							APIObserver.Instance.LoadRequest("/kcsapi/" + line, sr2.ReadToEnd());
						}));
					}
					else
					{
						Window.Dispatcher.Invoke((Action)(() =>
						{
							APIObserver.Instance.LoadResponse("/kcsapi/" + line, sr2.ReadToEnd());
						}));
					}

					//System.Diagnostics.Debug.WriteLine( "APIList Loader: API " + line + " File " + files[files.Length-1] + " Loaded." );
				}
			}
		}
	}

	[ICommand]
	private void LoadRecordFromOld()
	{

		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show("Please load normal api_start2 first.", Properties.Window.FormMain.ErrorCaption,
				MessageBoxButton.OK,
				MessageBoxImage.Information);
			return;
		}


		using OpenFileDialog ofd = new();

		ofd.Title = "Build Record from Old api_start2";
		ofd.Filter = "api_start2|*api_start2*.json|JSON|*.json|File|*";

		if (ofd.ShowDialog(App.Current.MainWindow) == System.Windows.Forms.DialogResult.OK)
		{

			try
			{
				using StreamReader sr = new(ofd.FileName);
				dynamic json = JsonObject.Parse(sr.ReadToEnd().Remove(0, 7));

				foreach (dynamic elem in json.api_data.api_mst_ship)
				{
					if (elem.api_name != "なし" && KCDatabase.Instance.MasterShips.ContainsKey((int)elem.api_id) &&
						KCDatabase.Instance.MasterShips[(int)elem.api_id].Name == elem.api_name)
					{
						RecordManager.Instance.ShipParameter.UpdateParameter((int)elem.api_id, 1,
							(int)elem.api_tais[0], (int)elem.api_tais[1], (int)elem.api_kaih[0],
							(int)elem.api_kaih[1], (int)elem.api_saku[0], (int)elem.api_saku[1]);

						int[] defaultslot = Enumerable.Repeat(-1, 5).ToArray();
						((int[])elem.api_defeq).CopyTo(defaultslot, 0);
						RecordManager.Instance.ShipParameter.UpdateDefaultSlot((int)elem.api_id, defaultslot);
					}
				}
			}
			catch (Exception ex)
			{

				MessageBox.Show("Failed to load API.\r\n" + ex.Message, Properties.Window.FormMain.ErrorCaption,
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}

	[ICommand]
	private void LoadDataFromOld()
	{

		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show("Please load normal api_start2 first.", Properties.Window.FormMain.ErrorCaption,
				MessageBoxButton.OK,
				MessageBoxImage.Information);
			return;
		}


		using OpenFileDialog ofd = new();

		ofd.Title = "Restore Abyssal Data from Old api_start2";
		ofd.Filter = "api_start2|*api_start2*.json|JSON|*.json|File|*";
		ofd.InitialDirectory = Utility.Configuration.Config.Connection.SaveDataPath;

		if (ofd.ShowDialog(App.Current.MainWindow) == System.Windows.Forms.DialogResult.OK)
		{

			try
			{

				using (StreamReader sr = new(ofd.FileName))
				{

					dynamic json = JsonObject.Parse(sr.ReadToEnd().Remove(0, 7));

					foreach (dynamic elem in json.api_data.api_mst_ship)
					{

						var ship = KCDatabase.Instance.MasterShips[(int)elem.api_id];

						if (elem.api_name != "なし" && ship != null && ship.IsAbyssalShip)
						{

							KCDatabase.Instance.MasterShips[(int)elem.api_id].LoadFromResponse("api_start2", elem);
						}
					}
				}

				Utility.Logger.Add(1, "Restored data from old api_start2");

			}
			catch (Exception ex)
			{

				MessageBox.Show("Failed to load API.\r\n" + ex.Message, Properties.Window.FormMain.ErrorCaption,
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}

	[ICommand]
	private async void DeleteOldAPI()
	{

		if (MessageBox.Show("This will delete old API data.\r\nAre you sure?", "Confirmation",
				MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No)
			== MessageBoxResult.Yes)
		{

			try
			{

				int count = await Task.Factory.StartNew(() => DeleteOldAPIInternal());

				MessageBox.Show("Delete successful.\r\n" + count + " files deleted.", "Delete Successful",
					MessageBoxButton.OK, MessageBoxImage.Information);

			}
			catch (Exception ex)
			{

				MessageBox.Show("Failed to delete.\r\n" + ex.Message, Properties.Window.FormMain.ErrorCaption,
					MessageBoxButton.OK,
					MessageBoxImage.Error);
			}


		}

	}

	private int DeleteOldAPIInternal()
	{


		//適当極まりない
		int count = 0;

		var apilist = new Dictionary<string, List<KeyValuePair<string, string>>>();

		foreach (string s in Directory.EnumerateFiles(Utility.Configuration.Config.Connection.SaveDataPath,
			"*.json", SearchOption.TopDirectoryOnly))
		{

			int start = s.IndexOf('@');
			int end = s.LastIndexOf('.');

			start--;
			string key = s.Substring(start, end - start + 1);
			string date = s.Substring(0, start);


			if (!apilist.ContainsKey(key))
			{
				apilist.Add(key, new List<KeyValuePair<string, string>>());
			}

			apilist[key].Add(new KeyValuePair<string, string>(date, s));
		}

		foreach (var l in apilist.Values)
		{
			var l2 = l.OrderBy(el => el.Key).ToList();
			for (int i = 0; i < l2.Count - 1; i++)
			{
				File.Delete(l2[i].Value);
				count++;
			}
		}

		return count;
	}

	[ICommand]
	private async void RenameShipResource()
	{

		if (KCDatabase.Instance.MasterShips.Count == 0)
		{
			MessageBox.Show("Ship data is not loaded.", Properties.Window.FormMain.ErrorCaption, MessageBoxButton.OK,
				MessageBoxImage.Error);
			return;
		}

		if (MessageBox.Show("通信から保存した艦船リソース名を持つファイル及びフォルダを、艦船名に置換します。\r\n" +
							"対象は指定されたフォルダ以下のすべてのファイル及びフォルダです。\r\n" +
							"続行しますか？", "艦船リソースをリネーム", MessageBoxButton.YesNo, MessageBoxImage.Question,
				MessageBoxResult.Yes)
			== MessageBoxResult.Yes)
		{

			string path = null;

			using (FolderBrowserDialog dialog = new())
			{
				dialog.SelectedPath = Configuration.Config.Connection.SaveDataPath;
				if (dialog.ShowDialog(App.Current.MainWindow) == DialogResult.OK)
				{
					path = dialog.SelectedPath;
				}
			}

			if (path == null) return;



			try
			{

				int count = await Task.Factory.StartNew(() => RenameShipResource(path));

				MessageBox.Show(string.Format("リネーム処理が完了しました。\r\n{0} 個のアイテムをリネームしました。", count), "処理完了",
					MessageBoxButton.OK, MessageBoxImage.Information);


			}
			catch (Exception ex)
			{

				Utility.ErrorReporter.SendErrorReport(ex, "艦船リソースのリネームに失敗しました。");
				MessageBox.Show("艦船リソースのリネームに失敗しました。\r\n" + ex.Message, Properties.Window.FormMain.ErrorCaption,
					MessageBoxButton.OK,
					MessageBoxImage.Error);

			}



		}

	}

	private int RenameShipResource(string path)
	{

		int count = 0;

		foreach (var p in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
		{

			string name = Path.GetFileName(p);

			foreach (var ship in KCDatabase.Instance.MasterShips.Values)
			{

				if (name.Contains(ship.ResourceName))
				{

					name = name.Replace(ship.ResourceName,
						string.Format("{0}({1})", ship.NameWithClass, ship.ShipID)).Replace(' ', '_');

					try
					{

						File.Move(p, Path.Combine(Path.GetDirectoryName(p), name));
						count++;
						break;

					}
					catch (IOException)
					{
						//ファイルが既に存在する：＊にぎりつぶす＊
					}

				}

			}

		}

		foreach (var p in Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories))
		{

			string name = Path.GetFileName(p); //GetDirectoryName だと親フォルダへのパスになってしまうため

			foreach (var ship in KCDatabase.Instance.MasterShips.Values)
			{

				if (name.Contains(ship.ResourceName))
				{

					name = name.Replace(ship.ResourceName,
						string.Format("{0}({1})", ship.NameWithClass, ship.ShipID)).Replace(' ', '_');

					try
					{

						Directory.Move(p, Path.Combine(Path.GetDirectoryName(p), name));
						count++;
						break;

					}
					catch (IOException)
					{
						//フォルダが既に存在する：＊にぎりつぶす＊
					}
				}

			}

		}


		return count;
	}

	[ICommand]
	private async Task GenerateMasterData()
	{
#if DEBUG
		void GetMissingDataFromWiki(IShipDataMaster ship, Dictionary<ShipId, IShipDataMaster> wikiShips)
		{
			if (!wikiShips.TryGetValue(ship.ShipId, out IShipDataMaster wikiShip)) return;

			if (!ship.ASW.IsDetermined)
			{
				if (wikiShip.ASW.Minimum >= 0)
				{
					ship.ASW.MinimumEstMin = wikiShip.ASW.Minimum;
					ship.ASW.MinimumEstMax = wikiShip.ASW.Minimum;
				}

				if (wikiShip.ASW.Maximum >= 0)
				{
					ship.ASW.Maximum = wikiShip.ASW.Maximum;
				}
			}

			if (!ship.LOS.IsDetermined)
			{
				if (wikiShip.LOS.Minimum >= 0)
				{
					ship.LOS.MinimumEstMin = wikiShip.LOS.Minimum;
					ship.LOS.MinimumEstMax = wikiShip.LOS.Minimum;
				}

				if (wikiShip.LOS.Maximum >= 0)
				{
					ship.LOS.Maximum = wikiShip.LOS.Maximum;
				}
			}

			if (!ship.Evasion.IsDetermined)
			{
				if (wikiShip.Evasion.Minimum >= 0)
				{
					ship.Evasion.MinimumEstMin = wikiShip.Evasion.Minimum;
					ship.Evasion.MinimumEstMax = wikiShip.Evasion.Minimum;
				}

				if (wikiShip.Evasion.Maximum >= 0)
				{
					ship.Evasion.Maximum = wikiShip.Evasion.Maximum;
				}
			}

			if (wikiShip.DefaultSlot is not null)
			{
				RecordManager.Instance.ShipParameter.UpdateDefaultSlot(ship.ShipID, wikiShip.DefaultSlot.ToArray());
			}
		}

		void GetMissingAbyssalDataFromWiki(IShipDataMaster ship, Dictionary<ShipId, IShipDataMaster> wikiAbyssalShips)
		{
			if (!wikiAbyssalShips.TryGetValue(ship.ShipId, out IShipDataMaster wikiShip)) return;

			if (!ship.ASW.IsDetermined)
			{
				if (wikiShip.ASW.Minimum >= 0)
				{
					ship.ASW.MinimumEstMin = wikiShip.ASW.MinimumEstMin;
					ship.ASW.MinimumEstMax = wikiShip.ASW.MinimumEstMax;
				}

				if (wikiShip.ASW.Maximum >= 0)
				{
					ship.ASW.Maximum = wikiShip.ASW.Maximum;
				}
			}

			if (!ship.LOS.IsDetermined)
			{
				if (wikiShip.LOS.Minimum >= 0)
				{
					ship.LOS.MinimumEstMin = wikiShip.LOS.MinimumEstMin;
					ship.LOS.MinimumEstMax = wikiShip.LOS.MinimumEstMax;
				}

				if (wikiShip.LOS.Maximum >= 0)
				{
					ship.LOS.Maximum = wikiShip.LOS.Maximum;
				}
			}

			if (!ship.Evasion.IsDetermined)
			{
				if (wikiShip.Evasion.Minimum >= 0)
				{
					ship.Evasion.MinimumEstMin = wikiShip.Evasion.MinimumEstMin;
					ship.Evasion.MinimumEstMax = wikiShip.Evasion.MinimumEstMax;
				}

				if (wikiShip.Evasion.Maximum >= 0)
				{
					ship.Evasion.Maximum = wikiShip.Evasion.Maximum;
				}
			}

			if (wikiShip.DefaultSlot is not null)
			{
				RecordManager.Instance.ShipParameter.UpdateDefaultSlot(ship.ShipID, wikiShip.DefaultSlot.ToArray());
			}
			RecordManager.Instance.ShipParameter.UpdateAircraft(ship.ShipID, wikiShip.Aircraft.ToArray());

			ShipParameterRecord.ShipParameterElement e = RecordManager.Instance.ShipParameter[ship.ShipID] ?? new();

			e.HPMin = wikiShip.HPMin;
			e.HPMax = wikiShip.HPMax;
			e.FirepowerMin = wikiShip.FirepowerMin;
			e.FirepowerMax = wikiShip.FirepowerMax;
			e.TorpedoMin = wikiShip.TorpedoMin;
			e.TorpedoMax = wikiShip.TorpedoMax;
			e.AAMin = wikiShip.AAMin;
			e.AAMax = wikiShip.AAMax;
			e.ArmorMin = wikiShip.ArmorMin;
			e.ArmorMax = wikiShip.ArmorMax;
			e.LuckMin = wikiShip.LuckMin;
			e.LuckMax = wikiShip.LuckMax;

			RecordManager.Instance.ShipParameter.Update(e);
		}

		Directory.CreateDirectory(GeneratedDataFolder);

		await using TestData.TestDataContext db = new();
		await db.Database.MigrateAsync();

		await db.Database.ExecuteSqlRawAsync($"DELETE FROM [{nameof(TestData.TestDataContext.MasterShips)}]");
		await db.Database.ExecuteSqlRawAsync($"DELETE FROM [{nameof(TestData.TestDataContext.MasterEquipment)}]");

		List<IEquipmentDataMaster> wikiEquipment = TestData.Wiki.WikiDataParser.Equipment();
		List<IEquipmentDataMaster> wikiAbyssalEquipment = TestData.Wiki.WikiDataParser.AbyssalEquipment();

		Dictionary<ShipId, IShipDataMaster> wikiShips = TestData.Wiki.WikiDataParser.Ships(wikiEquipment);
		Dictionary<ShipId, IShipDataMaster> wikiAbyssalShips = TestData.Wiki.WikiDataParser.AbyssalShips(wikiAbyssalEquipment);

		foreach (ShipDataMaster ship in KCDatabase.Instance.MasterShips.Values)
		{
			if (ship.IsAbyssalShip)
			{
				GetMissingAbyssalDataFromWiki(ship, wikiAbyssalShips);
			}
			else
			{
				GetMissingDataFromWiki(ship, wikiShips);
			}
			await db.MasterShips.AddAsync(new(ship));
		}

		foreach (EquipmentDataMaster equipment in KCDatabase.Instance.MasterEquipments.Values)
		{
			await db.MasterEquipment.AddAsync(new(equipment));
		}

		await db.SaveChangesAsync();

		var masterShips = db.MasterShips.ToList();
		var masterEquipment = db.MasterEquipment.ToList();

		JsonSerializerOptions options = new()
		{
			WriteIndented = true,
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
		};

		await File.WriteAllTextAsync(Path.Combine(GeneratedDataFolder, "ships.json"), JsonSerializer.Serialize(masterShips, options));
		await File.WriteAllTextAsync(Path.Combine(GeneratedDataFolder, "equipment.json"), JsonSerializer.Serialize(masterEquipment, options));
#endif
	}

	[ICommand]
	private void GenerateShipIdEnum()
	{
		static string CleanName(string name) => name
			.Replace(" ", "")
			.Replace(")", "")
			.Replace("-", "")
			.Replace("/", "")
			.Replace(".", "")
			.Replace("(", "")
			.Replace("+", "")
			.Replace("'", "");

		List<string> enumValues = KCDatabase.Instance.MasterShips.Values
			.Where(e => !e.IsAbyssalShip)
			.Select(s => (Name: CleanName(s.NameEN), Id: s.ShipID))
			.GroupBy(t => t.Name)
			.SelectMany(g => g.Count() switch
			{
				// if the name is the same, append the ID (Souya)
				> 1 => g.Select(t => (Name: $"{t.Name}{t.Id}", t.Id)).ToList(),
				1 => new List<(string Name, int Id)> { (g.First().Name, g.First().Id) }
			})
			.OrderBy(t => t.Id)
			.Select(t => $"{t.Name} = {t.Id}")
			.ToList();

		System.Windows.Clipboard.SetText(string.Join(",\n", enumValues));
	}

	[ICommand]
	private void GenerateEquipmentIdEnum()
	{
		static string CleanName(string name) => name
			.Replace(" ", "")
			.Replace(")", "")
			.Replace("-", "")
			.Replace("/", "")
			.Replace(".", "_")
			.Replace("(", "_")
			.Replace("&", "_")
			.Replace("+", "_");

		List<string> enumValues = KCDatabase.Instance.MasterEquipments.Values
			.Where(e => !e.IsAbyssalEquipment)
			.Select(eq => $"{eq.CategoryType}_{CleanName(eq.NameEN)} = {eq.ID}")
			.ToList();

		System.Windows.Clipboard.SetText(string.Join(",\n", enumValues));
	}

	#endregion

	#region Help

	[ICommand]
	private void ViewHelp()
	{

		if (MessageBox.Show(Properties.Window.FormMain.OpenEOWiki, Properties.Window.FormMain.HelpCaption,
				MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes)
			== MessageBoxResult.Yes)
		{
			ProcessStartInfo psi = new()
			{
				FileName = "https://github.com/silfumus/ElectronicObserver/wiki",
				UseShellExecute = true
			};
			Process.Start(psi);
		}

	}

	[ICommand]
	private void ReportIssue()
	{

		if (MessageBox.Show(Properties.Window.FormMain.ReportIssue, Properties.Window.FormMain.ReportIssueCaption,
				MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes)
			== MessageBoxResult.Yes)
		{
			ProcessStartInfo psi = new()
			{
				FileName = "https://github.com/ElectronicObserverEN/ElectronicObserver/issues",
				UseShellExecute = true
			};
			Process.Start(psi);
		}

	}

	[ICommand]
	private void JoinDiscord()
	{
		try
		{
			ProcessStartInfo psi = new()
			{
				FileName = @"https://discord.gg/6ZvX8DG",
				UseShellExecute = true
			};
			Process.Start(psi);
		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, Properties.Window.FormMain.FailedToOpenBrowser);
		}
	}

	[ICommand]
	private async void CheckForUpdate()
	{
		// translations and maintenance state
		await SoftwareUpdater.CheckUpdateAsync();
		// EO
		SoftwareInformation.CheckUpdate();
	}

	[ICommand]
	private void ViewVersion()
	{
		VersionInformationWindow? window = new VersionInformationWindow();
		window.ShowDialog(Window);
	}

	#endregion

	private void CallPumpkinHead(string apiname, dynamic data)
	{
		new DialogHalloween().Show(Window);
		APIObserver.Instance.ApiPort_Port.ResponseReceived -= CallPumpkinHead;
	}


	void Logger_LogAdded(Utility.Logger.LogData data)
	{
		// bottom bar
		StripStatus.Information = data.Message.Replace("\r", " ").Replace("\n", " ");
	}

	private void ConfigurationChanged()
	{
		var c = Configuration.Config;

		DebugEnabled = c.Debug.EnableDebugMenu;

		StripStatus.Visible = c.Life.ShowStatusBar;

		// Load で TopMost を変更するとバグるため(前述)
		if (UIUpdateTimer.Enabled)
			Topmost = c.Life.TopMost;


		ClockFormat = c.Life.ClockFormat;
		SetTheme();

		/*
		//StripMenu.Font = Font;
		StripStatus.Font = Font;
		MainDockPanel.Skin.AutoHideStripSkin.TextFont = Font;
		MainDockPanel.Skin.DockPaneStripSkin.TextFont = Font;

		foreach (var f in SubForms)
		{
			f.BackColor = this.BackColor;
			f.ForeColor = this.ForeColor;
			if (f is FormShipGroup)
			{ // 暂时不对舰队编成窗口应用主题
				f.BackColor = SystemColors.Control;
				f.ForeColor = SystemColors.ControlText;
			}
		}*/

		if (FormShipGroup.ShipGroup is not null)
		{
			FormShipGroup.ShipGroup.BackColor = System.Drawing.SystemColors.Control;
			FormShipGroup.ShipGroup.ForeColor = System.Drawing.SystemColors.ControlText;
		}

		/*

		StripStatus_Information.BackColor = System.Drawing.Color.Transparent;
		StripStatus_Information.Margin = new Padding(-1, 1, -1, 0);


		if (c.Life.LockLayout)
		{
			MainDockPanel.AllowChangeLayout = false;
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		}
		else
		{
			MainDockPanel.AllowChangeLayout = true;
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
		}

		StripMenu_File_Layout_LockLayout.Checked = c.Life.LockLayout;
		MainDockPanel.CanCloseFloatWindowInLock = c.Life.CanCloseFloatWindowInLock;
		*/
		LockLayout = c.Life.LockLayout;
		CanChangeGridSplitterSize = !LockLayout;
		GridSplitterSize = LockLayout switch
		{
			true => 0,
			_ => 1
		};
		SetAnchorableProperties();
		Topmost = c.Life.TopMost;
		/*
		StripMenu_File_Notification_MuteAll.Checked = Notifier.NotifierManager.Instance.GetNotifiers().All(n => n.IsSilenced);

		if (!c.Control.UseSystemVolume)
			_volumeUpdateState = -1;
		*/
	}

	private void SetAnchorableProperties()
	{
		foreach (AnchorableViewModel view in Views)
		{
			view.CanFloat = !LockLayout;
			view.CanClose = !LockLayout;
		}
	}

	private void SetFont()
	{
		Font = new FontFamily(Config.UI.MainFont.FontData.FontFamily.Name);
		FontSize = Config.UI.MainFont.FontData.ToSize();
		FontBrush = Config.UI.ForeColor.ToBrush();

		SubFont = new FontFamily(Config.UI.SubFont.FontData.FontFamily.Name);
		SubFontSize = Config.UI.SubFont.FontData.ToSize();
		SubFontBrush = Config.UI.SubForeColor.ToBrush();
	}

	private void SetTheme()
	{
		// todo switching themes doesn't update everything in runtime
		Utility.Configuration.Instance.ApplyTheme();

		BackgroundColor = Config.UI.BackColor.ToBrush().Color;

		CurrentTheme = Utility.Configuration.Config.UI.ThemeMode switch
		{
			0 => Themes[0], // light theme => light theme
			1 => Themes[2], // dark theme => dark theme
			_ => Themes[2], // custom theme => dark theme
		};

		ThemeManager.Current.ApplicationTheme = Utility.Configuration.Config.UI.ThemeMode switch
		{
			0 => ApplicationTheme.Light, // light theme => light theme
			1 => ApplicationTheme.Dark, // dark theme => dark theme
			_ => ApplicationTheme.Dark, // custom theme => dark theme
		};

		SetFont();
	}

	private void UIUpdateTimer_Tick(object sender, EventArgs e)
	{

		SystemEvents.OnUpdateTimerTick();

		// 東京標準時
		DateTime now = Utility.Mathematics.DateTimeHelper.GetJapanStandardTimeNow();

		switch (ClockFormat)
		{
			case 0: //時計表示
				var pvpReset = now.Date.AddHours(3);
				while (pvpReset < now)
					pvpReset = pvpReset.AddHours(12);
				var pvpTimer = pvpReset - now;

				var questReset = now.Date.AddHours(5);
				if (questReset < now)
					questReset = questReset.AddHours(24);
				var questTimer = questReset - now;

				TimeSpan maintTimer = new(0);
				MaintenanceState eventState = SoftwareUpdater.LatestVersion.EventState;
				DateTime maintDate = SoftwareUpdater.LatestVersion.MaintenanceDate;

				if (eventState != MaintenanceState.None)
				{
					if (maintDate < now)
						maintDate = now;
					maintTimer = maintDate - now;
				}

				bool eventOrMaintenanceStarted = maintDate <= now;

				string message = (eventState, eventOrMaintenanceStarted) switch
				{
					(MaintenanceState.EventStart, false) => FormMain.EventStartsIn,
					(MaintenanceState.EventStart, _) => FormMain.EventHasStarted,

					(MaintenanceState.EventEnd, false) => FormMain.EventEndsIn,
					(MaintenanceState.EventEnd, _) => FormMain.EventHasEnded,

					(MaintenanceState.Regular, false) => FormMain.MaintenanceStartsIn,
					(MaintenanceState.Regular, _) => FormMain.MaintenanceHasStarted,

					_ => string.Empty,
				};

				string maintState = eventOrMaintenanceStarted switch
				{
					false => string.Format(message, maintTimer.ToString("dd\\ hh\\:mm\\:ss")),
					_ => message
				};

				var resetMsg =
					$"{FormMain.NextExerciseReset} {pvpTimer:hh\\:mm\\:ss}\r\n" +
					$"{FormMain.NextQuestReset} {questTimer:hh\\:mm\\:ss}\r\n" +
					$"{maintState}";

				StripStatus.Clock = now.ToString("HH\\:mm\\:ss");
				StripStatus.ClockToolTip = now.ToString("yyyy\\/MM\\/dd (ddd)\r\n") + resetMsg;

				break;

			case 1: //演習更新まで
			{
				var border = now.Date.AddHours(3);
				while (border < now)
					border = border.AddHours(12);

				var ts = border - now;
				StripStatus.Clock = ts.ToString("hh\\:mm\\:ss");
				StripStatus.ClockToolTip = now.ToString("yyyy\\/MM\\/dd (ddd) HH\\:mm\\:ss");

			}
			break;

			case 2: //任務更新まで
			{
				var border = now.Date.AddHours(5);
				if (border < now)
					border = border.AddHours(24);

				var ts = border - now;
				StripStatus.Clock = ts.ToString("hh\\:mm\\:ss");
				StripStatus.ClockToolTip = now.ToString("yyyy\\/MM\\/dd (ddd) HH\\:mm\\:ss");

			}
			break;
		}

		/*
		// WMP コントロールによって音量が勝手に変えられてしまうため、前回終了時の音量の再設定を試みる。
		// 10回試行してダメなら諦める(例外によるラグを防ぐため)
		// 起動直後にやらないのはちょっと待たないと音量設定が有効にならないから
		if (_volumeUpdateState != -1 && _volumeUpdateState < 10 && Utility.Configuration.Config.Control.UseSystemVolume)
		{

			try
			{
				uint id = (uint)System.Diagnostics.Process.GetCurrentProcess().Id;
				float volume = Utility.Configuration.Config.Control.LastVolume;
				bool mute = Utility.Configuration.Config.Control.LastIsMute;

				BrowserLibCore.VolumeManager.SetApplicationVolume(id, volume);
				BrowserLibCore.VolumeManager.SetApplicationMute(id, mute);

				SyncBGMPlayer.Instance.SetInitialVolume((int)(volume * 100));
				foreach (var not in NotifierManager.Instance.GetNotifiers())
					not.SetInitialVolume((int)(volume * 100));

				_volumeUpdateState = -1;

			}
			catch (Exception)
			{

				_volumeUpdateState++;
			}
		}
		*/
	}

	private void UpdatePlayTime()
	{
		var c = Configuration.Config.Log;
		DateTime now = DateTime.Now;

		double span = (now - PrevPlayTimeRecorded).TotalSeconds;
		if (span < c.PlayTimeIgnoreInterval)
		{
			c.PlayTime += span;
		}

		PrevPlayTimeRecorded = now;
	}

	[ICommand]
	private void Closing(CancelEventArgs e)
	{
		string name = CultureInfo.CurrentCulture.Name switch
		{
			"en-US" => SoftwareInformation.SoftwareNameEnglish,
			_ => SoftwareInformation.SoftwareNameJapanese
		};

		if (Configuration.Config.Life.ConfirmOnClosing)
		{
			if (MessageBox.Show(
					string.Format(Properties.Window.FormMain.ExitConfirmation, name),
					Properties.Window.FormMain.ConfirmatonCaption,
					MessageBoxButton.YesNo,
					MessageBoxImage.Question,
					MessageBoxResult.No)
				== MessageBoxResult.No)
			{
				e.Cancel = true;
				return;
			}
		}

		Logger.Add(2, name + Resources.IsClosing);

		UIUpdateTimer.Stop();

		if (FormBrowserHost.WinformsControl is FormBrowserHost host)
		{
			host.CloseBrowser();
		}

		UpdatePlayTime();
		SystemEvents.OnSystemShuttingDown();

		// SaveLayout(Configuration.Config.Life.LayoutFilePath);

		// 音量の保存
		{
			try
			{
				uint id = (uint)Process.GetCurrentProcess().Id;
				Configuration.Config.Control.LastVolume = BrowserLibCore.VolumeManager.GetApplicationVolume(id);
				Configuration.Config.Control.LastIsMute = BrowserLibCore.VolumeManager.GetApplicationMute(id);

			}
			catch (Exception)
			{
				/* ぷちっ */
			}

		}
	}

	[ICommand]
	private void Closed()
	{
		NotifierManager.Instance.ApplyToConfiguration();
		Configuration.Instance.Save();
		RecordManager.Instance.SavePartial();
		KCDatabase.Instance.Save();
		APIObserver.Instance.Stop();

		Logger.Add(2, Resources.ClosingComplete);

		if (Configuration.Config.Log.SaveLogFlag)
			Logger.Save();
	}
}
