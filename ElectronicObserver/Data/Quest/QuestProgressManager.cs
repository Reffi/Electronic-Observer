using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Utility.Storage;
using ElectronicObserverTypes;

namespace ElectronicObserver.Data.Quest;

/// <summary>
/// 任務の進捗を管理します。
/// </summary>
[DataContract(Name = "QuestProgress")]
[KnownType(typeof(ProgressData))]
[KnownType(typeof(ProgressAGo))]
[KnownType(typeof(ProgressBattle))]
[KnownType(typeof(ProgressMultiBattle))]
[KnownType(typeof(ProgressSpecialBattle))]
[KnownType(typeof(ProgressConstruction))]
[KnownType(typeof(ProgressDestruction))]
[KnownType(typeof(ProgressDevelopment))]
[KnownType(typeof(ProgressDiscard))]
[KnownType(typeof(ProgressMultiDiscard))]
[KnownType(typeof(ProgressDocking))]
[KnownType(typeof(ProgressExpedition))]
[KnownType(typeof(ProgressMultiExpedition))]
[KnownType(typeof(ProgressImprovement))]
[KnownType(typeof(ProgressModernization))]
[KnownType(typeof(ProgressPractice))]
[KnownType(typeof(ProgressSlaughter))]
[KnownType(typeof(ProgressSupply))]
public sealed class QuestProgressManager : DataStorage
{


	public const string DefaultFilePath = @"Settings\QuestProgress.xml";


	[IgnoreDataMember]
	public IDDictionary<ProgressData> Progresses { get; private set; }

	[DataMember]
	private List<ProgressData> SerializedProgresses
	{
		get
		{
			return Progresses.Values.ToList();
		}
		set
		{
			Progresses = new IDDictionary<ProgressData>(value);
		}
	}

	[DataMember]
	public DateTime LastUpdateTime { get; set; }

	/*
	[DataMember]
	private string LastUpdateTimeSerializer {
		get { return DateTimeHelper.TimeToCSVString( LastUpdateTime ); }
		set { LastUpdateTime = DateTimeHelper.CSVStringToTime( value ); }
	}
	*/

	[IgnoreDataMember]
	private DateTime _prevTime;


	public QuestProgressManager()
	{
		Initialize();
	}


	public override void Initialize()
	{
		Progresses = new IDDictionary<ProgressData>();
		LastUpdateTime = DateTime.Now;

		RemoveEvents();     //二重登録防止


		var ao = APIObserver.Instance;

		ao.APIList["api_get_member/questlist"].ResponseReceived += QuestUpdated;

		ao.ApiReqMap_Start.ResponseReceived += StartSortie;

		ao.APIList["api_req_map/next"].ResponseReceived += NextSortie;

		ao.APIList["api_req_sortie/battleresult"].ResponseReceived += BattleFinished;
		ao.APIList["api_req_combined_battle/battleresult"].ResponseReceived += BattleFinished;

		ao.APIList["api_req_practice/battle_result"].ResponseReceived += PracticeFinished;

		ao.APIList["api_req_mission/result"].ResponseReceived += ExpeditionCompleted;

		ao.ApiReqNyukyo_Start.RequestReceived += StartRepair;

		ao.APIList["api_req_hokyu/charge"].ResponseReceived += Supplied;

		ao.APIList["api_req_kousyou/createitem"].ResponseReceived += EquipmentDeveloped;

		ao.APIList["api_req_kousyou/createship"].RequestReceived += ShipConstructed;

		ao.ApiReqKousyou_Destroyship.RequestReceived += ShipDestructed;

		// 装備廃棄はイベント前に装備データが削除されてしまうので destroyitem2 から直接呼ばれる

		ao.APIList["api_req_kousyou/remodel_slot"].ResponseReceived += EquipmentRemodeled;

		ao.APIList["api_req_kaisou/powerup"].ResponseReceived += Modernized;

		ao.ApiPort_Port.ResponseReceived += TimerSave;


		_prevTime = DateTime.Now;
	}

	public void RemoveEvents()
	{

		var ao = APIObserver.Instance;

		ao.APIList["api_get_member/questlist"].ResponseReceived -= QuestUpdated;

		ao.ApiReqMap_Start.ResponseReceived -= StartSortie;

		ao.APIList["api_req_map/next"].ResponseReceived -= NextSortie;

		ao.APIList["api_req_sortie/battleresult"].ResponseReceived -= BattleFinished;
		ao.APIList["api_req_combined_battle/battleresult"].ResponseReceived -= BattleFinished;

		ao.APIList["api_req_practice/battle_result"].ResponseReceived -= PracticeFinished;

		ao.APIList["api_req_mission/result"].ResponseReceived -= ExpeditionCompleted;

		ao.ApiReqNyukyo_Start.RequestReceived -= StartRepair;

		ao.APIList["api_req_hokyu/charge"].ResponseReceived -= Supplied;

		ao.APIList["api_req_kousyou/createitem"].ResponseReceived -= EquipmentDeveloped;

		ao.APIList["api_req_kousyou/createship"].RequestReceived -= ShipConstructed;

		ao.ApiReqKousyou_Destroyship.ResponseReceived -= ShipDestructed;

		// 装備廃棄は(ry

		ao.APIList["api_req_kousyou/remodel_slot"].ResponseReceived -= EquipmentRemodeled;

		ao.APIList["api_req_kaisou/powerup"].ResponseReceived -= Modernized;

		ao.ApiPort_Port.ResponseReceived -= TimerSave;

	}

	public ProgressData this[int key] => Progresses[key];



	void TimerSave(string apiname, dynamic data)
	{

		bool iscleared;

		switch (Utility.Configuration.Config.FormQuest.ProgressAutoSaving)
		{
			case 0:
			default:
				iscleared = false;
				break;
			case 1:
				iscleared = DateTimeHelper.IsCrossedHour(_prevTime);
				break;
			case 2:
				iscleared = DateTimeHelper.IsCrossedDay(_prevTime, 0, 0, 0);
				break;
		}


		if (iscleared)
		{
			_prevTime = DateTime.Now;

			Save();
			Utility.Logger.Add(1, QuestTracking.AutoSavedProgress);
		}

	}


