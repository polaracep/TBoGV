using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TBoGV;

public abstract class EntityPassive : Entity
{
    protected Texture2D Sprite;
    protected float Scale;
    public string Name;
    public virtual Dialogue Dialogue { get; set; } = null;
    public EntityPassive(Vector2 position, string name)
    {
        Name = name;
        Position = position;
        Sprite = GetSprite();
        Scale = 50f / Math.Max(Sprite.Width, Sprite.Height);
        Size = new Vector2(Sprite.Width * Scale, Sprite.Height * Scale);
    }
    public EntityPassive(string name) : this(Vector2.Zero, name) { }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(GetSprite(), new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), Color.White);
    }

    public override abstract Texture2D GetSprite();
}