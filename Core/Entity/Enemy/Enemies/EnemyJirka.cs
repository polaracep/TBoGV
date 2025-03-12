using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
namespace TBoGV;

class EnemyJirka : EnemyRanged
{
	static Texture2D Sprite = TextureManager.GetTexture("jirka");
	static float Scale;
	protected Vector2 PrioDirection;

	protected DateTime DirectionChanged;
	protected int DirectionChangeTime = 4000;
	public EnemyJirka(Vector2 position)
	{
		InitStats(Storyline.Difficulty);
		Position = position;
		Scale = 50f / Math.Max(Sprite.Width, Sprite.Height);
		Size = new Vector2(Sprite.Width * Scale, Sprite.Height * Scale);
		PickNewDirection();
		AttackDelay();
	}
	public EnemyJirka() : this(Vector2.Zero) { }

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle((int)(Position.X), (int)(Position.Y), (int)(Size.X), (int)(Size.Y)), Color.White);
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
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
		if ((DateTime.UtcNow - DirectionChanged).TotalMilliseconds >= DirectionChangeTime)
		{
			PrioDirection = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1));
			DirectionChanged = DateTime.UtcNow;
		}

		Vector2 randomDirection = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1));
		HeadedDirection = Vector2.Normalize(Vector2.Lerp(randomDirection, PrioDirection, 0.7f));

	}
	public override List<Projectile> Attack()
	{
		PickNewDirection();
		List<Projectile> projectiles = base.Attack();
		projectiles.Add(new ProjectileNokia(Position + Size / 2, Direction, AttackDmg));
		LastAttackElapsed = 0;
		return projectiles;
	}

	protected override void InitStats(int difficulty)
	{
		Hp = 2 + (3f * (difficulty - 1));
		MovementSpeed = 1;
		AttackSpeed = 800;
		AttackDmg = 1;
        XpValue = 1 + difficulty / 2;
        Weight = EnemyWeight.HARD;
	}
}


