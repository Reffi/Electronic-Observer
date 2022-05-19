﻿using System;
using CommunityToolkit.Mvvm.DependencyInjection;
using Jot;

namespace ElectronicObserver.Common;

public class WindowBase<TViewModel> : System.Windows.Window where TViewModel : WindowViewModelBase
{
	private Tracker Tracker { get; }
	public TViewModel ViewModel { get; }

	[Obsolete("This is only needed so WPF doesn't complain, don't use this.", true)]
#pragma warning disable CS8618
	public WindowBase()
#pragma warning restore CS8618
	{
		
	}

	protected WindowBase(TViewModel viewModel)
	{
		Tracker = Ioc.Default.GetService<Tracker>()!;

		ViewModel = viewModel;
		DataContext = ViewModel;

		SetBinding(FontSizeProperty, nameof(WindowViewModelBase.FontSize));
		SetBinding(FontFamilyProperty, nameof(WindowViewModelBase.Font));
		SetBinding(ForegroundProperty, nameof(WindowViewModelBase.FontBrush));

		Loaded += (_, _) =>
		{
			ViewModel.Loaded();
			StartJotTracking();
		};
		Closed += (_, _) => ViewModel.Closed();
	}

	private void StartJotTracking()
	{
		Tracker.Track(this);
	}
}
