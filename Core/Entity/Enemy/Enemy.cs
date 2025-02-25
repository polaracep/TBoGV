using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TBoGV;
public abstract class Enemy : Entity, IRecieveDmg, IDealDmg
{
  public double LastAttackElapsed { get; set; }
  public float AttackSpeed { get; set; }
  public float AttackDmg { get; set; }
  public float Hp { get; set; }
  public int MaxHp { get; set; }
  public int XpValue { get; set; }
  public List<Projectile> projectilesRecieved = new List<Projectile>();
	public Vector2 HeadedDirection { get; set; }
  public abstract bool ReadyToAttack();
  public abstract void Update(Vector2 playerPosition, double dt);
	public abstract void Move(Place place);
  public abstract bool IsDead();
  public virtual List<Projectile> Attack()
	{
		return new List<Projectile>() { new ProjectileMelee(Position + Size / 2, Size * new Vector2(0.6f)) };
	}
  public abstract List<Item> Drop(int looting);
  public virtual float RecieveDmg(Projectile projectile)
  {
    if (!projectilesRecieved.Contains(projectile))
    {
      Hp -= projectile.Damage;
      projectilesRecieved.Add(projectile);
    }
    return Hp < 0 ? -Hp : 0;
  }
}
