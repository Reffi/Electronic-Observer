﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ElectronicObserver.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.AntiAir;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Wpf;

public static class Extensions
{
	public static Visibility ToVisibility(this bool visible) => visible switch
	{
		true => Visibility.Visible,
		_ => Visibility.Collapsed
	};

	public static SolidColorBrush ToBrush(this System.Drawing.Color color) =>
		new(Color.FromArgb(color.A, color.R, color.G, color.B));

	public static float ToSize(this System.Drawing.Font font) => font.Size * font.Unit switch
	{
		System.Drawing.GraphicsUnit.Point => 4 / 3f,
		_ => 1
	};

	public static Uri ToAbsolute(this Uri uri) => uri switch
	{
		{ IsAbsoluteUri: true } => uri,
		_ => new(new Uri(Process.GetCurrentProcess().MainModule.FileName), uri)
	};

	public static int ToSerializableValue(this ListSortDirection? sortDirection) => sortDirection switch
	{
		null => -1,
		{ } => (int)sortDirection
	};

	public static ListSortDirection? ToSortDirection(this int sortDirection) => sortDirection switch
	{
		0 or 1 => (ListSortDirection)sortDirection,
		_ => null
	};

	// todo: move to EOTypes.AA, not possible right now cause of ship class names
	public static string? ConditionDisplay(this AntiAirCutIn aaci, IKCDatabase db)
	{
		StringBuilder sb = new();

		List<AntiAirCutInCondition> aaciConditions = aaci.Conditions;

		if (aaciConditions is not { Count: > 0 })
		{
			return null;
		}

		// ships/classes should be the same for all possible conditions so only write them once
		if (aaci.Conditions.FirstOrDefault()?.ShipClasses is { } shipClasses)
		{
			foreach (ShipClass shipClass in shipClasses)
			{
				sb.AppendLine(Constants.GetShipClass(shipClass));
			}

			sb.AppendLine();
		}

		if (aaci.Conditions.FirstOrDefault()?.Ships is { } ships)
		{
			foreach (ShipId shipId in ships)
			{
				sb.AppendLine(db.MasterShips[(int)shipId].NameEN);
			}

			sb.AppendLine();
		}

		IEnumerable<string> conditions = aaciConditions
			.Select(c => string.Join("\n", c.EquipmentConditions()));

		sb.Append(string.Join("\n==OR==\n", conditions));

		return sb.ToString();
	}

	public static string EquipmentConditionsSingleLineDisplay(this AntiAirCutIn cutIn) =>
		cutIn.Conditions switch
		{
			null => $"{ConstantsRes.Unknown}({cutIn.Id})",

			{ } conditions => string.Join(" OR ", conditions
				.Select(c => string.Join(", ", c.EquipmentConditions())))
		};

	public static string EquipmentConditionsMultiLineDisplay(this AntiAirCutIn cutIn) =>
		cutIn.Conditions switch
		{
			null => $"{ConstantsRes.Unknown}({cutIn.Id})",

			{ } conditions => string.Join("\nOR\n", conditions
				.Select(c => string.Join("\n", c.EquipmentConditions())))
		};

