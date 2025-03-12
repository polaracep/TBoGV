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

    protected override List<Enemy> validEnemies { get; set; } = [
        new EnemyTriangle(),
        new EnemyCat(),
        new EnemyPolhreich(),
        new EnemySoldier()
    ];

    public RoomClassroom(Player p, List<Entity> entityList) : base((11, 15, 19), p, entityList) { }
    public RoomClassroom(Vector2 dimensions, Player p, List<Entity> entityList) : base(dimensions, p, entityList) { }
    public RoomClassroom(Player p) : base((11, 15, 19), p, null) { }

    public override void Generate()
    {
        if (direction == null)
            direction = (Directions)Random.Shared.Next(4);

        GenerateBase(FloorTypes.BASIC, WallTypes.WHITE, DoorTypes.BASIC);
        GenerateDecor();
        GenerateEnemies();
    }

    protected override void GenerateBase(FloorTypes floors, WallTypes walls, DoorTypes doors)
    {
        ClearRoom();
        Floor = new Tile[(int)Dimensions.X, (int)Dimensions.Y];
        Decorations = new Tile[(int)Dimensions.X, (int)Dimensions.Y];

        Floor.GenerateFilledRectangleWRotation(
            new Rectangle(0, 0, (int)Dimensions.X, (int)Dimensions.Y),
            new TileFloor(FloorTypes.BASIC),
            new TileWall(WallTypes.WHITE),
            new TileWall(WallTypes.WHITE_CORNER)
        );
        //this.Floor[2, 2] = new TileFloor(FloorTypes.STAIRS);

        GenerateDoors(doors);
        IsGenerated = true;
    }

    protected override void GenerateDecor()
    {
        if (direction == Directions.LEFT || direction == Directions.RIGHT)
        {
            // Horizontal layout
            int cntX = (int)Math.Floor((Dimensions.X - 6) / 3);
            int cntY = (int)Math.Ceiling((Dimensions.Y - 4) / 2);

            for (int y = 0; y < cntX; y++)
                for (int i = 0; i < cntY; i++)
                {
                    AddDecoTile(new Vector2(2 + 3 * y, 2 + 2 * i), new TileDecoration(false, DecorationTypes.CHAIR_CLASSROOM));
                    AddDecoTile(new Vector2(3 + 3 * y, 2 + 2 * i), new TileDecoration(true, DecorationTypes.DESK));
                }

            AddDecoTile(new Vector2(Dimensions.X - 4, 3), new TileComputer(0f, SpriteEffects.FlipHorizontally));
            AddDecoTile(new Vector2(Dimensions.X - 3, 3), new TileDecoration(false, DecorationTypes.CHAIR_CLASSROOM, SpriteEffects.FlipHorizontally));
            AddDecoTile(new Vector2(Dimensions.X - 1, Dimensions.Y / 2), new TileDecoration(false, DecorationTypes.BLACKBOARD));

        }
        else if (direction == Directions.UP || direction == Directions.DOWN)
        {
            // Vertical layout
            int cntY = (int)Math.Floor((Dimensions.Y - 6) / 3);
            int cntX = (int)Math.Ceiling((Dimensions.X - 4) / 2);

            for (int x = 0; x < cntX; x++)
                for (int y = 0; y < cntY; y++)
                {
                    // For vertical, the top tile (desk) now has collision and the bottom tile (chair) does not.
                    AddDecoTile(new Vector2(2 + 2 * x, 2 + 3 * y), new TileDecoration(false, DecorationTypes.CHAIR_CLASSROOM, MathHelper.PiOver2));
                    AddDecoTile(new Vector2(2 + 2 * x, 3 + 3 * y), new TileDecoration(true, DecorationTypes.DESK, MathHelper.PiOver2));
                }

            AddDecoTile(new Vector2(3, Dimensions.Y - 4), new TileComputer(-MathHelper.PiOver2));
            AddDecoTile(new Vector2(3, Dimensions.Y - 3), new TileDecoration(false, DecorationTypes.CHAIR_CLASSROOM, -MathHelper.PiOver2));
            AddDecoTile(new Vector2(Dimensions.X / 2, Dimensions.Y - 1), new TileDecoration(true, DecorationTypes.BLACKBOARD, MathHelper.PiOver2));
        }
    }
    protected override void GenerateEnemies()
    {
        GenerateEnemies((int)(2 + (Storyline.Difficulty -1)*1.5));
    }
    Texture2D SpriteIconExit = TextureManager.GetTexture("iconExit");
    Texture2D SpriteIconNotCleared = TextureManager.GetTexture("iconNotCleared");
    public override void DrawMinimapIcon(SpriteBatch spriteBatch, Vector2 position, float scale = 20, bool active = false)
    {
        base.DrawMinimapIcon(spriteBatch, position, scale, active);

        int width = (int)(IconBaseSize.X * scale);
        int height = (int)(IconBaseSize.Y * scale);
        if (IsEndRoom)
        {
            spriteBatch.Draw(SpriteIconExit, position + (new Vector2(width, height) - new Vector2(SpriteIconExit.Width, SpriteIconExit.Height)) / 2, Color.White);
        }
        else if (!IsGenerated || Enemies.Count > 0)
        {
            spriteBatch.Draw(SpriteIconNotCleared, position + (new Vector2(width, height) - new Vector2(SpriteIconNotCleared.Width, SpriteIconNotCleared.Height)) / 2, Color.White);
        }
    }
}