﻿using System.Collections.Generic;
using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_member;

public class updatedeckname : APIBase
{


	public override void OnRequestReceived(Dictionary<string, string> data)
	{

		KCDatabase.Instance.Fleet.Fleets[int.Parse(data["api_deck_id"])].LoadFromRequest(APIName, data);

		base.OnRequestReceived(data);
	}


	public override bool IsRequestSupported => true;
	public override bool IsResponseSupported => false;

	public override string APIName => "api_req_member/updatedeckname";
}
