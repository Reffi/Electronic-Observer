﻿using System.Collections.Generic;
using ElectronicObserver.Data.Battle.Phase;

namespace ElectronicObserver.Data.Battle;

/// <summary>
/// 連合艦隊 vs 通常艦隊 長距離空襲戦
/// </summary>
public class BattleCombinedAirRaid : BattleDay
{

	public override void LoadFromResponse(string apiname, dynamic data)
	{
		base.LoadFromResponse(apiname, (object)data);

		JetBaseAirAttack = new PhaseJetBaseAirAttack(this, "噴式基地航空隊攻撃");
		JetAirBattle = new PhaseJetAirBattle(this, "噴式航空戦");
		BaseAirAttack = new PhaseBaseAirAttack(this, "基地航空隊攻撃");
		AirBattle = new PhaseAirBattle(this, "空襲戦");
		// 支援はないものとする

		foreach (var phase in GetPhases())
			phase.EmulateBattle(_resultHPs, _attackDamages);

	}


	public override string APIName => "api_req_combined_battle/ld_airbattle";

	public override string BattleName => ConstantsRes.Title_CombinedFleetAirRaid;


	public override IEnumerable<PhaseBase> GetPhases()
	{
		yield return Initial;
		yield return Searching;
		yield return JetBaseAirAttack;
		yield return JetAirBattle;
		yield return BaseAirAttack;
		yield return AirBattle;
	}

}
