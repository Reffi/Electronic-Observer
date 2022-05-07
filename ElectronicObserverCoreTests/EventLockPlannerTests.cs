﻿using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver;
using ElectronicObserver.Window.Tools.EventLockPlanner;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;
using Xunit;

namespace ElectronicObserverCoreTests;

[Collection(DatabaseCollection.Name)]
public class EventLockPlannerTests
{
	private DatabaseFixture Db { get; }
	private List<IShipData> AllShips { get; }
	private List<IShipData> AllShipsCopy => AllShips
		.Cast<ShipDataMock>()
		.Select(s => s.Clone())
		.Cast<IShipData>()
		.ToList();

	public EventLockPlannerTests(DatabaseFixture db)
	{
		Db = db;

		AllShips = new List<IShipData>
		{
			new ShipDataMock(Db.MasterShips[ShipId.Kamikaze]) {ID = 1},
			new ShipDataMock(Db.MasterShips[ShipId.Asakaze]) {ID = 2},
			new ShipDataMock(Db.MasterShips[ShipId.Harukaze]) {ID = 3},
			new ShipDataMock(Db.MasterShips[ShipId.Matsukaze]) {ID = 4},
			new ShipDataMock(Db.MasterShips[ShipId.Hatakaze]) {ID = 5},
		};
	}

	[Fact(DisplayName = "Planner initialization")]
	public void EventLockPlannerTest1()
	{
		EventLockPlannerViewModel planner = new(AllShips);

		Assert.Equal(5, planner.NoLockGroup.Ships.Count);
		Assert.Empty(planner.LockGroups);
	}

	[Fact(DisplayName = "Add lock")]
	public void EventLockPlannerTest2()
	{
		EventLockPlannerViewModel planner = new(AllShips);

		planner.AddLockCommand.Execute(null);

		Assert.Single(planner.LockGroups);
	}

	[Fact(DisplayName = "Lock ship")]
	public void EventLockPlannerTest3()
	{
		EventLockPlannerViewModel planner = new(AllShips);

		planner.AddLockCommand.Execute(null);

		ShipLockViewModel kamikaze = planner.NoLockGroup.Ships
			.First(s => s.Ship.MasterShip.ShipId is ShipId.Kamikaze);

		planner.LockGroups.First().Move(kamikaze, planner.NoLockGroup.Ships);

		Assert.Equal(4, planner.NoLockGroup.Ships.Count);
		Assert.Single(planner.LockGroups);
		Assert.Single(planner.LockGroups.First().Ships);
	}

	[Fact(DisplayName = "Real lock initialization")]
	public void EventLockPlannerTest4()
	{
		List<IShipData> allShips = AllShipsCopy;
		ShipDataMock kamikaze = (allShips.First(s => s.MasterShip.ShipId is ShipId.Kamikaze) as ShipDataMock)!;
		kamikaze.SallyArea = 2;

		EventLockPlannerViewModel planner = new(allShips);

		Assert.Equal(4, planner.NoLockGroup.Ships.Count);
		Assert.Equal(2, planner.LockGroups.Count);
		Assert.Single(planner.LockGroups[1].Ships);
	}

	[Fact(DisplayName = "Can't move locked ship to a different lock plan")]
	public void EventLockPlannerTest5()
	{
		List<IShipData> allShips = AllShipsCopy;
		ShipDataMock kamikaze = (allShips.First(s => s.MasterShip.ShipId is ShipId.Kamikaze) as ShipDataMock)!;
		kamikaze.SallyArea = 2;

		EventLockPlannerViewModel planner = new(allShips);

		planner.LockGroups[0].Move(planner.LockGroups[1].Ships.First(), planner.LockGroups[1].Ships);

		Assert.Equal(4, planner.NoLockGroup.Ships.Count);
		Assert.Equal(2, planner.LockGroups.Count);
		Assert.Empty(planner.LockGroups[0].Ships);
		Assert.Single(planner.LockGroups[1].Ships);
	}
}
