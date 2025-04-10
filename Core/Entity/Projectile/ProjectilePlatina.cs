using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace TBoGV;

class ProjectilePlatina : Projectile
{
    public static Texture2D Sprite { get; protected set; }
    public ProjectilePlatina(Vector2 position, Vector2 direction, float damage)
    {
        Sprite = TextureManager.GetTexture("platina");
        Size = new Vector2(25, 25);
        Position = position - Size / 2;
        Direction = direction;
		MovementSpeed = Storyline.Player.IsEasyMode() ? 4 : 5;
		Damage = damage;

    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), Color.White);
    }
    public override Texture2D GetSprite()
    {
        return Sprite;
    }
}


