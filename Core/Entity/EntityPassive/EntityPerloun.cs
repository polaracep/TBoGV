using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

class EntityPerloun : EntityPassive, IInteractable
{
    public EntityPerloun(Vector2 position) : base(position)
    {
    }
    public EntityPerloun() : base() { }

    public override Texture2D GetSprite()
    {
        return TextureManager.GetTexture("perloun");
    }

    public void Interact(Entity e, Place p)
    {
        Screen c = TBoGVGame.screenCurrent;
        if (c is not ScreenGame)
            return;

        ScreenGame sg = (ScreenGame)c;

        sg.openShop = ShopState.PERLOUN;
    }
}