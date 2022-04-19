﻿using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Resource;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels;

public class FleetConditionViewModel : ObservableObject
{
	public string? Text { get; set; }
	public int? Tag { get; set; }
	public string? ToolTip { get; set; }
	public System.Drawing.Color ForeColor { get; set; }
	public System.Drawing.Color BackColor { get; set; }
	public System.Drawing.ContentAlignment ImageAlign { get; set; }
	public int ImageIndex { get; set; }

	public SolidColorBrush Foreground => ForeColor.ToBrush();
	public SolidColorBrush Background => BackColor.ToBrush();
	public ImageSource? Icon => ImageSourceIcons.GetIcon((IconContent)ImageIndex);


	public void SetDesign(int cond)
	{
		if (ImageAlign == System.Drawing.ContentAlignment.MiddleCenter)
		{
			// icon invisible
			ImageIndex = -1;

			(BackColor, ForeColor) = cond switch
			{
				< 20 => (System.Drawing.Color.LightCoral, System.Drawing.Color.Black),
				< 30 => (System.Drawing.Color.LightSalmon, System.Drawing.Color.Black),
				< 40 => (System.Drawing.Color.Moccasin, System.Drawing.Color.Black),
				< 50 => (System.Drawing.Color.Transparent, Utility.Configuration.Config.UI.ForeColor),
				_ => (System.Drawing.Color.LightGreen, System.Drawing.Color.Black)
			};
		}
		else
		{
			BackColor = System.Drawing.Color.Transparent;
			ForeColor = Utility.Configuration.Config.UI.ForeColor;

			ImageIndex = cond switch
			{
				< 20 => (int)IconContent.ConditionVeryTired,
				< 30 => (int)IconContent.ConditionTired,
				< 40 => (int)IconContent.ConditionLittleTired,
				< 50 => (int)IconContent.ConditionNormal,
				_ => (int)IconContent.ConditionSparkle
			};
		}
	}
}
