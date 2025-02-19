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
        this.Floor = new Tile[(int)Dimensions.X, (int)Dimensions.Y];
        this.Decorations = new Tile[(int)Dimensions.X, (int)Dimensions.Y];

        Floor.GenerateFilledRectangleWRotation(
            new Rectangle(0, 0, (int)Dimensions.X, (int)Dimensions.Y),
            new TileFloor(FloorTypes.BASIC),
            new TileWall(WallTypes.WHITE),
            new TileWall(WallTypes.WHITE_CORNER)
        );

        // Generace dveri
        foreach (TileDoor door in this.Doors)
        {
            door.SetDoorType(doors);
            switch (door.Direction)
            {
                case Directions.LEFT:
                    door.DoorTpPosition = new Vector2(1, (int)Dimensions.Y / 2);
                    Decorations[0, (int)Dimensions.Y / 2] = door;
                    break;
                case Directions.RIGHT:
                    door.DoorTpPosition = new Vector2((int)Dimensions.X - 2, (int)Dimensions.Y / 2);
                    Decorations[(int)Dimensions.X - 1, (int)Dimensions.Y / 2] = door;
                    break;
                case Directions.UP:
                    door.DoorTpPosition = new Vector2((int)Dimensions.X / 2, 1);
                    Decorations[(int)Dimensions.X / 2, 0] = door;
                    break;
                case Directions.DOWN:
                    door.DoorTpPosition = new Vector2((int)Dimensions.X / 2, (int)Dimensions.Y - 2);
                    Decorations[(int)Dimensions.X / 2, (int)Dimensions.Y - 1] = door;
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