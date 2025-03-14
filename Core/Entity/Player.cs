﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
					subjectValue = BaseStats[item.Key] * (1 - item.Value * 0.025f);
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
		if (ReadyToAttack() && mouseState.LeftButton == ButtonState.Pressed)
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

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite,
			new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)),
			LastRecievedDmgElapsed >= InvulnerabilityFrame ? Color.White : Color.DarkRed);
		spriteBatch.Draw(TextureManager.GetTexture("projectile"), InteractionPoint, Color.White);
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
		if (Inventory.GetEffect().Contains(EffectTypes.LIFE_STEAL))
		{
			Heal(0.5f);
			var existingEffect = Inventory.Effects.FirstOrDefault(effect => effect is EffectCooked);
			if (existingEffect != null)
				Inventory.AddEffect(new EffectCooked(-1));
		}
	}
	public int XpForLevel()
	{
		return 5 + Level * 2;
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
			{ StatTypes.MOVEMENT_SPEED, 5 }
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
		LastRecievedDmgElapsed = InvulnerabilityFrame;
		// fuj
		Position = new Lobby(this).SpawnPos * 50;

	}
	public class PlayerData
	{
		public int CurrentLevelNumber { get; set; }
		public int Difficulty { get; set; }
		public int Level { get; set; }
		public float Xp { get; set; }
		public float Hp { get; set; }
		public int Coins { get; set; }
		public Dictionary<string, float> LevelUpStats { get; set; }
		public double LastAttackElapsed { get; set; }
		public double LastRecievedDmgElapsed { get; set; }
		public float[] Position { get; set; }
		public List<ItemContainerData> ItemContainers { get; set; }
		public List<EffectData> Effects { get; set; }
		public PlayerData() { }
	}

	public class ItemContainerData
	{
		public bool IsEmpty { get; set; }
		public string ItemName { get; set; }
		public ItemTypes Type { get; set; }
		public ItemContainerData() { }
	}

	public class EffectData
	{
		public string Name { get; set; }
		public int Level { get; set; }
		public Dictionary<string, float> Stats { get; set; } = new();
		public EffectData() { }
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
			EffectsData.Add(new EffectData { Level = e.Level, Name = e.Name, Stats = StatConverter.ConvertToStringDictionary(e.Stats) });

		PlayerData data = new PlayerData
		{
			Position = new float[2] { Position.X, Position.Y },
			Level = Level,
			Xp = Xp,
			Hp = Hp,
			Coins = Coins,
			LevelUpStats = StatConverter.ConvertToStringDictionary(LevelUpStats),
			LastAttackElapsed = LastAttackElapsed,
			LastRecievedDmgElapsed = LastRecievedDmgElapsed,
			ItemContainers = containerData,
			Effects = EffectsData,
			CurrentLevelNumber = Storyline.CurrentLevelNumber,
			Difficulty = Storyline.Difficulty,
		};
		FileHelper.Save("tbogv_player.json", data, saveType);
	}
	public void Load(SaveType saveType)
	{
		PlayerData data = FileHelper.Load<PlayerData>("tbogv_player.json", saveType);
		if (data != null)
		{
			Position = new Vector2(data.Position[0], data.Position[1]);
			Level = data.Level;
			Xp = data.Xp;
			Hp = data.Hp;
			Coins = data.Coins;
			LevelUpStats = StatConverter.ConvertToStatDictionary(data.LevelUpStats);
			LastAttackElapsed = data.LastAttackElapsed;
			LastRecievedDmgElapsed = data.LastRecievedDmgElapsed;
			Storyline.CurrentLevelNumber = data.CurrentLevelNumber;
			Storyline.Difficulty = data.Difficulty;
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
				effect.Stats = StatConverter.ConvertToStatDictionary(effectData.Stats);
				Inventory.Effects.Add(effect);
			}
		}
	}
}

