using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomBossToilet : RoomBoss
{
	protected static Vector2 dimensions = new Vector2(10);
	protected static List<Entity> entityList = new List<Entity>() { new BossToilet() };
	public RoomBossToilet(Player p) : base(dimensions, p, entityList) { }
}

