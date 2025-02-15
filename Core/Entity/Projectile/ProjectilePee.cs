using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace TBoGV;

internal class ProjectilePee : Projectile
{
	public static Texture2D Sprite { get; protected set; }
	public ProjectilePee(Vector2 position, Vector2 direction, float damage)
	{
		Sprite = TextureManager.GetTexture("projectile");
		// Size = new Vector2(7, 7);
		Size = new Vector2(Sprite.Width, Sprite.Height);
		Position = position - Size / 2;
		Direction = direction;
		MovementSpeed = 5;
		Damage = damage;

	}
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), Color.White);
	}
}
