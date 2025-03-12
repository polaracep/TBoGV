using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

public class RoomLocker : Room
{
    private static HashSet<int> usedLockerIds = new HashSet<int>();
    private static Random rand = new Random();
    public RoomLocker(Player p) : base((17, 29, 30), p) { }
    public RoomLocker(Vector2 dimensions, Player p) : base(dimensions, p) { }
    public RoomLocker(Player p, List<Entity> entityList) : base((17, 29, 30), p, entityList) { }
    public RoomLocker(Vector2 dimensions, Player p, List<Entity> entityList) : base(dimensions, p, entityList) { }

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
        var skolnik = new EntitySkolnik();
        while (true)
        {
            Vector2 spawnPos = new Vector2(
                Random.Shared.Next(50 * ((int)Dimensions.X - 3)) + 50,
                Random.Shared.Next(50 * ((int)Dimensions.Y - 3)) + 50);

            if (Doors.Any(d => ((d.DoorTpPosition * 50) - spawnPos).Length() < 100))
                continue;

            if (!ShouldCollideAt(new Rectangle(spawnPos.ToPoint(), skolnik.Size.ToPoint())))
            {
                skolnik.Position = spawnPos;
                Entities.Add(skolnik);
                break;
            }
        }
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
        int openLockerCount = rand.Next(1, 3); // 1 or 2 open lockers
        Dictionary<Vector2, TileLocker> lockerTiles = new Dictionary<Vector2, TileLocker>(); // Dictionary to store LockerTile objects by position

        if (direction == Directions.LEFT || direction == Directions.RIGHT)
        {
            // Vertical double-row lockers
            for (int x = 3; x < Dimensions.X - 3; x += 3) // Space every 3 tiles
            {
                for (int y = 2; y < Dimensions.Y - 2; y++)
                {
                    // First locker in the row
                    Vector2 lockerPos1 = new Vector2(x, y);
                    TileLocker locker1 = new TileLocker();
                    lockerTiles[lockerPos1] = locker1; // Add to the dictionary with position as key

                    // Second locker in the row (directly next to the first)
                    if (x + 1 < Dimensions.X - 1) // Prevent out-of-bounds
                    {
                        Vector2 lockerPos2 = new Vector2(x + 1, y);
                        TileLocker locker2 = new TileLocker(MathHelper.Pi, false); // Set rotation (adjust as needed)
                        lockerTiles[lockerPos2] = locker2; // Add to the dictionary with position as key
                    }
                }
            }
        }
        else if (direction == Directions.UP || direction == Directions.DOWN)
        {
            // Horizontal double-row lockers
            for (int y = 3; y < Dimensions.Y - 3; y += 3) // Space every 3 tiles
            {
                for (int x = 2; x < Dimensions.X - 2; x++)
                {
                    // First locker in the row
                    Vector2 lockerPos1 = new Vector2(x, y);
                    TileLocker locker1 = new TileLocker(MathHelper.PiOver2, false); // Set rotation (adjust as needed)
                    lockerTiles[lockerPos1] = locker1; // Add to the dictionary with position as key

                    // Second locker in the row (directly below the first)
                    if (y + 1 < Dimensions.Y - 1) // Prevent out-of-bounds
                    {
                        Vector2 lockerPos2 = new Vector2(x, y + 1);
                        TileLocker locker2 = new TileLocker(-MathHelper.PiOver2, false); // Set rotation (adjust as needed)
                        lockerTiles[lockerPos2] = locker2; // Add to the dictionary with position as key
                    }
                }
            }
        }

        // Randomly select open lockers from the dictionary
        List<Vector2> lockerKeys = new List<Vector2>(lockerTiles.Keys); // Get list of all locker positions (keys)
        for (int i = 0; i < openLockerCount; i++)
        {
            if (lockerKeys.Count > 0)
            {
                int randomIndex = rand.Next(lockerKeys.Count); // Randomly select an index
                Vector2 selectedPos = lockerKeys[randomIndex]; // Get the selected position
                lockerKeys.RemoveAt(randomIndex); // Remove the selected position from the list

                // Open the selected locker
                TileLocker selectedLocker = lockerTiles[selectedPos];
                selectedLocker.Open();
                selectedLocker.SetId(GetUniqueLockerId()); // Set unique ID
                this.AddDecoTile(selectedPos, selectedLocker); // Add the open locker to the decor
            }
        }

        // Add closed lockers for the rest of the positions
        foreach (Vector2 lockerPos in lockerKeys)
        {
            TileLocker closedLocker = lockerTiles[lockerPos];
            this.AddDecoTile(lockerPos, closedLocker); // Add closed lockers
        }
    }



    private int GetUniqueLockerId()
    {
        int id;
        do
        {
            id = rand.Next(1000, 9999); // Generate a random ID
        } while (usedLockerIds.Contains(id)); // Ensure it's unique

        usedLockerIds.Add(id);
        return id;
    }



    protected override void GenerateEnemies()
    {
        GenerateEnemies((Storyline.Difficulty / 2) + 1);
    }
    public override void Update(double dt)
    {
        base.Update(dt);
        foreach (var e in Entities)
        {
            if (e is EntitySkolnik skolnik)
            {
                skolnik.Move(this);
                skolnik.Update(dt);
            }
        }
    }
    protected static Texture2D SpriteIcon = TextureManager.GetTexture("lockerIcon");
    public override void DrawMinimapIcon(SpriteBatch spriteBatch, Vector2 position, float scale = 2, bool active = false)
    {
        base.DrawMinimapIcon(spriteBatch, position, scale, active);
        int width = (int)(IconBaseSize.X * scale);
        int height = (int)(IconBaseSize.Y * scale);

        spriteBatch.Draw(SpriteIcon, position + (new Vector2(width, height) - new Vector2(SpriteIcon.Width, SpriteIcon.Height)) / 2, Color.White);
    }
}