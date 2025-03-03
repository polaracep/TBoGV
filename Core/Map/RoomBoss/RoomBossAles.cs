using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomBossAles : RoomBoss
{
	protected static Vector2 dimensions = new Vector2(20);
	protected static List<Entity> entityList = [];
	public RoomBossAles(Player p) : base(dimensions, p) { }

	protected override EnemyBoss Boss { get; set; } = new BossAles();
}

