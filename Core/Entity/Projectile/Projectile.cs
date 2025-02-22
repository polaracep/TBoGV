namespace TBoGV;

public abstract class Projectile : Entity
{
	public bool ShotByPlayer;
	public float Damage { get; set; }
	public virtual void Update()
	{
		Position += Direction * MovementSpeed;
	}
}

