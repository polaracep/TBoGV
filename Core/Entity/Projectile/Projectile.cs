using System;

namespace TBoGV;

public abstract class Projectile : Entity
{
	public bool ShotByPlayer;
	public float Damage { get; set; }
	protected float Scale = 1f;
	public virtual void Update()
	{
		Position += Direction * MovementSpeed;
	}
	public virtual void SetDamage(float damage)
	{
		Size = Size * Math.Max((0.6f*damage+0.4f)/Damage, 0.2f);
		Scale = Scale * Math.Max((0.6f * damage + 0.4f) / Damage, 0.2f);
		Damage = damage;
	}
}

