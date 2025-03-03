using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TBoGV;

public abstract class RoomBoss : Room
{
	public RoomBoss(Vector2 dimensions, Player p) : base(dimensions, p) { }

	// this won't be used, bosses have a different way of generating
	protected override List<Enemy> validEnemies { get; set; } = [];
	protected abstract EnemyBoss Boss { get; set; }

	public override void Generate()
	{
		GenerateBase();
		GenerateEnemies(0);
		IsGenerated = true;
	}

	protected override void GenerateBase()
	{
		this.ClearRoom();

		Floor.GenerateFilledRectangleWRotation(
			new Rectangle(0, 0, (int)Dimensions.X, (int)Dimensions.Y),
			new TileFloor(FloorTypes.LOBBY),
			new TileWall(WallTypes.LOBBY),
			new TileWall(WallTypes.LOBBY_CORNER)
		);

		GenerateDoors(DoorTypes.BASIC);

		GenerateDecor();

	}
	protected override void GenerateEnemies(int _)
	{
		if (Boss == null)
			throw new NullReferenceException("Provide a boss.");

		Random rand = new Random();
		while (true)
		{
			Vector2 spawnPos = new Vector2(rand.Next((int)Dimensions.X - 2) + 1, rand.Next((int)Dimensions.Y - 2) + 1) * 50;

			if (Doors.Any(d => (d.DoorTpPosition - spawnPos).Length() < 250))
				continue;

			if (!ShouldCollideAt(new Rectangle(spawnPos.ToPoint(), Boss.Size.ToPoint())))
			{
				Boss.Position = spawnPos;
				AddEnemy(Boss);
				break;
			}
		}
	}
}

