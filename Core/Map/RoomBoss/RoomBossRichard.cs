using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomBossRichard : RoomBoss
{
	protected static Vector2 dimensions = new Vector2(10);
	protected static List<Entity> entityList = new List<Entity>() { new BossRichard() };
	public RoomBossRichard(Player p) : base(dimensions, p, entityList) { }
}

