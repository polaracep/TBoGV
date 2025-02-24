using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

public abstract class EntityPassive : Entity
{
    protected Texture2D Sprite;
    public EntityPassive(Vector2 position)
    {
        this.Position = position;
        this.Sprite = GetSprite();
        this.Size = new Vector2(this.Sprite.Width, this.Sprite.Height);
    }
    public EntityPassive() : this(Vector2.Zero) { }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(this.GetSprite(), this.Position, Color.White);
    }

    public override abstract Texture2D GetSprite();
}