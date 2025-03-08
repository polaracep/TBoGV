using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

class EntitySarka : EntityPassive, IInteractable
{
    private const string NAME = "Šárka";
    public override Dialogue Dialogue { get; set; } = new DialogueIntro();
    public EntitySarka(Vector2 position) : base(NAME) { }
    public EntitySarka() : base(NAME) { }

    public override Texture2D GetSprite()
    {
        return TextureManager.GetTexture("sarka");
    }

    public void Interact(Entity e, Place p)
    {
        Screen c = TBoGVGame.screenCurrent;
        if (c is not ScreenGame)
            return;

        ScreenGame sg = (ScreenGame)c;
        sg.OpenShop(ShopTypes.SARKA);
    }

}