using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
namespace TBoGV;

class BossRichard : EnemyBoss
{

	protected Vector2 PrioDirection;

	protected int DirectionChangeTime = 4000;
	protected double DirectionChangeElapsed = 0;

	static Texture2D Spritesheet = TextureManager.GetTexture("richardSpritesheet");
	static SoundEffect SfxRickroll = SoundManager.GetSound("rickroll");
	static SoundEffectInstance SfxRickrollInstance = SfxRickroll.CreateInstance();
	protected double phaseChangeElapsed = 0;
	protected int chillDuration = 3000;
	protected int rageDuration = (int)SfxRickroll.Duration.TotalMilliseconds;

	float Scale;
	int frameWidth = 420;
	int frameHeight = 454;
	int frameCount = 28;
	int frameColumns = 5;
	int currentFrame = 0;
	double lastFrameChangeElapsed = 0;
	double frameSpeed = SfxRickroll.Duration.TotalMilliseconds / (27 * 4);

	public BossRichard(Vector2 position)
	{
		InitStats(0);
		Position = position;
		phaseChangeElapsed = 0;
		Scale = 100f / Math.Max(frameWidth, frameHeight);
		Size = new Vector2(frameWidth * Scale, frameHeight * Scale);
		PickNewDirection();
		Spawn();
	}
	public BossRichard() : this(Vector2.Zero) { }
	public override void Update(Vector2 playerPosition, double dt)
	{
		phaseChangeElapsed += dt;
		lastFrameChangeElapsed += dt;
		LastAttackElapsed += dt;
		DirectionChangeElapsed += dt;
		Direction = new Vector2(playerPosition.X, playerPosition.Y) - Position - Size / 2;
		Direction.Normalize();
		SfxRickrollInstance.Volume = Settings.SfxVolume;
		UpdatePhase(playerPosition);
		UpdateAnimation();
	}
	protected void UpdatePhase(Vector2 playerPosition)
	{

	}

	private void UpdateAnimation()
	{
		if (lastFrameChangeElapsed > frameSpeed)
		{
			currentFrame = (currentFrame + 1) % frameCount;
			lastFrameChangeElapsed = 0;
		}
	}
	public override bool ReadyToAttack()
	{
		return LastAttackElapsed > AttackSpeed;
	}
	public override bool IsDead()
	{
		return Hp <= 0;
	}
	public override List<Item> Drop(int looting)
	{
		return new List<Item>() { };
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		int row = currentFrame / frameColumns;
		int col = currentFrame % frameColumns;

		Rectangle sourceRect = new Rectangle(col * frameWidth, row * frameHeight, frameWidth, frameHeight);
		spriteBatch.Draw(Spritesheet, new Rectangle((int)Position.X, (int)Position.Y, (int)(Size.X), (int)(Size.Y)), sourceRect, Color.White);
	}
	public override Texture2D GetSprite()
	{
		return Spritesheet;
	}

	public override void Move(Place place)
	{
		if (LastAttackElapsed >= AttackSpeed + 0.4)
			return;

		// Break movement into smaller increments
		float stepSize = 1.0f; // Move 1 pixel at a time to avoid skipping walls
		int steps = (int)Math.Ceiling(MovementSpeed / stepSize); // Total steps needed
		Vector2 stepDirection = Vector2.Normalize(HeadedDirection) * stepSize; // Ensure direction is normalized

		for (int i = 0; i < steps; i++)
		{
			Vector2 nextPositionX = new Vector2(Position.X + stepDirection.X, Position.Y);
			Vector2 nextPositionY = new Vector2(Position.X, Position.Y + stepDirection.Y);

			bool collidesX = CollidesWithWall(nextPositionX, place);
			bool collidesY = CollidesWithWall(nextPositionY, place);

			if (collidesX)
			{
				HeadedDirection = new Vector2(-HeadedDirection.X, HeadedDirection.Y); // Bounce on X-axis
			}
			if (collidesY)
			{
				HeadedDirection = new Vector2(HeadedDirection.X, -HeadedDirection.Y);
			}
			if (collidesX || collidesY)
			{
				DirectionChangeElapsed = DirectionChangeTime;
				break; // Stop moving if a collision occurs
			}

			Position += stepDirection; // Apply the step movement
		}
	}
	private bool CollidesWithWall(Vector2 testPosition, Place place)
	{
		return place.ShouldCollideAt(new Vector2(testPosition.X + Size.X / 2, testPosition.Y + Size.Y / 2));
	}
	private void PickNewDirection()
	{
		if (DirectionChangeElapsed >= DirectionChangeTime)
		{
			PrioDirection = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1));
			DirectionChangeElapsed = 0;
		}

		Vector2 randomDirection = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1));
		HeadedDirection = Vector2.Normalize(Vector2.Lerp(randomDirection, PrioDirection, 0.7f));

	}
	public override List<Projectile> Attack()
	{
		PickNewDirection();
		List<Projectile> projectiles = base.Attack();
		projectiles.Add(new ProjectileNote(Position + Size / 2, Direction, AttackDmg));
		LastAttackElapsed = 0;
		return projectiles;
	}

	protected override void InitStats(int difficulty)
	{
		Hp = 60;
		MovementSpeed = 2;
		AttackDmg = 1;
		AttackSpeed = 300;
		XpValue = 50;
	}
}


