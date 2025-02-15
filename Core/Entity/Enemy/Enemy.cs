using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TBoGV;
public abstract class Enemy : Entity, IRecieveDmg, IDealDmg
{
    public DateTime LastAttackTime { get; set; }
    public float AttackSpeed { get; set; }
    public float AttackDmg { get; set; }
    public float Hp { get; set; }
    public int MaxHp { get; set; }
    public int XpValue { get; set; }

    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract bool ReadyToAttack();
    public abstract void Update(Vector2 playerPosition);
    public abstract bool IsDead();
    public abstract List<Projectile> Attack();

    public virtual void RecieveDmg(float damage)
    {
        Hp -= damage;
    }
}
