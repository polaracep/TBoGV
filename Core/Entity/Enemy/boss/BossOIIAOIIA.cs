using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TBoGV;
internal class BossOIIAOIIA : EnemyBoss
{
	static Texture2D Spritesheet = TextureManager.GetTexture("spritesheetOIIA");
	static SoundEffect SfxOIIA = SoundManager.GetSound("OIIAOIIA");
	protected double phaseChangeElapsed = 0;
	protected int chillDuration = 3000;
	protected int rageDuration = (int)SfxOIIA.Duration.TotalMilliseconds;

	float Scale;
	int frameWidth = 122;
	int frameHeight = 132;
	int frameCount = 27;
	int currentFrame = 0;
	double lastFrameChangeElapsed = 0;
	double frameSpeed = SfxOIIA.Duration.TotalMilliseconds / 27;

	protected bool Rage { get; set; }
	protected int rageCount = 0;

	private Vector2 direction;
	private Random random = new Random();

	protected new enum BossPhases : int
	{
		IDLE = 0,
		STANDING = 1,
	}
	protected BossPhases Phase { get; set; }

	public BossOIIAOIIA(Vector2 position)
	{
		Rage = false;
		Position = position;
		Hp = 60;
		MovementSpeed = 2;
		AttackDmg = 1;
		Scale = 100f / Math.Max(frameWidth, frameHeight);
		Size = new Vector2(frameWidth * Scale, frameHeight * Scale);
		XpValue = 50;
		phaseChangeElapsed = 0;
	}
	public BossOIIAOIIA() : this(Vector2.Zero) { }

	public override void Draw(SpriteBatch spriteBatch)
	{
		Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
		spriteBatch.Draw(Spritesheet, new Rectangle((int)Position.X, (int)Position.Y, (int)(Size.X), (int)(Size.Y)), sourceRect, Color.White);
	}

	public override List<Projectile> Attack()
	{
		return new List<Projectile>() { new ProjectileMelee(Position + Size / 2, Size * new Vector2(0.6f)) };
	}

	public override Texture2D GetSprite()
	{
		throw new NotImplementedException();
	}

	public override void Update(Vector2 playerPosition, double dt)
	{
		phaseChangeElapsed += dt;
		lastFrameChangeElapsed += dt;
		UpdatePhase(playerPosition);
		UpdateAnimation();
	}
	protected void UpdatePhase(Vector2 playerPosition)
	{
		if ((phaseChangeElapsed > rageDuration && Rage) ||
			(phaseChangeElapsed > chillDuration && !Rage))
		{
			Rage = !Rage;
			phaseChangeElapsed = 0;
			chillDuration = new Random().Next(100, 1500);

			if (Rage)
			{
				rageCount++; // Increment Rage phase count
				if (rageCount % 3 == 0)
				{
					rageDuration *= 3; // Every third Rage phase lasts 3x longer
					PlaySfxMultipleTimes(3); // Play the sound effect three times
				}
				else
				{
					rageDuration = (int)SfxOIIA.Duration.TotalMilliseconds; // Reset to normal
					SfxOIIA.Play();
				}

				PickNewDirection(playerPosition);
			}
		}
	}

	private async void PlaySfxMultipleTimes(int times)
	{
		for (int i = 0; i < times; i++)
		{
			SfxOIIA.Play();
			await Task.Delay((int)SfxOIIA.Duration.TotalMilliseconds);
		}
	}


	private void UpdateAnimation()
	{
		if (lastFrameChangeElapsed > frameSpeed)
		{
			currentFrame = (currentFrame + 1) % frameCount;
			lastFrameChangeElapsed = 0;
		}
		if (!Rage)
			currentFrame = 0;
	}

	private void PickNewDirection(Vector2 playerPosition)
	{
		float accuracy = 1f - Hp / 120f; // Accuracy increases as HP decreases
		Vector2 randomDirection = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1));
		Vector2 playerDirection = Vector2.Normalize(playerPosition - Position);
		direction = Vector2.Normalize(Vector2.Lerp(randomDirection, playerDirection, accuracy));
	}

	public override void Move(Place place)
	{
		if (!Rage)
			return;

		// Scale movement speed from 2 (max HP) to 8 (1 HP)
		MovementSpeed = 2 + (8 - 2) * (1 - (Hp / 60f));

		// Break movement into smaller increments
		float stepSize = 1.0f; // Move 1 pixel at a time to avoid skipping walls
		int steps = (int)Math.Ceiling(MovementSpeed / stepSize); // Total steps needed
		Vector2 stepDirection = Vector2.Normalize(direction) * stepSize; // Ensure direction is normalized

		for (int i = 0; i < steps; i++)
		{
			Vector2 nextPositionX = new Vector2(Position.X + stepDirection.X, Position.Y);
			Vector2 nextPositionY = new Vector2(Position.X, Position.Y + stepDirection.Y);

			bool collidesX = CollidesWithWall(nextPositionX, place);
			bool collidesY = CollidesWithWall(nextPositionY, place);

			if (collidesX)
			{
				direction.X *= -1; // Bounce on X-axis
			}
			if (collidesY)
			{
				direction.Y *= -1; // Bounce on Y-axis
			}
			if (collidesX || collidesY)
			{
				break; // Stop moving if a collision occurs
			}

			Position += stepDirection; // Apply the step movement
		}
	}
	private bool CollidesWithWall(Vector2 testPosition, Place place)
	{
		return place.ShouldCollideAt(new Rectangle((int)testPosition.X, (int)testPosition.Y, (int)Size.X, (int)Size.Y));
	}
	public override bool ReadyToAttack()
	{
		return Rage;
	}
	public override bool IsDead()
	{
		return Hp <= 0;
	}
	public override List<Item> Drop(int looting)
	{
		return new List<Item>() { new ItemTeeth(this.Position) };
	}
	public override float RecieveDmg(Projectile projectile)
	{
		if (!projectilesRecieved.Contains(projectile) && !Rage)
		{
			Hp -= projectile.Damage;
			projectilesRecieved.Add(projectile);
		}
		return Rage ? projectile.Damage : 0;
	}
}
