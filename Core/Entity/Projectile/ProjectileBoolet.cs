using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace TBoGV;

class ProjectileBoolet : Projectile
{
	public static Texture2D Sprite { get; protected set; }
	public ProjectileBoolet(Vector2 position, Vector2 direction, float damage)
	{
		Sprite = TextureManager.GetTexture("boolet");
		Size = new Vector2(Sprite.Width, Sprite.Height);
		Position = position - Size / 2;
		Direction = direction;
		MovementSpeed = 5;
		Damage = damage;

	}
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, Position, null, Color.White, MathF.Atan2(Direction.Y, Direction.X) + MathHelper.Pi, Size / 2, Vector2.One, SpriteEffects.None, 0f);
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}

}
