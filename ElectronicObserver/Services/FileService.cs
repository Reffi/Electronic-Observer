﻿using System;
using System.IO;
using System.Linq;
using System.Windows;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Dialog;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;

namespace ElectronicObserver.Services;

public class FileService
{
	private System.Windows.Window MainWindow => App.Current!.MainWindow!;

	private string LayoutFilter => "Layout File|*.xml";

	/// <summary>
	/// Opens a file selection dialog to select a layout file.
	/// </summary>
	/// <param name="path">Current layout path.</param>
	/// <returns>Selected file path or null if no path was selected.</returns>
	public string? OpenLayoutPath(string path)
	{
		OpenFileDialog dialog = new()
		{
			Filter = LayoutFilter,
			Title = Properties.Window.FormMain.OpenLayoutCaption,
		};

		PathHelper.InitOpenFileDialog(path, dialog);

		return dialog.ShowDialog(MainWindow) switch
		{
			true => PathHelper.GetPathFromOpenFileDialog(dialog),
			_ => null,
		};
	}

	/// <summary>
	/// Opens a file save dialog to save a layout file.
	/// </summary>
	/// <param name="path">Current layout path.</param>
	/// <returns>Selected file path or null if no path was selected.</returns>
	public string? SaveLayoutPath(string path)
	{
		SaveFileDialog dialog = new()
		{
			Filter = LayoutFilter,
			Title = Properties.Window.FormMain.OpenLayoutCaption,
		};

		PathHelper.InitSaveFileDialog(path, dialog);

		return dialog.ShowDialog(MainWindow) switch
		{
			true => PathHelper.GetPathFromSaveFileDialog(dialog),
			_ => null,
		};
	}

	/// <summary>
	/// Opens a folder browser dialog to select a folder.
	/// </summary>
	/// <param name="path">Current folder path.</param>
	/// <returns>Selected folder path or null if no path was selected.</returns>
	public string? SelectFolder(string path)
	{
		VistaFolderBrowserDialog dialog = new()
		{
			SelectedPath = path,
		};

		return dialog.ShowDialog(MainWindow) switch
		{
			true => dialog.SelectedPath,
			_ => null,
		};
	}

	public void ExportConnectionScript(int port)
	{
		string? serverAddress = APIObserver.Instance.ServerAddress;

		if (serverAddress is null)
		{
			MessageBox.Show(Properties.Window.Dialog.DialogConfiguration.PleaseStartKancolle, Properties.Window.Dialog.DialogConfiguration.DialogCaptionErrorTitle,
				MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return;
		}

		SaveFileDialog dialog = new()
		{
			Filter = "Proxy Script|*.pac|File|*",
			Title = Properties.Window.Dialog.DialogConfiguration.SavePacFileAs,
			InitialDirectory = Directory.GetCurrentDirectory(),
			FileName = Directory.GetCurrentDirectory() + "\\proxy.pac",
		};

		if (dialog.ShowDialog(MainWindow) != true) return;

		try
		{
			using (StreamWriter sw = new(dialog.FileName))
			{
				sw.WriteLine("function FindProxyForURL(url, host) {");
				sw.WriteLine("  if (/^" + serverAddress.Replace(".", @"\.") + "/.test(host)) {");
				sw.WriteLine("    return \"PROXY localhost:{0}; DIRECT\";", port);
				sw.WriteLine("  }");
				sw.WriteLine("  return \"DIRECT\";");
				sw.WriteLine("}");
			}

			Clipboard.SetData(DataFormats.StringFormat, "file:///" + dialog.FileName.Replace('\\', '/'));

			MessageBox.Show(Properties.Window.Dialog.DialogConfiguration.ProxyAutoConfigSaved,
				Properties.Window.Dialog.DialogConfiguration.PacSavedTitle,
				MessageBoxButton.OK, MessageBoxImage.Information);
		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, Properties.Window.Dialog.DialogConfiguration.FailedToSavePac);
			MessageBox.Show(Properties.Window.Dialog.DialogConfiguration.FailedToSavePac + "\r\n" + ex.Message,
				Properties.Window.Dialog.DialogConfiguration.DialogCaptionErrorTitle,
				MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	public string? OpenApiListPath(string path)
	{
		OpenFileDialog dialog = new()
		{
			Filter = "Text File|*.txt|File|*",
			Title = "API リストを開く",
		};

		PathHelper.InitOpenFileDialog(path, dialog);

		return dialog.ShowDialog(MainWindow) switch
		{
			true => PathHelper.GetPathFromOpenFileDialog(dialog),
			_ => null,
		};
	}

	public string? OpenSoundPath(string? path)
	{
		OpenFileDialog dialog = new()
		{
			Filter = "音楽ファイル|" + string.Join(";", MediaPlayer.SupportedExtensions.Select(s => "*." + s)) + "|File|*",
			Title = NotifyRes.OpenSound,
		};

		if (!string.IsNullOrEmpty(path))
		{
			try
			{
				dialog.InitialDirectory = Path.GetDirectoryName(path);

			}
			catch (Exception) { }
		}

		return dialog.ShowDialog(MainWindow) switch
		{
			true => dialog.FileName,
			_ => null,
		};
	}

	public string? OpenImagePath(string? path)
	{
		OpenFileDialog dialog = new()
		{
			Filter = "Image|*.bmp;*.div;*.jpg;*.jpeg;*.jpe;*.jfif;*.gif;*.png;*.tif;*.tiff|BMP|*.bmp;*.div|JPEG|*.jpg;*.jpeg;*.jpe;*.jfif|GIF|*.gif|PNG|*.png|TIFF|*.tif;*.tiff|File|*",
			Title = NotifyRes.OpenImage,
		};

		if (!string.IsNullOrEmpty(path))
		{
			try
			{
				dialog.InitialDirectory = Path.GetDirectoryName(path);

			}
			catch (Exception) { }
		}

		return dialog.ShowDialog(MainWindow) switch
		{
			true => dialog.FileName,
			_ => null,
		};
	}
}
