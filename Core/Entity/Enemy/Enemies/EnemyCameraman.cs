﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;


namespace TBoGV;
class EnemyCameraman : EnemyMelee
{
	static Texture2D Sprite = TextureManager.GetTexture("cameraman");
	static float Scale;
	protected Vector2 SpotToVisit;
	public EnemyCameraman(Vector2 position)
	{
		InitStats(Storyline.Difficulty);
		movingDuration = 3200;
		chillDuration = 0;
		Position = position;
		Scale = 50f / Math.Max(Sprite.Width, Sprite.Height);
		Size = new Vector2(Sprite.Width * Scale, Sprite.Height * Scale);
		SpotToVisit = position;
		AttackDelay();

	}
	public EnemyCameraman() : this(Vector2.Zero) { }

	public override void Draw(SpriteBatch spriteBatch)
	{
		float rotation = MathF.Atan2(Direction.Y, Direction.X);
		Vector2 origin = new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);
		spriteBatch.Draw(
			Sprite,
			new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y),
			null,
			Color.White,
			rotation,
			origin, // Střed rotace
			SpriteEffects.None,
			0
		);
		DrawHealthBar(spriteBatch);
	}

	public override Texture2D GetSprite()
	{
		return Sprite;
	}
	public override void Update(Vector2 playerPosition, double dt)
	{
		base.Update(playerPosition, dt);

	}
	protected override void UpdateMoving(double dt)
	{
		if ((phaseChangeElapsed > movingDuration && Moving) ||
			(phaseChangeElapsed > chillDuration && !Moving))
		{
			Moving = !Moving;
			phaseChangeElapsed = 0;
		}
	}
    public override void InitStats(int difficulty)
	{
		Hp = 3;
		MovementSpeed = 2;
		AttackSpeed = 10;
		AttackDmg = 1;
		XpValue = 1;
		Weight = EnemyWeight.EASY;
		base.InitStats(difficulty);
	}
	public override void Move(Place place)
	{
		if ((place.player.Position - Position).Length() <= 225)
		{
			Direction = (place.player.Position + place.player.Size/2 - Position);
		}
		else
		{
			//vyber nahodny spot pokud uz jsi blizko
			if ((SpotToVisit - Position).Length() <= 100)
			{
				SpotToVisit = new Vector2(random.Next(1, (int)place.Dimensions.X - 1), random.Next(1, (int)place.Dimensions.Y) - 1)* 50;
			}
			Direction = (SpotToVisit - Position);
		}
		Direction.Normalize();

		Position += Direction * MovementSpeed;
	}
	public override List<Projectile> Attack()
	{
		return new List<Projectile>() { new ProjectileMelee(Position, Size * new Vector2(0.6f)) };
	}
}

