using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomBossCat : RoomBoss
{
	protected static Vector2 dimensions = new Vector2(12);
	public RoomBossCat(Player p) : base(dimensions, p) { }

	protected override EnemyBoss Boss { get; set; } = new BossOIIAOIIA();
}

