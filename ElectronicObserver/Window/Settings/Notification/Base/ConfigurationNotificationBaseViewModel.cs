﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Notifier;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.Notification.Base;

public partial class ConfigurationNotificationBaseViewModel : ObservableValidator, IDisposable
{
	public ConfigurationNotificationBaseTranslationViewModel Translation { get; }
	private FileService FileService { get; }

	private Configuration.ConfigurationData.ConfigNotifierBase Config { get; }
	protected virtual NotifierBase NotifierBase { get; }

	public List<CheckBoxEnumViewModel> ClickFlags { get; }
	public List<NotifierDialogAlignment> DialogAlignments { get; }

	public bool IsEnabled { get; set; }

	public bool IsSilenced { get; set; }

	public bool ShowsDialog { get; set; }

	[ObservableProperty]
	[CustomValidation(typeof(ConfigurationNotificationBaseViewModel), nameof(ValidateImagePath))]
	private string _imagePath;

	public bool DrawsImage { get; set; }

	[ObservableProperty]
	[CustomValidation(typeof(ConfigurationNotificationBaseViewModel), nameof(ValidateSoundPath))]
	private string _soundPath;

	public bool PlaysSound { get; set; }

	public int SoundVolume { get; set; }

	public bool LoopsSound { get; set; }

	public bool DrawsMessage { get; set; }

	public int ClosingInterval { get; set; }

	public int AccelInterval { get; set; }

	public bool CloseOnMouseMove { get; set; }

	public NotifierDialogClickFlags ClickFlag { get; set; }

	public NotifierDialogAlignment Alignment { get; set; }

	public int LocationX { get; set; }

	public int LocationY { get; set; }

	public bool HasFormBorder { get; set; }

	public bool TopMost { get; set; }

	public bool ShowWithActivation { get; set; }

	public Color ForeColor { get; set; }

	public Color BackColor { get; set; }

	private bool SoundChanged { get; set; }

	private bool ImageChanged { get; set; }

	public ConfigurationNotificationBaseViewModel(
		Configuration.ConfigurationData.ConfigNotifierBase config,
		NotifierBase notifier)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationNotificationBaseTranslationViewModel>();
		FileService = Ioc.Default.GetRequiredService<FileService>();

		ClickFlags = Enum.GetValues<NotifierDialogClickFlags>()
			.Where(f => f is not (NotifierDialogClickFlags.None or NotifierDialogClickFlags.HighestBit))
			.Select(f => new CheckBoxEnumViewModel(f))
			.ToList();

		DialogAlignments = Enum.GetValues<NotifierDialogAlignment>().ToList();

		Config = config;
		NotifierBase = notifier;

