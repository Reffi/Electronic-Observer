﻿using System.Collections.Generic;
using ElectronicObserver.Data.Battle.Phase;

namespace ElectronicObserver.Data.Battle;

/// <summary>
/// 基地防空戦
/// </summary>
public class BattleBaseAirRaid : BattleDay
{

	public PhaseBaseAirRaid BaseAirRaid { get; protected set; }

	public override void LoadFromResponse(string apiname, dynamic data)
	{
		base.LoadFromResponse(apiname, (object)data);

		BaseAirRaid = new PhaseBaseAirRaid(this, "防空戦");

		foreach (var phase in GetPhases())
			phase.EmulateBattle(_resultHPs, _attackDamages);

	}


	public override string APIName => "api_req_map/next";

	public override string BattleName => ConstantsRes.Title_BaseAirRaid;

	public override bool IsBaseAirRaid => true;

	public override IEnumerable<PhaseBase> GetPhases()
	{
		yield return Initial;
		yield return Searching;
		yield return BaseAirRaid;
	}
}
