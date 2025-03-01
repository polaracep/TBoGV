using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TBoGV;

public class RoomHallway : Room
{
    public RoomHallway(Vector2 dimensions, Player p) : base(dimensions, p) { }
    public RoomHallway(Player p, List<Entity> entityList) : base(p, entityList) { }
    public RoomHallway(Vector2 dimensions, Player p, List<Entity> entityList) : base(dimensions, p, entityList) { }

    protected override List<Enemy> validEnemies { get; set; } = new List<Enemy>
    {
        new EnemyZdena(),
    };

    public override void Generate()
    {
        GenerateBase();
        GenerateEnemies(25);
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