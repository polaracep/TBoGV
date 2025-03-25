using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;

namespace TBoGV;

public class Player : Entity, IRecieveDmg, IDealDmg
{
    static Texture2D Sprite;
    public int Level { get; set; }
    public bool IsPlaying = false;
    public float Xp { get; set; }
    public float AttackSpeed { get; set; }
    public float AttackDmg { get; set; }
    public int ItemCapacity { get; set; }
    public float Hp { get; set; }
    public int MaxHp { get; set; }
    public float XpGain { get; set; }
    public int ProjectileCount { get; set; }
    public int Coins { get; set; }
    public bool TutorialCompleted = false;
    public Dictionary<StatTypes, float> BaseStats { get; set; }
    public Dictionary<StatTypes, float> LevelUpStats { get; set; }
    public double LastAttackElapsed { get; set; }
    public double LastRecievedDmgElapsed { get; set; }
    public int InvulnerabilityFrame = 1000;
    public List<Projectile> Projectiles { get; set; }
    List<Projectile> projectilesRecieved = new List<Projectile>();
    public Inventory Inventory { get; set; }
    protected List<Item> ItemsToDrop = new List<Item>();
    private MouseState previousMouseState;
    private KeyboardState prevKeyboardState;
    private string dataPath = "tbogv_player.json";

    public Player(Vector2 position)
    {
        BaseStats = new Dictionary<StatTypes, float>()
        {
            { StatTypes.MAX_HP, 3 },
            { StatTypes.DAMAGE, 1 },
            { StatTypes.PROJECTILE_COUNT, 1 },
            { StatTypes.XP_GAIN, 1 },
            { StatTypes.ATTACK_SPEED, 1500 },
            { StatTypes.MOVEMENT_SPEED, 3.5f }
        };
        LevelUpStats = new Dictionary<StatTypes, float>()
        {
            { StatTypes.MAX_HP, 0 },
            { StatTypes.DAMAGE, 0 },
            { StatTypes.PROJECTILE_COUNT, 0 },
            { StatTypes.XP_GAIN, 0 },
            { StatTypes.ATTACK_SPEED, 0 },
            { StatTypes.MOVEMENT_SPEED, 0 }
        };

        Position = position;
        Size = new Vector2(50, 50);
        Projectiles = new List<Projectile>();
        Sprite = TextureManager.GetTexture("vitekElegan");
        Coins = 1;
        ItemCapacity = 3;
        Inventory = new();
        SetStats();
        Hp = MaxHp;
        LastRecievedDmgElapsed = InvulnerabilityFrame;
    }

    public Player() : this(Vector2.One) { }

    Vector2 InteractionPoint = Vector2.Zero;
    public void SetStats()
    {
        BaseStats[StatTypes.DAMAGE] = Inventory.GetWeaponDmg();
        BaseStats[StatTypes.ATTACK_SPEED] = Inventory.GetWeaponAttackSpeed();
        Dictionary<StatTypes, float> finalStats = new Dictionary<StatTypes, float>();
        Dictionary<StatTypes, float> subjectStats = Inventory.SetStats(LevelUpStats);
        foreach (var item in subjectStats)
        {
            float subjectValue;
            switch (item.Key)
            {
                case StatTypes.MAX_HP:
                    subjectValue = (int)item.Value * 0.25f + BaseStats[item.Key];
                    break;
                case StatTypes.DAMAGE:
                    subjectValue = ((item.Value * 0.1f) + 1) * BaseStats[item.Key];
                    break;
                case StatTypes.PROJECTILE_COUNT:
                    subjectValue = (int)item.Value * 0.25f + BaseStats[item.Key];
                    break;
                case StatTypes.XP_GAIN:
                    subjectValue = ((item.Value * 0.1f) + 1) * BaseStats[item.Key];
                    break;
                case StatTypes.ATTACK_SPEED:
                    subjectValue = BaseStats[item.Key] / (1 + 0.1f * item.Value);
                    break;
                case StatTypes.MOVEMENT_SPEED:
                    subjectValue = ((item.Value * 0.05f) + 1) * BaseStats[item.Key];
                    break;
                default:
                    subjectValue = 0;
                    break;
            }
            finalStats[item.Key] = subjectValue;
        }

        // Aktualizace hráčských atributů podle finalStats
        MaxHp = (int)finalStats[StatTypes.MAX_HP];
        Hp = Math.Min(Hp, MaxHp); // Zajistíme, že HP nepřesáhne MaxHp
        AttackDmg = finalStats[StatTypes.DAMAGE];
        AttackSpeed = finalStats[StatTypes.ATTACK_SPEED];
        MovementSpeed = Inventory.GetEffect().Contains(EffectTypes.ROOTED) ? 0 : (int)Math.Max(finalStats[StatTypes.MOVEMENT_SPEED], 1);
        XpGain = finalStats[StatTypes.XP_GAIN];
        ProjectileCount = (int)Math.Max(finalStats[StatTypes.PROJECTILE_COUNT], 1);
    }

