using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TBoGV;

internal class ParticleExplosion : Particle, IDraw
{
	static Texture2D Sprite;
	public ParticleExplosion(Vector2 position)
	{
		Size = new Vector2(150, 150);
		Position = position - Size / 2;
		Sprite = TextureManager.GetTexture("coin");
		Visible = true;
		initTime = DateTime.UtcNow;
		DurationMilliseconds = 200;
	}
	public override void Draw(SpriteBatch spriteBatch)
	{
		//if (Visible) 
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), Color.White);
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}

}

