﻿using System.Collections.Generic;
using ElectronicObserver.Database.KancolleApi;

namespace ElectronicObserver.Database.Sortie;

public class SortieRecord
{
	public int Id { get; set; }
	public int World { get; set; }
	public int Map { get; set; }
	public List<ApiFile> ApiFiles { get; set; } = new();
}
