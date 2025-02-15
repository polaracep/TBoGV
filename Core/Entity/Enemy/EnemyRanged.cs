using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV;
abstract class EnemyRanged : Enemy, IDraw
{
    public override void Update(Vector2 playerPosition)
    {
        Direction = new Vector2(playerPosition.X, playerPosition.Y) - Position - Size / 2;
        Direction.Normalize();
    }
    public override List<Projectile> Attack()
    {
        LastAttackTime = DateTime.UtcNow;
        return new List<Projectile>() {new ProjectilePee(Position + Size / 2, Direction, AttackDmg)};
    }
    public override bool ReadyToAttack()
    {
        return (DateTime.UtcNow - LastAttackTime).TotalMilliseconds >= AttackSpeed;
    }
	public override bool IsDead()
	{
		return Hp <= 0;
	}
    public override List<Item> Drop(int looting)
    {
        int rnd = new Random().Next(0, 100);
        if (50 / looting > rnd)
            return new List<Item>();
        else return new List<Item>() {new Coin(Position+Size/2)};
    }
}

