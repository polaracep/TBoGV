using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace TBoGV;

class Coin : Item
{
	static Texture2D Sprite;
	public Coin(Vector2 position)
	{
		Sprite = TextureManager.GetTexture("coin");
		Size = new Vector2(15, 15);
		Position = position - Size / 2;
		if (position != Vector2.Zero)
			InitMovement();
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
