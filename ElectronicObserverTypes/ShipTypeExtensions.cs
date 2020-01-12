using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserverTypes
{
	public static class ShipTypeExtensions
	{
		public static IEnumerable<ShipType> ToTypes(this ShipTypeGroup group) => group switch
		{
			ShipTypeGroup.Battleships => new[]
			{
				ShipType.Battleship,
				ShipType.AviationBattleship,
				ShipType.Battlecruiser
			},

			ShipTypeGroup.Carriers => new[]
			{
				ShipType.AircraftCarrier,
				ShipType.ArmoredAircraftCarrier,
				ShipType.LightAircraftCarrier
			},

			ShipTypeGroup.HeavyCruisers => new[]
			{
				ShipType.HeavyCruiser,
				ShipType.AviationCruiser
			},

			ShipTypeGroup.LightCruisers => new[]
			{
				ShipType.LightCruiser,
				ShipType.TrainingCruiser,
				ShipType.TorpedoCruiser
			},

			ShipTypeGroup.Destroyers => new[]
			{
				ShipType.Destroyer
			},

			ShipTypeGroup.Escorts => new[]
			{
				ShipType.Escort
			},

			ShipTypeGroup.Submarines => new[]
			{
				ShipType.Submarine, 
				ShipType.SubmarineAircraftCarrier
			},

			ShipTypeGroup.Auxiliaries => new[]
			{
				ShipType.SeaplaneTender,
				ShipType.FleetOiler,
				ShipType.RepairShip,
				ShipType.AmphibiousAssaultShip,
				ShipType.SubmarineTender
			},

			_ => Enumerable.Empty<ShipType>()
		};

		public static ShipTypeGroup ToGroup(this ShipType type) => type switch
		{
			ShipType.Battleship => ShipTypeGroup.Battleships,
			ShipType.AviationBattleship => ShipTypeGroup.Battleships,
			ShipType.Battlecruiser => ShipTypeGroup.Battleships,

			ShipType.AircraftCarrier => ShipTypeGroup.Carriers,
			ShipType.ArmoredAircraftCarrier => ShipTypeGroup.Carriers,
			ShipType.LightAircraftCarrier => ShipTypeGroup.Carriers,

			ShipType.HeavyCruiser => ShipTypeGroup.HeavyCruisers,
			ShipType.AviationCruiser => ShipTypeGroup.HeavyCruisers,

			ShipType.LightCruiser => ShipTypeGroup.LightCruisers,
			ShipType.TrainingCruiser => ShipTypeGroup.LightCruisers,
			ShipType.TorpedoCruiser => ShipTypeGroup.LightCruisers,

			ShipType.Destroyer => ShipTypeGroup.Destroyers,

			ShipType.Escort => ShipTypeGroup.Escorts,

			ShipType.Submarine => ShipTypeGroup.Submarines,
			ShipType.SubmarineAircraftCarrier => ShipTypeGroup.Submarines,

			ShipType.SeaplaneTender => ShipTypeGroup.Auxiliaries,
			ShipType.FleetOiler => ShipTypeGroup.Auxiliaries,
			ShipType.RepairShip => ShipTypeGroup.Auxiliaries,
			ShipType.AmphibiousAssaultShip => ShipTypeGroup.Auxiliaries,
			ShipType.SubmarineTender => ShipTypeGroup.Auxiliaries,

			ShipType.SuperDreadnoughts => throw new NotImplementedException(),
			ShipType.Transport => throw new NotImplementedException(),
			ShipType.Unknown => throw new NotImplementedException(),
			_ => throw new NotImplementedException()
		};
	}
}