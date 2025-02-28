using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TBoGV;

public class RoomBoss : Room
{
	public RoomBoss(Vector2 dimensions, Player p) : base(dimensions, p) { }
	public RoomBoss(Vector2 dimensions, Player p, List<Entity> entityList) : base(dimensions, p, entityList) { }

	public override void Generate()
	{
		GenerateBase();
		GenerateEnemies();
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

	protected override void GenerateDecor()
	{

	}
}

