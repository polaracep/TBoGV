using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TBoGV;
class BossToilet : EnemyBoss
{
	static Texture2D Spritesheet = TextureManager.GetTexture("toiletSpritesheet");
	static SoundEffect Sfx = SoundManager.GetSound("toilet");
	static SoundEffectInstance SfxInstance = Sfx.CreateInstance();
	protected double phaseChangeElapsed = 0;
	protected int chillDuration = 3000;
	protected int rageDuration = (int)Sfx.Duration.TotalMilliseconds;
	protected bool SpawnAttack = false;
	static float Scale;
	static int frameWidth = 155;
	static int frameHeight = 191;
	static int frameCount = 115;
	int currentFrame = 0;
	double lastFrameChangeElapsed = 0;
	static double frameSpeed = Sfx.Duration.TotalMilliseconds / frameCount;
	Action SpawnCameraman;
	protected bool Rage { get; set; }

	protected new enum BossPhases : int
	{
		IDLE = 0,
		STANDING = 1,
	}
	protected BossPhases Phase { get; set; }

	public BossToilet(Vector2 position, Place place)
	{
		SpawnCameraman = () =>
		{
			var cameraman = new EnemyCameraman(Position + new Vector2(80f, 115f) * Scale);
			place.Enemies.Add(cameraman);
		};

		InitStats(Storyline.Difficulty);
		Rage = false;
		Position = position;
		Scale = 125f / Math.Max(frameWidth, frameHeight);
		Size = new Vector2(frameWidth * Scale, frameHeight * Scale);
		phaseChangeElapsed = 0;
	}
	public BossToilet(Place place) : this(Vector2.Zero, place) { }

	public override void Draw(SpriteBatch spriteBatch)
	{
		int framesPerRow = 20; // Maximální počet snímků v řádku
		int row = currentFrame / framesPerRow;
		int col = currentFrame % framesPerRow;

		Rectangle sourceRect = new Rectangle(col * frameWidth, row * frameHeight, frameWidth, frameHeight);
		spriteBatch.Draw(Spritesheet, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), sourceRect, Color.White);
	}


	public override List<Projectile> Attack()
	{
		List<Projectile> projectiles = base.Attack();
		var projectile = new ProjectilePee(Position + new Vector2(80f, 115f) * Scale, Direction, AttackDmg);
		projectile.MovementSpeed = 5 + 7 * Hp / MaxHp;
		projectiles.Add(projectile);
		LastAttackElapsed = 0;
		return projectiles;
	}

	public override Texture2D GetSprite()
	{
		throw new NotImplementedException();
	}

	public override void Update(Vector2 playerPosition, double dt)
	{
		phaseChangeElapsed += dt;
		lastFrameChangeElapsed += dt;
		LastAttackElapsed += dt;
		Direction = new Vector2(playerPosition.X, playerPosition.Y) - Position - Size / 2;
		Direction.Normalize();
		SfxInstance.Volume = (float)Settings.SfxVolume.GetValue();
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

			if (Rage)
			{
				if (random.Next(0, 100) < 50)
				{
					Rage = false;
					int count = random.Next(1, (int)(Hp / (MaxHp / 3)) + 1);
					for (int i = 0; i < count; i++)
						SpawnCameraman();
				}
				else
					SfxInstance.Play();
			}
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
	public override bool ReadyToAttack()
	{
		return Rage && LastAttackElapsed > AttackSpeed;
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
		var item = new ItemCross(Position + Size / 2);
		item.InitMovement();
		droppedItems.Add(item);
		return droppedItems;
	}
	public override float RecieveDmg(Projectile projectile)
	{
		if (!projectilesRecieved.Contains(projectile) && Rage)
		{
			Hp -= projectile.Damage;
			projectilesRecieved.Add(projectile);
		}
		return !Rage ? projectile.Damage : 0;
	}

	public override void Move(Place place)
	{
	}

	protected override void InitStats(int difficulty)
	{
		MaxHp = 200;
		Hp = 200;
		MovementSpeed = 0;
		AttackSpeed = 10;
		AttackDmg = 1;
		XpValue = 50;
	}
}

