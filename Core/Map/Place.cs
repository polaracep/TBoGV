using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;


/// <summary>
/// I don't even know how to name these
/// </summary>
public abstract class Place : IDraw
{
    /// <summary>
    /// Use for room layout
    /// </summary>
    protected Tile[,] Floor;

    /// <summary>
    /// Use for interactable and changing tiles
    /// </summary>
    protected Tile[,] Decorations;

    /// <summary>
    /// List of living passive entities
    /// </summary>
    public List<EntityPassive> Entities = new List<EntityPassive>();

    /// <summary>
    /// List of living entities
    /// </summary>
    protected List<Enemy> Enemies = new List<Enemy>();

    /// <summary>
    /// Dimensions of the room
    /// </summary>
    public Vector2 Dimensions { get; protected set; }

    /// <summary>
    /// All dropped items (items on the ground)
    /// </summary>
    public List<Item> Drops = new List<Item>();

    /// <summary>
    /// All particles
    /// </summary>
    protected List<Particle> Particles = new List<Particle>();

    public bool IsGenerated { get; protected set; } = false;
    public Player player;

    public Vector2 GetTileWorldPos(Vector2 coords)
    {
        if (float.IsNaN(coords.X) || float.IsNaN(coords.Y))
            throw new ArgumentOutOfRangeException();
        if (coords.X >= Dimensions.X * Tile.GetSize().X || coords.Y >= Dimensions.Y * Tile.GetSize().Y || coords.X < 0 || coords.Y < 0)
            throw new ArgumentOutOfRangeException();
        return new Vector2((int)(coords.X * Tile.GetSize().X), (int)(coords.Y * Tile.GetSize().Y));
    }
    public Tile GetTileFloor(Vector2 coords)
    {
        return GetTile(coords).floor;
    }
    public Tile GetTileDecoration(Vector2 coords)
    {
        return GetTile(coords).decor;
    }
    public IInteractable GetTileInteractable(Vector2 coords)
    {
        (Tile, Tile) t = GetTile(coords);
        if (t.Item2 is IInteractable)
            return (IInteractable)t.Item2;
        else if (t.Item1 is IInteractable)
            return (IInteractable)t.Item1;

        return null;
    }
    public Item GetItemInteractable(Vector2 coords)
    {
        if (!IsGenerated)
            return null;
        foreach (var item in Drops)
        {
            if (ObjectCollision.RectCircleCollision(item.GetRectangle(), coords, 5))
                return item;
        }
        return null;
    }

    public IInteractable GetEntityInteractable(Vector2 coords)
    {
        if (!IsGenerated)
            return null;

        foreach (var entity in this.Entities)
        {
            if (entity is IInteractable && ObjectCollision.RectCircleCollision(entity.GetRectangle(), coords, 5))
            {
                return (IInteractable)entity;
            }
        }
        return null;
    }
    public (Tile floor, Tile decor) GetTile(Vector2 coords)
    {
        if (!IsGenerated)
            return (null, null);

        if (float.IsNaN(coords.X) || float.IsNaN(coords.Y))
            return (null, null);
        if (coords.X >= Dimensions.X * Tile.GetSize().X || coords.Y >= Dimensions.Y * Tile.GetSize().Y || coords.X < 0 || coords.Y < 0)
            return (null, null);

        return (Floor[(int)(coords.X / Tile.GetSize().X), (int)(coords.Y / Tile.GetSize().Y)],
                Decorations[(int)(coords.X / Tile.GetSize().X), (int)(coords.Y / Tile.GetSize().Y)]);
    }
    public bool ShouldCollideAt(Vector2 coords) { return ShouldCollideAt(coords, false); }
    public bool ShouldCollideAt(Vector2 coords, bool projectilesOnly)
    {
        if (projectilesOnly)
        {
            return this.GetTileFloor(coords)?.DoCollision ?? false;
        }
        else
        {
            return (this.GetTileFloor(coords)?.DoCollision ?? false) ||
                    (this.GetTileDecoration(coords)?.DoCollision ?? false);
        }
    }
    public bool ShouldCollideAt(Rectangle rect)
    {
        //experimental
        Vector2 tileSize = Tile.GetSize();

        // Calculate tile indices for the rectangle bounds
        int startX = Math.Max(0, rect.Left / (int)tileSize.X);
        int startY = Math.Max(0, rect.Top / (int)tileSize.Y);
        int endX = Math.Min((int)Dimensions.X - 1, rect.Right / (int)tileSize.X);
        int endY = Math.Min((int)Dimensions.Y - 1, rect.Bottom / (int)tileSize.Y);

        // Iterate through all tiles the rectangle covers
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                Vector2 tileCoords = new Vector2(x * tileSize.X, y * tileSize.Y);
                if (ShouldCollideAt(tileCoords))
                    return true; // If any tile collides, return true
            }
        }
        return false;
    }

    public bool AddFloorTile(Vector2 position, Tile tile)
    {
        if (Floor[(int)position.X, (int)position.Y] == null)
            return false;
        Floor[(int)position.X, (int)position.Y] = tile;
        return true;
    }
    public bool AddDecoTile(Vector2 position, Tile tile)
    {
        if (Decorations[(int)position.X, (int)position.Y] != null)
            return false;
        Decorations[(int)position.X, (int)position.Y] = tile;
        return true;
    }
    public abstract void Reset();
    public abstract void Generate();
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        if (!this.IsGenerated)
            this.Generate();
        for (int i = 0; i < Dimensions.X; i++)
            for (var j = 0; j < Dimensions.Y; j++)
            {
                Tile t = Floor[i, j];
                if (t != null)
                {
                    Vector2 origin = new Vector2(25, 25);
                    spriteBatch.Draw(t.Sprite, new Vector2(i * Tile.GetSize().X, j * Tile.GetSize().Y) + origin, null, Color.White, t.Rotation, origin, 1f, t.SpriteEffects, 0f);
                }
                t = Decorations[i, j];
                if (t != null)
                {
                    Vector2 origin = new Vector2(25, 25);
                    spriteBatch.Draw(t.Sprite, new Vector2(i * Tile.GetSize().X, j * Tile.GetSize().Y) + origin, null, Color.White, t.Rotation, origin, 1f, t.SpriteEffects, 0f);
                }
            }
    }
    public abstract void Update(GameTime gameTime);
}