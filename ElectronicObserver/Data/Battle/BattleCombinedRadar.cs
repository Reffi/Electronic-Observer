﻿using System.Collections.Generic;
using ElectronicObserver.Data.Battle.Phase;
using ElectronicObserver.Properties.Data;

namespace ElectronicObserver.Data.Battle;

/// <summary>
/// 連合艦隊 vs 通常艦隊 レーダー射撃
/// </summary>
public class BattleCombinedRadar : BattleDay
{
	public override void LoadFromResponse(string apiname, dynamic data)
	{
		base.LoadFromResponse(apiname, (object)data);

		JetBaseAirAttack = new PhaseJetBaseAirAttack(this, "噴式基地航空隊攻撃");
		BaseAirAttack = new PhaseBaseAirAttack(this, "基地航空隊攻撃");
		Shelling1 = new PhaseRadar(this, "レーダー射撃");

		foreach (var phase in GetPhases())
			phase.EmulateBattle(_resultHPs, _attackDamages);
	}

	public override string APIName => "api_req_combined_battle/ld_shooting";

	public override string BattleName => BattleRes.CombinedFleetRadarAmbush;

	public override IEnumerable<PhaseBase> GetPhases()
	{
		yield return Initial;
		yield return Searching;     // ?
		yield return JetBaseAirAttack;
		yield return BaseAirAttack;
		yield return Shelling1;
	}
}
