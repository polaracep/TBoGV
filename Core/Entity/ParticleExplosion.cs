using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TBoGV;

internal class ParticleExplosion : Particle, IDraw
{
	static Texture2D Sprite;
	private float rotation;
	private float scale;
	private float alpha;
	private float rotationSpeed;
	private Vector2 velocity;

	// Time tracking for animation progress (0 to 1)
	private float elapsedTime;

	public ParticleExplosion(Vector2 position)
	{
		Random rnd = new Random();
		Size = new Vector2(150, 150);
		Position = position - Size / 2;
		Sprite = TextureManager.GetTexture("boom");

		// Initial animation properties
		rotation = 0f;
		scale = 1f;
		alpha = 1f;
		rotationSpeed = MathHelper.ToRadians(180); // 180 degrees per second rotation
		velocity = new Vector2(
			(float)(rnd.NextDouble() - 0.5) * 200f, // random X velocity
			(float)(rnd.NextDouble() - 0.5) * 200f  // random Y velocity
		);

		Visible = true;
		initTime = DateTime.UtcNow;
		DurationMilliseconds = 200; // total duration in ms
		elapsedTime = 0f;
	}

	public override void Update(GameTime gameTime)
	{
		float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
		elapsedTime += delta;

		// Calculate progress (0 to 1)
		float progress = MathHelper.Clamp((float)(DateTime.UtcNow - initTime).TotalMilliseconds / DurationMilliseconds, 0f, 1f);

		// Update rotation and scale over time
		rotation += rotationSpeed * delta;
		scale = 1f + progress/10; // explosion grows over time
		alpha = 1f - progress; // fades out

		// Update position (simulate slight dispersion)
		Position += velocity * delta;

		// Optionally, set Visible to false when finished
		if (progress >= 1f)
			Visible = false;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		if (!Visible)
			return;

		// Calculate origin for rotation (center of sprite)
		Vector2 origin = new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);

		// Draw the sprite with rotation, scaling, and fading (alpha)
		spriteBatch.Draw(
			Sprite,
			Position + Size / 2,  // center of explosion
			null,
			Color.White * alpha,
			rotation,
			origin,
			scale,
			SpriteEffects.None,
			0f);
	}

	public override Texture2D GetSprite()
	{
		return Sprite;
	}
}
