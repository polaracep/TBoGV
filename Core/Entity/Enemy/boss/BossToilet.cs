using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TBoGV;
internal class BossToilet : EnemyBoss
{
	static Texture2D Spritesheet = TextureManager.GetTexture("toiletSpritesheet");
	static SoundEffect Sfx = SoundManager.GetSound("toilet");
	protected double phaseChangeElapsed = 0;
	protected int chillDuration = 3000;
	protected int rageDuration = (int)Sfx.Duration.TotalMilliseconds;

	static float Scale;
	static int frameWidth = 155;
	static int frameHeight = 191;
	static int frameCount = 115;
	int currentFrame = 0;
	double lastFrameChangeElapsed = 0;
	static double frameSpeed = Sfx.Duration.TotalMilliseconds / frameCount;

	protected bool Rage { get; set; }

	private Random random = new Random();

	protected new enum BossPhases : int
	{
		IDLE = 0,
		STANDING = 1,
	}
	protected BossPhases Phase { get; set; }

	public BossToilet(Vector2 position)
	{
		Rage = false;
		Position = position;
		Hp = 60;
		MovementSpeed = 2;
		AttackSpeed = 25;
		AttackDmg = 1;
		Scale = 150f / Math.Max(frameWidth, frameHeight);
		Size = new Vector2(frameWidth * Scale, frameHeight * Scale);
		XpValue = 50;
		phaseChangeElapsed = 0;
	}
	public BossToilet() : this(Vector2.Zero) { }

	public override void Draw(SpriteBatch spriteBatch)
	{
		int framesPerRow = 20; // Maximální počet snímků v řádku
		int row = currentFrame / framesPerRow;
		int col = currentFrame % framesPerRow;

		Rectangle sourceRect = new Rectangle(col * frameWidth, row * frameHeight, frameWidth, frameHeight);
		spriteBatch.Draw(Spritesheet, new Vector2(Position.X, Position.Y), sourceRect, Color.White);
	}


	public override List<Projectile> Attack()
	{
		List<Projectile> projectiles = base.Attack();
		projectiles.Add(new ProjectilePee(Position + Size / 2, Direction, AttackDmg));
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
				rageDuration = (int)Sfx.Duration.TotalMilliseconds; // Reset to normal
				Sfx.Play();
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
		return new List<Item>() { new ItemTeeth(this.Position) };
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
}

