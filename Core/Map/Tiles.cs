using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        return MemberwiseClone();
    }
    public virtual void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.Draw(Sprite, position, null, Color.White, Rotation, new Vector2(25, 25), 1f, SpriteEffects, 0f);
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

public class TileDecoration : Tile
{
    public TileDecoration(bool collide, DecorationTypes type, float rotation, SpriteEffects fx) : base(collide, rotation, fx)
    {
        this.Sprite = TextureManager.GetTexture(type.Value);
    }
    public TileDecoration(bool collide, DecorationTypes type, SpriteEffects fx) : this(collide, type, 0f, fx) { }
    public TileDecoration(bool collide, DecorationTypes type, float rotation) : this(collide, type, rotation, SpriteEffects.None) { }
    public TileDecoration(bool collide, DecorationTypes type) : this(collide, type, 0f, SpriteEffects.None) { }

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
    public static event EventHandler<TileInteractEventArgs> TileInteract;
    public TileDoor(DoorTypes type, Directions direction, Vector2 doorPos, TileDoor oppositeDoor, SpriteEffects fx) : base(true, ComputeRotation(direction), fx)
    {
        this.DoorTpPosition = doorPos;
        this.OppositeDoor = oppositeDoor;
        this.Direction = direction;
        SetDoorType(type);
    }
    public TileDoor(DoorTypes type, Directions direction, Vector2 doorPos, TileDoor oppositeDoor) : this(type, direction, doorPos, oppositeDoor, SpriteEffects.None) { }
    public TileDoor(DoorTypes door, Directions direction, Vector2 doorPos) : this(door, direction, doorPos, null, SpriteEffects.None) { }
    public TileDoor(DoorTypes door, Directions direction) : this(door, direction, Vector2.Zero, null, SpriteEffects.None) { }
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

public class TileExit : Tile, IInteractable
{
    public TileExit(float rotation, SpriteEffects effects) : base(false, rotation, effects)
    {
        this.Sprite = TextureManager.GetTexture("exit");
    }
    public TileExit() : this(0f, SpriteEffects.None) { }

    public void Interact(Entity e, Place _)
    {
        if (e is not Player)
            return;

        Player p = (Player)e;

        // we're coming to an end!!!
        if (Storyline.CurrentLevelNumber == Storyline.LevelList.Count)
        {
            // Maturita!
            Storyline.End();
        }

        // return to lobby
        ScreenManager.ScreenGame.SendPlayerToLobby();

        // reset shops
        InGameMenuShop.ResetShop();

        p.Inventory.RemoveEffect(new EffectFyjalovaDrahota(1));
    }
}
public class TileStart : Tile, IInteractable
{
    public TileStart(float rotation, SpriteEffects effects) : base(false, rotation, effects)
    {
        this.Sprite = TextureManager.GetTexture("exit");
    }
    public TileStart() : this(0f, SpriteEffects.None) { }
    public TileStart(float rotation) : this(rotation, SpriteEffects.None) { }

    public void Interact(Entity e, Place p)
    {
        if (e is not Player)
            return;

        ScreenManager.ScreenGame.SendPlayerToLevel();
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
        if (e is Player p)
        {
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
        if (e is Player p)
        {
            Random r = new Random();
            p.Coins += r.Next(20);
        }
    }
}
public class TileShower : Tile, IInteractable
{
    Texture2D SpriteWater = TextureManager.GetTexture("showerWater");
    private SoundEffectInstance showerSoundInstance = SoundManager.GetSound("shower").CreateInstance();
    double InteractElapsed = SoundManager.GetSound("shower").Duration.TotalMilliseconds;
    double InteractDuration = SoundManager.GetSound("shower").Duration.TotalMilliseconds;
    public TileShower(float rotation, SpriteEffects fx) : base(false, rotation, fx)
    {
        List<string> spriteNames = new() { "showerClean", "showerPiss", "showerVomit", "showerRust" };
        this.Sprite = TextureManager.GetTexture(spriteNames[new Random().Next(spriteNames.Count)]);
    }

    public TileShower(float rotation) : this(0f, SpriteEffects.None) { }

