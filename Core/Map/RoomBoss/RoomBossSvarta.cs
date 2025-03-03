using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomBossSvarta : RoomBoss
{
	protected static Vector2 dimensions = new Vector2(10);
	public RoomBossSvarta(Player p) : base(dimensions, p) { }

	protected override EnemyBoss Boss { get; set; } = new BossSvarta();
}

