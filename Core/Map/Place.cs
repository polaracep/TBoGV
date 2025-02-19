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
    /// List of passive entities
    /// </summary>
    public List<Entity> Entities = new List<Entity>();

    /// <summary>
    /// Dimensions of the room
    /// </summary>
    public Vector2 Dimensions { get; protected set; }

    /// <summary>
    /// All dropped items -> items on the ground
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
    public Tile GetTileInteractable(Vector2 coords)
    {
        (Tile, Tile) t = GetTile(coords);
        if (t.Item2 is IInteractable)
            return t.Item2;
        else if (t.Item1 is IInteractable)
            return t.Item1;

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
    public bool ShouldCollideAt(Vector2 coords)
    {
        return (this.GetTileFloor(coords)?.DoCollision ?? false) ||
               (this.GetTileDecoration(coords)?.DoCollision ?? false);
    }

    public abstract void Reset();
    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void Update(GameTime gameTime);
}