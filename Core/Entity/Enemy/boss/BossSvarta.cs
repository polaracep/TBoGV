using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TBoGV;
class BossSvarta : EnemyBoss
{
	static Texture2D SpriteChill = TextureManager.GetTexture("svartaChill");
	static Texture2D SpriteJump = TextureManager.GetTexture("svartaJump");
	static Texture2D SpriteSpinks = TextureManager.GetTexture("svartaSpinks");
	Texture2D currentSprite = SpriteChill;
	static float ScaleChill = 60f / Math.Max(SpriteChill.Height, SpriteChill.Height);
	static float ScaleJump = 60f / Math.Max(SpriteJump.Height, SpriteJump.Height);
	static float ScaleSpinks = 60f / Math.Max(SpriteSpinks.Height, SpriteSpinks.Height);
	float currentScale = ScaleChill;
	static SoundEffect SfxJeMuHodne = SoundManager.GetSound("takToJsemNevidel");
	static SoundEffectInstance SfxJeMuHodneInstance = SfxJeMuHodne.CreateInstance();
	static SoundEffect SfxSvartaDelej = SoundManager.GetSound("svartaDelej");
	static SoundEffectInstance SfxSvartaDelejInstance = SfxSvartaDelej.CreateInstance();
	protected double phaseChangeElapsed = 0;
	protected int chillDuration = (int)SfxSvartaDelej.Duration.TotalMilliseconds;
	protected int spinksDuration = 7000;
	protected enum SvartaState
	{
		CHILL,
		JUMP,
		SPINKS,
	}
	protected SvartaState State = SvartaState.SPINKS;

	public BossSvarta(Vector2 position)
	{
		InitStats(Storyline.Difficulty);
		Position = position;
		phaseChangeElapsed = spinksDuration;
	}
	public BossSvarta() : this(Vector2.Zero) { }

	public override void Draw(SpriteBatch spriteBatch)
	{
		SpriteEffects effect = Direction.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
		spriteBatch.Draw(currentSprite, Position, null, Color.White, 0f, Vector2.Zero, currentScale, effect, 0f);
	}

	public override void Update(Vector2 playerPosition, double dt)
	{
		phaseChangeElapsed += dt;
		SfxJeMuHodneInstance.Volume = Settings.SfxVolume;
		SfxSvartaDelejInstance.Volume = Settings.SfxVolume;
		UpdatePhase(playerPosition);
	}

	protected void UpdatePhase(Vector2 playerPosition)
	{
		switch (State)
		{
			case SvartaState.CHILL:
				currentSprite = SpriteChill;
				currentScale = ScaleChill;
				PickNewDirection(playerPosition);
				if (phaseChangeElapsed > chillDuration)
				{
					State = SvartaState.JUMP;
					phaseChangeElapsed = 0;
				}
				break;
			case SvartaState.JUMP:
				currentSprite = SpriteJump;
				currentScale = ScaleJump;
				break;
			case SvartaState.SPINKS:
				currentSprite = SpriteSpinks;
				currentScale = ScaleSpinks;
				if (phaseChangeElapsed > spinksDuration)
				{
					State = SvartaState.CHILL;
					phaseChangeElapsed = 0;
					SfxSvartaDelejInstance.Play();
				}
				break;
		}
		UpdateSize();
	}
	private void UpdateSize()
	{
		Size = new Vector2(currentSprite.Width * currentScale, currentSprite.Height * currentScale);
	}
	private void PickNewDirection(Vector2 playerPosition)
	{
		Direction = Vector2.Normalize(playerPosition - Position - Size / 2);
	}

	public override bool ReadyToAttack()
	{
		return State == SvartaState.JUMP;
	}

	public override bool IsDead()
	{
		return Hp <= 0;
	}
    public override List<Item> Drop(int looting)
    {
        Random random = new Random();
        List<Item> droppedItems = new List<Item>();
        droppedItems = base.Drop(looting);
        var item = new ItemMonster(Position + Size / 2);
        item.InitMovement();
        droppedItems.Add(item);
        return droppedItems;
    }
    public override float RecieveDmg(Projectile projectile)
	{
		if (!projectilesRecieved.Contains(projectile) && State == SvartaState.SPINKS)
		{
			Hp -= projectile.Damage;
			projectilesRecieved.Add(projectile);
			return 0;
		}
		return projectile.Damage;
	}
	public override void Move(Place place)
	{
		if (State != SvartaState.JUMP)
			return;

		float stepSize = 1.0f;
		int steps = (int)Math.Ceiling(MovementSpeed / stepSize);
		Vector2 stepDirection = Vector2.Normalize(Direction) * stepSize;

		for (int i = 0; i < steps; i++)
		{
			Vector2 nextPositionX = new Vector2(Position.X + stepDirection.X, Position.Y);
			Vector2 nextPositionY = new Vector2(Position.X, Position.Y + stepDirection.Y);

			bool collidesX = CollidesWithWall(nextPositionX, place);
			bool collidesY = CollidesWithWall(nextPositionY, place);

			if (collidesX || collidesY)
			{
				JeMuHodne();
				break; // Stop moving if a collision occurs
			}
			Position += stepDirection; // Apply the step movement
		}
	}
	private void JeMuHodne()
	{
		State = SvartaState.SPINKS;
		SfxJeMuHodneInstance.Play();
	}
	private bool CollidesWithWall(Vector2 testPosition, Place place)
	{
		return place.ShouldCollideAt(new Rectangle((int)testPosition.X, (int)testPosition.Y, (int)(Size.X * 0.6), (int)(Size.Y * 0.6)));
	}

	public override Texture2D GetSprite()
	{
		throw new NotImplementedException();
	}

	protected override void InitStats(int difficulty)
	{
		Hp = 15;
		MovementSpeed = 12;
		AttackSpeed = 0;
		AttackDmg = 2;
		XpValue = 50;
	}
}
