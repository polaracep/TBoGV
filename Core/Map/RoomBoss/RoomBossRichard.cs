﻿using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomBossRichard : RoomBoss
{
	protected static Vector2 dimensions = new Vector2(15);
	public RoomBossRichard(Player p) : base(dimensions, p) { }

	protected override EnemyBoss Boss { get; set; } = new BossRichard();
    protected override void GenerateEnemies()
    {
        while (true)
        {
            Vector2 spawnPos = new Vector2((int)Dimensions.X / 2, (int)Dimensions.Y / 2) * 50 - Boss.Size / 2;

            Boss.Position = spawnPos;
            AddEnemy(Boss);
            break;
        }
    }
}

