using Microsoft.Xna.Framework;
using System;

namespace TBoGV;

public abstract class Particle : Entity
{
	public bool Visible;
	protected DateTime initTime { get; set; }
	protected int DurationMilliseconds { get; set; }
	public virtual void Update(GameTime gameTime)
	{
		Visible = (DateTime.UtcNow - initTime).TotalMilliseconds < DurationMilliseconds;
	}
}
