﻿using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ElectronicObserver.Database.MapData;
using ElectronicObserver.Window.Tools.EventLockPlanner;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ElectronicObserver.Database;

public class ElectronicObserverContext : DbContext
{
	public DbSet<EventLockPlannerModel> EventLockPlans { get; set; } = null!;
	public DbSet<WorldModel> Worlds { get; set; } = null!;
	public DbSet<MapModel> Maps { get; set; } = null!;
	public DbSet<CellModel> Cells { get; set; } = null!;

	private string DbPath { get; }

	public ElectronicObserverContext()
	{
		DbPath = Path.Join("Record", "ElectronicObserver.sqlite");
	}

	protected override void OnConfiguring(DbContextOptionsBuilder options)
		=> options.UseSqlite($"Data Source={DbPath}");

	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<EventLockPlannerModel>()
			.HasKey(s => s.Id);

		builder.Entity<EventLockPlannerModel>()
			.Property(s => s.Locks)
			.HasConversion(JsonConverter<List<EventLockModel>>());

		builder.Entity<EventLockPlannerModel>()
			.Property(s => s.ShipLocks)
			.HasConversion(JsonConverter<List<ShipLockModel>>());

		builder.Entity<EventLockPlannerModel>()
			.Property(s => s.Phases)
			.HasConversion(JsonConverter<List<EventPhaseModel>>());
		
		builder.Entity<WorldModel>()
			.HasKey(w => w.Id);

		builder.Entity<MapModel>()
			.HasKey(m => m.Id);

		builder.Entity<CellModel>()
			.HasKey(n => n.Id);
		
	}

	private static ValueConverter<T, string> JsonConverter<T>() where T : new() => new
	(
		list => JsonSerializer.Serialize(list, (JsonSerializerOptions?)null),
		s => FromJson<T>(s)
	);

	private static T FromJson<T>(string s) where T : new() => s switch
	{
		null or "" => new T(),
		_ => JsonSerializer.Deserialize<T>(s)!
	};
}
