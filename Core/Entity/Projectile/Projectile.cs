using Microsoft.Xna.Framework.Graphics;


namespace TBoGV;

public abstract class Projectile : Entity, IDraw
{
	public bool ShotByPlayer;
	public float Damage { get; set; }
	public virtual void Update()
	{
		Position += Direction * MovementSpeed;
	}
	public abstract void Draw(SpriteBatch spriteBatch);

}

