﻿using System.Collections.Generic;
using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_hensei;

public class combined : APIBase
{

	public override bool IsRequestSupported => true;
	public override bool IsResponseSupported => true;

	public override void OnRequestReceived(Dictionary<string, string> data)
	{

		KCDatabase.Instance.Fleet.LoadFromRequest(APIName, data);

		base.OnRequestReceived(data);
	}

	public override string APIName => "api_req_hensei/combined";
}
