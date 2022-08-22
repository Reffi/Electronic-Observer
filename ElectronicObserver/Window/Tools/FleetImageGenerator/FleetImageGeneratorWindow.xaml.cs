﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ElectronicObserver.Properties.Window.Dialog;


namespace ElectronicObserver.Window.Tools.FleetImageGenerator;
/// <summary>
/// Interaction logic for FleetImageGeneratorWindow.xaml
/// </summary>
public partial class FleetImageGeneratorWindow
{
	public FleetImageGeneratorWindow(FleetImageGeneratorImageDataModel model)
		: base(new FleetImageGeneratorViewModel(model))
	{
		InitializeComponent();
	}

	private BitmapSource GenerateFleetImage(UIElement source)
	{
		int marginLeft = 10;
		int marginTop = 10;
		int marginRight = 10;
		int marginBottom = 10;

		double renderHeight = source.RenderSize.Height;
		double renderWidth = source.RenderSize.Width;

		int width = (int)renderWidth + marginLeft + marginRight;
		int height = (int)renderHeight + marginTop + marginBottom;

		RenderTargetBitmap renderTarget = new(width, height, 96, 96, PixelFormats.Pbgra32);
		VisualBrush sourceBrush = new(source);
		DrawingVisual drawingVisual = new();
		DrawingContext drawingContext = drawingVisual.RenderOpen();

		using (drawingContext)
		{
			drawingContext.DrawRectangle(Background, null, new Rect(new Point(0, 0), new Point(width, height)));
			drawingContext.DrawRectangle(sourceBrush, null, new Rect(new Point(marginLeft, marginTop), new Point(renderWidth, renderHeight)));
		}

		renderTarget.Render(drawingVisual);
		PngBitmapEncoder pngEncoder = new();
		BitmapFrame frame = BitmapFrame.Create(renderTarget);
		pngEncoder.Frames.Add(frame);

		using MemoryStream outputStream = new();
		pngEncoder.Save(outputStream);
		outputStream.Seek(0, SeekOrigin.Begin);

		BitmapImage bitmap = new();
		bitmap.BeginInit();
		bitmap.StreamSource = outputStream;
		bitmap.EndInit();

		return bitmap.EmbedText(JsonSerializer.Serialize(ViewModel.GetImageDataModel()));
	}

	private void CopyImageToClipboard(object sender, RoutedEventArgs e)
	{
		Clipboard.SetImage(GenerateFleetImage(ImageContent));

		Utility.Logger.Add(2, DialogFleetImageGenerator.FleetImageExportedSuccessfully);
	}

	private void LoadImageFromClipboard(object sender, RoutedEventArgs e)
	{
		BitmapSource? bitmapSource = Clipboard.GetImage();

		if (bitmapSource is null)
		{
			// log error
			return;
		}

		string imageData = bitmapSource.ExtractText();

		try
		{
			FleetImageGeneratorImageDataModel? model = JsonSerializer.Deserialize<FleetImageGeneratorImageDataModel>(imageData);

			if (model is null)
			{
				// failed to parse image data
				return;
			}

			ViewModel.SetImageDataModel(model);
		}
		catch (Exception exception)
		{

		}
	}

	private void SaveImage(object sender, RoutedEventArgs e)
	{
		if (string.IsNullOrWhiteSpace(ViewModel.ImageSaveLocation))
		{
			MessageBox.Show(DialogFleetImageGenerator.EnterDestinationPath, DialogFleetImageGenerator.InputError, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return;
		}

		if (ViewModel.ImageSaveLocation.ToCharArray().Intersect(Path.GetInvalidPathChars()).Any())
		{
			MessageBox.Show(DialogFleetImageGenerator.PathContainsInvalidCharacters, DialogFleetImageGenerator.InputError, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return;
		}

		if (!ViewModel.DisableOverwritePrompt && File.Exists(ViewModel.ImageSaveLocation))
		{
			if (MessageBox.Show(string.Format(DialogFleetImageGenerator.OverwriteExistingFile, Path.GetFileName(ViewModel.ImageSaveLocation)), DialogFleetImageGenerator.OverwriteConfirmation,
					MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No)
				== MessageBoxResult.No)
			{
				return;
			}
		}

		BitmapSource image = GenerateFleetImage(ImageContent);

		Directory.CreateDirectory(ViewModel.ImageSaveLocation);

		string fileName = Path.Combine(ViewModel.ImageSaveLocation, $"{DateTime.Now:yyyyMMdd_HHmmssff}.png");
		using FileStream file = new(fileName, FileMode.OpenOrCreate);

		PngBitmapEncoder encoder = new();
		encoder.Frames.Add(BitmapFrame.Create(image));
		encoder.Save(file);

		Utility.Logger.Add(2, DialogFleetImageGenerator.FleetImageExportedSuccessfully);

		if (!ViewModel.OpenImageAfterOutput) return;

		ProcessStartInfo psi = new()
		{
			FileName = fileName,
			UseShellExecute = true,
		};
		Process.Start(psi);
	}
}
