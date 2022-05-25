﻿using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_kousyou;

public class getship : APIBase
{


	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase db = KCDatabase.Instance;

		//api_kdock
		foreach (var ars in data.api_kdock)
		{

			int id = (int)ars.api_id;

			if (!db.Arsenals.ContainsKey(id))
			{
				var a = new ArsenalData();
				a.LoadFromResponse(APIName, ars);
				db.Arsenals.Add(a);

			}
			else
			{
				db.Arsenals[id].LoadFromResponse(APIName, ars);
			}
		}

		//api_slotitem
		if (data.api_slotitem != null)
		{               //装備なしの艦はnullになる
			foreach (var elem in data.api_slotitem)
			{

				var eq = new EquipmentData();
				eq.LoadFromResponse(APIName, elem);
				db.Equipments.Add(eq);

			}
		}

		//api_ship
		{
			ShipData ship = new ShipData();
			ship.LoadFromResponse(APIName, data.api_ship);
			db.Ships.Add(ship);

			Utility.Logger.Add(2, string.Format(LoggerRes.ShipConstructed, ship.MasterShip.ShipTypeName, ship.MasterShip.NameWithClass));
		}


		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_req_kousyou/getship";
}