    public void Update(KeyboardState keyboardState, MouseState mouseState, Matrix transform, Place place, Viewport viewport, double dt)
    {
        LastRecievedDmgElapsed += dt;
        LastAttackElapsed += dt;

        float dx = 0, dy = 0;

        InteractionPoint = Position + (Direction * 50) + Size / 2;

        if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            dx -= MovementSpeed;
        if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            dx += MovementSpeed;
        if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            dy -= MovementSpeed;
        if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            dy += MovementSpeed;

        if (Math.Abs(dx) == Math.Abs(dy) && dx != 0)
        {
            int _dx = (int)(dx / Math.Sqrt(2));
            int _dy = (int)(dy / Math.Sqrt(2));
            if (_dx == 0)
                dx = Math.Sign(dx);
            if (_dy == 0)
                dy = Math.Sign(dy);
        }
        // --- Begin Movement ---
        int tolerance = 4;

        // Move horizontally in small increments
        if (dx != 0)
        {
            int stepX = Math.Sign(dx);
            int remainingX = (int)Math.Abs(dx);
            while (remainingX > 0)
            {
                Vector2 testPosition = new Vector2(Position.X + stepX, Position.Y);
                if (!place.ShouldCollideAt(new Rectangle((int)testPosition.X + tolerance, (int)testPosition.Y + tolerance, (int)Size.X - tolerance * 2, (int)Size.Y - tolerance * 2)))
                {
                    Position.X += stepX;
                }
                else
                {
                    break;
                }
                remainingX--;
            }
        }

        // Move vertically in small increments
        if (dy != 0)
        {
            int stepY = Math.Sign(dy);
            int remainingY = (int)Math.Abs(dy);
            while (remainingY > 0)
            {
                Vector2 testPosition = new Vector2(Position.X, Position.Y + stepY);

                if (!place.ShouldCollideAt(new Rectangle((int)testPosition.X + tolerance, (int)testPosition.Y + tolerance, (int)Size.X - tolerance * 2, (int)Size.Y - tolerance * 2)))
                {
                    Position.Y += stepY;
                }
                else
                {
                    break;
                }
                remainingY--;
            }
        }
        // --- End Movement ---


        // Interaction
        if ((previousMouseState.RightButton == ButtonState.Pressed &&
             mouseState.RightButton == ButtonState.Released) ||
             (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyUp(Keys.E)))
        {
            IInteractable t = place.GetTileInteractable(InteractionPoint);
            if (t != null)
            {
                IInteractable tile = t;
                tile.Interact(this, place);
            }

            Item item = place.GetItemInteractable(InteractionPoint);
            if (item != null)
            {
                item.Interact(this, place);
                place.Drops.Remove(item);
            }

            IInteractable entity = place.GetEntityInteractable(InteractionPoint);
            if (entity != null)
            {
                entity.Interact(this, place);
            }
        }

        // Pickup?
        for (int i = 0; i < place.Drops.Count; i++)
        {
            if (place.Drops[i] is not ItemContainerable && ObjectCollision.CircleCircleCollision(place.Drops[i], this))
            {
                place.Drops[i].Interact(this, place);
                place.Drops.Remove(place.Drops[i]);
            }
        }
        if (keyboardState.IsKeyDown(Keys.Q))
        {
            var item = Inventory.DropItem(Position + Size / 2, this);
            if (item != null)
                place.Drops.Add(item);
        }
        if (keyboardState.IsKeyDown(Keys.Q) && keyboardState.IsKeyDown(Keys.LeftShift))
        {
            var items = Inventory.DropAllItems(Position + Size / 2, this);
            foreach (var item in items)
                place.Drops.Add(item);
        }
        // pokud ma hrac prezuvky tak nemuze mit postih za to ze nema prezuvky
        var existingEffect = Inventory.Effects.FirstOrDefault(effect => effect is EffectPrezuvky);
        if (Inventory.GetEffect().Contains(EffectTypes.BOOTS) && existingEffect != null)
            Inventory.RemoveEffect(existingEffect);

        Inventory.Update(viewport, this, mouseState, dt);

        // Calculate the direction from the player to the world mouse position
        Vector2 screenMousePos = new Vector2(mouseState.X, mouseState.Y);
        Vector2 worldMousePos = Vector2.Transform(screenMousePos, Matrix.Invert(transform));
        Vector2 direction = worldMousePos - Position - Size / 2;
        if (!float.IsNaN(direction.X) && !float.IsNaN(direction.Y))
        {
            direction.Normalize();
            Direction = direction;
        }

        // Handle attacking if ready and left mouse button is pressed
        if (ReadyToAttack() && (mouseState.LeftButton == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Space)))
        {
            foreach (var projectile in Attack())
            {
                Projectiles.Add(projectile);
            }
        }
        foreach (var item in ItemsToDrop)
        {
            item.Position = Position;
            place.Drops.Add(item);
        }

