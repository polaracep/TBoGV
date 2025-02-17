using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TBoGV;

public class RoomClassroom : Room
{
    public RoomClassroom(Vector2 dimensions, Player p) : base(dimensions, p) { }
    public RoomClassroom(Vector2 dimensions, Vector2 pos, Player p) : base(dimensions, pos, p) { }
    public RoomClassroom(Vector2 dimensions, Vector2 pos, Player p, List<Enemy> enemyList) : base(dimensions, pos, p, enemyList) { }
    public RoomClassroom(Vector2 dimensions, Player p, List<Enemy> enemyList) : base(dimensions, Vector2.Zero, p, enemyList) { }

    public override void GenerateRoom()
    {
        GenerateRoomBase(FloorTypes.BASIC, WallTypes.WHITE, DoorTypes.BASIC);
        GenerateEnemies();
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
            roomFloor[i, 0] = new TileWall(WallTypes.WHITE);
            roomFloor[i, (int)Dimensions.Y - 1] = new TileWall(WallTypes.WHITE, MathHelper.Pi);
        }

        for (int i = 0; i < Dimensions.Y; i++)
        {
            roomFloor[0, i] = new TileWall(WallTypes.WHITE, -MathHelper.PiOver2);
            roomFloor[(int)Dimensions.X - 1, i] = new TileWall(WallTypes.WHITE, MathHelper.PiOver2);
        }
        roomFloor[0, 0] = new TileWall(WallTypes.WHITE_CORNER, -MathHelper.PiOver2);
        roomFloor[(int)Dimensions.X - 1, 0] = new TileWall(WallTypes.WHITE_CORNER, 0f);
        roomFloor[0, (int)Dimensions.Y - 1] = new TileWall(WallTypes.WHITE_CORNER, MathHelper.Pi);
        roomFloor[(int)Dimensions.X - 1, (int)Dimensions.Y - 1] = new TileWall(WallTypes.WHITE_CORNER, MathHelper.PiOver2);

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
        Random rand = new Random();
        foreach (var enemy in EnemyPool)
        {
            while (true)
            {
                Vector2 spawnPos = new Vector2(rand.Next((int)Dimensions.X), rand.Next((int)Dimensions.Y)) * 50;
                if (!this.ShouldCollideAt(spawnPos))
                {
                    enemy.Position = spawnPos;
                    this.AddEnemy(enemy);
                    break;
                }
            }
        }
    }
}