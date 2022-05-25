﻿using System.Collections.Generic;
using ElectronicObserver.Data;
using static ElectronicObserver.Observer.DiscordRPC;

namespace ElectronicObserver.Observer.kcsapi.api_req_practice;

public class battle : APIBase
{

	public override bool IsRequestSupported => true;

	public override void OnRequestReceived(Dictionary<string, string> data)
	{

		KCDatabase.Instance.Fleet.LoadFromRequest(APIName, data);

		base.OnRequestReceived(data);
	}


	public override void OnResponseReceived(dynamic data)
	{
		KCDatabase.Instance.Battle.LoadFromResponse(APIName, data);
		KCDatabase.Instance.Replays.LoadFromResponse(APIName, data);

		if (Utility.Configuration.Config.Control.EnableDiscordRPC)
		{
			DiscordFormat dataForWS = Instance.data;
			dataForWS.top = ObserverRes.DoingExercises;
		}


		KCDatabase.Instance.Battle.LoadFromResponse(APIName, data);


		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_req_practice/battle";
}
