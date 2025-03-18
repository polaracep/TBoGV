using System;
using System.Collections.Generic;
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


    public RoomHallway(Vector2 dimensions, Player p) : base(dimensions, p) { }
    public RoomHallway(Player p) : base((7, 9, 17), p, null) { }
    public RoomHallway(Vector2 dimensions, Player p, List<Entity> entityList) : base(dimensions, p, entityList) { }

    protected override List<Enemy> validEnemies { get; set; } = [
        new EnemyJirka(),
        new EnemyCat()
    ];

    public override void Generate()
    {
        GenerateBase();
        GenerateEnemies();
        IsGenerated = true;
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

        GenerateDoors(DoorTypes.BASIC);

        GenerateDecor();

    }
    protected override void GenerateEnemies()
    {
        GenerateEnemies(Storyline.Difficulty);
    }

    protected override void GenerateDecor()
    {

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