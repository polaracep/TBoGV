using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TBoGV;

class EffectDelej : Effect
{
	protected static Texture2D Sprite = TextureManager.GetTexture("jirkaEffect");
	public EffectDelej(int level)
	{
		Name = "Deleeej";
		Description = "Uz jsi tam mel byt 4 minuty ty magore";
		Positive = false;
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.MOVEMENT_SPEED, -8 } };
		Effects = new List<EffectTypes>();
		Level = 0;
		LevelCap = 4;
		ChangeLevel(level);
		// Get original sprite dimensions
		float originalWidth = Sprite.Width;
		float originalHeight = Sprite.Height;

		// Calculate scaling factor
		scale = 45f / Math.Max(originalWidth, originalHeight);
		effectTime = 1444;
	}
	public EffectDelej() : this(1) { }
	public override void ChangeLevel(int delta)
	{
		Level += delta;
		effectTime += delta * 400;
		EnsureLevelCap();
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

