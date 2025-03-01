using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

class EntityFyjala : EntityPassive, IInteractable
{
	public EntityFyjala(Vector2 position) : base(position)
	{
	}
	public EntityFyjala() : base() { }

	public override Texture2D GetSprite()
	{
		return TextureManager.GetTexture("petr");
	}

	public void Interact(Entity e, Place p)
	{
		Screen c = TBoGVGame.screenCurrent;
		if (c is not ScreenGame)
			return;

		ScreenGame sg = (ScreenGame)c;

		sg.openShop = true;
	}
}