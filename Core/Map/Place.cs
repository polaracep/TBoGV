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
    protected List<Entity> Entities;

    /// <summary>
    /// Dimensions of the room
    /// </summary>
    public Vector2 Dimensions { get; protected set; } = new Vector2(13);

    /// <summary>
    /// All dropped items -> items on the ground
    /// </summary>
    protected List<Item> Drops;

    /// <summary>
    /// All particles
    /// </summary>
    protected List<Particle> Particles = new List<Particle>();

    public Player player;

    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void Update(GameTime gameTime);
}