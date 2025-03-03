using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomBossMaturita : RoomBoss
{
	protected static Vector2 dimensions = new Vector2(11);
	protected override EnemyBoss Boss { get; set; } = new BossMaturita();

	public RoomBossMaturita(Player p) : base(dimensions, p) { }

	public override void Generate()
	{
		base.Generate();
		Drops.Add(new ItemVysvedceni(dimensions * 25));
		GenerateExit();
	}
}

