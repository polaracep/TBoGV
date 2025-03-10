using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics.CodeAnalysis;


namespace TBoGV;
class EnemyCat : EnemyMelee
{
	static Texture2D Spritesheet = TextureManager.GetTexture("vibeCatSpritesheet");
	static SoundEffect vibeSfx = SoundManager.GetSound("vibe");
	static SoundEffectInstance vibeSfxInstance = vibeSfx.CreateInstance();
	static float Scale;

	int frameWidth = 112;
	int frameHeight = 112;
	int frameCount = 116;
	int currentFrame = 0;
	double lastFrameChangeElapsed = 0;
	double frameSpeed = vibeSfx.Duration.TotalMilliseconds / (116 * 1.5);

	public EnemyCat(Vector2 position)
	{
		InitStats(Storyline.Difficulty);
		movingDuration = (int)vibeSfx.Duration.TotalMilliseconds;
		chillDuration = 500;
		Position = position;
		Scale = 50f / Math.Max(frameWidth, frameHeight);
		Size = new Vector2(frameWidth * Scale, frameHeight * Scale);
		AttackDelay();
	}
	public EnemyCat() : this(Vector2.Zero) { }

	public override void Draw(SpriteBatch spriteBatch)
	{
		Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
		spriteBatch.Draw(Spritesheet, new Rectangle((int)Position.X, (int)Position.Y, (int)(Size.X), (int)(Size.Y)), sourceRect, Color.White);
	}
	public override Texture2D GetSprite()
	{
		return Spritesheet;
	}
	public override void Update(Vector2 playerPosition, double dt)
	{
		base.Update(playerPosition, dt);
		vibeSfxInstance.Volume = (float)(double)Settings.SfxVolume.Value;
		lastFrameChangeElapsed += dt;
		UpdateAnimation();
	}
	protected override void UpdateMoving(double dt)
	{
		if ((phaseChangeElapsed > movingDuration && Moving) ||
			(phaseChangeElapsed > chillDuration && !Moving))
		{
			Moving = !Moving;
			phaseChangeElapsed = 0;
			if (Moving)
				vibeSfxInstance.Play();

		}
	}
	private void UpdateAnimation()
	{
		if (lastFrameChangeElapsed > frameSpeed)
		{
			currentFrame = (currentFrame + 1) % frameCount;
			lastFrameChangeElapsed = 0;
		}
		if (!Moving)
			currentFrame = 0;
	}

	protected override void InitStats(int difficulty)
	{
		Hp = 3 + (int)(1.5 * difficulty);
		MovementSpeed = 1.5f + (float)(0.2 * difficulty);
		AttackSpeed = 444 + (5 * difficulty);
		AttackDmg = 1;
		XpValue = difficulty;
		Weight = EnemyWeight.EASY;
	}
}
