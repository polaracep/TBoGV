using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TBoGV;
abstract class EnemyRanged : Enemy
{
    public override void Update(Vector2 playerPosition, double dt)
    {
        LastAttackElapsed += dt;
        Direction = new Vector2(playerPosition.X, playerPosition.Y) - Position - Size / 2;
        Direction.Normalize();
    }
    public override List<Projectile> Attack()
    {
        List<Projectile> projectiles = base.Attack();
        LastAttackElapsed = 0;
        return projectiles;
    }
    public override bool ReadyToAttack()
    {
        return LastAttackElapsed >= AttackSpeed;
    }
    public override bool IsDead()
    {
        return Hp <= 0;
    }
}

