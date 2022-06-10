﻿using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserverTypes;

namespace ElectronicObserver.Notifier;

public class NotifierRemodelLevel : NotifierBase
{
	public NotifierRemodelLevel()
		: base()
	{
		Initialize();
	}

	public NotifierRemodelLevel(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
		: base(config)
	{
		Initialize();
	}

	private void Initialize()
	{
		DialogData.Title = NotifierRes.RemodelTitle;

		var db = KCDatabase.Instance;

		db.Battle.ShipLevelUp += (ship, nextLevel) =>
		{
			IShipDataMaster? nextRemodelShip = db.MasterShips.Values
				.Where(s => s.BaseShip() == ship.MasterShip.BaseShip())
				.Where(s => s.RemodelBeforeShip != null)
				.FirstOrDefault(s => s.RemodelBeforeShip!.RemodelAfterLevel > ship.Level &&
									 s.RemodelBeforeShip!.RemodelAfterLevel <= nextLevel);

			if (nextRemodelShip is null) return;

			Notify(ship, nextRemodelShip);
		};
	}

	private void Notify(IShipData ship, IShipDataMaster nextRemodelShip)
	{
		DialogData.Message = string.Format(NotifierRes.RemodelText, ship.Name, nextRemodelShip.Name);

		base.Notify();
	}
}
