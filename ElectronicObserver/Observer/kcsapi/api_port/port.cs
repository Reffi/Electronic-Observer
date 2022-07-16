﻿using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.Data.DiscordRPC;

namespace ElectronicObserver.Observer.kcsapi.api_port;

public class port : APIBase
{


	public override void OnResponseReceived(dynamic data)
	{

		KCDatabase db = KCDatabase.Instance;

		db.Fleet.EvacuatePreviousShips();


		//api_material
		db.Material.LoadFromResponse(APIName, data.api_material);

		//api_basic
		db.Admiral.LoadFromResponse(APIName, data.api_basic);

		//api_ship
		db.Ships.Clear();
		foreach (var elem in data.api_ship)
		{

			var a = new ShipData();
			a.LoadFromResponse(APIName, elem);
			db.Ships.Add(a);

		}


		//api_ndock
		foreach (var elem in data.api_ndock)
		{

			int id = (int)elem.api_id;

			if (!db.Docks.ContainsKey(id))
			{
				var a = new DockData();
				a.LoadFromResponse(APIName, elem);
				db.Docks.Add(a);

			}
			else
			{
				db.Docks[id].LoadFromResponse(APIName, elem);
			}
		}

		//api_deck_port
		db.Fleet.LoadFromResponse(APIName, data.api_deck_port);
		db.Fleet.CombinedFlag = data.api_combined_flag() ? (int)data.api_combined_flag : 0;

		if (Utility.Configuration.Config.Control.EnableDiscordRPC)
		{
			DiscordRpcModel dataForWS = DiscordRpcManager.Instance.GetRPCData();
			dataForWS.TopDisplayText = Utility.Configuration.Config.Control.DiscordRPCMessage.Replace("{{secretary}}", db.Fleet[1].MembersInstance[0].Name);

			if (db.Fleet[1].CanAnchorageRepair)
			{
				dataForWS.TopDisplayText = string.Format(ObserverRes.RepairingShips, (db.Fleet[1].MembersInstance.Count(s => s != null) - 1).ToString());
			}

			dataForWS.BottomDisplayText = new List<string>();

			dataForWS.ImageKey = Utility.Configuration.Config.Control.UseFlagshipIconForRPC ? db.Fleet[1].MembersInstance[0].ShipID.ToString() : "kc_logo_512x512";
			dataForWS.CurrentShipId = db.Fleet[1].MembersInstance[0].ShipID;

			if (db.Admiral.Senka != null && db.Server?.Name != null)
			{
				dataForWS.BottomDisplayText.Add(string.Format(ObserverRes.ServerRank, db.Admiral.Senka, db.Server.Name));
			}

			if (!string.IsNullOrEmpty(dataForWS.MapInfo))
			{
				dataForWS.BottomDisplayText.Add(dataForWS.MapInfo);
			}

			if (Utility.Configuration.Config.Control.DiscordRPCShowFCM)
			{
				dataForWS.BottomDisplayText.Add(string.Format(ObserverRes.FirstClassMedals, db.Admiral.Medals));
			}


			dataForWS.LargeImageHoverText = string.Format(ObserverRes.AdmiralNameLevel, db.Admiral.AdmiralName, db.Admiral.Level);

			dataForWS.SmallIconHoverText = db.Admiral.RankString;
		}


		// 基地航空隊　配置転換系の処理
		if (data.api_plane_info() && data.api_plane_info.api_base_convert_slot())
		{

			var prev = db.RelocatedEquipments.Keys.ToArray();
			var current = (int[])data.api_plane_info.api_base_convert_slot;

			foreach (int deleted in prev.Except(current))
			{
				db.RelocatedEquipments.Remove(deleted);
			}

			foreach (int added in current.Except(prev))
			{
				db.RelocatedEquipments.Add(new RelocationData(added, DateTime.Now));
			}

		}
		else
		{

			db.RelocatedEquipments.Clear();
		}
		db.Replays.LoadFromResponse(APIName, data);
		db.Battle.LoadFromResponse(APIName, data);

		// --- Reset airbase strike points
		foreach (BaseAirCorpsData airCorpBase in db.BaseAirCorps.Values)
		{
			airCorpBase.LoadFromResponse(APIName, data);
		}

		// --- Debuff sound
		// todo when this gets checked compass is already null, adjust that if this info is ever needed
		/*if (data.api_event_object() && data.api_event_object.api_m_flag2() && (int)data.api_event_object.api_m_flag2 > 0)
		{
			MapRecordElement? record = RecordManager.Instance.Map.Record
				.Find(_record => _record.MapAreaId == db.Battle.Compass.MapInfo.MapAreaID && _record.MapId == db.Battle.Compass.MapInfo.MapID);

			if (record == null)
			{
				RecordManager.Instance.Map.Add(db.Battle.Compass.MapInfo.MapAreaID, db.Battle.Compass.MapInfo.MapID, 1);
			}
			else
			{
				record.DebuffCount += 1;
			}
		}*/

		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_port/port";
}