    public TileShower() : this(0f) { }
    public void Interact(Entity e, Place _)
    {
        if (e is Player p)
        {
            showerSoundInstance.Volume = Convert.ToSingle(Settings.SfxVolume.Value);
            showerSoundInstance.Play();

            var existingEffect = p.Inventory.Effects.FirstOrDefault(effect => effect is EffectLol);
            if (existingEffect != null)
                p.Inventory.AddEffect(new EffectLol(-1));

            existingEffect = p.Inventory.Effects.FirstOrDefault(effect => effect is EffectCooked);
            if (existingEffect != null)
                p.Inventory.AddEffect(new EffectCooked(-1));
            InteractElapsed = 0;
        }
    }
    public override void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        spriteBatch.Draw(Sprite, position, null, Color.White, Rotation, new Vector2(25, 25), 1f, SpriteEffects, 0f);
        if (IsAnimated())
        {
            spriteBatch.Draw(SpriteWater, position, null, Color.White * (float)Math.Min(1f, Math.Min(InteractElapsed / 400f, InteractDuration / 400f - InteractElapsed / 400f)), Rotation, new Vector2(25, 25), 1f, SpriteEffects, 0f);
        }
    }
    public void Update(double dt)
    {
        InteractElapsed += dt;
    }
    public bool IsAnimated()
    {
        return InteractElapsed <= InteractDuration;
    }
}

public class TileFridge : Tile, IInteractable
{
    public TileFridge(DecorationTypes sprite, float rotation, SpriteEffects fx) : base(true, rotation, fx)
    {
        Sprite = TextureManager.GetTexture(sprite.Value);
    }

    public TileFridge(DecorationTypes sprite, float rotation) : this(sprite, rotation, SpriteEffects.None) { }

    public TileFridge() : this(DecorationTypes.FRIDGE1, 0f) { }
    public void Interact(Entity e, Place _)
    {
        if (e is Player p)
        {
            var existingEffect = p.Inventory.Effects.FirstOrDefault(effect => effect is EffectCooked);
            if (((existingEffect != null || p.Hp < p.MaxHp) && p.Coins >= 1 && !Storyline.Endless) ||
				((existingEffect != null || p.Hp < p.MaxHp) && p.Coins >= 10 && Storyline.Endless))

			{
                if (existingEffect != null)
                    p.Inventory.AddEffect(new EffectCooked(-3));
                p.Heal(1);
                p.Coins -= 1;
				if (Storyline.Endless)
					p.Coins -= 9;
			}
		}
    }
}

public class TileCoffeeMachine : Tile, IInteractable
{
    public TileCoffeeMachine(float rotation, SpriteEffects fx) : base(true, rotation, fx)
    {
        this.Sprite = TextureManager.GetTexture("coffeeMachine");
    }
    public TileCoffeeMachine(float rotation) : this(rotation, SpriteEffects.None) { }
    public TileCoffeeMachine() : this(0f) { }
    public void Interact(Entity e, Place _)
    {
        if (e is Player p)
        {
            if (
                (p.Hp < p.MaxHp && p.Coins >= 1 && !Storyline.Endless) || 
                (p.Hp < p.MaxHp && p.Coins >= 10 && Storyline.Endless))
            {
                p.Heal(2);
                p.Coins -= 1;
				if (Storyline.Endless)
					p.Coins -= 9;
			}
        }
    }
}

public class TileComputer : Tile, IInteractable
{
    public TileComputer(float rotation, SpriteEffects fx) : base(true, rotation, fx)
    {
        this.Sprite = TextureManager.GetTexture(DecorationTypes.KATEDRA.Value);
    }
    public TileComputer(float rotation) : this(rotation, SpriteEffects.None) { }
    public TileComputer() : this(0f) { }
    public void Interact(Entity e, Place _)
    {
        if (e is Player p)
        {
            p.Inventory.AddEffect(new EffectLol(1));
        }
    }
}

public class TileLocker : Tile, IInteractable
{
    protected bool IsOpen { get; set; }
    protected int Id { get; set; }
    public TileLocker(float rotation, SpriteEffects fx, bool isOpen) : base(true, rotation, fx)
    {
        Sprite = TextureManager.GetTexture("lockerClosed");
        IsOpen = isOpen;
    }
    public TileLocker(float rotation, bool isOpen) : this(rotation, SpriteEffects.None, isOpen) { }
    public TileLocker(bool isOpen) : this(0f, isOpen) { }
    public TileLocker() : this(0f, false) { }
    public void Interact(Entity e, Place _)
    {
        Screen c = TBoGVGame.screenCurrent;
        if (c is not ScreenGame || !IsOpen)
            return;

        ScreenGame sg = (ScreenGame)c;
        sg.OpenShop(ShopTypes.LOCKER, Id);
    }
    public void SetId(int id)
    {
        Id = id;
    }
    public void Open()
    {
        IsOpen = true;
        this.Sprite = TextureManager.GetTexture("lockerOpen");
    }
}