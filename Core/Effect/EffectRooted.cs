using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TBoGV;

internal class EffectRooted : Effect
{
	protected static Texture2D Sprite = TextureManager.GetTexture("koren");
	public EffectRooted(int level)
	{
		Name = "Korenovy vezen";
		Description = "bouchani nikdo neslysel, \nOdskocit jsi si nemohl, takze te zahriva tepla moc";
		Positive = false;
		Stats = new Dictionary<StatTypes, int>() { };
		Effects = new List<EffectTypes> { EffectTypes.ROOTED};
		Level = 0;
		ChangeLevel(level);
		// Get original sprite dimensions
		float originalWidth = Sprite.Width;
		float originalHeight = Sprite.Height;

		// Calculate scaling factor
		scale= 45f / Math.Max(originalWidth, originalHeight);
	}
	public override void ChangeLevel(int delta)
	{
		Level += delta;
		UpdateSize();
	}

	public override Texture2D GetSprite()
	{
		return Sprite;
	}
	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);

		// Calculate the origin as the center of the sprite.
		Vector2 origin = new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);
		// Calculate draw position with offsets.
		Vector2 drawPosition = new Vector2(Border + Position.X + 25, Border / 2 + Position.Y + 25);
		spriteBatch.Draw(
			Sprite,                  // texture
			drawPosition,            // position
			null,                    // source rectangle (null uses the entire texture)
			Color.White,             // color
			0f,                      // rotation
			origin,                  // origin (center of the sprite)
			scale,                   // scale factor
			SpriteEffects.None,      // effects
			0f                       // layer depth
		);
	}

	public override void IconDraw(SpriteBatch spriteBatch)
	{
		base.IconDraw(spriteBatch);

		// Use the same drawing logic for the icon.
		Vector2 origin = new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);
		Vector2 drawPosition = new Vector2(Position.X + 25, Position.Y + 25);
		spriteBatch.Draw(
			Sprite,
			drawPosition,
			null,
			Color.White,
			0f,
			origin,
			scale,
			SpriteEffects.None,
			0f
		);
	}

}

