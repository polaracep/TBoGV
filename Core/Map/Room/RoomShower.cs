using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

public class RoomShower : Room
{
    public RoomShower(Player p) : base((7, 9, 11), p) { }
    public RoomShower(Vector2 dimensions, Player p) : base(dimensions, p) { }
    public RoomShower(Player p, List<Entity> entityList) : base((7, 9, 11), p, entityList) { }
    public RoomShower(Vector2 dimensions, Player p, List<Entity> entityList) : base(dimensions, p, entityList) { }

    protected override List<Enemy> validEnemies { get; set; } = new List<Enemy>
    {
        new EnemyZdena(),
        new EnemyTriangle(),
    };

    public override void Generate()
    {
        GenerateBase();
        GenerateEnemies();
        GeneratePassive();
        IsGenerated = true;
    }
    protected override void GeneratePassive()
    {
        if (Random.Shared.Next(6) == 0)
            Entities.Add(new EntityPerloun(GetTileWorldPos(new Vector2(3, 3))));
    }

    protected override void GenerateBase()
    {
        ClearRoom();

        Floor.GenerateFilledRectangleWRotation(
            new Rectangle(0, 0, (int)Dimensions.X, (int)Dimensions.Y),
            new TileFloor(FloorTypes.SHOWER),
            new TileWall(WallTypes.SHOWER),
            new TileWall(WallTypes.SHOWER_CORNER)
        );
        GenerateDecor();
        GenerateDoors(DoorTypes.BASIC, false);
    }
    protected override void GenerateDoors(DoorTypes doors, bool _)
    {
        if (Doors == null)
            throw new ArgumentNullException("This room does not have any doors!");

        Random rand = new Random();
        int _x = 0, _y = 0;

        foreach (TileDoor door in Doors)
        {
            if (door.Sprite != TextureManager.GetTexture("doorBoss"))
                door.SetDoorType(doors);

            bool validPosition = false;

            while (!validPosition)
            {
                _x = rand.Next((int)Dimensions.X - 2) + 1;
                _y = rand.Next((int)Dimensions.Y - 2) + 1;

                // tp pos
                int tpX = _x, tpY = _y;

                switch (door.Direction)
                {
                    case Directions.LEFT: tpX = 1; break;
                    case Directions.RIGHT: tpX = (int)Dimensions.X - 2; break;
                    case Directions.DOWN: tpY = (int)Dimensions.Y - 2; break;
                    case Directions.UP: tpY = 1; break;
                }

                if (Decorations[tpX, tpY] == null)
                    validPosition = true;

            }

            // Place the door and assign a valid teleport position
            switch (door.Direction)
            {
                case Directions.LEFT:
                    door.DoorTpPosition = new Vector2(1, _y);
                    Decorations[0, _y] = door;
                    break;
                case Directions.RIGHT:
                    door.DoorTpPosition = new Vector2((int)Dimensions.X - 2, _y);
                    Decorations[(int)Dimensions.X - 1, _y] = door;
                    break;
                case Directions.UP:
                    door.DoorTpPosition = new Vector2(_x, 1);
                    Decorations[_x, 0] = door;
                    break;
                case Directions.DOWN:
                    door.DoorTpPosition = new Vector2(_x, (int)Dimensions.Y - 2);
                    Decorations[_x, (int)Dimensions.Y - 1] = door;
                    break;
            }
        }
    }

    protected override void GenerateDecor()
    {
        if (direction == Directions.LEFT || direction == Directions.RIGHT)
        {
            for (int y = 1; y < Dimensions.Y - 1; y += 5) // Step by 5 to space groups two tiles apart
            {
                for (int i = 0; i < 3 && y + i < Dimensions.Y - 1; i++) // Place 3 showers in a row
                {
                    Vector2 showerPos = new Vector2(1, y + i);
                    this.AddDecoTile(showerPos, new TileShower());
                }
            }

            for (int y = 1; y < Dimensions.Y - 1; y += 2) // Sinks one tile apart
            {
                Vector2 sinkPos = new Vector2(Dimensions.X - 2, y);
                this.AddDecoTile(sinkPos, new TileDecoration(false, DecorationTypes.SINK, MathHelper.Pi));
            }
        }
        else if (direction == Directions.UP || direction == Directions.DOWN)
        {
            for (int x = 1; x < Dimensions.X - 1; x += 5) // Step by 5 to space groups two tiles apart
            {
                for (int i = 0; i < 3 && x + i < Dimensions.X - 1; i++) // Place 3 showers in a row
                {
                    Vector2 showerPos = new Vector2(x + i, 1);
                    this.AddDecoTile(showerPos, new TileShower());
                }
            }

            for (int x = 1; x < Dimensions.X - 1; x += 2) // Sinks one tile apart
            {
                Vector2 sinkPos = new Vector2(x, Dimensions.Y - 2);
                this.AddDecoTile(sinkPos, new TileDecoration(false, DecorationTypes.SINK, -MathHelper.PiOver2));
            }
        }
    }

    protected override void GenerateEnemies()
    {
        GenerateEnemies((Storyline.Difficulty / 2) + 1);
    }
    protected static Texture2D SpriteIcon = TextureManager.GetTexture("showerIcon");
    public override void DrawMinimapIcon(SpriteBatch spriteBatch, Vector2 position, float scale = 2, bool active = false)
    {
        base.DrawMinimapIcon(spriteBatch, position, scale, active);
        int width = (int)(IconBaseSize.X * scale);
        int height = (int)(IconBaseSize.Y * scale);

        spriteBatch.Draw(SpriteIcon, position + (new Vector2(width, height) - new Vector2(SpriteIcon.Width, SpriteIcon.Height)) / 2, Color.White);
    }
}