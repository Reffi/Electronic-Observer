﻿using System.Collections.Generic;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using ElectronicObserverTypes.Serialization.FitBonus;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class FitBonusTest
{
	private DatabaseFixture Db { get; }

	private static ElectronicObserver.Data.Translation.FitBonusData BonusData { get; } = new ();

	public FitBonusTest(DatabaseFixture db)
	{
		Db = db;
	}

	[Fact]
	public void FitBonusTest1()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData hachijou = new ShipDataMock(Db.MasterShips[ShipId.HachijouKai])
		{
			Level = 175,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_12cmSingleHighangleGunModelE]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_12cmSingleHighangleGunModelE])
				{
					Level = 10
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarSmall_GFCSMk_37]),
			}
		};

		FitBonusValue expectedBonus = new FitBonusValue()
		{
			Firepower = 2,
			Evasion = 3 + 3 + (2 * 2),
			AntiAir = 2 + (2 * 2),
			ASW = 1 * 2,
			Accuracy = 0,
			Armor = 0,
			LOS = 0,
			Range = 0,
			Torpedo = 0,
		};

		FitBonusValue finalBonus = hachijou.GetFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}

	[Fact]
	public void FitBonusTest2()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData yikikaze = new ShipDataMock(Db.MasterShips[ShipId.YukikazeKaiNi])
		{
			Level = 151,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_10cmTwinHighangleGun_AAFD]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_10cmTwinHighangleGun_AAFD])
				{
					Level = 4
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Torpedo_61cmQuadruple_OxygenTorpedoMountLateModel])
				{
					Level = 3
				},
			}
		};

		FitBonusValue expectedBonus = new FitBonusValue()
		{
			Firepower = 5,
			Torpedo = 2,
			AntiAir = 3,
			Accuracy = 0,
			ASW = 0,
			Armor = 0,
			Evasion = 2 + 1,
			LOS = 0,
			Range = 0,
		};

		FitBonusValue finalBonus = yikikaze.GetFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}

	[Fact]
	public void FitBonusTest3()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData yikikaze = new ShipDataMock(Db.MasterShips[ShipId.YukikazeKaiNi])
		{
			Level = 151,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_10cmTwinHighangleGun_AAFD])
				{
					Level = 10
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunSmall_10cmTwinHighangleGun_AAFD])
				{
					Level = 10
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.Torpedo_61cmQuadruple_OxygenTorpedoMountLateModel])
				{
					Level = 10
				},
			},
			ExpansionSlotInstance = new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SurfaceShipPersonnel_TorpedoSquadronSkilledLookouts])
			{
				Level = 4
			},
		};

		FitBonusValue expectedBonus = new FitBonusValue()
		{
			Firepower = 5 + 5 + 1 + 3,
			Torpedo = 3 + 4,
			AntiAir = 3 + 3,
			Accuracy = 0,
			ASW = 2,
			Armor = 0,
			Evasion = 2 + 2 + 1 + 3,
			LOS = 1,
			Range = 0,
		};

		FitBonusValue finalBonus = yikikaze.GetFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}

	[Fact]
	public void FitBonusTest4()
	{
		Assert.NotEmpty(BonusData.FitBonusList);

		IShipData yamato = new ShipDataMock(Db.MasterShips[ShipId.YamatoKaiNiJuu])
		{
			Level = 99,
			SlotInstance = new List<IEquipmentData?>
			{
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_51cmTwinGun])
				{
					Level = 2
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.MainGunLarge_Prototype51cmTwinGun])
				{
					Level = 1
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SeaplaneRecon_Type0ReconSeaplaneModel11B_Skilled]),
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.APShell_Type1ArmorPiercingShellKai])
				{
					Level = 4
				},
				new EquipmentDataMock(Db.MasterEquipment[EquipmentId.SecondaryGun_15_5cmTripleSecondaryGunMountKaiNi])
				{
					Level = 8
				},
			},
			ExpansionSlotInstance = new EquipmentDataMock(Db.MasterEquipment[EquipmentId.RadarLarge_15mDuplexRangefinder_Type21AirRADARKai2]),
		};

		FitBonusValue expectedBonus = new FitBonusValue()
		{
			Firepower = 1 + 1 + 2 + 2 + 1,
			Torpedo = 0,
			AntiAir = 2 + 1,
			Accuracy = 1 + 1 + 3 + 3,
			ASW = 0,
			Armor = 0,
			Evasion = 2 + 1,
			LOS = 0,
			Range = 0,
		};

		FitBonusValue finalBonus = yamato.GetFitBonus(BonusData.FitBonusList);

		Assert.Equal(expectedBonus, finalBonus);
	}
}
