using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV;

public abstract class Tile
{
    protected Vector2 screenPos;
    protected string SpriteName;
    public Texture2D Sprite { get; protected set; }
    public bool DoCollision { get; protected set; } = false;

    // Vsechny tiles50x50.
    protected static Vector2 tileSize = new Vector2(50, 50);

    public static readonly TileVoid NoTile = new TileVoid();

    protected Tile(bool collide)
    {
        DoCollision = collide;
    }

    public static Vector2 GetSize()
    {
        return tileSize;
    }
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Sprite, this.screenPos, Color.White);
    }
}

public class TileVoid : Tile
{
    public TileVoid() : base(false) { }
}
public class TileFloor : Tile, IDraw
{
    public TileFloor(FloorTypes floor) : base(false)
    {
        switch (floor)
        {
            case FloorTypes.BASIC:
                Sprite = TextureManager.GetTexture("floor1");
                break;
            default:
                throw new Exception();
        }
    }
}

public class TileWall : Tile, IDraw
{
    public TileWall(WallTypes wall) : base(true)
    {
        switch (wall)
        {
            case WallTypes.BASIC:
                Sprite = TextureManager.GetTexture("wall1");
                break;
            default:
                throw new Exception();
        }
    }

}

public class TileInteractEventArgs : EventArgs
{
    public Directions Directions;
    public TileDoor OppositeDoor;
    public TileInteractEventArgs(Directions dir, TileDoor oppositeDoor)
    {
        OppositeDoor = oppositeDoor;
        Directions = dir;
    }
}
public class TileDoor : Tile, IDraw, IInteractable
{
    public Directions Direction { get; private set; }
    public TileDoor OppositeDoor;
    public Vector2 DoorTpPosition;
    public bool IsBossDoor = false;
    // public Vector2 TeleportPosition;
    public static event EventHandler<TileInteractEventArgs> TileInteract;
    public TileDoor(DoorTypes type, Directions direction, Vector2 doorPos, TileDoor oppositeDoor) : base(true)
    {
        this.DoorTpPosition = doorPos;
        this.OppositeDoor = oppositeDoor;
        this.Direction = direction;
        SetDoorType(type);
    }
    public TileDoor(DoorTypes door, Directions direction, Vector2 doorPos) : this(door, direction, doorPos, null) { }
    public TileDoor(DoorTypes door, Directions direction) : this(door, direction, Vector2.Zero, null) { }
    public void Interact(Entity e, Room r)
    {
        // put player in the left-top corne
        if (OppositeDoor == null)
        {
            Console.WriteLine("No destinaiton door provided");
            return;
        }
        OnTileInteract(new TileInteractEventArgs(this.Direction, this.OppositeDoor));
    }
    protected virtual void OnTileInteract(TileInteractEventArgs e)
    {
        TileInteract?.Invoke(this, e);
    }

    public void SetDoorType(DoorTypes newType)
    {
        switch (newType)
        {
            case DoorTypes.BASIC:
                Sprite = TextureManager.GetTexture("door");
                break;
            default:
                Sprite = TextureManager.GetTexture("door");
                break;
        }
    }
}

public class TileHeal : Tile, IDraw, IInteractable
{
    public TileHeal() : base(true)
    {
        this.Sprite = TextureManager.GetTexture("heal");
    }
    public void Interact(Entity e, Room r)
    {
        if (e is Player)
        {
            Player p = (Player)e;
            p.Heal(1);
        }
    }
}