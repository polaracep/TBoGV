using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace TBoGV;

class ProjectileMelee : Projectile
{
	public static Texture2D Sprite { get; set; }
	public ProjectileMelee(Vector2 position, Vector2 size, float damage = 1)
	{
		Sprite = TextureManager.GetTexture("whiteSquare");
		Size = size;
		Position = position - Size / 2;
		Direction = Vector2.Zero;
		MovementSpeed = 0;
		Damage = damage;
	}
	public override void Draw(SpriteBatch spriteBatch)
	{
		//spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), Color.White);
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}
}
