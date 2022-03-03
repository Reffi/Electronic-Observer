using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserverTypes;

namespace ElectronicObserver.Data.Quest;

/// <summary>
/// 演習任務の進捗を管理します。
/// </summary>
[DataContract(Name = "ProgressPractice")]
public class ProgressPractice : ProgressData
{

	/// <summary>
	/// 勝利のみカウントする
	/// </summary>
	[DataMember]
	private bool WinOnly { get; set; }

	/// <summary>
	/// 条件を満たす最低ランク
	/// </summary>
	[DataMember]
	private int LowestRank { get; set; }

	public ProgressPractice(QuestData quest, int maxCount, bool winOnly)
		: base(quest, maxCount)
	{
		LowestRank = winOnly ? Constants.GetWinRank("B") : Constants.GetWinRank("");
		WinOnly = winOnly;
	}

	public ProgressPractice(QuestData quest, int maxCount, string lowestRank)
		: base(quest, maxCount)
	{
		LowestRank = Constants.GetWinRank(lowestRank);
	}

	public void Increment(string rank)
	{
		if (Constants.GetWinRank(rank) < LowestRank) return;

		if (!MeetsSpecialRequirements(QuestID)) return;

		Increment();
	}

	private bool MeetsSpecialRequirements(int questId)
	{
		FleetData fleet = KCDatabase.Instance.Fleet.Fleets.Values
			.FirstOrDefault(f => f.IsInPractice);

		if (fleet == null) return false;

		List<IShipData> ships = fleet.MembersInstance.Where(s => s != null).ToList();

		return questId switch
		{
			//C16 m 給糧艦「伊良湖」の支援
			318 => ships.Count(s => s.MasterShip.ShipType == ShipTypes.LightCruiser) >= 2,

			// C29 q 空母機動部隊、演習始め！
			330 => ships[0].MasterShip.IsAircraftCarrier &&
				   ships.Count(s => s.MasterShip.IsAircraftCarrier) >= 2 &&
				   ships.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer) >= 2,
			// C38 q 「十八駆」演習！
			337 => ships.Any(s => s.MasterShip.BaseShip().ShipID == (int)ShipId.Kagerou) &&
				   ships.Any(s => s.MasterShip.BaseShip().ShipID == (int)ShipId.Shiranui) &&
				   ships.Any(s => s.MasterShip.BaseShip().ShipID == (int)ShipId.Arare) &&
				   ships.Any(s => s.MasterShip.BaseShip().ShipID == (int)ShipId.Kasumi),
			// C39 q 「十九駆」演習！
			339 => ships.Count(s => s.MasterShip.BaseShip().ShipId switch
			{
				ShipId.Isonami => true,
				ShipId.Uranami => true,
				ShipId.Ayanami => true,
				ShipId.Shikinami => true,

				_ => false
			}) >= 4,
			// C44 q 小艦艇群演習強化任務
			342 => ships.Count(s => s.MasterShip.ShipType switch
				   {
					   ShipTypes.Destroyer => true,
					   ShipTypes.Escort => true,

					   _ => false
				   }) >= 3
				   && ships.Count(s => s.MasterShip.ShipType switch
				   {
					   ShipTypes.Destroyer => true,
					   ShipTypes.Escort => true,
					   ShipTypes.LightCruiser => true,

					   _ => false
				   }) >= 4,

			// C53 02 「精鋭軽巡」演習！
			348 => ships.Count(s => s.MasterShip.ShipType is
					   ShipTypes.LightCruiser or
					   ShipTypes.TrainingCruiser
				   ) >= 3 &&
				   ships[0].MasterShip.ShipType is ShipTypes.LightCruiser or ShipTypes.TrainingCruiser &&
				   ships.Count(s => s.MasterShip.ShipType is ShipTypes.Destroyer) >= 2,

			//C55 03 精鋭「第七駆逐隊」演習開始！
			350 => ships.Count(s => s.MasterShip.BaseShip().ShipId switch
			{
				ShipId.Oboro => true,
				ShipId.Akebono => true,
				ShipId.Sazanami => true,
				ShipId.Ushio => true,

				_ => false
			}) >= 2,

			// C58 06 「巡洋艦戦隊」演習！
			353 => ships.Count(s => s.MasterShip.ShipType is
					   ShipTypes.HeavyCruiser or
					   ShipTypes.AviationCruiser
				   ) >= 4 &&
				   ships[0].MasterShip.ShipType is ShipTypes.HeavyCruiser or ShipTypes.AviationCruiser &&
				   ships.Count(s => s.MasterShip.ShipType is ShipTypes.Destroyer) >= 2,

			 //C60 07 「改装特務空母」任務部隊演習！
			354 => ships.Count(s => s.MasterShip.BaseShip().ShipId switch
			{
				ShipId.Fletcher => true,
				ShipId.Johnston => true,
				ShipId.SamuelBRoberts => true,

				_ => false
			}) >= 2 &&
			ships[0].MasterShip.ShipID == 707,

			//C49 09 演習ティータイム！
			345 => ships.Count(s => s.MasterShip.BaseShip().ShipId switch
			{
				ShipId.Warspite => true,
				ShipId.Kongou => true,
				ShipId.ArkRoyal => true,
				ShipId.Nelson => true,
				ShipId.Jervis => true,
				ShipId.Janus => true,

				_ => false
			}) >= 4,

			//C50 09 最精鋭！主力オブ主力、演習開始！
			346 => ships.Any(s => s.MasterShip.ShipID == (int)ShipId.YuugumoKaiNi) &&
				   ships.Any(s => s.MasterShip.ShipID == (int)ShipId.NaganamiKaiNi) &&
				   ships.Any(s => s.MasterShip.ShipID == (int)ShipId.KazagumoKaiNi) &&
				   ships.Any(s => s.MasterShip.ShipID == (int)ShipId.AkigumoKaiNi),

			//C62 09 精鋭「第十五駆逐隊」第一小隊演習！
			355 => ships[0].MasterShip.ShipID == 568 &&
			       ships[1].MasterShip.ShipID == 670,

			//C23 改夕雲型、演習始め！
			325 => ships.Any(s => s.MasterShip.ShipID == (int)ShipId.YuugumoKaiNi) &&
				   ships.Any(s => s.MasterShip.ShipID == (int)ShipId.NaganamiKaiNi),
			//C26 精強「十七駆」、猛特訓！
			328 => ships.Any(s => s.MasterShip.ShipID == (int)ShipId.IsokazeBKai) &&
				   ships.Any(s => s.MasterShip.ShipID == (int)ShipId.HamakazeBKai) &&
				   ships.Any(s => s.MasterShip.ShipID == (int)ShipId.UrakazeDKai) &&
				   ships.Any(s => s.MasterShip.ShipID == (int)ShipId.TanikazeDKai),
			//C31 艦載機演習
			331 => ships[0].MasterShip.IsRegularCarrier &&
				   ships.Count(s => s.MasterShip.IsRegularCarrier) >= 2 &&
				   ships.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer) >= 2,
			//C37 輸送船団演習
			336 => ships.Count(s => s.MasterShip.ShipType == ShipTypes.AmphibiousAssaultShip ||
									s.MasterShip.ShipType == ShipTypes.FleetOiler ||
									s.MasterShip.ShipType == ShipTypes.Escort) >= 2,

