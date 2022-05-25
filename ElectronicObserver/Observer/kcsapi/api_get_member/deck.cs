﻿using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_get_member;

public class deck : APIBase
{

	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase.Instance.Fleet.LoadFromResponse(APIName, data);

		base.OnResponseReceived((object)data);
	}


	public override string APIName => "api_get_member/deck";
}
