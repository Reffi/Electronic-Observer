﻿using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.DeckBuilder;

public class DeckBuilderFleet
{
	[JsonPropertyName("s1")] public DeckBuilderShip? Ship1 { get; set; }
	[JsonPropertyName("s2")] public DeckBuilderShip? Ship2 { get; set; }
	[JsonPropertyName("s3")] public DeckBuilderShip? Ship3 { get; set; }
	[JsonPropertyName("s4")] public DeckBuilderShip? Ship4 { get; set; }
	[JsonPropertyName("s5")] public DeckBuilderShip? Ship5 { get; set; }
	[JsonPropertyName("s6")] public DeckBuilderShip? Ship6 { get; set; }
	[JsonPropertyName("s7")] public DeckBuilderShip? Ship7 { get; set; }
}