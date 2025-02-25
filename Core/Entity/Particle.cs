using Microsoft.Xna.Framework;
using System;

namespace TBoGV;

public abstract class Particle : Entity
{
	public bool Visible;
	protected int DurationMilliseconds { get; set; }
	protected double elapsedTime {  get; set; }
	public virtual void Update(double dt)
	{
		elapsedTime += dt;
	}
}
