﻿using System.Collections.Generic;
using ElectronicObserver.Data;
using ElectronicObserver.Notifier;
using static ElectronicObserver.Observer.DiscordRPC;

namespace ElectronicObserver.Observer.kcsapi.api_req_map;

public class start : APIBase
{

	public override void OnResponseReceived(dynamic data)
	{


		KCDatabase db = KCDatabase.Instance;

		db.Battle.LoadFromResponse(APIName, data);
		db.Replays.LoadFromResponse(APIName, data);
		if (Utility.Configuration.Config.Control.EnableDiscordRPC)
		{
			DiscordFormat dataForWS = Instance.data;
			dataForWS.top = string.Format(NotifierRes.SortieingTo, db.Battle.Compass.MapAreaID, db.Battle.Compass.MapInfoID, db.Battle.Compass.DestinationID, db.Battle.Compass.MapInfo.NameEN);
		}

		KCDatabase.Instance.TsunDbSubmission.LoadFromResponse(APIName, data);

		base.OnResponseReceived((object)data);



		// 表示順の関係上、UIの更新をしてからデータを更新する
		if (KCDatabase.Instance.Battle.Compass.EventID == 3)
		{
			next.EmulateWhirlpool();
		}

	}


	public override bool IsRequestSupported => true;

	public override void OnRequestReceived(Dictionary<string, string> data)
	{

		KCDatabase.Instance.Fleet.LoadFromRequest(APIName, data);

		int deckID = int.Parse(data["api_deck_id"]);
		int maparea = int.Parse(data["api_maparea_id"]);
		int mapinfo = int.Parse(data["api_mapinfo_no"]);

		Utility.Logger.Add(2, string.Format(NotifierRes.HasSortiedTo, deckID, KCDatabase.Instance.Fleet[deckID].Name, maparea, mapinfo, KCDatabase.Instance.MapInfo[maparea * 10 + mapinfo].NameEN));

		base.OnRequestReceived(data);
	}


	public override string APIName => "api_req_map/start";
}
