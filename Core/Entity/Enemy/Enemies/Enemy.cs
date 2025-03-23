using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

  /// <summary>
  /// 'Difficulty' of the enemy. The higher the weight, the less enemies of this type will spawn.
  /// </summary>
  public EnemyWeight Weight { get; protected set; } = EnemyWeight.EASY;
  public List<Projectile> projectilesRecieved = new List<Projectile>();
  public Vector2 HeadedDirection { get; set; }
  protected static readonly Random random = new Random();
  public virtual void AttackDelay()
  {
    LastAttackElapsed = -random.Next(0, 1000);
  }
  public abstract bool ReadyToAttack();
  public abstract void Update(Vector2 playerPosition, double dt);
  public abstract void Move(Place place);
  public abstract bool IsDead();
  public virtual List<Projectile> Attack()
  {
    return new List<Projectile>() { new ProjectileMelee(Position + Size / 2, Size * new Vector2(0.6f)) };
  }
  public virtual void InitStats(int difficulty)
	{
		MaxHp = (int)Hp;
    }
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
    int coinCount = random.Next(0, maxCoins - minCoins + 1);
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
        AttackDelay();
        Enemy clone = (Enemy)this.MemberwiseClone();
        clone.projectilesRecieved = new List<Projectile>(this.projectilesRecieved); // Creates a new list instead of sharing reference
        return clone;
    }

    public bool IsDamaged()
	{
		return (int)Hp != MaxHp;
	}
	protected static Texture2D SpriteWhiteSquare = TextureManager.GetTexture("whiteSquare");
	public virtual void DrawHealthBar(SpriteBatch spriteBatch)
	{
		if (!IsDamaged())
			return;

		int barWidth = (int)Size.X;
		int barHeight = 5;
		Vector2 barPosition = new Vector2(Position.X, Position.Y - barHeight - 2);

		spriteBatch.Draw(SpriteWhiteSquare, new Rectangle(barPosition.ToPoint(), new Point(barWidth, barHeight)), Color.Black);

		float healthPercent = (float)Hp / MaxHp;
		int healthBarWidth = (int)(barWidth * healthPercent);

		spriteBatch.Draw(SpriteWhiteSquare, new Rectangle(barPosition.ToPoint(), new Point(healthBarWidth, barHeight)), Color.Green);

		spriteBatch.Draw(SpriteWhiteSquare, new Rectangle(new Point(barPosition.ToPoint().X + healthBarWidth, barPosition.ToPoint().Y), new Point(barWidth - healthBarWidth, barHeight)), Color.Red);
	}

}

public enum EnemyWeight : int
{
  EASY = 1,
  MEDIUM = 2,
  HARD = 3,
}