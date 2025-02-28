namespace TBoGV;

public abstract class Item : Entity, IInteractable
{
	public virtual void Interact(Entity e, Place p)
    {
        p.player.Coins++;
    }
}

