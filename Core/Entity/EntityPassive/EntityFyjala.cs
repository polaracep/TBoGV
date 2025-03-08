using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

class EntityFyjala : EntityPassive
{
	const string NAME = "Pert Fyjala";

	public override Dialogue Dialogue { get; set; }

	public EntityFyjala(Vector2 position) : base(position, NAME)
	{
	}
	public EntityFyjala() : base(NAME) { }

	public override Texture2D GetSprite()
	{
		return TextureManager.GetTexture("petr");
	}
}