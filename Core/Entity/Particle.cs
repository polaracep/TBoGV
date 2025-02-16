using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBoGV;

public abstract class Particle : Entity
{
	public bool Visible;
	protected DateTime initTime { get; set; }
	protected int DurationMilliseconds {  get; set; }
	public virtual void Update()
	{
		Visible = (DateTime.UtcNow - initTime).TotalMilliseconds < DurationMilliseconds;
	}
	public abstract void Draw(SpriteBatch spriteBatch);
}
