using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TBoGV;

class EnemyZdena : EnemyRanged
{
	static Texture2D Sprite;
	public EnemyZdena(Vector2 position)
	{
		InitStats(0);
		Position = position;
		Sprite = TextureManager.GetTexture("korenovy_vezen");
		Size = new Vector2(Sprite.Width, Sprite.Height);
		Spawn();
	}
	public EnemyZdena() : this(Vector2.Zero) { }
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), Color.White);
	}
	public override List<Projectile> Attack()
	{
		List<Projectile> projectiles = base.Attack();
		projectiles.Add(new ProjectileRoot(Position + Size / 2, Direction, AttackDmg));
		LastAttackElapsed = 0;
		return projectiles;
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}

	public override void Move(Place place)
	{
		return;
	}

	protected override void InitStats(int difficulty)
	{
		Hp = 3;
		MovementSpeed = 4;
		AttackSpeed = 2500;
		AttackDmg = 1;
		XpValue = 1;
	}
}
