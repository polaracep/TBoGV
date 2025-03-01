using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TBoGV;

public abstract class EntityPassive : Entity
{
    protected Texture2D Sprite;
    protected float Scale;
    public EntityPassive(Vector2 position)
    {
        this.Position = position;
        this.Sprite = GetSprite();
        this.Scale = 50f / Math.Max(Sprite.Width, Sprite.Height);
        this.Size = new Vector2(this.Sprite.Width * Scale, this.Sprite.Height * Scale);
    }
    public EntityPassive() : this(Vector2.Zero) { }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(this.GetSprite(), new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)this.Size.X, (int)this.Size.Y), Color.White);
    }

    public override abstract Texture2D GetSprite();
}