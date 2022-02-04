using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace ElectronicObserver.ViewModels;

public static class DialogExtensions
{
	public static void ShowDialogExt(this System.Windows.Window window, FormMainViewModel formMainViewModel)
	{
		window.Owner = formMainViewModel.Window;
		window.ShowDialog();
	}
	public static void ShowExt(this System.Windows.Forms.Form form, FormMainViewModel formMainViewModel)
	{
		WindowInteropHelper helper = new WindowInteropHelper(formMainViewModel.Window);
		SetParent(form.Handle, helper.Handle);
		form.ShowInTaskbar = true;
		form.Show();
	}
	[DllImport("User32.dll")]
	static extern IntPtr SetParent(IntPtr hWnd, IntPtr hParent);
}