			// 2201LC01 【節分任務】節分演習！二〇二二
			329 => ships.Count(s => s.MasterShip.ShipType switch
				   {
					   ShipTypes.LightCruiser => true,
					   ShipTypes.TrainingCruiser => true,

					   _ => false
				   }) >= 2
				   && ships.Count(s => s.MasterShip.ShipType switch
				   {
					   ShipTypes.Destroyer => true,
					   ShipTypes.Escort => true,

					   _ => false
				   }) >= 2,
			// 7thAnvLC1 七周年任務【七駆演習】
			341 => ships.Count(s => s.MasterShip.BaseShip().ShipId switch
			{
				ShipId.Oboro => true,
				ShipId.Akebono => true,
				ShipId.Sazanami => true,
				ShipId.Ushio => true,

				_ => false
			}) >= 2,
			// 2102LQ3 バレンタイン限定任務 【特別演習】
			349 =>
				ships.Count(s => s.MasterShip.ShipType is ShipTypes.Destroyer) >= 2 &&
				ships.Count(s => s.MasterShip.ShipType is ShipTypes.SeaplaneTender) >= 1 &&
				ships.Count(s => s.MasterShip.ShipType is
					ShipTypes.LightCruiser or
					ShipTypes.TrainingCruiser or
					ShipTypes.TorpedoCruiser
				) >= 1 &&
				ships.Count(s => s.MasterShip.ShipType is
					ShipTypes.HeavyCruiser or
					ShipTypes.AviationCruiser
				) >= 1,

			_ => true,
		};
	}

	public override string GetClearCondition()
	{
		return QuestTracking.Exercise + (WinOnly ? QuestTracking.ClearConditionVictories : "×") + ProgressMax;
	}
}
