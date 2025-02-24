using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV;

public abstract class Tile : ICloneable
{
    public Texture2D Sprite { get; protected set; }
    public bool DoCollision { get; protected set; } = false;
    public float Rotation = 0f;

    public SpriteEffects SpriteEffects;

    // Vsechny tiles50x50.
    protected static Vector2 tileSize = new Vector2(50, 50);

    protected Tile(bool collide, float rotation, SpriteEffects effects)
    {
        this.Rotation = rotation;
        this.DoCollision = collide;
        this.SpriteEffects = effects;
    }
    public static Vector2 GetSize()
    {
        return tileSize;
    }

    public void FlipHorizontally()
    {
        Sprite.FlipHorizontally();
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}

public class TileFloor : Tile
{
    public TileFloor(FloorTypes floor, float rotation, SpriteEffects effects)
        : base(false, rotation, effects)
    {
        Sprite = TextureManager.GetTexture(floor.Value);
    }

    public TileFloor(FloorTypes floor)
        : this(floor, 0f, SpriteEffects.None) { }
}

public class TileWall : Tile
{
    public TileWall(WallTypes wall, float rotation, SpriteEffects effects)
        : base(true, rotation, effects)
    {
        Sprite = TextureManager.GetTexture(wall.Value);
    }

    public TileWall(WallTypes wall)
        : this(wall, 0f, SpriteEffects.None) { }
}

public class TileInteractEventArgs : EventArgs
{
    public Directions Directions;
    public TileDoor OppositeDoor;
    public Place Place;
    public TileInteractEventArgs(Directions dir, TileDoor oppositeDoor, Place place)
    {
        OppositeDoor = oppositeDoor;
        Directions = dir;
        Place = place;
    }
}
public class TileDoor : Tile, IInteractable
{
    public Directions Direction { get; private set; }
    public TileDoor OppositeDoor;
    public Vector2 DoorTpPosition;
    public bool IsBossDoor = false;
    public static event EventHandler<TileInteractEventArgs> TileInteract;
    public TileDoor(DoorTypes type, Directions direction, Vector2 doorPos, TileDoor oppositeDoor, SpriteEffects fx)
        : base(true, ComputeRotation(direction), fx)
    {
        this.DoorTpPosition = doorPos;
        this.OppositeDoor = oppositeDoor;
        this.Direction = direction;
        SetDoorType(type);
    }
    public TileDoor(DoorTypes type, Directions direction, Vector2 doorPos, TileDoor oppositeDoor)
        : this(type, direction, doorPos, oppositeDoor, SpriteEffects.None) { }
    public TileDoor(DoorTypes door, Directions direction, Vector2 doorPos)
        : this(door, direction, doorPos, null, SpriteEffects.None) { }
    public TileDoor(DoorTypes door, Directions direction)
        : this(door, direction, Vector2.Zero, null, SpriteEffects.None) { }
    public void Interact(Entity e, Place p)
    {
        // put player in the left-top corne
        if (OppositeDoor == null)
        {
            Console.WriteLine("No destinaiton door provided");
            return;
        }
        OnTileInteract(new TileInteractEventArgs(this.Direction, this.OppositeDoor, p));
    }
    protected virtual void OnTileInteract(TileInteractEventArgs e)
    {
        TileInteract?.Invoke(this, e);
    }

    public void SetDoorType(DoorTypes newType)
    {
        Sprite = TextureManager.GetTexture(newType.Value);
    }
    private static float ComputeRotation(Directions direction)
    {
        switch (direction)
        {
            case Directions.UP: return 0.0f;
            case Directions.DOWN: return MathHelper.Pi; // 180 degrees
            case Directions.RIGHT: return MathHelper.PiOver2; // 90 degrees
            case Directions.LEFT: return -MathHelper.PiOver2; // -90 degrees
            default: return 0.0f;
        }
    }
}

public class TileHeal : Tile, IInteractable
{
    public TileHeal(float rotation, SpriteEffects fx) : base(true, rotation, fx)
    {
        this.Sprite = TextureManager.GetTexture("heal");
    }

    public TileHeal(float rotation) : this(0f, SpriteEffects.None) { }

    public TileHeal() : this(0f) { }
    public void Interact(Entity e, Place _)
    {
        if (e is Player)
        {
            Player p = (Player)e;
            p.Heal(1);
        }
    }
}

public class TileTreasure : Tile, IInteractable
{
    public TileTreasure(float rotation, SpriteEffects fx) : base(false, rotation, fx)
    {
        this.Sprite = TextureManager.GetTexture("gold");
    }

    public TileTreasure(float rotation) : this(0f, SpriteEffects.None) { }

    public TileTreasure() : this(0f) { }
    public void Interact(Entity e, Place _)
    {
        if (e is Player)
        {
            Random r = new Random();
            Player p = (Player)e;
            p.Coins += r.Next(20);
        }
    }
}

public class TileDecoration : Tile
{
    public TileDecoration(bool collide, float rotation, DecorationTypes type, SpriteEffects fx) : base(collide, rotation, fx)
    {
        this.Sprite = TextureManager.GetTexture(type.Value);
    }
    public TileDecoration(bool collide, DecorationTypes type, SpriteEffects fx) : this(collide, 0f, type, fx) { }
    public TileDecoration(bool collide, DecorationTypes type, float rotation) : this(collide, rotation, type, SpriteEffects.None) { }
    public TileDecoration(bool collide, DecorationTypes type) : this(collide, 0f, type, SpriteEffects.None) { }

}
