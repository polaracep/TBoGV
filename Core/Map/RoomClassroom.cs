using System;
using System.Collections.Generic;
using System.Threading;
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

    public override void Generate()
    {
        GenerateBase(FloorTypes.BASIC, WallTypes.WHITE, DoorTypes.BASIC);
        GenerateDecor();
        GenerateEnemies();
    }

    protected override void GenerateBase(FloorTypes floors, WallTypes walls, DoorTypes doors)
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
        this.Floor[2, 2] = new TileFloor(FloorTypes.STAIRS);

        GenerateDoors(doors);
        IsGenerated = true;
    }

    protected override void GenerateDecor()
    {
        if (Random.Shared.Next(2) == 1)
        {
            // Horizontal layout: same as your updated GenerateDecor()
            int cntX = (int)Math.Floor((Dimensions.X - 6) / 3);
            int cntY = (int)Math.Floor((Dimensions.Y - 4) / 2);

            for (int y = 0; y < cntX; y++)
                for (int i = 0; i < cntY; i++)
                {
                    AddDecoTile(new Vector2(2 + 3 * y, 2 + 2 * i), new TileDecoration(false, DecorationTypes.CHAIR));
                    AddDecoTile(new Vector2(3 + 3 * y, 2 + 2 * i), new TileDecoration(true, DecorationTypes.DESK));
                }

            AddDecoTile(new Vector2(Dimensions.X - 4, 3), new TileDecoration(true, DecorationTypes.KATEDRA));
            AddDecoTile(new Vector2(Dimensions.X - 3, 3), new TileDecoration(false, DecorationTypes.CHAIR, SpriteEffects.FlipHorizontally));
            AddDecoTile(new Vector2(Dimensions.X - 1, Dimensions.Y / 2), new TileDecoration(false, DecorationTypes.BLACKBOARD));

        }
        else
        {
            // Vertical layout: swap roles so that the upper decoration is the desk and the lower is the chair.
            int cntY = (int)Math.Floor((Dimensions.Y - 6) / 3);
            int cntX = (int)Math.Floor((Dimensions.X - 4) / 2);

            for (int x = 0; x < cntX; x++)
                for (int y = 0; y < cntY; y++)
                {
                    // For vertical, the top tile (desk) now has collision and the bottom tile (chair) does not.
                    AddDecoTile(new Vector2(2 + 2 * x, 2 + 3 * y), new TileDecoration(false, DecorationTypes.CHAIR));
                    AddDecoTile(new Vector2(2 + 2 * x, 3 + 3 * y), new TileDecoration(true, DecorationTypes.DESK));
                }

            AddDecoTile(new Vector2(3, Dimensions.Y - 4), new TileDecoration(true, DecorationTypes.KATEDRA));
            AddDecoTile(new Vector2(3, Dimensions.Y - 3), new TileDecoration(false, DecorationTypes.CHAIR));
            AddDecoTile(new Vector2(Dimensions.X / 2, Dimensions.Y - 1), new TileDecoration(true, DecorationTypes.BLACKBOARD, MathHelper.PiOver2));

        }
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