	private static List<string> EquipmentConditions(this AntiAirCutInCondition condition)
	{
		List<string> conditions = new();

		if (condition.HighAngle > 0)
		{
			conditions.Add($"{AaciStrings.HighAngle} >= {condition.HighAngle}");
		}

		if (condition.HighAngleDirector > 0)
		{
			conditions.Add($"{AaciStrings.HighAngleDirector} >= {condition.HighAngleDirector}");
		}

		if (condition.HighAngleWithoutDirector > 0)
		{
			conditions.Add($"{AaciStrings.HighAngleWithoutDirector} >= {condition.HighAngleWithoutDirector}");
		}

		if (condition.AaDirector > 0)
		{
			conditions.Add($"{AaciStrings.AaDirector} >= {condition.AaDirector}");
		}

		if (condition.Radar > 0)
		{
			conditions.Add($"{AaciStrings.Radar} >= {condition.Radar}");
		}

		if (condition.AntiAirRadar > 0)
		{
			conditions.Add($"{AaciStrings.AntiAirRadar} >= {condition.AntiAirRadar}");
		}

		if (condition.MainGunLarge > 0)
		{
			conditions.Add($"{AaciStrings.MainGunLarge} >= {condition.MainGunLarge}");
		}

		if (condition.MainGunLargeFcr > 0)
		{
			conditions.Add($"{AaciStrings.MainGunLargeFcr} >= {condition.MainGunLargeFcr}");
		}

		if (condition.AaShell > 0)
		{
			conditions.Add($"{AaciStrings.AaShell} >= {condition.AaShell}");
		}

		if (condition.AaGun > 0)
		{
			conditions.Add($"{AaciStrings.AaGun} >= {condition.AaGun}");
		}

		if (condition.AaGun3Aa > 0)
		{
			conditions.Add($"{AaciStrings.AaGun3AaOrMore} >= {condition.AaGun3Aa}");
		}

		if (condition.AaGun4Aa > 0)
		{
			conditions.Add($"{AaciStrings.AaGun4AaOrMore} >= {condition.AaGun4Aa}");
		}

		if (condition.AaGun6Aa > 0)
		{
			conditions.Add($"{AaciStrings.AaGun6AaOrMore} >= {condition.AaGun6Aa}");
		}

		if (condition.AaGun3To8Aa > 0)
		{
			conditions.Add($"{AaciStrings.AaGun3To8Aa} >= {condition.AaGun3To8Aa}");
		}

		if (condition.AaGunConcentrated > 0)
		{
			conditions.Add($"{AaciStrings.AaGunConcentrated} >= {condition.AaGunConcentrated}");
		}

		if (condition.AaGunPompom > 0)
		{
			conditions.Add($"{AaciStrings.AaGunPompom} >= {condition.AaGunPompom}");
		}

		if (condition.AaRocketBritish > 0)
		{
			conditions.Add($"{AaciStrings.AaRocketBritish} >= {condition.AaRocketBritish}");
		}

		if (condition.AaRocketMod > 0)
		{
			conditions.Add($"{AaciStrings.AaRocketMod} >= {condition.AaRocketMod}");
		}

		if (condition.HighAngleMusashi > 0)
		{
			conditions.Add($"{AaciStrings.HighAngleMusashi} >= {condition.HighAngleMusashi}");
		}

		if (condition.HighAngleAmerican > 0)
		{
			conditions.Add($"{AaciStrings.HighAngleAmerican} >= {condition.HighAngleAmerican}");
		}

		if (condition.HighAngleAmericanKai > 0)
		{
			conditions.Add($"{AaciStrings.HighAngleAmericanKai} >= {condition.HighAngleAmericanKai}");
		}

		if (condition.HighAngleAmericanGfcs > 0)
		{
			conditions.Add($"{AaciStrings.HighAngleAmericanGfcs} >= {condition.HighAngleAmericanGfcs}");
		}

		if (condition.RadarGfcs > 0)
		{
			conditions.Add($"{AaciStrings.RadarGfcs} >= {condition.RadarGfcs}");
		}

		if (condition.HighAngleAtlanta > 0)
		{
			conditions.Add($"{AaciStrings.HighAngleAtlanta} >= {condition.HighAngleAtlanta}");
		}

		if (condition.HighAngleAtlantaGfcs > 0)
		{
			conditions.Add($"{AaciStrings.HighAngleAtlantaGfcs} >= {condition.HighAngleAtlantaGfcs}");
		}

		if (condition.HighAngleConcentrated > 0)
		{
			conditions.Add($"{AaciStrings.HighAngleConcentrated} >= {condition.HighAngleConcentrated}");
		}

		if (condition.RadarYamato > 0)
		{
			conditions.Add($"{AaciStrings.RadarYamato} >= {condition.RadarYamato}");
		}

		return conditions;
	}
}
