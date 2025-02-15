﻿using System;
using System.Collections.Generic;

namespace TBoGV;
internal interface IDealDmg
{
	DateTime LastAttackTime { get; set; }
    float AttackSpeed { get; set; }
	float AttackDmg { get; set; }
	bool ReadyToAttack();
    List<Projectile> Attack();
}