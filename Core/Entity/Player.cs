using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;

public class Player : Entity, IRecieveDmg, IDealDmg, IDraw
{
	static Texture2D Sprite;
	public int Level { get; set; }
	public float Xp { get; set; }
	public float AttackSpeed { get; set; }
	public float AttackDmg { get; set; }
	public int ItemCapacity { get; set; }
	public float Hp { get; set; }
	public int MaxHp { get; set; }
	public float XpGain { get; set; }
    public int ProjectileCount { get; set; }
    public int Coins { get; set; }
	public Dictionary<StatTypes, float> BaseStats { get; set; }
	public Dictionary<StatTypes, int> LevelUpStats { get; set; }
    public DateTime LastAttackTime { get; set; }
	public DateTime LastRecievedDmgTime { get; set; }
	public int InvulnerabilityFrame = 1000;
	public List<Projectile> Projectiles { get; set; }
	public Inventory Inventory { get; set; }
    private MouseState previousMouseState; 
    public Player(Vector2 position)
	{
		BaseStats = new Dictionary<StatTypes, float>()
		{
			{ StatTypes.MAX_HP, 18 },         
			{ StatTypes.DAMAGE, 1 },          
			{ StatTypes.PROJECTILE_COUNT, 5 }, 
			{ StatTypes.XP_GAIN, 1 },        
			{ StatTypes.ATTACK_SPEED, 1500 },   
			{ StatTypes.MOVEMENT_SPEED, 2 }    
		};
        LevelUpStats = new Dictionary<StatTypes, int>()
        {
            { StatTypes.MAX_HP, 0 },
            { StatTypes.DAMAGE, 0 },
            { StatTypes.PROJECTILE_COUNT, 0 },
            { StatTypes.XP_GAIN, 0 },        
			{ StatTypes.ATTACK_SPEED, 0 },
            { StatTypes.MOVEMENT_SPEED, 0 }
        };
        Hp = 9;
		XpGain = 1;
		Position = position;
		Size = new Vector2(50, 50);
		Projectiles = new List<Projectile>();		
		Sprite = TextureManager.GetTexture("vitekElegan");
		Coins = 1;
		ItemCapacity = 3;
		Inventory = new();
		SetStats();
	}

	public Player() : this(Vector2.One) { }

