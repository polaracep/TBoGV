using Microsoft.Xna.Framework.Graphics;

namespace TBoGV;

public abstract class Item : Entity, IDraw, IInteractable
{
    public bool Small;
    public abstract void Draw(SpriteBatch spriteBatch);

    public virtual void Interact(Entity e, Room r)
    {
        r.player.Coins++;
    }
}

