using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

class EntityPerloun : EntityPassive, IInteractable
{
    private const string NAME = "Perloun";
    public EntityPerloun(Vector2 position) : base(position, NAME)
    {
    }
    public EntityPerloun() : base(NAME) { }

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
        sg.OpenShop(ShopTypes.PERLOUN);

    }
}