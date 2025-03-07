using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace TBoGV;

public class BossAles : EnemyBoss
{
	private static Texture2D SpriteChill = TextureManager.GetTexture("chillAles");
	private static Texture2D SpriteRage = TextureManager.GetTexture("dzojkAles");
	private static SoundEffectInstance SfxDzojkInstance = SoundManager.GetSound("dzojkShorter").CreateInstance();
	private static SoundEffect SfxKaves = SoundManager.GetSound("kaves");
	private static SoundEffectInstance SfxKavesInstance = SfxKaves.CreateInstance();
	private static float ScaleChill;
	private static float ScaleRage;

	protected double phaseChangeElapsed = 0;
	protected int chillDuration = 5000;
	protected int rageDuration = (int)SfxKaves.Duration.TotalMilliseconds;
	protected bool Rage { get; set; }

	private double rotationOffset = 0;
	protected new enum BossPhases : int
	{
		IDLE = 0,
		KAVES = 1,
	}
	protected BossPhases Phase { get; set; }
	private Vector2 center = new Vector2(450, 450); // Example center position
	private List<Vector2> corners;
	public BossAles(Vector2 position)
	{
		InitStats(Storyline.Difficulty);
		Rage = false;
		Position = position;
		ScaleRage = 100f / Math.Max(SpriteRage.Width, SpriteRage.Height);
		ScaleChill = 100f / Math.Max(SpriteChill.Width, SpriteChill.Height);
		Size = new Vector2(SpriteChill.Width * ScaleChill, SpriteChill.Height * ScaleChill);
		phaseChangeElapsed = 0;
		int radius = 250;
		corners = new List<Vector2>
	{
		center + new Vector2(-radius, -radius),
		center + new Vector2(radius, -radius),
		center + new Vector2(-radius, radius),
		center + new Vector2(radius, radius),
		center
	};
	}
	public BossAles() : this(Vector2.Zero) { }
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Rage ? SpriteRage : SpriteChill, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), Color.White);
	}


	public override List<Projectile> Attack()
	{
		LastAttackElapsed = 0;
		List<Projectile> projectiles = new List<Projectile>();

		int projectileCount = 6;
		double angleStep = 360.0 / projectileCount; // Spread evenly in a circle

		for (int i = 0; i < projectileCount; i++)
		{
			double angle = i * angleStep + rotationOffset;
			double radians = Math.PI * angle / 180.0;
			Vector2 direction = new Vector2((float)Math.Cos(radians), (float)Math.Sin(radians));

			projectiles.Add(new ProjectileKaves(Position + Size / 2, direction, AttackDmg));
		}

		rotationOffset += 8;

		return projectiles;
	}



	public override Texture2D GetSprite()
	{
		throw new NotImplementedException();
	}

	public override void Update(Vector2 playerPosition, double dt)
	{
		if (!corners.Contains(Position))
			Teleport();
		LastAttackElapsed += dt;
		phaseChangeElapsed += dt;
		SfxDzojkInstance.Volume = (float)Settings.SfxVolume.GetValue();
		SfxKavesInstance.Volume = (float)Settings.SfxVolume.GetValue();
		UpdatePhase();
	}
	protected void UpdatePhase()
	{
		if ((phaseChangeElapsed > rageDuration && Rage) || (phaseChangeElapsed > chillDuration && !Rage))
		{
			Rage = !Rage;
			phaseChangeElapsed = 0;
			chillDuration = new Random().Next(2000, 7000);
			if (Rage)
			{
				if (Random.Shared.Next(2) == 0)
				{
					SfxDzojkInstance.Play();
					Teleport();
				}
				else
					SfxKavesInstance.Play();
			}

		}
	}
	private void Teleport()
	{
		List<Vector2> possibleSpots = new List<Vector2>(corners);
		possibleSpots.Remove(Position); // Remove current position

		if (possibleSpots.Count > 0)
		{
			Position = possibleSpots[Random.Shared.Next(possibleSpots.Count)];
		}
	}
	public override bool ReadyToAttack()
	{
		return Rage && LastAttackElapsed >= AttackSpeed;
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
		var item = new ItemBookPE(Position + Size / 2);
		item.InitMovement();
		droppedItems.Add(item);
		return droppedItems;
	}

	public override void Move(Place place)
	{
		return;
	}

	protected override void InitStats(int difficulty)
	{
		Hp = 50;
		MovementSpeed = 4;
		AttackSpeed = 350;
		AttackDmg = 1;
		XpValue = 50;
	}
}
