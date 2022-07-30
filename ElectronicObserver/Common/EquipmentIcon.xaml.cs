﻿using System.Windows;
using System.Windows.Controls;
using ElectronicObserverTypes;

namespace ElectronicObserver.Common;
/// <summary>
/// Interaction logic for EquipmentIcon.xaml
/// </summary>
public partial class EquipmentIcon : UserControl
{
	public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
		nameof(Type), typeof(EquipmentIconType), typeof(EquipmentIcon), new PropertyMetadata(default(EquipmentIconType)));

	public EquipmentIconType Type
	{
		get => (EquipmentIconType)GetValue(TypeProperty);
		set => SetValue(TypeProperty, value);
	}

	public EquipmentIcon()
	{
		InitializeComponent();
	}
}
