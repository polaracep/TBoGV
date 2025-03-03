﻿using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomBossRichard : RoomBoss
{
	protected static Vector2 dimensions = new Vector2(10);
	public RoomBossRichard(Player p) : base(dimensions, p) { }

	protected override EnemyBoss Boss { get; set; } = new BossRichard();
}

