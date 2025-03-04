using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TBoGV;
public abstract class Enemy : Entity, IRecieveDmg, IDealDmg, ICloneable
{
  public double LastAttackElapsed { get; set; }
  public float AttackSpeed { get; set; }
  public float AttackDmg { get; set; }
  public float Hp { get; set; }
  public int MaxHp { get; set; }
  public int XpValue { get; set; }
  public List<Projectile> projectilesRecieved = new List<Projectile>();
  public Vector2 HeadedDirection { get; set; }
  protected readonly Random random = new Random();
  public virtual void Spawn()
  {
    LastAttackElapsed = -random.Next(50, 800);
  }
  public abstract bool ReadyToAttack();
  public abstract void Update(Vector2 playerPosition, double dt);
  public abstract void Move(Place place);
  public abstract bool IsDead();
  public virtual List<Projectile> Attack()
  {
    return new List<Projectile>() { new ProjectileMelee(Position + Size / 2, Size * new Vector2(0.6f)) };
  }
  protected abstract void InitStats(int difficulty);
	public virtual List<Item> Drop(int looting)
	{
		Random random = new Random();
		List<Item> droppedItems = new List<Item>();

		// Define min and max coins to drop
		int minCoins = 0; 
		int maxCoins = 2 + looting; 

		// Drop chance calculation
		int dropChance = 50 / (looting + 1);
		for (int i = 0; i < minCoins; i++)
		{
			droppedItems.Add(new Coin(Position + Size / 2));
		}
		int coinCount = random.Next(0, maxCoins-minCoins + 1);
		for (int i = 0; i < coinCount; i++)
		{
			if (random.Next(0, 100) >= dropChance)
				droppedItems.Add(new Coin(Position + Size / 2));
		}
		
		return droppedItems;
	}
  public virtual float RecieveDmg(Projectile projectile)
  {
    if (!projectilesRecieved.Contains(projectile))
    {
      Hp -= projectile.Damage;
      projectilesRecieved.Add(projectile);
    }
    return Hp < 0 ? -Hp : 0;
  }

  public object Clone()
  {
    return this.MemberwiseClone();
  }
}
