﻿using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_kaisou;

public class marriage : APIBase
{

	public override void OnResponseReceived(dynamic data)
	{

		Utility.Logger.Add(2, string.Format(LoggerRes.JustMarried, KCDatabase.Instance.Ships[(int)data.api_id].Name));

		var db = KCDatabase.Instance;
		int id = (int)data.api_id;
		var ship = db.Ships[id];

		if (ship != null)
			ship.LoadFromResponse(APIName, data);
		else
		{
			var a = new ShipData();
			a.LoadFromResponse(APIName, data);
			db.Ships.Add(a);
		}

		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_req_kaisou/marriage";
}
