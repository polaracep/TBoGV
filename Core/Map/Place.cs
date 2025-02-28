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

    /// <summary>
    /// Returns the left-top world position for any tile position
    /// </summary>
    /// <param name="coords"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Vector2 GetTileWorldPos(Vector2 wCoords)
    {
        if (float.IsNaN(wCoords.X) || float.IsNaN(wCoords.Y))
            throw new ArgumentOutOfRangeException();
        if (wCoords.X >= Dimensions.X * Tile.GetSize().X || wCoords.Y >= Dimensions.Y * Tile.GetSize().Y || wCoords.X < 0 || wCoords.Y < 0)
            throw new ArgumentOutOfRangeException();
        return new Vector2((int)(wCoords.X * Tile.GetSize().X), (int)(wCoords.Y * Tile.GetSize().Y));
    }
    public Tile GetTileFloor(Vector2 wCoords)
    {
        return GetTile(wCoords).floor;
    }
    public Tile GetTileDecoration(Vector2 wCoords)
    {
        return GetTile(wCoords).decor;
    }
    public IInteractable GetTileInteractable(Vector2 wCoords)
    {
        (Tile, Tile) t = GetTile(wCoords);
        if (t.Item2 is IInteractable)
            return (IInteractable)t.Item2;
        else if (t.Item1 is IInteractable)
            return (IInteractable)t.Item1;

        return null;
    }
    public Item GetItemInteractable(Vector2 wCoords)
    {
        if (!IsGenerated)
            return null;
        foreach (var item in Drops)
        {
            if (ObjectCollision.RectCircleCollision(item.GetRectangle(), wCoords, 5))
                return item;
        }
        return null;
    }

    public IInteractable GetEntityInteractable(Vector2 wCoords)
    {
        if (!IsGenerated)
            return null;

        foreach (var entity in this.Entities)
        {
            if (entity is IInteractable && ObjectCollision.RectCircleCollision(entity.GetRectangle(), wCoords, 5))
            {
                return (IInteractable)entity;
            }
        }
        return null;
    }
    public (Tile floor, Tile decor) GetTile(Vector2 worldCoords)
    {
        if (!IsGenerated)
            return (null, null);

        if (float.IsNaN(worldCoords.X) || float.IsNaN(worldCoords.Y))
            return (null, null);
        if (worldCoords.X >= Dimensions.X * Tile.GetSize().X || worldCoords.Y >= Dimensions.Y * Tile.GetSize().Y || worldCoords.X < 0 || worldCoords.Y < 0)
            return (null, null);

        return (Floor[(int)(worldCoords.X / Tile.GetSize().X), (int)(worldCoords.Y / Tile.GetSize().Y)],
                Decorations[(int)(worldCoords.X / Tile.GetSize().X), (int)(worldCoords.Y / Tile.GetSize().Y)]);
    }
    public bool ShouldCollideAt(Vector2 wCoords) { return ShouldCollideAt(wCoords, false); }
    public bool ShouldCollideAt(Vector2 wCoords, bool projectilesOnly)
    {
        (Tile, Tile) t = GetTile(wCoords);
        if (projectilesOnly)
            return t.Item2?.DoCollision ?? false;
        else
            return (t.Item1?.DoCollision ?? false) || (t.Item2?.DoCollision ?? false);
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

    public abstract void Update(double dt);

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
}