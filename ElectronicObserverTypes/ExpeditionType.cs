﻿using System.ComponentModel.DataAnnotations;

namespace ElectronicObserverTypes;

public enum ExpeditionType
{
	Unknown = -1,
	[Display(ResourceType = typeof(Properties.ExpeditionType), Name = "Normal")]
	Normal = 0,
	[Display(ResourceType = typeof(Properties.ExpeditionType), Name = "CombatTypeOneExpedition")]
	CombatTypeOneExpedition = 1,
	[Display(ResourceType = typeof(Properties.ExpeditionType), Name = "CombatTypeTwoExpedition")]
	CombatTypeTwoExpedition = 2,

}
