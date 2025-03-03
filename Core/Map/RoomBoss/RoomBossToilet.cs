using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomBossToilet : RoomBoss
{
	protected static Vector2 dimensions = new Vector2(10);
	public RoomBossToilet(Player p) : base(dimensions, p) { }

	protected override EnemyBoss Boss { get; set; } = new BossToilet();
}

