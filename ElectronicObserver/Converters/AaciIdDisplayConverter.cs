﻿using System;
using System.Globalization;
using System.Windows.Data;
using ElectronicObserverTypes.AntiAir;

namespace ElectronicObserver.Converters;

public class AaciIdDisplayConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		=> value switch
		{
			> 0 => $"{value}",
			_ => AaciStrings.FailedAntiAirCutIn
		};

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
