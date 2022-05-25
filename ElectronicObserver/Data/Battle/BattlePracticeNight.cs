﻿using System.Collections.Generic;
using ElectronicObserver.Data.Battle.Phase;

namespace ElectronicObserver.Data.Battle;

/// <summary>
/// 演習 夜戦
/// </summary>
public class BattlePracticeNight : BattleNight
{

	public override void LoadFromResponse(string apiname, dynamic data)
	{
		base.LoadFromResponse(apiname, (object)data);

		NightInitial = new PhaseNightInitial(this, "夜戦開始", false);
		NightBattle = new PhaseNightBattle(this, "夜戦", 0);

		NightBattle.EmulateBattle(_resultHPs, _attackDamages);

	}


	public override string APIName => "api_req_practice/midnight_battle";

	public override string BattleName => ConstantsRes.Title_PracticeNight;

	public override bool IsPractice => true;


	public override IEnumerable<PhaseBase> GetPhases()
	{
		yield return Initial;
		yield return NightInitial;
		yield return NightBattle;
	}
}
