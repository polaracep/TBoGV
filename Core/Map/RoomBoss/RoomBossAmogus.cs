using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV;

public class RoomBossAmogus : RoomBoss
{
	protected static Vector2 dimensions = new Vector2(10);
	protected static List<Entity> entityList = new List<Entity>() { new BossAmogus() };
	public RoomBossAmogus(Player p) : base(dimensions, p, entityList) { }
}