		foreach (CheckBoxEnumViewModel clickFlag in ClickFlags)
		{
			clickFlag.PropertyChanged += (sender, args) =>
			{
				if (args.PropertyName is not nameof(CheckBoxEnumViewModel.IsChecked)) return;
				if (clickFlag.Value is not NotifierDialogClickFlags clickFlagValue) return;

				ClickFlag ^= clickFlagValue;
			};
		}

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SoundPath)) return;

			SoundChanged = true;
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ImagePath)) return;

			ImageChanged = true;
		};
	}

	public virtual void Load()
	{
		IsEnabled = NotifierBase.IsEnabled;
		IsSilenced = NotifierBase.IsSilenced;
		ShowsDialog = NotifierBase.ShowsDialog;
		ImagePath = NotifierBase.DialogData.ImagePath;
		DrawsImage = NotifierBase.DialogData.DrawsImage;
		SoundPath = NotifierBase.SoundPath;
		PlaysSound = NotifierBase.PlaysSound;
		SoundVolume = NotifierBase.SoundVolume;
		LoopsSound = NotifierBase.LoopsSound;
		DrawsMessage = NotifierBase.DialogData.DrawsMessage;
		ClosingInterval = NotifierBase.DialogData.ClosingInterval / 1000;
		AccelInterval = NotifierBase.AccelInterval / 1000;
		CloseOnMouseMove = NotifierBase.DialogData.CloseOnMouseMove;
		ClickFlag = NotifierBase.DialogData.ClickFlag;
		Alignment = NotifierBase.DialogData.Alignment;
		LocationX = NotifierBase.DialogData.Location.X;
		LocationY = NotifierBase.DialogData.Location.Y;
		HasFormBorder = NotifierBase.DialogData.HasFormBorder;
		TopMost = NotifierBase.DialogData.TopMost;
		ShowWithActivation = NotifierBase.DialogData.ShowWithActivation;
		ForeColor = Color.FromArgb(NotifierBase.DialogData.ForeColor.A, NotifierBase.DialogData.ForeColor.R, NotifierBase.DialogData.ForeColor.G, NotifierBase.DialogData.ForeColor.B);
		BackColor = Color.FromArgb(NotifierBase.DialogData.BackColor.A, NotifierBase.DialogData.BackColor.R, NotifierBase.DialogData.BackColor.G, NotifierBase.DialogData.BackColor.B);

		foreach (CheckBoxEnumViewModel clickFlag in ClickFlags)
		{
			clickFlag.IsChecked = ClickFlag.HasFlag(clickFlag.Value);
		}
	}

	public bool TrySaveConfiguration()
	{
		List<ValidationResult> errors = GetErrors()
			.DistinctBy(v => v.ErrorMessage)
			.ToList();

		if (errors.Any())
		{
			string caption = Translation.Error;
			string errorMessage = string.Join("\n", errors);

			MessageBox.Show(App.Current!.MainWindow!, errorMessage, caption, MessageBoxButton.OK, MessageBoxImage.Error);

			return false;
		}

		Save();

		return true;
	}

	public virtual void Save()
	{
		NotifierBase.IsEnabled = IsEnabled;

		NotifierBase.PlaysSound = PlaysSound;
		NotifierBase.DialogData.DrawsImage = DrawsImage;
		NotifierBase.SoundVolume = SoundVolume;
		NotifierBase.LoopsSound = LoopsSound;

		NotifierBase.ShowsDialog = ShowsDialog;
		NotifierBase.DialogData.TopMost = TopMost;
		NotifierBase.DialogData.Alignment = Alignment;
		NotifierBase.DialogData.Location = new(LocationX, LocationY);
		NotifierBase.DialogData.DrawsMessage = DrawsMessage;
		NotifierBase.DialogData.HasFormBorder = HasFormBorder;
		NotifierBase.AccelInterval = AccelInterval * 1000;
		NotifierBase.DialogData.ClosingInterval = ClosingInterval * 1000;

		NotifierBase.DialogData.ClickFlag = ClickFlag;
		NotifierBase.DialogData.CloseOnMouseMove = ClickFlag.HasFlag(NotifierDialogClickFlags.MouseOver);
		NotifierBase.DialogData.ForeColor = System.Drawing.Color.FromArgb(ForeColor.A, ForeColor.R, ForeColor.G, ForeColor.B);
		NotifierBase.DialogData.BackColor = System.Drawing.Color.FromArgb(BackColor.A, BackColor.R, BackColor.G, BackColor.B);
		NotifierBase.DialogData.ShowWithActivation = ShowWithActivation;
	}

	public static ValidationResult ValidateImagePath(string path, ValidationContext context)
	{
		if (context.ObjectInstance is not ConfigurationNotificationBaseViewModel viewModel)
		{
			throw new NotImplementedException();
		}

		return (viewModel.ImageChanged && viewModel.DrawsImage && !viewModel.NotifierBase.DialogData.LoadImage(viewModel.ImagePath)) switch
		{
			true => new(NotifyRes.FailedLoadSound),
			_ => ValidationResult.Success!,
		};
	}

	public static ValidationResult ValidateSoundPath(string path, ValidationContext context)
	{
		if (context.ObjectInstance is not ConfigurationNotificationBaseViewModel viewModel)
		{
			throw new NotImplementedException();
		}

		return (viewModel.SoundChanged && viewModel.PlaysSound && !viewModel.NotifierBase.LoadSound(viewModel.SoundPath)) switch
		{
			true => new(NotifyRes.FailedLoadSound),
			_ => ValidationResult.Success!,
		};
	}

	[RelayCommand]
	private void OpenSoundPath()
	{
		string? newPath = FileService.OpenSoundPath(SoundPath);

		if (newPath is null) return;

		SoundPath = newPath;
	}

	[RelayCommand]
	private void OpenImagePath()
	{
		string? newPath = FileService.OpenImagePath(ImagePath);

		if (newPath is null) return;

		ImagePath = newPath;
	}

	[RelayCommand]
	private void SoundPathDirectorize()
	{
		if (string.IsNullOrWhiteSpace(SoundPath)) return;

		try
		{
			SoundPath = System.IO.Path.GetDirectoryName((string?)SoundPath);
		}
		catch (Exception)
		{
			// *ぷちっ*
		}
	}

	[RelayCommand]
	private void Test()
	{
		if (!TrySaveConfiguration()) return;

		NotifierBase.DialogData.Message = Translation.TestNotification;
		NotifierBase.Notify();
	}

	public void Dispose()
	{
		NotifierBase.Sound.Close();
	}
}

