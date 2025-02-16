using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

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
}

