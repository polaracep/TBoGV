using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

class EntitySarka : EntityPassive, IInteractable
{
    public EntitySarka(Vector2 position) : base(position)
    {
    }
    public EntitySarka() : base() { }

    public override Texture2D GetSprite()
    {
        return TextureManager.GetTexture("sarka");
    }

    public void Interact(Entity e, Place p)
    {
        Storyline.NextLevel();
    }
}