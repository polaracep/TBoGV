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
		// Calculate rotation based on the direction vector
		float rotation = MathF.Atan2(Direction.Y, Direction.X);

		// Draw the sprite with rotation
		spriteBatch.Draw(
			Sprite,
			Position + Size / 2, // Draw position (centered)
			null,                 // Source rectangle (null means entire texture)
			Color.White,          // Color
			rotation,             // Rotation angle
			Size / 2,             // Origin (center of sprite)
			1.0f,                 // Scale
			SpriteEffects.None,   // No flipping
			0f                    // Layer depth
		);
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}
}

