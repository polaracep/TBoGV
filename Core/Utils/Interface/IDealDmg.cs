using System;
using System.Collections.Generic;

namespace TBoGV;
internal interface IDealDmg
{
	double LastAttackElapsed { get; set; }
    float AttackSpeed { get; set; }
	float AttackDmg { get; set; }
	bool ReadyToAttack();
    List<Projectile> Attack();
}