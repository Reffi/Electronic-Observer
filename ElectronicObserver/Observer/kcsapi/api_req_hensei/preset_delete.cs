﻿using System.Collections.Generic;
using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_hensei;

public class preset_delete : APIBase
{
	public override void OnRequestReceived(Dictionary<string, string> data)
	{
		KCDatabase.Instance.FleetPreset.LoadFromRequest(APIName, data);

		base.OnRequestReceived(data);
	}

	public override bool IsRequestSupported => true;

	public override string APIName => "api_req_hensei/preset_delete";
}
