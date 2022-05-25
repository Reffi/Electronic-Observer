﻿using System.Collections.Generic;
using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_air_corps;

public class change_name : APIBase
{

	public override bool IsRequestSupported => true;

	public override void OnRequestReceived(Dictionary<string, string> data)
	{

		var corps = KCDatabase.Instance.BaseAirCorps;
		int aircorpsID = BaseAirCorpsData.GetID(data);

		if (corps.ContainsKey(aircorpsID))
		{
			corps[aircorpsID].LoadFromRequest(APIName, data);
		}

		base.OnRequestReceived(data);
	}

	public override string APIName => "api_req_air_corps/change_name";
}
