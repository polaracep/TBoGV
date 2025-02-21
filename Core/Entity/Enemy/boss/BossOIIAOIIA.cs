using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TBoGV;
internal class BossOIIAOIIA : EnemyBoss
{
	static Texture2D Spritesheet = TextureManager.GetTexture("spritesheetOIIA");
	static SoundEffect SfxOIIA = SoundManager.GetSound("OIIAOIIA");
	protected DateTime phaseChange = DateTime.UtcNow;
	protected int chillDuration = 5000;
	protected int rageDuration = (int)SfxOIIA.Duration.TotalMilliseconds;

	float Scale;
	int frameWidth = 122;
	int frameHeight = 132;
	int frameCount = 27;
	int currentFrame = 0;
	DateTime lastFrameChange = DateTime.UtcNow;
	TimeSpan frameSpeed = TimeSpan.FromMilliseconds((SfxOIIA.Duration.TotalMilliseconds / 27));


	protected bool Rage { get; set; }

	private double rotationOffset = 0;
	protected new enum bossPhases : int
	{
		IDLE = 0,
		KAVES = 1,
	}
	protected bossPhases Phase { get; set; }

	public BossOIIAOIIA(Vector2 position)
	{
		Rage = false;
		Position = position;
		Hp = 60;
		MovementSpeed = 2;
		AttackDmg = 1;
		Scale = 100f/ Math.Max(frameWidth,frameHeight);
		Size = new Vector2(frameWidth, frameHeight);
		XpValue = 50;
		phaseChange = DateTime.UtcNow;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
		spriteBatch.Draw(Spritesheet, new Rectangle((int)Position.X, (int)Position.Y, (int)(Size.X* Scale), (int)(Size.Y* Scale)), sourceRect, Color.White);
	}

	public override List<Projectile> Attack()
	{
		return new List<Projectile>();
	}

	public override Texture2D GetSprite()
	{
		throw new NotImplementedException();
	}

	public override void Update(Vector2 playerPosition)
	{
		UpdatePhase();
		UpdateAnimation();
	}

	protected void UpdatePhase()
	{
		if (((DateTime.UtcNow - phaseChange).TotalMilliseconds > rageDuration && Rage) || ((DateTime.UtcNow - phaseChange).TotalMilliseconds > chillDuration && !Rage))
		{
			Rage = !Rage;
			phaseChange = DateTime.UtcNow;
			chillDuration = new Random().Next(100, 1500);
			if (Rage)
				SfxOIIA.Play();
		}
	}

	private void UpdateAnimation()
	{
		if (DateTime.UtcNow - lastFrameChange > frameSpeed)
		{
			currentFrame = (currentFrame + 1) % frameCount;
			lastFrameChange = DateTime.UtcNow;
		}
		if (!Rage)
			currentFrame = 0;
	}

	public override bool ReadyToAttack()
	{
		return false;
	}

	public override bool IsDead()
	{
		return Hp <= 0;
	}

	public override List<Item> Drop(int looting)
	{
		return new List<Item>() { new ItemTeeth(Vector2.Zero) };
	}
}
