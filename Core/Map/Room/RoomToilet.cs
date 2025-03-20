using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

class RoomToilet : Room
{
    public RoomToilet(Player p) : base(new Vector2(13, 8), p, null)
    {
    }

    protected override List<Enemy> validEnemies { get; set; } = [
        new EnemyLol(),
    ];

    public override void Generate()
    {
        GenerateBase();
        GenerateDecor();
        GenerateEnemies();
		GeneratePassive();
		IsGenerated = true;
    }

    protected override void GenerateBase(FloorTypes floors, WallTypes walls, DoorTypes doors)
    {
        ClearRoom();
        Floor = new Tile[(int)Dimensions.X, (int)Dimensions.Y];
        Decorations = new Tile[(int)Dimensions.X, (int)Dimensions.Y];

        Floor.GenerateFilledRectangleWRotation(
            new Rectangle(0, 0, (int)Dimensions.X, (int)Dimensions.Y),
            new TileFloor(FloorTypes.TOILET),
            new TileWall(WallTypes.TOILET),
            new TileWall(WallTypes.TOILET_CORNER)
        );

        // Prevent spawning
        for (int x = 0; x < 7; x++)
            for (int y = 0; y < Dimensions.Y; y++)
                AddDecoTile(new Vector2(x, y), new TileDecoration(false, DecorationTypes.EMPTY));

        // one possible spawn pos for left doors
        Decorations[1, (int)Dimensions.Y / 2] = null;

        // stalls
        GenStall(Vector2.One);
        GenStall(new Vector2(3, 1));
        GenStall(new Vector2(5, 1));
		
        // urinals
        AddDecoTile(new Vector2(1, 7), new TileDecoration(false, DecorationTypes.URINAL, MathHelper.Pi), true);
        AddDecoTile(new Vector2(3, 7), new TileDecoration(false, DecorationTypes.URINAL, MathHelper.Pi), true);
        AddDecoTile(new Vector2(5, 7), new TileDecoration(false, DecorationTypes.URINAL, MathHelper.Pi), true);

        // wall next to urinals
        AddFloorTile(new Vector2(6, 6), new TileWall(WallTypes.TOILET_DIVIDER_END_ROT));
        AddFloorTile(new Vector2(6, 7), new TileWall(WallTypes.TOILET_T, MathHelper.Pi, SpriteEffects.None));

        // sinks
        AddDecoTile(new Vector2(7, 6), new TileDecoration(false, DecorationTypes.SINK, -MathHelper.PiOver2));
        AddDecoTile(new Vector2(8, 6), new TileDecoration(false, DecorationTypes.SINK, -MathHelper.PiOver2));
        AddDecoTile(new Vector2(9, 6), new TileDecoration(false, DecorationTypes.SINK, -MathHelper.PiOver2));

        // mirrors
        AddDecoTile(new Vector2(7, 7), new TileDecoration(false, DecorationTypes.MIRROR, MathHelper.PiOver2));
        AddDecoTile(new Vector2(8, 7), new TileDecoration(false, DecorationTypes.MIRROR, MathHelper.PiOver2));
        AddDecoTile(new Vector2(9, 7), new TileDecoration(false, DecorationTypes.MIRROR, MathHelper.PiOver2));

        // wall next to sinks
        AddFloorTile(new Vector2(10, 5), new TileWall(WallTypes.TOILET_DIVIDER_END_ROT));
        AddFloorTile(new Vector2(10, 6), new TileWall(WallTypes.TOILET_DIVIDER));
        AddFloorTile(new Vector2(10, 7), new TileWall(WallTypes.TOILET_T, MathHelper.Pi, SpriteEffects.None));

        GenerateDoors(doors);
    }
	protected override void GeneratePassive()
	{
		if (Random.Shared.Next(6) != 0)
			return;

		List<Vector2> positions = [new Vector2(1,1), new Vector2(3,1), new Vector2(5,1)];
		for (int i = 0; i < positions.Count; i++)
			positions[i] = GetTileWorldPos(positions[i]);
		
		Entities.Add(new EntityPerloun(positions[Random.Shared.Next(positions.Count)]));
	}

	private void GenStall(Vector2 toiletPos)
    {
        AddDecoTile(toiletPos, new TileDecoration(false, DecorationTypes.TOILET), true);
        AddFloorTile(toiletPos + new Vector2(1, -1), new TileWall(WallTypes.TOILET_T));
        AddFloorTile(toiletPos + Vector2.UnitX, new TileWall(WallTypes.TOILET_DIVIDER));
        AddFloorTile(toiletPos + Vector2.One, new TileWall(WallTypes.TOILET_DIVIDER));
        AddFloorTile(toiletPos + new Vector2(1, 2), new TileWall(WallTypes.TOILET_DIVIDER_END));
    }

    protected override void GenerateEnemies()
    {
        GenerateEnemies((Storyline.Difficulty / 2) + 1);
    }
}