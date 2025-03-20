using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomBossZeman : RoomBoss
{
	protected static Vector2 dimensions = new Vector2(18);
	public RoomBossZeman(Player p) : base(dimensions, p) { }

	protected override EnemyBoss Boss { get; set; } = new BossZeman();
}

