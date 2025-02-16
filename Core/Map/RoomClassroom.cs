using System;
using Microsoft.Xna.Framework;
using TBoGV;

public class RoomClassroom : Room
{
    public RoomClassroom(Vector2 dimensions, Player p) : base(dimensions, p) { }

    public RoomClassroom(Vector2 dimensions, Vector2 pos, Player p) : base(dimensions, pos, p) { }

    public override void GenerateRoom()
    {
        GenerateRoomBase(FloorTypes.BASIC, WallTypes.WHITE, DoorTypes.BASIC);
        player.Position = this.GetTileWorldPos(Vector2.One);
    }

    protected override void GenerateRoomBase(FloorTypes floors, WallTypes walls, DoorTypes doors)
    {
        if (this.Doors == null)
            throw new ArgumentNullException("This room does not have any doors!");

        this.ClearRoom();
        this.roomFloor = new Tile[(int)Dimensions.X, (int)Dimensions.Y];
        this.roomDecorations = new Tile[(int)Dimensions.X, (int)Dimensions.Y];

        for (int i = 0; i < Dimensions.X; i++)
            for (var j = 0; j < Dimensions.Y; j++)
                roomFloor[i, j] = new TileFloor(floors);

        for (int i = 0; i < Dimensions.X; i++)
        {
            roomFloor[i, 0] = new TileWall(walls);
            roomFloor[i, (int)Dimensions.Y - 1] = new TileWall(walls, MathHelper.Pi);
        }

        for (int i = 0; i < Dimensions.Y; i++)
        {
            roomFloor[0, i] = new TileWall(walls);
            roomFloor[(int)Dimensions.X - 1, i] = new TileWall(walls);
        }

        // Generace dveri
        foreach (TileDoor door in this.Doors)
        {
            door.SetDoorType(doors);
            switch (door.Direction)
            {
                case Directions.LEFT:
                    door.DoorTpPosition = new Vector2(1, (int)Dimensions.Y / 2);
                    roomDecorations[0, (int)Dimensions.Y / 2] = door;
                    break;
                case Directions.RIGHT:
                    door.DoorTpPosition = new Vector2((int)Dimensions.X - 2, (int)Dimensions.Y / 2);
                    roomDecorations[(int)Dimensions.X - 1, (int)Dimensions.Y / 2] = door;
                    break;
                case Directions.UP:
                    door.DoorTpPosition = new Vector2((int)Dimensions.X / 2, 1);
                    roomDecorations[(int)Dimensions.X / 2, 0] = door;
                    break;
                case Directions.DOWN:
                    door.DoorTpPosition = new Vector2((int)Dimensions.X / 2, (int)Dimensions.Y - 2);
                    roomDecorations[(int)Dimensions.X / 2, (int)Dimensions.Y - 1] = door;
                    break;
            }
        }
        IsGenerated = true;
    }

    protected override void GenerateEnemies()
    {
    }
}