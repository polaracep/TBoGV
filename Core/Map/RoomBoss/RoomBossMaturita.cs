using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomBossMaturita : RoomBoss
{
	protected static Vector2 dimensions = new Vector2(11);
	protected override List<Enemy> validEnemies { get; set; } = [];
	public RoomBossMaturita(Player p) : base(dimensions, p, null) { }

	public override void Generate()
	{
		base.Generate();
		Drops.Add(new ItemVysvedceni(dimensions * 25));
		GenerateExit();
	}
}

