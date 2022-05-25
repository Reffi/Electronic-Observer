﻿using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_member;

public class get_practice_enemyinfo : APIBase
{

	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase.Instance.Battle.LoadFromResponse(APIName, data);

		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_req_member/get_practice_enemyinfo";
}