	Vector2 InteractionPoint = Vector2.Zero;
	public void SetStats()
	{
		BaseStats[StatTypes.DAMAGE] = Inventory.GetWeaponDmg();
        BaseStats[StatTypes.ATTACK_SPEED] = Inventory.GetWeaponAttackSpeed();
        Dictionary<StatTypes, float> finalStats = new Dictionary<StatTypes, float>();
        Dictionary<StatTypes, int> subjectStats = Inventory.SetStats(LevelUpStats);
		foreach (var item in subjectStats)
		{
			float subjectValue; 
			switch (item.Key)
			{
				case StatTypes.MAX_HP:
					subjectValue = (int)item.Value * 0.5f + BaseStats[item.Key];
					break;
				case StatTypes.DAMAGE:
                    subjectValue = ((item.Value * 0.5f) + 1) * BaseStats[item.Key];
                    break;
				case StatTypes.PROJECTILE_COUNT:
                    subjectValue = (int)item.Value * 1/3 + BaseStats[item.Key];
                    break;
				case StatTypes.XP_GAIN:
                    subjectValue = ((item.Value * 0.1f) + 1) * BaseStats[item.Key];
                    break;
				case StatTypes.ATTACK_SPEED:
                    subjectValue = BaseStats[item.Key] * (1 - item.Value * 0.025f);
                    break;
				case StatTypes.MOVEMENT_SPEED:
                    subjectValue = ((item.Value * 0.1f) + 1) * BaseStats[item.Key];
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
		MovementSpeed = (int)finalStats[StatTypes.MOVEMENT_SPEED];
        XpGain = finalStats[StatTypes.XP_GAIN];
        ProjectileCount = (int)finalStats[StatTypes.PROJECTILE_COUNT];
    }

	public void Update(KeyboardState keyboardState, MouseState mouseState, Matrix transform, Room room, Viewport viewport)
	{
		int dx = 0, dy = 0;

		InteractionPoint = Position + (Direction * 50) + Size / 2;

		if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
		{
			dx -= MovementSpeed;
		}
		if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
		{
			dx += MovementSpeed;
		}
		if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
		{
			dy -= MovementSpeed;
		}
		if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
		{
			dy += MovementSpeed;
		}
		if (previousMouseState.RightButton == ButtonState.Pressed &&
                                    mouseState.RightButton == ButtonState.Released)
        {
			Tile t = room.GetTileInteractable(InteractionPoint);
			if (t != null)
			{
				IInteractable tile = (IInteractable)t;
				tile.Interact(this, room);
			}
            Item item = room.GetItemInteractable(InteractionPoint);
            if (item != null)
            {
                item.Interact(this, room);
                if (item is not ItemContainerable)
                    room.RemoveItem(item);
            }
        }
		for (int i = 0; i < room.drops.Count; i++)
		{
			if (room.drops[i] is not ItemContainerable && ObjectCollision.CircleCircleCollision(room.drops[i],this))
			{
                room.drops[i].Interact(this, room);
                room.RemoveItem(room.drops[i]);
            }
		}

		Inventory.Update(viewport, this, mouseState);
		/* === */
		int tolerance = 1;
        Vector2 newPosition = Position;
		if (dx != 0)
		{
			newPosition.X += dx;
			if (!room.ShouldCollideAt(new Vector2(newPosition.X+ tolerance, newPosition.Y+tolerance)) && 
				!room.ShouldCollideAt(new Vector2(newPosition.X- tolerance + Size.X, newPosition.Y - tolerance + Size.Y)) && 
				!room.ShouldCollideAt(new Vector2(newPosition.X- tolerance + Size.X, newPosition.Y+ tolerance)) && 
				!room.ShouldCollideAt(new Vector2(newPosition.X+ tolerance, newPosition.Y- tolerance + Size.Y)))
				Position.X = newPosition.X;
		}
		newPosition = Position;
		// Try moving on the Y-axis next
		if (dy != 0)
		{
			newPosition.Y += dy;
            if (!room.ShouldCollideAt(new Vector2(newPosition.X + tolerance, newPosition.Y + tolerance)) &&
				!room.ShouldCollideAt(new Vector2(newPosition.X - tolerance + Size.X, newPosition.Y - tolerance + Size.Y)) &&
				!room.ShouldCollideAt(new Vector2(newPosition.X - tolerance + Size.X, newPosition.Y + tolerance)) &&
				!room.ShouldCollideAt(new Vector2(newPosition.X + tolerance, newPosition.Y - tolerance + Size.Y))) 
				Position.Y = newPosition.Y;
		}
		Vector2 screenMousePos = new Vector2(mouseState.X, mouseState.Y);
		Vector2 worldMousePos = Vector2.Transform(screenMousePos, Matrix.Invert(transform));

		Vector2 direction = worldMousePos - Position - Size / 2;

		if (!float.IsNaN(direction.X) && !float.IsNaN(direction.Y))
		{
			direction.Normalize(); // Normalize to get unit direction vector
			Direction = direction;
		}
		if (ReadyToAttack() && mouseState.LeftButton == ButtonState.Pressed)
		{
			foreach (var projectile in Attack())
			{
                Projectiles.Add(projectile);
            }
		}

		SetStats();
        previousMouseState = mouseState;
    }

    public void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite,
			new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)),
			(DateTime.UtcNow - LastRecievedDmgTime).TotalMilliseconds >= InvulnerabilityFrame ? Color.White : Color.DarkRed);
		spriteBatch.Draw(TextureManager.GetTexture("projectile"), InteractionPoint, Color.White);
	}
	public bool ReadyToAttack()
	{
		return (DateTime.UtcNow - LastAttackTime).TotalMilliseconds >= AttackSpeed;
	}
    public List<Projectile> Attack()
    {
        LastAttackTime = DateTime.UtcNow;
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

            Projectile projectile = new ProjectilePee(Position + Size / 2, rotatedDirection, AttackDmg);
            projectile.ShotByPlayer = true;
            firedProjectiles.Add(projectile);
        }

        return firedProjectiles;
    }

    public void RecieveDmg(float damage)
	{
		if ((DateTime.UtcNow - LastRecievedDmgTime).TotalMilliseconds >= InvulnerabilityFrame)
		{
			Hp -= damage;

			LastRecievedDmgTime = DateTime.UtcNow;
		}

	}
	public void Kill(int xpGain)
	{
		Xp += xpGain * XpGain;
		if(Xp >= XpForLevel())
		{
			LevelUp();
		}
	}
	public int XpForLevel()
	{
		return 5 + Level*2;
	}
	private void LevelUp()
	{
		Level += 1;
		Xp = 0;
	}
	public void Heal(uint healAmount)
	{
		if (Hp < MaxHp)
		{
			Hp += (int)healAmount;
		}
	}

}

