﻿using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.ApiReqMap.Start.Response;

public class ApiHappening
{

	[JsonPropertyName("api_count")]
	public int ApiCount { get; set; }

	[JsonPropertyName("api_dentan")]
	public int ApiDentan { get; set; }

	[JsonPropertyName("api_icon_id")]
	public int ApiIconId { get; set; }

	[JsonPropertyName("api_mst_id")]
	public int ApiMstId { get; set; }

	[JsonPropertyName("api_type")]
	public int ApiType { get; set; }

	[JsonPropertyName("api_usemst")]
	public int ApiUsemst { get; set; }

}