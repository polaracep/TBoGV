using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

class EntityFyjala : EntityPassive
{
	public EntityFyjala(Vector2 position) : base(position)
	{
	}
	public EntityFyjala() : base() { }

	public override Texture2D GetSprite()
	{
		return TextureManager.GetTexture("petr");
	}
}