        ItemsToDrop.Clear();

        SetStats();

        previousMouseState = mouseState;
        prevKeyboardState = keyboardState;
    }
    public void Drop(Item item)
    {
        ItemsToDrop.Add(item);
    }
	protected static Texture2D SpriteSelect = TextureManager.GetTexture("logo");

	public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Sprite,
            new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)),
            LastRecievedDmgElapsed >= InvulnerabilityFrame ? Color.White : Color.DarkRed);
		float scale = 10f / Math.Max(SpriteSelect.Width, SpriteSelect.Height);
		spriteBatch.Draw(SpriteSelect, new Rectangle(InteractionPoint.ToPoint(), new Point((int)(SpriteSelect.Width * scale), (int)(SpriteSelect.Height * scale))), Color.White);
    }
    public bool ReadyToAttack()
    {
        return LastAttackElapsed >= AttackSpeed && IsPlaying;
    }
    public List<Projectile> Attack()
    {
        LastAttackElapsed = 0;
        List<Projectile> firedProjectiles = new List<Projectile>();

        float spreadAngle = 10f; // Angle between projectiles in degrees
        float startAngle = -((ProjectileCount - 1) * spreadAngle) / 2; // Centering the spread

        for (int i = 0; i < ProjectileCount; i++)
        {
            float angle = MathHelper.ToRadians(startAngle + i * spreadAngle);
            Vector2 rotatedDirection = new Vector2(
                Direction.X * (float)Math.Cos(angle) - Direction.Y * (float)Math.Sin(angle),
                Direction.X * (float)Math.Sin(angle) + Direction.Y * (float)Math.Cos(angle)
            );
            rotatedDirection.Normalize();

            ProjectileMissile projectile = new ProjectileMissile(Position + Size / 2, rotatedDirection, AttackDmg);
            projectile.ShotByPlayer = true;
            projectile.ChangeSprite(Inventory.GetWeaponSprite());
            firedProjectiles.Add(projectile);
        }

        return firedProjectiles;
    }

    public float RecieveDmg(Projectile projectile)
    {
        if (!projectilesRecieved.Contains(projectile))
        {
            if (LastRecievedDmgElapsed >= InvulnerabilityFrame)
            {
                if (Inventory.GetEffect().Contains(EffectTypes.DODGE) && GetSuccess(50))
                {
                    projectilesRecieved.Add(projectile);
                    return projectile.Damage;
                }
                Hp -= projectile.Damage;
                LastRecievedDmgElapsed = 0;
                Inventory.AddEffect(new EffectCooked(1));
                if (projectile.GetType() == typeof(ProjectileRoot))
                    Inventory.AddEffect(new EffectRooted(1));
                if (projectile.GetType() == typeof(ProjectileNokia))
                    Inventory.AddEffect(new EffectDelej(1));
                if (projectile.GetType() == typeof(ProjectileNote))
                    Inventory.AddEffect(new EffectRickroll(1));
                if (projectile.GetType() == typeof(ProjectilePlatina))
                    Inventory.AddEffect(new EffectLol(1));
            }
            return 0;
        }
        return projectile.Damage;
    }
    public bool GetSuccess(int percent)
    {
        return new Random().Next(0, 100) < percent;
    }
    public void Kill(int xpGain)
    {
        Xp += xpGain * XpGain;
        while (Xp >= XpForLevel())
        {
            LevelUp();
        }
        if (Inventory.GetEffect().Contains(EffectTypes.LIFE_STEAL) && Random.Shared.Next(4) == 0)
        {
            Heal(1f);
        }
    }
    public int XpForLevel()
    {
        return 1 + Level * 2;
    }
    private void LevelUp()
    {
        Xp -= XpForLevel();
        Level += 1;
    }
    public void Heal(float healAmount)
    {
        if (Hp < MaxHp)
        {
            Hp += healAmount;
        }
    }
    public override Texture2D GetSprite()
    {
        return Sprite;
    }
    public void Reset()
    {
        BaseStats = new Dictionary<StatTypes, float>()
        {
            { StatTypes.MAX_HP, 3 },
            { StatTypes.DAMAGE, 1 },
            { StatTypes.PROJECTILE_COUNT, 1 },
            { StatTypes.XP_GAIN, 1 },
            { StatTypes.ATTACK_SPEED, 1500 },
            { StatTypes.MOVEMENT_SPEED, 3.5f }
        };
        LevelUpStats = new Dictionary<StatTypes, float>()
        {
            { StatTypes.MAX_HP, 0 },
            { StatTypes.DAMAGE, 0 },
            { StatTypes.PROJECTILE_COUNT, 0 },
            { StatTypes.XP_GAIN, 0 },
            { StatTypes.ATTACK_SPEED, 0 },
            { StatTypes.MOVEMENT_SPEED, 0 }
        };

        Level = 0;
        Coins = 1;
        ItemCapacity = 3;
        Inventory = new();
        SetStats();
        Hp = MaxHp;
        Xp = 0;
        LastRecievedDmgElapsed = InvulnerabilityFrame;
        // fuj
        Position = new Lobby(this).SpawnPos * 50;

    }


    public void Save(SaveType saveType)
    {
        List<ItemContainerData> containerData = new List<ItemContainerData>();
        foreach (var i in Inventory.ItemContainers)
            if (!i.IsEmpty())
                containerData.Add(new ItemContainerData { ItemName = i.Item.Name, IsEmpty = false, Type = i.ContainerType });
            else
                containerData.Add(new ItemContainerData { ItemName = "null", IsEmpty = true, Type = i.ContainerType });
        List<EffectData> EffectsData = new List<EffectData>();
        foreach (var e in Inventory.Effects)
            EffectsData.Add(new EffectData { Level = e.Level, Name = e.Name, Stats = e.Stats });

        PlayerData data = new PlayerData
        {
            Position = Position,
            Level = Level,
            Endless = Storyline.Endless,
            Xp = Xp,
            Hp = Hp,
            Coins = Coins,
            LevelUpStats = LevelUpStats,
            LastAttackElapsed = LastAttackElapsed,
            LastRecievedDmgElapsed = LastRecievedDmgElapsed,
            ItemContainers = containerData,
            Effects = EffectsData,
            CurrentLevelNumber = Storyline.CurrentLevelNumber,
            FailedTimes = Storyline.FailedTimes,
            CompletedTutorial = TutorialCompleted,
        };
        FileHelper.Save(dataPath, data.GetDict(), saveType);
    }
    public void Load(SaveType saveType)
    {
        PlayerData data = new();
        data.SetDict(FileHelper.Load<Dictionary<string, object>>(dataPath, saveType));
        if (data.Position.X != 0 && data.Position.Y != 0)
        {
            Position = data.Position;
            Level = data.Level;
            Xp = data.Xp;
            Hp = data.Hp;
            Coins = data.Coins;
            LevelUpStats = data.LevelUpStats;
            LastAttackElapsed = data.LastAttackElapsed;
            LastRecievedDmgElapsed = data.LastRecievedDmgElapsed;
            TutorialCompleted = data.CompletedTutorial;
            Storyline.CurrentLevelNumber = data.CurrentLevelNumber;
            Storyline.FailedTimes = data.FailedTimes;
            Storyline.Endless = data.Endless;
            // Restore Item Containers
            Inventory.ItemContainers.Clear();
            foreach (var itemData in data.ItemContainers)
            {
                if (!itemData.IsEmpty && itemData.ItemName != "null")
                {
                    var item = ItemDatabase.GetItemByName(itemData.ItemName);
                    Inventory.ItemContainers.Add(new ItemContainer() { Item = item, ContainerType = itemData.Type });
                }
                else
                {
                    Inventory.ItemContainers.Add(new ItemContainer() { ContainerType = itemData.Type });
                }
            }

            // Restore Effects
            Inventory.Effects.Clear();
            foreach (var effectData in data.Effects)
            {
                var effect = EffectDatabase.GetEffectByName(effectData.Name);
                effect.Level = effectData.Level;
                effect.Stats = effectData.Stats;
                Inventory.Effects.Add(effect);
            }
        }
    }
}
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public class PlayerData
{
    public int CurrentLevelNumber { get; set; }
    public int FailedTimes { get; set; }
    public bool Endless { get; set; }
    public int Level { get; set; }
    public float Xp { get; set; }
    public float Hp { get; set; }
    public int Coins { get; set; }
    public Dictionary<StatTypes, float> LevelUpStats { get; set; }
    public double LastAttackElapsed { get; set; }
    public double LastRecievedDmgElapsed { get; set; }
    public Vector2 Position { get; set; }
    public List<ItemContainerData> ItemContainers { get; set; }
    public List<EffectData> Effects { get; set; }
    public bool CompletedTutorial { get; set; }
    public PlayerData() { }

    public Dictionary<string, object> GetDict()
    {
        List<Dictionary<string, object>> eff = new();
        foreach (var e in Effects)
            eff.Add(e.Serialize());

        List<Dictionary<string, object>> con = new();
        foreach (var c in ItemContainers)
            con.Add(c.Serialize());

        Dictionary<string, object> dict = new()
        {
            { "cln", this.CurrentLevelNumber },
            { "ft", this.FailedTimes },
            { "endless", this.Endless },
            { "l", this.Level },
            { "x", this.Xp },
            { "h", this.Hp },
            { "coin", this.Coins },
            { "lus", this.LevelUpStats },
            { "lae", this.LastAttackElapsed },
            { "lrde", this.LastRecievedDmgElapsed },
            { "p", this.Position },
            { "tut", this.CompletedTutorial},
            { "con", con},
            { "ef", eff},
        };

        return dict;
    }

    public void SetDict(Dictionary<string, object> dict)
    {
        if (dict == null)
            return;

        // Use Convert.ToXxx to safely convert numeric types.
        if (dict.TryGetValue("cln", out object clnObj))
            this.CurrentLevelNumber = Convert.ToInt32(clnObj);

        if (dict.TryGetValue("ft", out object ftObj))
            this.FailedTimes = Convert.ToInt32(ftObj);

        if (dict.TryGetValue("l", out object lObj))
            this.Level = Convert.ToInt32(lObj);

        if (dict.TryGetValue("endless", out object endlessObj))
            this.Endless = Convert.ToBoolean(endlessObj);

        if (dict.TryGetValue("x", out object xObj))
            this.Xp = Convert.ToSingle(xObj);

        if (dict.TryGetValue("h", out object hObj))
            this.Hp = Convert.ToSingle(hObj);

        if (dict.TryGetValue("coin", out object coinObj))
            this.Coins = Convert.ToInt32(coinObj);

        if (dict.TryGetValue("lus", out object lusObj))
        {
#if DEBUG
            Console.WriteLine($"Raw lusObj type: {lusObj?.GetType()}");
#endif

            if (lusObj is JObject lusJObject)  // If stored as JObject, convert to dictionary
            {
                LevelUpStats = new Dictionary<StatTypes, float>();

                foreach (var kvp in lusJObject)
                {
#if DEBUG
                    Console.WriteLine($"Key: {kvp.Key} (Type: {kvp.Key.GetType()}), Value: {kvp.Value} (Type: {kvp.Value?.Type})");
#endif

                    if (Enum.TryParse(kvp.Key, out StatTypes statType))
                    {
                        if (kvp.Value.Type == JTokenType.Float)
                        {
                            LevelUpStats[statType] = kvp.Value.ToObject<float>();
                        }
                        else if (kvp.Value.Type == JTokenType.Integer)
                        {
                            LevelUpStats[statType] = kvp.Value.ToObject<int>();  // Convert int to float
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("lusObj is not a JObject.");
                LevelUpStats = new Dictionary<StatTypes, float>();  // Default empty dictionary
            }
        }
        else
        {
            Console.WriteLine("Key 'lus' not found in dictionary.");
        }


        if (dict.TryGetValue("lae", out object laeObj))
            this.LastAttackElapsed = Convert.ToDouble(laeObj);

        if (dict.TryGetValue("lrde", out object lrdeObj))
            this.LastRecievedDmgElapsed = Convert.ToDouble(lrdeObj);

        if (dict.TryGetValue("tut", out object tutObj))
            this.CompletedTutorial = Convert.ToBoolean(tutObj);

        if (dict.TryGetValue("p", out object pObj))
        {
#if DEBUG
            Console.WriteLine($"Raw pObj type: {pObj?.GetType()}");
#endif

            if (pObj is string pStr)
            {
                string[] parts = pStr.Split(',');
                if (parts.Length == 2 &&
                    float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float x) &&
                    float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float y))
                {
                    this.Position = new Vector2(x, y);
                }
                else
                {
                    Console.WriteLine("Failed to parse position string.");
                    this.Position = Vector2.Zero; // Default to (0,0) if invalid
                }
            }
            else
            {
                Console.WriteLine("pObj is not a valid string.");
                this.Position = Vector2.Zero;
            }
        }

        if (dict.TryGetValue("con", out object conObj))
        {
            if (conObj is JArray jArray)
            {
                this.ItemContainers = new();
                foreach (var item in jArray)
                {
                    var itemDict = item.ToObject<Dictionary<string, object>>();
                    if (itemDict != null)
                    {
                        ItemContainerData data = new ItemContainerData();
                        data.Deserialize(itemDict);
                        ItemContainers.Add(data);
                    }
                }
            }
            else
            {
                Debug.WriteLine("conObj is not a JArray");
            }
        }



        if (dict.TryGetValue("ef", out object efObj))
        {
            // Assuming Effects is stored as List<EffectData>
            this.Effects = new();
            if (efObj is JArray jArray)
            {
                foreach (var item in jArray)
                {
                    var itemDict = item.ToObject<Dictionary<string, object>>();
                    if (itemDict != null)
                    {
                        EffectData data = new EffectData();
                        data.Deserialize(itemDict);
                        Effects.Add(data);
                    }
                }
            }
            else
            {
                Debug.WriteLine("efObj is not a JArray");
            }
        }
    }

}

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public class ItemContainerData
{
    public bool IsEmpty { get; set; }
    public string ItemName { get; set; }
    public ItemTypes Type { get; set; }
    public ItemContainerData() { }

    public Dictionary<string, object> Serialize()
    {
        return new Dictionary<string, object>(){
            {"name", ItemName},
            {"type", Type},
            {"empty", IsEmpty}
        };
    }

    public void Deserialize(Dictionary<string, object> data)
    {
        if (data == null) return;

        if (data.TryGetValue("name", out object nameObj) && nameObj is string name)
        {
            this.ItemName = name != "null" ? name : string.Empty; // Ochrana proti "null" stringu
        }
        else
        {
            this.ItemName = string.Empty;
        }

        if (data.TryGetValue("type", out object typeObj) && typeObj is string typeStr)
        {
            if (Enum.TryParse(typeStr, true, out ItemTypes parsedType))  // 'true' allows case-insensitive matching
            {
                this.Type = parsedType;
            }
            else
            {
                Console.WriteLine($"Invalid value for 'type': {typeStr}");
                this.Type = default;  // Default to the first enum value or handle it as needed
            }
        }
        else
        {
            Console.WriteLine("'type' not found or invalid.");
            this.Type = default;  // Default to the first enum value or handle it as needed
        }


        if (data.TryGetValue("empty", out object emptyObj) && emptyObj is bool empty)
        {
            this.IsEmpty = empty;
        }
    }

}

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public class EffectData
{
    public string Name { get; set; }
    public int Level { get; set; }
    public Dictionary<StatTypes, float> Stats { get; set; } = new();
    public EffectData() { }
    public Dictionary<string, object> Serialize()
    {
        return new Dictionary<string, object>(){
            {"name", Name},
            {"level", Level},
            {"stats", Stats}
        };
    }

    public void Deserialize(Dictionary<string, object> dict)
    {
        if (dict == null) return;

        if (dict.TryGetValue("name", out object nameObj) && nameObj is string name)
        {
            Name = name;
        }
        else
        {
            Name = string.Empty; // Default to empty string if missing or invalid
        }

        if (dict.TryGetValue("level", out object levelObj))
        {
            Level = Convert.ToInt32(levelObj);
        }
        else
        {
            Level = 0; // Default to 0 if missing or invalid
        }

		if (dict.TryGetValue("stats", out object statsObj) && statsObj is Newtonsoft.Json.Linq.JObject jObject)
		{
			var rawStats = jObject.ToObject<Dictionary<string, float>>();
			if (rawStats != null)
			{
				Stats = new Dictionary<StatTypes, float>();
				foreach (var kvp in rawStats)
				{
					if (Enum.TryParse(kvp.Key, out StatTypes key))
					{
						Stats[key] = kvp.Value;
					}
				}
			}
		}
		else
		{
			Stats = new Dictionary<StatTypes, float>();
		}

	}
}