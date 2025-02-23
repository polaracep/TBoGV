using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TBoGV;

public class RoomHallway : Room
{
    public RoomHallway(Vector2 dimensions, Player p) : base(dimensions, p) { }
    public RoomHallway(Vector2 dimensions, Vector2 pos, Player p) : base(dimensions, pos, p) { }
    public RoomHallway(Vector2 dimensions, Player p, List<Entity> entityList) : base(dimensions, p, entityList) { }
    public RoomHallway(Vector2 dimensions, Vector2 pos, Player p, List<Entity> entityList) : base(dimensions, pos, p, entityList) { }

    public override void GenerateRoom()
    {
        GenerateRoomBase();
        GenerateEnemies();
        IsGenerated = true;
    }

    protected override void GenerateRoomBase()
    {
        this.ClearRoom();

        Floor.GenerateFilledRectangleWRotation(
            new Rectangle(0, 0, (int)Dimensions.X, (int)Dimensions.Y),
            new TileFloor(FloorTypes.LOBBY),
            new TileWall(WallTypes.LOBBY),
            new TileWall(WallTypes.LOBBY_CORNER)
        );

        GenerateDecor();

        GenerateDoors(DoorTypes.BASIC);
    }

}