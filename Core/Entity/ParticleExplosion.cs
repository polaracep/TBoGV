using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TBoGV;

internal class ParticleExplosion : Particle
{
	static Texture2D Sprite;
	private float rotation;
	private float scale;
	private float alpha;
	private float rotationSpeed;
	private Vector2 velocity;

	// Time tracking for animation progress (0 to 1)
	private float elapsedTime;
	private Random rnd = new Random();
	public ParticleExplosion(Vector2 position)
	{
		Size = new Vector2(75, 75);
		Position = position - Size / 2;
		Sprite = TextureManager.GetTexture("boom");

		// Initial animation properties
		rotation = 0f;
		scale = 1f;
		alpha = 1f;
		velocity = new Vector2(
			(float)(rnd.Next(0, 100) - 50) * 2f, // random X velocity
			(float)(rnd.Next(0, 100) - 50) * 2f  // random Y velocity
		);

		rotationSpeed = MathHelper.ToRadians(180 * velocity.X / Math.Abs(velocity.X)); // 180 degrees per second rotation

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
		scale = Size.X / Sprite.Width + progress / 10; // explosion grows over time
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
