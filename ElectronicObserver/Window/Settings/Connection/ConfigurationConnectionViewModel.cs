﻿using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Properties.Window.Dialog;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.Connection;

public partial class ConfigurationConnectionViewModel : ConfigurationViewModelBase
{
	public ConfigurationConnectionTranslationViewModel Translation { get; }
	private FileService FileService { get; }

	private Configuration.ConfigurationData.ConfigConnection ConfigConnection { get; }

	[ObservableProperty]
	[CustomValidation(typeof(ConfigurationConnectionViewModel), nameof(ValidatePorts))]
	private ushort _port;

	public bool SaveReceivedData { get; set; }

	[ObservableProperty]
	[CustomValidation(typeof(ConfigurationConnectionViewModel), nameof(ValidateFolder))]
	private string _saveDataPath;

	public bool SaveRequest { get; set; }

	public bool SaveResponse { get; set; }

	public bool SaveOtherFile { get; set; }

	public bool ApplyVersion { get; set; }

	public bool RegisterAsSystemProxy { get; set; }

	public bool UseUpstreamProxy { get; set; }

	[ObservableProperty]
	[CustomValidation(typeof(ConfigurationConnectionViewModel), nameof(ValidatePorts))]
	private ushort _upstreamProxyPort;

	public string UpstreamProxyAddress { get; set; }

	public bool UseSystemProxy { get; set; }

	public string DownstreamProxy { get; set; }

	public ConfigurationConnectionViewModel(Configuration.ConfigurationData.ConfigConnection configConnection)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationConnectionTranslationViewModel>();
		FileService = Ioc.Default.GetRequiredService<FileService>();

		ConfigConnection = configConnection;
		Load(configConnection);
	}

	private void Load(Configuration.ConfigurationData.ConfigConnection configConnection)
	{
		Port = configConnection.Port;
		SaveReceivedData = configConnection.SaveReceivedData;
		SaveDataPath = configConnection.SaveDataPath;
		SaveRequest = configConnection.SaveRequest;
		SaveResponse = configConnection.SaveResponse;
		SaveOtherFile = configConnection.SaveOtherFile;
		ApplyVersion = configConnection.ApplyVersion;
		RegisterAsSystemProxy = configConnection.RegisterAsSystemProxy;
		UseUpstreamProxy = configConnection.UseUpstreamProxy;
		UpstreamProxyPort = configConnection.UpstreamProxyPort;
		UpstreamProxyAddress = configConnection.UpstreamProxyAddress;
		UseSystemProxy = configConnection.UseSystemProxy;
		DownstreamProxy = configConnection.DownstreamProxy;
	}

	public override void Save()
	{
		ConfigConnection.Port = Port;
		ConfigConnection.SaveReceivedData = SaveReceivedData;
		ConfigConnection.SaveDataPath = SaveDataPath;
		ConfigConnection.SaveRequest = SaveRequest;
		ConfigConnection.SaveResponse = SaveResponse;
		ConfigConnection.SaveOtherFile = SaveOtherFile;
		ConfigConnection.ApplyVersion = ApplyVersion;
		ConfigConnection.RegisterAsSystemProxy = RegisterAsSystemProxy;
		ConfigConnection.UseUpstreamProxy = UseUpstreamProxy;
		ConfigConnection.UpstreamProxyPort = UpstreamProxyPort;
		ConfigConnection.UpstreamProxyAddress = UpstreamProxyAddress;
		ConfigConnection.UseSystemProxy = UseSystemProxy;
		ConfigConnection.DownstreamProxy = DownstreamProxy;
	}

	public static ValidationResult ValidateFolder(string path) => Directory.Exists(path) switch
	{
		true => ValidationResult.Success!,
		_ => new(DialogConfiguration.SaveDataPathDoesNotExist),
	};

	public static ValidationResult ValidatePorts(int port, ValidationContext context)
	{
		if (context.ObjectInstance is not ConfigurationConnectionViewModel viewModel)
		{
			throw new NotImplementedException();
		}

		return (viewModel.Port == viewModel.UpstreamProxyPort) switch
		{
			true => new(DialogConfiguration.PortAndUpstreamPortMustBeDifferent),
			_ => ValidationResult.Success!,
		};
	}

	[ICommand]
	private void SelectSaveDataPath()
	{
		string? newSaveDataPath = FileService.SelectFolder(SaveDataPath);

		if (newSaveDataPath is null) return;

		SaveDataPath = newSaveDataPath;
	}

	[ICommand]
	private void ExportConnectionScript()
	{
		FileService.ExportConnectionScript(Port);
	}
}
