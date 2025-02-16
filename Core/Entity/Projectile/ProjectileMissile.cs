using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace TBoGV;

internal class ProjectileMissile : Projectile
{
	public Texture2D Sprite { get; protected set; }
	public ProjectileMissile(Vector2 position, Vector2 direction, float damage)
	{
		Sprite = TextureManager.GetTexture("projectile");
		// Size = new Vector2(7, 7);
		Size = new Vector2(25,25);
		Position = position - Size / 2;
		Direction = direction;
		MovementSpeed = 6;
		Damage = damage;

	}
	public void ChangeSprite(Texture2D sprite)
	{
		Sprite = sprite;
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

