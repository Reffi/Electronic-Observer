﻿using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog.EquipmentPicker;
using ElectronicObserverTypes;

namespace ElectronicObserver.Services;

public class EquipmentPickerService
{
	private System.Windows.Window MainWindow => App.Current!.MainWindow!;

	public IEquipmentData? OpenEquipmentPicker()
	{
		EquipmentDataPickerView equipmentPicker = new(new());

		equipmentPicker.ShowDialog(MainWindow);

		return equipmentPicker.PickedEquipment;
	}


	public IEquipmentDataMaster? OpenMasterEquipmentPicker()
	{
		MasterEquipmentPickerView equipmentPicker = new(new());

		equipmentPicker.ShowDialog(MainWindow);

		return equipmentPicker.PickedEquipment;
	}
}
