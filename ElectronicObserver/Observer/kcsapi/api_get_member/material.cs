﻿using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_get_member;

public class material : APIBase
{

	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase.Instance.Material.LoadFromResponse(APIName, data);

		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_get_member/material";
}
