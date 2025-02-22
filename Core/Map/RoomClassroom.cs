using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
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

}