	void QuestUpdated(string apiname, dynamic data)
	{


		var quests = KCDatabase.Instance.Quest;

		//消えている・達成済みの任務の進捗情報を削除
		if (quests.IsLoadCompleted)
			Progresses.RemoveAll(q => !quests.Quests.ContainsKey(q.QuestID) || quests[q.QuestID].State == 3);


		foreach (var q in quests.Quests.Values)
		{

			//達成済みはスキップ
			if (q.State == 3) continue;

			// 進捗情報の生成
			if (!Progresses.ContainsKey(q.QuestID))
			{

				#region 地 獄 の 任 務 I D べ た 書 き 祭 り

				switch (q.QuestID)
				{
					case 201:   //Bd1 敵艦隊を撃破せよ！
						Progresses.Add(new ProgressBattle(q, 1, "B", null, false));
						break;
					case 216:   //Bd2 敵艦隊主力を撃滅せよ！
						Progresses.Add(new ProgressBattle(q, 1, "E", null, false));
						break;
					case 210:   //Bd3 敵艦隊を10回邀撃せよ！
						Progresses.Add(new ProgressBattle(q, 10, "E", null, false));
						break;
					case 211:   //Bd4|敵空母を3隻撃沈せよ！
						Progresses.Add(new ProgressSlaughter(q, 3, new[] { 7, 11 }));
						break;
					case 212:   //Bd6 敵輸送船団を叩け！
						Progresses.Add(new ProgressSlaughter(q, 5, new[] { 15 }));
						break;
					case 218:   //Bd5 敵補給艦を3隻撃沈せよ！
						Progresses.Add(new ProgressSlaughter(q, 3, new[] { 15 }));
						break;
					case 226:   //Bd7 南西諸島海域の制海権を握れ！
						Progresses.Add(new ProgressBattle(q, 5, "B", new[] { 21, 22, 23, 24, 25 }, true));
						break;
					case 230:   //Bd8 敵潜水艦を制圧せよ！
						Progresses.Add(new ProgressSlaughter(q, 6, new[] { 13 }));
						break;

					case 213:   //Bw3 海上通商破壊作戦
						Progresses.Add(new ProgressSlaughter(q, 20, new[] { 15 }));
						break;
					case 214:   //Bw1 あ号作戦
						Progresses.Add(new ProgressAGo(q));
						break;
					case 220:   //Bw2 い号作戦
						Progresses.Add(new ProgressSlaughter(q, 20, new[] { 7, 11 }));
						break;
					case 221:   //Bw4 ろ号作戦
						Progresses.Add(new ProgressSlaughter(q, 50, new[] { 15 }));
						break;
					case 228:   //Bw5 海上護衛戦
						Progresses.Add(new ProgressSlaughter(q, 15, new[] { 13 }));
						break;
					case 229:   //Bw6 敵東方艦隊を撃滅せよ！
						Progresses.Add(new ProgressBattle(q, 12, "B", new[] { 41, 42, 43, 44, 45 }, true));
						break;
					case 241:   //Bw7 敵北方艦隊主力を撃滅せよ！
						Progresses.Add(new ProgressBattle(q, 5, "B", new[] { 33, 34, 35 }, true));
						break;
					case 242:   //Bw8 敵東方中枢艦隊を撃破せよ！
						Progresses.Add(new ProgressBattle(q, 1, "B", new[] { 44 }, true));
						break;
					case 243:   //Bw9 南方海域珊瑚諸島沖の制空権を握れ！
						Progresses.Add(new ProgressBattle(q, 2, "S", new[] { 52 }, true));
						break;
					case 261:   //Bw10 海上輸送路の安全確保に努めよ！
						Progresses.Add(new ProgressBattle(q, 3, "A", new[] { 15 }, true));
						break;

					case 249:   //Bm1 第五戦隊」出撃せよ！
						Progresses.Add(new ProgressSpecialBattle(q, 1, "S", new[] { 25 }, true));
						break;
					case 256:   //Bm2 「潜水艦隊」出撃せよ！
						Progresses.Add(new ProgressBattle(q, 3, "S", new[] { 61 }, true));
						break;
					case 257:   //Bm3 「水雷戦隊」南西へ！
						Progresses.Add(new ProgressSpecialBattle(q, 1, "S", new[] { 14 }, true));
						break;
					case 259:   //Bm4 「水上打撃部隊」南方へ！
						Progresses.Add(new ProgressSpecialBattle(q, 1, "S", new[] { 51 }, true));
						break;
					case 264:   //Bm5 「空母機動部隊」西へ！
						Progresses.Add(new ProgressSpecialBattle(q, 1, "S", new[] { 42 }, true));
						break;
					case 265:   //Bm6 海上護衛強化月間
						Progresses.Add(new ProgressBattle(q, 10, "A", new[] { 15 }, true));
						break;
					case 266:   //Bm7 「水上反撃部隊」突入せよ！
						Progresses.Add(new ProgressSpecialBattle(q, 1, "S", new[] { 25 }, true));
						break;
					case 280:   //Bm8 兵站線確保！海上警備を強化実施せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 12 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 13 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 14 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 21 }, true),
						}));
						break;

					case 822:   //Bq1 沖ノ島海域迎撃戦
						Progresses.Add(new ProgressBattle(q, 2, "S", new[] { 24 }, true));
						break;
					case 854:   //Bq2 戦果拡張任務！「Z作戦」前段作戦
						Progresses.Add(new ProgressMultiBattle(q, new[]{
							new ProgressBattle(q, 1, "A", new[]{ 24 }, true),
							new ProgressBattle(q, 1, "A", new[]{ 61 }, true),
							new ProgressBattle(q, 1, "A", new[]{ 63 }, true),
							new ProgressBattle(q, 1, "S", new[]{ 64 }, true),
						}));
						break;
					case 861:   //Bq3 強行輸送艦隊、抜錨！
						Progresses.Add(new ProgressSpecialBattle(q, 2, "x", new[] { 16 }, true));
						break;
					case 862:   //Bq4 前線の航空偵察を実施せよ！
						Progresses.Add(new ProgressSpecialBattle(q, 2, "A", new[] { 63 }, true));
						break;
					case 873:   //Bq5 北方海域警備を実施せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]{
							new ProgressSpecialBattle(q, 1, "A", new[]{ 31 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[]{ 32 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[]{ 33 }, true),
						}));
						break;
					case 875:   //Bq6 精鋭「三一駆」、鉄底海域に突入せよ！
						Progresses.Add(new ProgressSpecialBattle(q, 2, "S", new[] { 54 }, true));
						break;
					case 888:   //Bq7 新編成「三川艦隊」、鉄底海峡に突入せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 51 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 53 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 54 }, true),
						}));
						break;
					case 893:   //Bq8 泊地周辺海域の安全確保を徹底せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]{
							new ProgressBattle(q, 3, "S", new[]{ 15 }, true),
							new ProgressBattle(q, 3, "S", new[]{ 71 }, true),
							new ProgressSpecialBattle(q, 3, "S", new[]{ 72 }, true, 1),
							new ProgressSpecialBattle(q, 3, "S", new[]{ 72 }, true, 2),
						})); break;
					case 894:   //Bq9 空母戦力の投入による兵站線戦闘哨戒
						Progresses.Add(new ProgressMultiBattle(q, new[]{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 13 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 14 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 21 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 22 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 23 }, true),
						}));
						break;
					case 872:   //Bq10 戦果拡張任務！「Z作戦」後段作戦
						Progresses.Add(new ProgressMultiBattle(q, new[]{
							new ProgressBattle(q, 1, "S", new[]{ 55 }, true),
							new ProgressBattle(q, 1, "S", new[]{ 62 }, true),
							new ProgressBattle(q, 1, "S", new[]{ 65 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 72 }, true, 2),
						}));
						break;
					case 284:   //Bq11 南西諸島方面「海上警備行動」発令！
						Progresses.Add(new ProgressMultiBattle(q, new[]{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 14 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 21 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 22 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 23 }, true),
						}));
						break;
					case 845:   //Bq12 発令！「西方海域作戦」
						Progresses.Add(new ProgressMultiBattle(q, new[] {
							new ProgressBattle(q, 1, "S", new[] { 41 }, true),
							new ProgressBattle(q, 1, "S", new[] { 42 }, true),
							new ProgressBattle(q, 1, "S", new[] { 43 }, true),
							new ProgressBattle(q, 1, "S", new[] { 44 }, true),
							new ProgressBattle(q, 1, "S", new[] { 45 }, true),
						}));
						break;
					case 903:   //Bq13 拡張「六水戦」、最前線へ！
						Progresses.Add(new ProgressMultiBattle(q, new[] {
							new ProgressSpecialBattle(q, 1, "S", new[] { 51 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 54 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 64 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 65 }, true),
						}));
						break;

					case 904:   //By1 02 精鋭「十九駆」、躍り出る！|
						Progresses.Add(new ProgressMultiBattle(q, new[] {
							new ProgressSpecialBattle(q, 1, "S", new[] { 25 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 34 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 45 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 53 }, true),
						}));
						break;
					case 905:   //By2 02 「海防艦」、海を護る！
						Progresses.Add(new ProgressMultiBattle(q, new[] {
							new ProgressSpecialBattle(q, 1, "A", new[] { 11 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[] { 12 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[] { 13 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[] { 15 }, true),
							new ProgressSpecialBattle(q, 1, "x", new[] { 16 }, true),
						}));
						break;

					case 912:   //By3 03 工作艦「明石」護衛任務
						Progresses.Add(new ProgressMultiBattle(q, new[] {
							new ProgressSpecialBattle(q, 1, "A", new[] { 13 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[] { 21 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[] { 22 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[] { 23 }, true),
							new ProgressSpecialBattle(q, 1, "x", new[] { 16 }, true),
						}));
						break;
					case 914:   //By4 03 重巡戦隊、西へ！|4-1・4-2・4-3・4-4ボスA勝利各1|要重巡3/駆逐1
						Progresses.Add(new ProgressMultiBattle(q, new[] {
							new ProgressSpecialBattle(q, 1, "A", new[] { 41 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[] { 42 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[] { 43 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[] { 44 }, true),
						}));
						break;

					case 928:   //By5 09 歴戦「第十方面艦隊」、全力出撃！|4-2・7-2(第二)・7-3(第二)ボスS勝利各2|要(羽黒/足柄/妙高/高雄/神風)2
						Progresses.Add(new ProgressMultiBattle(q, new[] {
							new ProgressSpecialBattle(q, 2, "S", new[] { 42 }, true),
							new ProgressSpecialBattle(q, 2, "S", new[] { 72 }, true, 2),
							new ProgressSpecialBattle(q, 2, "S", new[] { 73 }, true, 2),
						}));
						break;

					case 944: //By6 06 鎮守府近海海域の哨戒を実施せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 2, "S", new[]{ 12 }, true),
							new ProgressSpecialBattle(q, 2, "S", new[]{ 13 }, true),
							new ProgressSpecialBattle(q, 2, "S", new[]{ 14 }, true),
						}));
						break;
					case 945: //By7 06 南西方面の兵站航路の安全を図れ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 2, "A", new[]{ 15 }, true),
							new ProgressSpecialBattle(q, 2, "A", new[]{ 21 }, true),
							new ProgressSpecialBattle(q, 2, "x", new[]{ 16 }, true),
						}));
						break;
					case 946: //By8 06 空母機動部隊、出撃！敵艦隊を迎撃せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 22 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 23 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 24 }, true),
						}));
						break;
					case 947: //By9 06 AL 作戦】
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 31 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 33 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 34 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 35 }, true),
						}));
						break;
					case 948: //By10 06 機動部隊決戦
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 2, "S", new[]{ 52 }, true),
							new ProgressSpecialBattle(q, 2, "S", new[]{ 55 }, true),
							new ProgressSpecialBattle(q, 2, "A", new[]{ 64 }, true),
							new ProgressSpecialBattle(q, 2, "S", new[]{ 65 }, true),
						}));
						break;

					case 883: // 7thAnvLB2 七周年任務【前段作戦】
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 23 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 31 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 32 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 33 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 34 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 35 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 72 }, true, 2),
						}));
						break;
					case 910: // 7thAnvLB3 七周年任務【拡張作戦】
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 61 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 62 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 63 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 64 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 65 }, true),
						}));
						break;

					case 235: // B135  	近海哨戒を実施せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 12 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 13 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 21 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 22 }, true),
						}));
						break;
					case 246: // WB02 二人でする初めての任務！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 43 }, true),
						}));
						break;
					case 251: // B26 精鋭「第二航空戦隊」抜錨せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 43 }, true),
						}));
						break;
					case 262: // B33 「西村艦隊」南方海域へ進出せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 51 }, true),
						}));
						break;
					case 276: // B44 海上突入部隊、進発せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 51 }, true),
						}));
						break;
					case 290: // B128 「比叡」の出撃
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 53 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 54 }, true),
						}));
						break;
					case 298: // B124 「第七駆逐隊」、南西諸島を駆ける！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 21 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 22 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 23 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 24 }, true),
						}));
						break;
					case 831: // SB43 春の海上警備行動！艦隊、抜錨せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 11 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 12 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 13 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 14 }, true),
						}));
						break;
					case 832: // SB44  	春！「三一駆」旗艦「長波」、出撃せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 2, "S", new[]{ 21 }, true),
							new ProgressSpecialBattle(q, 2, "S", new[]{ 22 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[]{ 54 }, true),
						}));
						break;
					case 833: // B139 秋刀魚漁：最新鋭の秋刀魚漁！もっとぉ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 2, "A", new[]{ 45 }, true),
							new ProgressSpecialBattle(q, 2, "A", new[]{ 64 }, true),
						}));
						break;
					case 856: // B99 新編「第一戦隊」、抜錨せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 45 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 55 }, true),
						}));
						break;
					case 859: // B102 精鋭「第四航空戦隊」、抜錨せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "A", new[]{ 25 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[]{ 35 }, true),
						}));
						break;
					case 863: // B104 精鋭「第二二駆逐隊」出撃せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 32 }, true),
						}));
						break;
					case 865: // B106  	夜間作戦空母、前線に出撃せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 65 }, true),
						}));
						break;
					case 874: // B110 北方海域戦闘哨戒を実施せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 2, "S", new[]{ 35 }, true),
						}));
						break;
					case 876: // B111 松輸送作戦、開始せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 2, "A", new[]{ 14 }, true),
							new ProgressSpecialBattle(q, 2, "X", new[]{ 16 }, true),
						}));
						break;
					case 877: // B112 精鋭「四水戦」、南方海域に展開せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "A", new[]{ 51 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[]{ 53 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[]{ 54 }, true),
						}));
						break;
					case 880: // B115 精鋭駆逐隊、獅子奮迅！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "A", new[]{ 23 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[]{ 32 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[]{ 42 }, true),
							new ProgressSpecialBattle(q, 1, "X", new[]{ 16 }, true),
						}));
						break;
					case 882: // 7thAnvLB1 七周年任務【前段作戦】
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 12 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 13 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 14 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 15 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 21 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 22 }, true),
						}));
						break;
					case 885: // B118  	戦闘航空母艦、出撃せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 35 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 45 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 64 }, true),
						}));
						break;
					case 887: // B120  	精鋭「第十八戦隊」、展開せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 12 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 14 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 15 }, true),
							new ProgressSpecialBattle(q, 1, "X", new[]{ 16 }, true),
						}));
						break;
					case 890: // B122 精鋭「四戦隊」第二小隊、抜錨せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 14 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 23 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 33 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 45 }, true),
						}));
						break;
					case 891: // B123 精強「十七駆」、北へ、南へ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "A", new[]{ 15 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[]{ 32 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[]{ 51 }, true),
							new ProgressSpecialBattle(q, 1, "A", new[]{ 71 }, true),
						}));
						break;
					case 892: // B126 主力オブ主力、抜錨開始！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 53 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 54 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 55 }, true),
						}));
						break;
					case 895: // B127 冬季北方海域作戦
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 2, "S", new[]{ 31 }, true),
							new ProgressSpecialBattle(q, 2, "S", new[]{ 33 }, true),
							new ProgressSpecialBattle(q, 2, "S", new[]{ 34 }, true),
							new ProgressSpecialBattle(q, 2, "S", new[]{ 35 }, true),
						}));
						break;
					case 901:   // B140 「夕張改二」試してみてもいいかしら？
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 25 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 33 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 53 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 63 }, true),
						}));
						break;
					case 902:   // B141 新編「六水戦」出撃！後で感想、聞かせてね！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 15 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 22 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 32 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 71 }, true),
							new ProgressSpecialBattle(q, 1, "x", new[]{ 16 }, true),
						}));
						break;
					case 896:   // B131 航空戦艦戦隊、戦闘哨戒！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 14 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 15 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 23 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 72 }, true, 2),
						}));
						break;
					case 897: // B132  	最精鋭｢第四航空戦隊｣、出撃せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 45 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 55 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 65 }, true),
							new ProgressSpecialBattle(q, 2, "x", new[]{ 16 }, true),
						}));
						break;
					case 913: // B143 「第五航空戦隊」、縦横無尽！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 35 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 52 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 65 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 72 }, true, 2),
						}));
						break;
					case 917: // B145 改装航空軽巡「Gotland andra」、出撃！
						Progresses.Add(new ProgressMultiBattle(q, new[]
						{
							new ProgressSpecialBattle(q, 1, "S", new[]{ 24 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 42 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 44 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[]{ 45 }, true),
						}));
						break;
					case 924:   // B152 【航空母艦特別任務】航空戦隊、精鋭無比！
						Progresses.Add(new ProgressMultiBattle(q, new[] {
							new ProgressSpecialBattle(q, 1, "S", new[] { 24 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 25 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 43 }, true),
						}));
						break;
					case 927:   // B155 重巡「羽黒」、出撃！ペナン沖海戦
						Progresses.Add(new ProgressMultiBattle(q, new[] {
							new ProgressSpecialBattle(q, 4, "A", new[] { 73 }, true),
						}));
						break;
					case 929:   // B156 静かな海を護る「鯨」、動き出す！
						Progresses.Add(new ProgressMultiBattle(q, new[] {
							new ProgressSpecialBattle(q, 1, "S", new[] { 12 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 13 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 21 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 23 }, true),
						}));
						break;
					case 932:   // 2103LQ1 【春限定】春の天津風！
						if (DateTime.Now < new DateTime(2021, 6, 1))
						{
							Progresses.Add(new ProgressMultiBattle(q, new[]
							{
								new ProgressSpecialBattle(q, 2, "A", new[] {22}, true),
								new ProgressSpecialBattle(q, 2, "A", new[] {23}, true),
								new ProgressSpecialBattle(q, 2, "x", new[] {73}, true, 2),
							}));
						}
						break;
					case 936: // B164 改装最新鋭軽巡「能代改二」、出撃せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[] {
							new ProgressSpecialBattle(q, 1, "S", new[] { 24 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 32 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 53 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 71 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 72 }, true, 2),
						}));
						break;
					case 937: // B165 精鋭「第七駆逐隊」、出撃せよ！
						Progresses.Add(new ProgressMultiBattle(q, new[] {
							new ProgressSpecialBattle(q, 1, "S", new[] { 23 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 32 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 44 }, true),
							new ProgressSpecialBattle(q, 1, "S", new[] { 54 }, true),
						}));
						break;

					case 840:   //|840 【節分任務】節分作戦二〇二二
						if (DateTime.Now < new DateTime(2022, 4, 1))
						{
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressSpecialBattle(q, 1, "A", new[] { 15 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[] { 13 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[] { 23 }, true),
							}));
						}
						break;
					case 841:   //|841 【節分任務】南西海域節分作戦二〇二二
						if (DateTime.Now < new DateTime(2022, 4, 1))
						{
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressSpecialBattle(q, 1, "S", new[] { 72 }, true, 2),
								new ProgressSpecialBattle(q, 1, "S", new[] { 73 }, true, 2),
								new ProgressSpecialBattle(q, 1, "A", new[] { 74 }, true),
							}));
						}
						break;
					case 843:   //|843|週|【節分拡張任務】令和三年節分作戦、全力出撃！|5-2・5-5・6-4ボスS勝利各1|要(戦艦系or空母系)旗艦/駆逐2, 期間限定(2021/01/13～????/??/??)
						if (DateTime.Now < new DateTime(2022, 4, 1))
						{
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressSpecialBattle(q, 1, "S", new[] { 25 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[] { 52 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[] { 55 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[] { 64 }, true),
							}));
						}
						break;

					case 234:   // 2102LQ1 バレンタイン限定任務 【一号作戦】
						if (DateTime.Now < new DateTime(2021, 5, 1))
						{
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressSpecialBattle(q, 1, "A", new[] { 14 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[] { 21 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[] { 22 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[] { 23 }, true),
							}));
						}
						break;
					case 238:   // 2102LQ2 バレンタイン限定任務 【二号作戦】
						if (DateTime.Now < new DateTime(2021, 5, 1))
						{
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressSpecialBattle(q, 1, "S", new[] { 13 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[] { 24 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[] { 31 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[] { 33 }, true),
								new ProgressSpecialBattle(q, 1, "S", new[] { 42 }, true),
							}));
						}
						break;

					case 906:   // 2103B1 【桃の節句】鎮守府近海、春の安全確保作戦
						if (DateTime.Now < new DateTime(2021, 6, 1))
						{
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressSpecialBattle(q, 1, "A", new[] { 12 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[] { 13 }, true),
								new ProgressSpecialBattle(q, 1, "A", new[] { 15 }, true),
								new ProgressSpecialBattle(q, 1, "X", new[] { 16 }, true),
							}));
						}
						break;
					case 907:   // 2103B2 【桃の節句】南西諸島海域、春の戦闘哨戒！
						if (DateTime.Now < new DateTime(2021, 6, 1))
						{
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressSpecialBattle(q, 2, "S", new[] { 21 }, true),
								new ProgressSpecialBattle(q, 2, "S", new[] { 22 }, true),
								new ProgressSpecialBattle(q, 2, "S", new[] { 23 }, true),
							}));
						}
						break;
					case 908:   // 2103B3 【桃の節句】春の決戦！敵機動部隊を叩け！
						if (DateTime.Now < new DateTime(2021, 6, 1))
						{
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressSpecialBattle(q, 2, "S", new[] { 24 }, true),
								new ProgressSpecialBattle(q, 2, "S", new[] { 25 }, true),
								new ProgressSpecialBattle(q, 2, "S", new[] { 72 }, true, 2),
							}));
						}
						break;
					case 909:   // 2103B4 【桃の節句：拡張作戦】春の攻勢作戦！
						if (DateTime.Now < new DateTime(2021, 6, 1))
						{
							Progresses.Add(new ProgressMultiBattle(q, new[] {
								new ProgressBattle(q, 1, "S", new[] { 35 }, true),
								new ProgressBattle(q, 1, "S", new[] { 45 }, true),
								new ProgressBattle(q, 1, "S", new[] { 64 }, true),
							}));
						}
						break;

					case 303: //C2 d 「演習」で練度向上！
						Progresses.Add(new ProgressPractice(q, 3, false));
						break;
					case 304: //C3 d 「演習」で他提督を圧倒せよ！
						Progresses.Add(new ProgressPractice(q, 5, true));
						break;

					case 302: //C4 w 大規模演習
						Progresses.Add(new ProgressPractice(q, 20, true));
						break;

					case 311: //C8 m 精鋭艦隊演習
						Progresses.Add(new ProgressPractice(q, 7, true));
						break;
					case 318: //C11 m 給糧艦「伊良湖」の支援
						Progresses.Add(new ProgressPractice(q, 3, true));
						break;

					case 330: //C29 q 空母機動部隊、演習始め！
						Progresses.Add(new ProgressPractice(q, 4, "B"));
						break;
					case 337: //C38 q 「十八駆」演習！
						Progresses.Add(new ProgressPractice(q, 3, "S"));
						break;
					case 339: //C42 q 「十九駆」演習！
						Progresses.Add(new ProgressPractice(q, 3, "S"));
						break;
					case 342: //C44 q 小艦艇群演習強化任務
						Progresses.Add(new ProgressPractice(q, 4, "A"));
						break;

					case 348: //C53 02 「精鋭軽巡」演習！
						Progresses.Add(new ProgressPractice(q, 4, "A"));
						break;

					case 350: //C55 03 精鋭「第七駆逐隊」演習開始！
						Progresses.Add(new ProgressPractice(q, 3, "A"));
						break;

					case 353: //C58 06 「巡洋艦戦隊」演習！
						Progresses.Add(new ProgressPractice(q, 5, "B"));
						break;

					case 354: //C60 07 「改装特務空母」任務部隊演習！
						Progresses.Add(new ProgressPractice(q, 4, "S"));
						break;

					case 345: //C49 09 演習ティータイム！
						Progresses.Add(new ProgressPractice(q, 4, "A"));
						break;
					case 346: //C50 09 	最精鋭！主力オブ主力、演習開始！
						Progresses.Add(new ProgressPractice(q, 4, "S"));
						break;
					case 355: //C62 09 精鋭「第十五駆逐隊」第一小隊演習！
						Progresses.Add(new ProgressPractice(q, 4, "S"));
						break;

					case 325: //C23 改夕雲型、演習始め！
						Progresses.Add(new ProgressPractice(q, 4, true));
						break;
					case 328: //C27 精強「十七駆」、猛特訓！
						Progresses.Add(new ProgressPractice(q, 4, true));
						break;
					case 331: //C31 艦載機演習
						Progresses.Add(new ProgressPractice(q, 3, "A"));
						break;
					case 336: //C37 輸送船団演習
						Progresses.Add(new ProgressPractice(q, 4, "A"));
						break;
					case 329: //2201LC01【節分任務】節分演習二〇二二
						Progresses.Add(new ProgressPractice(q, 3, "S"));
						break;
					case 341: //7thAnvLC1 七周年任務【七駆演習】
						Progresses.Add(new ProgressPractice(q, 3, "A"));
						break;
					case 349: //2102LQ3 バレンタイン限定任務 【特別演習】
						if (DateTime.Now < new DateTime(2021, 5, 1))
						{
							Progresses.Add(new ProgressPractice(q, 4, "S"));
						}
						break;

					case 402:   //D2 d「遠征」を3回成功させよう！
						Progresses.Add(new ProgressExpedition(q, 3, null));
						break;
					case 403:   //D3 d「遠征」を10回成功させよう！
						Progresses.Add(new ProgressExpedition(q, 10, null));
						break;

					case 404:   //D4 w 大規模遠征作戦、発令！|遠征成功30
						Progresses.Add(new ProgressExpedition(q, 30, null));
						break;
					case 410:   //D9 w 南方への輸送作戦を成功させよ！
						Progresses.Add(new ProgressExpedition(q, 1, new[] { 37, 38 }));
						break;
					case 411:   //D11 w 南方への鼠輸送を継続実施せよ！
						Progresses.Add(new ProgressExpedition(q, 6, new[] { 37, 38 }));
						Progresses[q.QuestID].SharedCounterShift = 1;
						break;

					case 424:   //D22 m 輸送船団護衛を強化せよ！
						Progresses.Add(new ProgressExpedition(q, 4, new[] { 5 }));
						Progresses[q.QuestID].SharedCounterShift = 1;
						break;

					case 425: //D23 q 海上護衛総隊、遠征開始！
						Progresses.Add(new ProgressMultiExpedition(q, new[]{
							new ProgressExpedition(q, 1, new[]{ 4 }),
							new ProgressExpedition(q, 1, new[]{ 5 }),
							new ProgressExpedition(q, 1, new[]{ 9 }),
						})); break;
					case 426: //D24 q 海上通商航路の警戒を厳とせよ！
						Progresses.Add(new ProgressMultiExpedition(q, new[]{
							new ProgressExpedition(q, 1, new[]{ 3 }),
							new ProgressExpedition(q, 1, new[]{ 4 }),
							new ProgressExpedition(q, 1, new[]{ 5 }),
							new ProgressExpedition(q, 1, new[]{ 10 }),
						}));
						break;
					case 428: //D26 q 近海に侵入する敵潜を制圧せよ！
						Progresses.Add(new ProgressMultiExpedition(q, new[]{
							new ProgressExpedition(q, 2, new[]{ 4 }),
							new ProgressExpedition(q, 2, new[]{ 101 }),
							new ProgressExpedition(q, 2, new[]{ 102 }),
						}));
						break;

					case 434:   //D32 02 特設護衛船団司令部、活動開始！
						Progresses.Add(new ProgressMultiExpedition(q, new[]{
							new ProgressExpedition(q, 1, new[]{ 3 }),
							new ProgressExpedition(q, 1, new[]{ 5 }),
							new ProgressExpedition(q, 1, new[]{ 100 }),
							new ProgressExpedition(q, 1, new[]{ 101 }),
							new ProgressExpedition(q, 1, new[]{ 9 }),
						}));
						break;
					case 442: //D38 02 西方連絡作戦準備を実施せよ！
						Progresses.Add(new ProgressMultiExpedition(q, new[]
						{
							new ProgressExpedition(q, 1, new[]{ 29 }),
							new ProgressExpedition(q, 1, new[]{ 30 }),
							new ProgressExpedition(q, 1, new[]{ 131 }),
							new ProgressExpedition(q, 1, new[]{ 133 }),
						}));
						break;

					case 436: //D33 03 練習航海及び警備任務を実施せよ！
						Progresses.Add(new ProgressMultiExpedition(q, new[]{
							new ProgressExpedition(q, 1, new[]{ 1 }),
							new ProgressExpedition(q, 1, new[]{ 2 }),
							new ProgressExpedition(q, 1, new[]{ 3 }),
							new ProgressExpedition(q, 1, new[]{ 4 }),
							new ProgressExpedition(q, 1, new[]{ 10 }),
						})); break;
					case 444: //D40 03 新兵装開発資材輸送を船団護衛せよ！
						Progresses.Add(new ProgressMultiExpedition(q, new[]
						{
							new ProgressExpedition(q, 1, new[]{ 5 }),
							new ProgressExpedition(q, 1, new[]{ 9 }),
							new ProgressExpedition(q, 1, new[]{ 11 }),
							new ProgressExpedition(q, 1, new[]{ 12 }),
							new ProgressExpedition(q, 1, new[]{ 110 }),
						}));
						break;

					case 437: //D35 05 小笠原沖哨戒線の強化を実施せよ！
						Progresses.Add(new ProgressMultiExpedition(q, new[]{
							new ProgressExpedition(q, 1, new[]{ 4 }),
							new ProgressExpedition(q, 1, new[]{ 104 }),
							new ProgressExpedition(q, 1, new[]{ 105 }),
							new ProgressExpedition(q, 1, new[]{ 110 }),
						})); break;

					case 438: //D35 08 南西諸島方面の海上護衛を強化せよ！
						Progresses.Add(new ProgressMultiExpedition(q, new[]{
							new ProgressExpedition(q, 1, new[]{ 4 }),
							new ProgressExpedition(q, 1, new[]{ 9 }),
							new ProgressExpedition(q, 1, new[]{ 100 }),
							new ProgressExpedition(q, 1, new[]{ 114 }),
						})); break;

					case 439: //D36 09 兵站強化遠征任務【基本作戦】
						Progresses.Add(new ProgressMultiExpedition(q, new[]{
							new ProgressExpedition(q, 1, new[]{ 5 }),
							new ProgressExpedition(q, 1, new[]{ 11 }),
							new ProgressExpedition(q, 1, new[]{ 100 }),
							new ProgressExpedition(q, 1, new[]{ 110 }),
						})); break;
					case 440: //D37 09 兵站強化遠征任務【拡張作戦】
						Progresses.Add(new ProgressMultiExpedition(q, new[]{
							new ProgressExpedition(q, 1, new[]{ 41 }),
							new ProgressExpedition(q, 1, new[]{ 5 }),
							new ProgressExpedition(q, 1, new[]{ 40 }),
							new ProgressExpedition(q, 1, new[]{ 142 }),
							new ProgressExpedition(q, 1, new[]{ 46 }),
						})); break;

					case 427: //D25 遠征「補給」支援体制を強化せよ！
						Progresses.Add(new ProgressExpedition(q, 1, new[] { 100 }));
						break;
					case 429: //D27 「捷一号作戦」、発動準備！
						Progresses.Add(new ProgressMultiExpedition(q, new[]{
							new ProgressExpedition(q, 1, new[]{ 3 }),
							new ProgressExpedition(q, 1, new[]{ 100 }),
							new ProgressExpedition(q, 1, new[]{ 110 }),
						})); break;
					case 430: //D28 「海防艦」、進発せよ！
						Progresses.Add(new ProgressMultiExpedition(q, new[]{
							new ProgressExpedition(q, 1, new[]{ 5 }),
							new ProgressExpedition(q, 1, new[]{ 9 }),
							new ProgressExpedition(q, 1, new[]{ 100 }),
							new ProgressExpedition(q, 1, new[]{ 101 }),
						})); break;
					case 431: //D29 艦隊司令部の強化 【準備段階】
						Progresses.Add(new ProgressMultiExpedition(q, new[]{
							new ProgressExpedition(q, 1, new[]{ 17 }),
							new ProgressExpedition(q, 1, new[]{ 100 }),
							new ProgressExpedition(q, 1, new[]{ 101 }),
							new ProgressExpedition(q, 1, new[]{ 110 }),
						})); break;
					case 432: // D30 警備及び哨戒偵察を強化せよ！
						Progresses.Add(new ProgressMultiExpedition(q, new[]
						{
							new ProgressExpedition(q, 1, new[]{ 10 }),
							new ProgressExpedition(q, 1, new[]{ 101 }),
							new ProgressExpedition(q, 1, new[]{ 110 }),
						}));
						break;
					case 433: //D31  南方戦線遠征を実施せよ！
						Progresses.Add(new ProgressMultiExpedition(q, new[]{
							new ProgressExpedition(q, 1, new[]{ 35 }),
							new ProgressExpedition(q, 1, new[]{ 36 }),
							new ProgressExpedition(q, 1, new[]{ 37 }),
							new ProgressExpedition(q, 1, new[]{ 38 }),
							new ProgressExpedition(q, 1, new[]{ 40 }),
						})); break;
					case 435: //20WiD1 【桃の節句任務】桃の節句遠征！
						Progresses.Add(new ProgressMultiExpedition(q, new[]{
							new ProgressExpedition(q, 1, new[]{ 4 }),
							new ProgressExpedition(q, 1, new[]{ 10 }),
							new ProgressExpedition(q, 1, new[]{ 11 }),
							new ProgressExpedition(q, 1, new[]{ 102 }),
							new ProgressExpedition(q, 1, new[]{ 110 }),
						})); break;

					case 503: //E3 艦隊大整備！
						Progresses.Add(new ProgressDocking(q, 5));
						break;
					case 504: //E4 艦隊酒保祭り！
						Progresses.Add(new ProgressSupply(q, 15));
						break;

					case 605: //F3 d 新装備「開発」指令
						Progresses.Add(new ProgressDevelopment(q, 1));
						break;
					case 606: //F4 d 新造艦「建造」指令
						Progresses.Add(new ProgressConstruction(q, 1));
						break;
					case 607: //F5 d 装備「開発」集中強化！
						Progresses.Add(new ProgressDevelopment(q, 3));
						Progresses[q.QuestID].SharedCounterShift = 1;
						break;
					case 608: //F6 d 艦娘「建造」艦隊強化！
						Progresses.Add(new ProgressConstruction(q, 3));
						Progresses[q.QuestID].SharedCounterShift = 1;
						break;
					case 609: //F7 d 軍縮条約対応！
						Progresses.Add(new ProgressDestruction(q, 2));
						break;
					case 619: //F18 d 装備の改修強化
						Progresses.Add(new ProgressImprovement(q, 1));
						break;
					case 673:   //|673|装備開発力の整備
						Progresses.Add(new ProgressDiscard(q, 4, true, new[] { 1 }));
						Progresses[q.QuestID].SharedCounterShift = 1;
						break;
					case 674:   //|674|工廠環境の整備
						Progresses.Add(new ProgressDiscard(q, 3, true, new[] { 21 }));
						Progresses[q.QuestID].SharedCounterShift = 2;
						break;

					case 613: //F12 w 資源の再利用
						Progresses.Add(new ProgressDiscard(q, 24, false, null));
						break;
					case 638: //F34 w 対空機銃量産
						Progresses.Add(new ProgressDiscard(q, 6, true, new[] { 21 }));
						break;
					case 676: //F68 w 装備開発力の集中整備
						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 3, true, new[]{ 2 }),
							new ProgressDiscard(q, 3, true, new[]{ 4 }),
							new ProgressDiscard(q, 1, true, new[]{ 30 }),
						}));
						break;
					case 677: //F69 w 継戦支援能力の整備|
						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 4, true, new[]{ 3 }),
							new ProgressDiscard(q, 2, true, new[]{ 10 }),
							new ProgressDiscard(q, 3, true, new[]{ 5 }),
						}));
						break;

					case 626: //F26 m 精鋭「艦戦」隊の新編成
						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 2, true, new[]{ 20 }, -1),
							new ProgressDiscard(q, 1, true, new[]{ 19 }, -1),
						}));
						break;
					case 628: //F25 m 機種転換
						Progresses.Add(new ProgressDiscard(q, 2, true, new[] { 21 }, -1));
						break;
					case 645: //F45 m 「洋上補給」物資の調達
						Progresses.Add(new ProgressDiscard(q, 1, true, new[] { 18 }));
						break;

					case 643: //F39 q 主力「陸攻」の調達
						Progresses.Add(new ProgressDiscard(q, 2, true, new[] { 20 }, -1));
						break;
					case 653: //F90 q 工廠稼働！次期作戦準備！
						Progresses.Add(new ProgressDiscard(q, 6, true, new[] { 4 }, -1));
						break;
					case 663: //F55 q 新型艤装の継続研究
						Progresses.Add(new ProgressDiscard(q, 10, true, new[] { 3 }));
						break;
					case 675: //F67 q 運用装備の統合整備
						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 6, true, new[]{ 6 }),
							new ProgressDiscard(q, 4, true, new[]{ 21 }),
						}));
						break;
					case 678: //F70 q 主力艦上戦闘機の更新
						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 3, true, new[]{ 19 }, -1),
							new ProgressDiscard(q, 5, true, new[]{ 20 }, -1),
						}));
						break;
					case 680: //F72 q 対空兵装の整備拡充
						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 4, true, new[]{ 21 }),
							new ProgressDiscard(q, 4, true, new[]{ 12, 13 }),
						}));
						break;
					case 686: //F77 q 戦時改修A型高角砲の量産
						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 4, true, new[]{ 3 }, -1),
							new ProgressDiscard(q, 1, true, new[]{ 121 }, -1),
						}));
						break;
					case 688: //F79 q 航空戦力の強化|
						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 3, true, new[]{ 6 }),
							new ProgressDiscard(q, 3, true, new[]{ 7 }),
							new ProgressDiscard(q, 3, true, new[]{ 8 }),
							new ProgressDiscard(q, 3, true, new[]{ 10 }),
						}));
						break;

					case 681: //F95 01 航空戦力の再編増強準備
						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 4, true, new[]{ 7 }),
							new ProgressDiscard(q, 4, true, new[]{ 8 }),
						}));
						break;

					case 1103: //F98 06 潜水艦強化兵装の量産
						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 3, true, new[]{ 125 }, -1),
						}));
						break;
					case 1104: //F99 06 潜水艦電子兵装の量産
						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 3, true, new[]{ 106 }, -1),
						}));
						break;

					case 1105: //F100 07 夏の格納庫整備＆航空基地整備
						Progresses.Add(new ProgressDiscard(q, 3, true, new[] { 47 }));
						break;

					case 657: //F92 09 新型兵装開発整備の強化
						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 6, true, new[]{ 1 }),
							new ProgressDiscard(q, 5, true, new[]{ 2 }),
							new ProgressDiscard(q, 4, true, new[]{ 5 }),
						}));
						break;
					case 1107: //F102 09 【鋼材輸出】基地航空兵力を増備せよ！
						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 2, true, new[]{ 6 }),
							new ProgressDiscard(q, 2, true, new[]{ 8 }),
						}));
						break;

					case 654: //F93 10 精鋭複葉機飛行隊の編成

						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 1, true, new[]{ 242 }, -1),
							new ProgressDiscard(q, 2, true, new[]{ 249 }, -1),
						}));
						break;

					case 655: //F94 11 工廠フル稼働！新兵装を開発せよ
						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 5, true, new[]{ 1 }),
							new ProgressDiscard(q, 5, true, new[]{ 2 }),
							new ProgressDiscard(q, 5, true, new[]{ 3 }),
							new ProgressDiscard(q, 5, true, new[]{ 8 }),
							new ProgressDiscard(q, 5, true, new[]{ 10 }),
						}));
						break;

					case 685: //F76 駆逐艦主砲兵装の戦時改修
						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 4, true, new[]{ 3 }, -1),
							new ProgressDiscard(q, 1, true, new[]{ 121 }, -1),
						}));
						break;
					case 690: //F81 基地航空隊戦力の拡充
						Progresses.Add(new ProgressMultiDiscard(q, new[]{
							new ProgressDiscard(q, 4, true, new[]{ 6 }),
							new ProgressDiscard(q, 4, true, new[]{ 7 }),
							new ProgressDiscard(q, 4, true, new[]{ 8 }),
						}));
						break;
					case 621: //F89 陸戦用装備の艦載運用研究
						Progresses.Add(new ProgressMultiDiscard(q, new[]
						{
							new ProgressDiscard(q, 2, true, new[]{ 49 }, -1),
							new ProgressDiscard(q, 2, true, new[]{ 75 }, -1),
							new ProgressDiscard(q, 1, true, new[]{ 51 }, -1),
						}));
						break;
					case 691: //F82 提督室のリフォーム
						Progresses.Add(new ProgressMultiDiscard(q, new[]
						{
							new ProgressDiscard(q, 4, true, new[]{ (int)EquipmentTypes.MainGunMedium }, 2),
							new ProgressDiscard(q, 4, true, new[]{ (int)EquipmentTypes.SecondaryGun }, 2),
							new ProgressDiscard(q, 4, true, new[]{ (int)EquipmentTypes.AAGun }, 2),
						}));
						break;
					case 692: //F83 水上艦艇装備工廠の整備
						Progresses.Add(new ProgressMultiDiscard(q, new[]
						{
							new ProgressDiscard(q, 5, true, new[]{ (int)EquipmentTypes.MainGunSmall }, 2),
							new ProgressDiscard(q, 5, true, new[]{ (int)EquipmentTypes.MainGunLarge, (int)EquipmentTypes.MainGunLarge }, 2),
							new ProgressDiscard(q, 5, true, new[]{ (int)EquipmentTypes.SeaplaneRecon }, 2),
						}));
						break;
					case 693: // F84 回転翼機の開発
						Progresses.Add(new ProgressMultiDiscard(q, new[]
						{
							new ProgressDiscard(q, 4, true, new[]{ (int)EquipmentTypes.SeaplaneRecon,(int)EquipmentTypes.SeaplaneBomber,(int)EquipmentTypes.SeaplaneFighter, }, 2),
							new ProgressDiscard(q, 3, true, new[]{ (int)EquipmentTypes.CarrierBasedFighter }, 2),
							new ProgressDiscard(q, 2, true, new[]{ (int)EquipmentTypes.CarrierBasedTorpedo }, 2),
						}));
						break;
					case 694: //F85 新型航空艤装の研究
						Progresses.Add(new ProgressMultiDiscard(q, new[]
						{
							new ProgressDiscard(q, 4, true, new[]{ 26 }, -1),
							new ProgressDiscard(q, 4, true, new[]{ 24 }, -1),
							new ProgressDiscard(q, 2, true, new[]{ 18 }, -1),
						}));
						break;
					case 695: //F86 「彗星」艦爆の新運用研究
						Progresses.Add(new ProgressMultiDiscard(q, new[]
						{
							new ProgressDiscard(q, 4, true, new[]{ 24 }, -1),
							new ProgressDiscard(q, 3, true, new[]{ 23 }, -1),
							new ProgressDiscard(q, 2, true, new[]{ 26 }, -1),
						}));
						break;
					case 702:   //|702|艦の「近代化改修」を実施せよ！|改修成功2
						Progresses.Add(new ProgressModernization(q, 2));
						break;
					case 703:   //|703|「近代化改修」を進め、戦備を整えよ！|改修成功15
						Progresses.Add(new ProgressModernization(q, 15));
						break;
				}

				#endregion

			}

			// 進捗度にずれがあった場合補正する
			var p = Progresses[q.QuestID];
			if (p != null)
				p.CheckProgress(q);

		}

		LastUpdateTime = DateTime.Now;
		OnProgressChanged();

	}


	void BattleFinished(string apiname, dynamic data)
	{

		var bm = KCDatabase.Instance.Battle;
		var battle = bm.SecondBattle ?? bm.FirstBattle;

		var hps = battle.ResultHPs;
		if (hps == null)
			return;


		#region Slaughter

		var slaughterList = Progresses.Values.OfType<ProgressSlaughter>();

		for (int i = 0; i < 6; i++)
		{
			if (hps[Battle.BattleIndex.Get(Battle.BattleSides.EnemyMain, i)] <= 0)
			{
				var ship = battle.Initial.EnemyMembersInstance[i];
				if (ship == null)
					continue;

				foreach (var p in slaughterList)
					p.Increment(ship.ShipType);
			}

			if (bm.IsEnemyCombined && hps[Battle.BattleIndex.Get(Battle.BattleSides.EnemyEscort, i)] <= 0)
			{
				var ship = battle.Initial.EnemyMembersEscortInstance[i];
				if (ship == null)
					continue;

				foreach (var p in slaughterList)
					p.Increment(ship.ShipType);
			}
		}

		#endregion


		#region Battle

		foreach (var p in Progresses.Values.OfType<ProgressBattle>())
		{
			p.Increment(bm.Result.Rank, bm.Compass.MapAreaID * 10 + bm.Compass.MapInfoID, bm.Compass.EventID == 5);
		}

		foreach (var p in Progresses.Values.OfType<ProgressMultiBattle>())
		{
			p.Increment(bm.Result.Rank, bm.Compass.MapAreaID * 10 + bm.Compass.MapInfoID, bm.Compass.EventID == 5);
		}

		#endregion


		var pago = Progresses.Values.OfType<ProgressAGo>().FirstOrDefault();
		if (pago != null)
			pago.IncrementBattle(bm.Result.Rank, bm.Compass.EventID == 5);


		OnProgressChanged();
	}

	void PracticeFinished(string apiname, dynamic data)
	{

		foreach (var p in Progresses.Values.OfType<ProgressPractice>())
		{
			p.Increment(data.api_win_rank);
		}

		OnProgressChanged();
	}

	void ExpeditionCompleted(string apiname, dynamic data)
	{

		if ((int)data.api_clear_result == 0)
			return;     //遠征失敗

		FleetData fleet = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.Members.Contains((int)data.api_ship_id[1]));

		int areaID = fleet.ExpeditionDestination;

		foreach (var p in Progresses.Values.OfType<ProgressExpedition>())
		{
			p.Increment(areaID);
		}
		foreach (var p in Progresses.Values.OfType<ProgressMultiExpedition>())
		{
			p.Increment(areaID);
		}

		OnProgressChanged();
	}


	void StartRepair(string apiname, dynamic data)
	{

		foreach (var p in Progresses.Values.OfType<ProgressDocking>())
		{
			p.Increment();
		}

		OnProgressChanged();
	}

	void Supplied(string apiname, dynamic data)
	{

		foreach (var p in Progresses.Values.OfType<ProgressSupply>())
		{
			p.Increment();
		}

		OnProgressChanged();
	}

	void EquipmentRemodeled(string apiname, dynamic data)
	{

		foreach (var p in Progresses.Values.OfType<ProgressImprovement>())
		{
			p.Increment();
		}

		OnProgressChanged();
	}



	void Modernized(string apiname, dynamic data)
	{

		if ((int)data.api_powerup_flag == 0) return;    //近代化改修失敗

		foreach (var p in Progresses.Values.OfType<ProgressModernization>())
		{
			p.Increment();
		}

		OnProgressChanged();
	}

	public void EquipmentDiscarded(string apiname, Dictionary<string, string> data)
	{

		var ids = data["api_slotitem_ids"].Split(",".ToCharArray()).Select(s => int.Parse(s));

		foreach (var p in Progresses.Values.OfType<ProgressDiscard>())
		{
			p.Increment(ids);
		}
		foreach (var p in Progresses.Values.OfType<ProgressMultiDiscard>())
		{
			p.Increment(ids);
		}

		OnProgressChanged();
	}

	void ShipDestructed(string apiname, dynamic data)
	{
		int amount = (data["api_ship_id"] as string).Split(",".ToCharArray()).Count();

		foreach (var p in Progresses.Values.OfType<ProgressDestruction>())
		{
			p.Increment(amount);
		}

		OnProgressChanged();
	}

	void ShipConstructed(string apiname, dynamic data)
	{
		foreach (var p in Progresses.Values.OfType<ProgressConstruction>())
		{
			p.Increment();
		}

		OnProgressChanged();
	}

	void EquipmentDeveloped(string apiname, dynamic data)
	{
		int trials = KCDatabase.Instance.Development.DevelopmentTrials;

		foreach (var p in Progresses.Values.OfType<ProgressDevelopment>())
		{
			for (int i = 0; i < trials; i++)
				p.Increment();
		}

		OnProgressChanged();
	}

	void StartSortie(string apiname, dynamic data)
	{
		foreach (var p in Progresses.Values.OfType<ProgressAGo>())
		{
			p.IncrementSortie();
		}

		OnProgressChanged();
	}

	private void NextSortie(string apiname, dynamic data)
	{
		var compass = KCDatabase.Instance.Battle.Compass;

		// 船団護衛成功イベント
		if (compass?.EventID == 8)
		{
			foreach (var p in Progresses.Values.OfType<ProgressBattle>())
			{
				p.Increment("x", compass.MapAreaID * 10 + compass.MapInfoID, compass.IsEndPoint);
			}

			foreach (var p in Progresses.Values.OfType<ProgressMultiBattle>())
			{
				p.Increment("x", compass.MapAreaID * 10 + compass.MapInfoID, compass.IsEndPoint);
			}

			OnProgressChanged();
		}
	}


	public void Clear()
	{
		Progresses.Clear();
		LastUpdateTime = DateTime.Now;
	}


	public QuestProgressManager Load()
	{
		return (QuestProgressManager)Load(DefaultFilePath);
	}

	public void Save()
	{
		Save(DefaultFilePath);
	}

	private void OnProgressChanged()
	{
		KCDatabase.Instance.Quest.OnQuestUpdated();
	}
}
