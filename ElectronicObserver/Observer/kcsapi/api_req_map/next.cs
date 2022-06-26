﻿using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.Data.DiscordRPC;
using ElectronicObserver.Notifier;

namespace ElectronicObserver.Observer.kcsapi.api_req_map;

public class next : APIBase
{

	public override void OnResponseReceived(dynamic data)
	{
		KCDatabase db = KCDatabase.Instance;
		db.Replays.LoadFromResponse(APIName, data);




		if (Utility.Configuration.Config.Control.EnableDiscordRPC)
		{
			DiscordRpcModel dataForWS = DiscordRpcManager.Instance.CurrentClient.CurrentRpcData;
			dataForWS.TopDisplayText = string.Format(NotifierRes.SortieingTo, db.Battle.Compass.MapAreaID, db.Battle.Compass.MapInfoID, db.Battle.Compass.DestinationID, db.Battle.Compass.MapInfo.NameEN);

		}

		KCDatabase.Instance.Battle.LoadFromResponse(APIName, data);
		KCDatabase.Instance.TsunDbSubmission.LoadFromResponse(APIName, data);

		base.OnResponseReceived((object)data);


		// 表示順の関係上、UIの更新をしてからデータを更新する
		if (KCDatabase.Instance.Battle.Compass.EventID == 3)
		{
			EmulateWhirlpool();
		}

	}


	/// <summary>
	/// 渦潮による燃料・弾薬の減少をエミュレートします。
	/// </summary>
	public static void EmulateWhirlpool()
	{

		int itemID = KCDatabase.Instance.Battle.Compass.WhirlpoolItemID;
		int materialmax = KCDatabase.Instance.Fleet.Fleets.Values
			.Where(f => f != null && f.IsInSortie)
			.SelectMany(f => f.MembersWithoutEscaped)
			.Max(s =>
			{
				if (s == null) return 0;
				switch (itemID)
				{
					case 1:
						return s.Fuel;
					case 2:
						return s.Ammo;
					default:
						return 0;
				}
			});

		double rate = (double)KCDatabase.Instance.Battle.Compass.WhirlpoolItemAmount / materialmax;

		foreach (var ship in KCDatabase.Instance.Fleet.Fleets.Values
			.Where(f => f != null && f.IsInSortie)
			.SelectMany(f => f.MembersWithoutEscaped))
		{

			if (ship == null) continue;

			switch (itemID)
			{
				case 1:
					ship.Fuel -= (int)(ship.Fuel * rate);
					break;
				case 2:
					ship.Ammo -= (int)(ship.Ammo * rate);
					break;
			}
		}

	}


	public override string APIName => "api_req_map/next";
}
