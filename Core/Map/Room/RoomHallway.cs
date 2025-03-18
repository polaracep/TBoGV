using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

public class RoomHallway : Room
{
    Texture2D SpriteIconExit = TextureManager.GetTexture("iconExit");
    Texture2D SpriteIconNotCleared = TextureManager.GetTexture("iconNotCleared");
    private List<(TileWall wall, TileWall corner)> colors = [
        (new TileWall(WallTypes.HALLWAY_GREEN), new TileWall(WallTypes.HALLWAY_GREEN_CORNER)),
        (new TileWall(WallTypes.HALLWAY_BLUE), new TileWall(WallTypes.HALLWAY_BLUE_CORNER)),
        (new TileWall(WallTypes.HALLWAY_ORANGE), new TileWall(WallTypes.HALLWAY_ORANGE_CORNER))
    ];
    private DecorationTypes[] infos = [
        DecorationTypes.INFO1,
        DecorationTypes.INFO2,
        DecorationTypes.INFO3,
        DecorationTypes.INFO4,
        DecorationTypes.PAINTING1,
        DecorationTypes.PAINTING2,
        DecorationTypes.PAINTING3,
        DecorationTypes.PAINTING4,
        DecorationTypes.PAINTING5,
        DecorationTypes.PAINTING6,
        DecorationTypes.PAINTING7,
        DecorationTypes.PAINTING8,
        DecorationTypes.PAINTING9,
        DecorationTypes.PAINTING10,
        DecorationTypes.PAINTING11,
    ];

    public RoomHallway(Vector2 dimensions, Player p) : base(dimensions, p) { }
    public RoomHallway(Player p) : base((7, 9, 17), p, null) { }
    public RoomHallway(Vector2 dimensions, Player p, List<Entity> entityList) : base(dimensions, p, entityList) { }

    protected override List<Enemy> validEnemies { get; set; } = [
        new EnemyJirka(),
        new EnemyCat()
    ];

    public override void Reset()
    {
        base.Reset();
        GenerateDoors(DoorTypes.BASIC);
    }
    public override void Generate()
    {
        GenerateBase();
        GenerateEnemies();
    }
    protected override void GenerateBase()
    {
        ClearRoom();

        (TileWall wall, TileWall corner) scheme;

        scheme = colors[Random.Shared.Next(colors.Count)];

        Floor.GenerateFilledRectangleWRotation(
            new Rectangle(0, 0, (int)Dimensions.X, (int)Dimensions.Y),
            new TileFloor(FloorTypes.LOBBY),
            scheme.wall,
            scheme.corner
        );
        GenerateDecor();

        GenerateDoors(DoorTypes.BASIC);


    }
    protected override void GenerateEnemies()
    {
        GenerateEnemies(Storyline.Difficulty);
    }

    protected override void GenerateDecor()
    {
        if (direction == Directions.LEFT || direction == Directions.RIGHT)
        {
            // bench every 3 tiles
            for (int x = 2; x < Dimensions.X - 1; x += 3)
            {
                AddDecoTile(new Vector2(x, 1), new TileDecoration(true, DecorationTypes.BENCH, MathHelper.PiOver2));
            }
            for (int x = 1; x < Dimensions.X - 2; x++)
                if (Random.Shared.Next(4) == 0)
                    AddDecoTile(new Vector2(x, Dimensions.Y - 1), new TileDecoration(true, infos[Random.Shared.Next(infos.Count())], MathHelper.PiOver2));


        }
        else if (direction == Directions.UP || direction == Directions.DOWN)
        {
            // bench every 3 tiles
            for (int y = 2; y < Dimensions.Y - 1; y += 3)
            {
                AddDecoTile(new Vector2(1, y), new TileDecoration(true, DecorationTypes.BENCH));
            }
            for (int y = 1; y < Dimensions.X - 2; y++)
                if (Random.Shared.Next(4) == 0)
                    AddDecoTile(new Vector2(Dimensions.X - 1, y), new TileDecoration(true, infos[Random.Shared.Next(infos.Count())]));
        }
    }

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