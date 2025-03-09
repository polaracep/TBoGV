using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

class EntitySkolnik : EntityPassive
{
	public const string NAME = "Školník";
	public EntitySkolnik(Vector2 position) : base(position, NAME)
	{
	}
	public EntitySkolnik() : base(NAME) { }

	public override Texture2D GetSprite()
	{
		return TextureManager.GetTexture("petr");
	}
}