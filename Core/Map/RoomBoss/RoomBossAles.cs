using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV;

public class RoomBossAles : RoomBoss
{
	protected static Vector2 dimensions = new Vector2(20);
	protected static List<Entity> entityList = new List<Entity>() { new BossAles() };
	public RoomBossAles(Player p) : base(dimensions, p, entityList) { }
}

