using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomBossAmogus : RoomBoss
{
	protected static Vector2 dimensions = new Vector2(10);
	public RoomBossAmogus(Player p) : base(dimensions, p) { }

	protected override EnemyBoss Boss { get; set; } = new BossAmogus();
}

