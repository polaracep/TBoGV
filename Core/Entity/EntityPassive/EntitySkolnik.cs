using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

class EntitySkolnik : EntityPassive
{
	public EntitySkolnik(Vector2 position) : base(position)
	{
	}
	public EntitySkolnik() : base() { }

	public override Texture2D GetSprite()
	{
		return TextureManager.GetTexture("petr");
	}
}