using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

/// <summary>
/// Klasicky encounter room. 
/// Dostacujici jsou nejake enemy entity.
/// </summary>
public class RoomClassroom : Room
{
    public RoomClassroom(Vector2 dimensions, Player p) : base(dimensions, p) { }
    public RoomClassroom(Vector2 dimensions, Vector2 pos, Player p) : base(dimensions, pos, p) { }
    public RoomClassroom(Vector2 dimensions, Vector2 pos, Player p, List<Entity> entityList) : base(dimensions, pos, p, entityList) { }
    public RoomClassroom(Vector2 dimensions, Player p, List<Entity> entityList) : base(dimensions, Vector2.Zero, p, entityList) { }

    public override void GenerateRoom()
    {
        GenerateRoomBase(FloorTypes.BASIC, WallTypes.WHITE, DoorTypes.BASIC);
        GenerateDecor();
        GenerateEnemies();
    }

    protected override void GenerateRoomBase(FloorTypes floors, WallTypes walls, DoorTypes doors)
    {

        this.ClearRoom();
        this.Floor = new Tile[(int)Dimensions.X, (int)Dimensions.Y];
        this.Decorations = new Tile[(int)Dimensions.X, (int)Dimensions.Y];

        Floor.GenerateFilledRectangleWRotation(
            new Rectangle(0, 0, (int)Dimensions.X, (int)Dimensions.Y),
            new TileFloor(FloorTypes.BASIC),
            new TileWall(WallTypes.WHITE),
            new TileWall(WallTypes.WHITE_CORNER)
        );

        GenerateDoors(doors);
        IsGenerated = true;
    }

    protected override void GenerateEnemies()
    {
        base.GenerateEnemies();
    }

    protected override void GenerateDecor()
    {
        // Generovat pary lavice + zidle
        int cntY = (int)Math.Ceiling((Dimensions.Y - 4) / 2);
        int cntX = (int)Math.Ceiling((Dimensions.X - 4) / 3) - 2;

        if (cntY % 2 == 1)
            cntY--;
        if (cntX % 2 == 1)
            cntX--;

        for (int y = 0; y < cntX; y++)
            for (int i = 0; i < cntY; i++)
            {
                Decorations[2 + 3 * y, 2 + 2 * i] = new TileDecoration(false, DecorationTypes.CHAIR);
                Decorations[3 + 3 * y, 2 + 2 * i] = new TileDecoration(true, DecorationTypes.DESK);
            }

        TileDecoration chair = new TileDecoration(false, DecorationTypes.CHAIR);
        // chair.FlipHorizontally();
        Decorations[(int)Dimensions.X - 4, 3] = new TileDecoration(true, DecorationTypes.KATEDRA);
        Decorations[(int)Dimensions.X - 3, 3] = chair;


    }
}