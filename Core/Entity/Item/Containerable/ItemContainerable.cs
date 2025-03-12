﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TBoGV;

public abstract class ItemContainerable : Item
{
	public string Name { get; set; }
	public string Description { get; set; }
	public int Rarity { get; set; }
	// rarity 1 - useless
	// rarity 2 - common
	// rarity 3 - epic
	// rarity 4 - legendary
	public ItemTypes ItemType { get; set; }
	public Dictionary<StatTypes, float> Stats { get; set; }
	public List<EffectTypes> Effects { get; set; }
	public bool IsKnown = true;
	public override void Interact(Entity e, Place p)
	{
		ItemContainerable itemToDrop = null;
		if (!p.player.Inventory.PickUpItem(this))
			itemToDrop = (p.player.Inventory.SwapItem(this));
		float hp = p.player.Hp;
		p.player.SetStats();
		if (p.player.MaxHp <= 0)
		{
			p.player.Inventory.AddEffect(new EffectCloseCall());
			p.player.Inventory.RemoveItem(this);
			if (itemToDrop != null)
				p.player.Inventory.PickUpItem(itemToDrop);
			p.player.Hp = hp;
			p.player.SetStats();
			p.player.Drop(this);
			InitMovement();
		}
		else
		{
			if (itemToDrop != null)
			{
				itemToDrop.InitMovement();
				p.player.Drop(itemToDrop);
			}
		}
	}
	public virtual int GetCost()
	{
		int rarityValue = GetRarityValue();
		return new Random().Next((int)(rarityValue * 0.6f), (int)(rarityValue * 1.3));
	}
	public virtual int GetRarityValue()
	{
		return 3 + Rarity * 7;
	}
	public abstract ItemContainerable Clone();
}

public enum StatTypes : int
{
	MAX_HP = 0,             // Biologie
	DAMAGE = 1,             // Matematika
	PROJECTILE_COUNT = 2,   // Fyzika
	XP_GAIN = 3,            // Zsv
	ATTACK_SPEED = 5,       // Cestina
	MOVEMENT_SPEED = 6,     // Telocvik
}
public enum EffectTypes : int
{
	MAP_REVEAL = 0,
	BOOTS = 1,
	LIFE_STEAL = 2,
	PIERCING = 3,
	EXPLOSIVE = 4,
	DODGE = 5,
	AIM = 6,
	ROOTED = 7,
	RICKROLL = 8,
	EXPENSIVE = 9,
}
public enum ItemTypes : int
{
	EFFECT = 0,
	WEAPON = 1,
	ARMOR = 2,
	BASIC = 3,
}
public static class StatConverter
{
	private static readonly Dictionary<string, StatTypes> stringToStatMap = new()
	{
		{ "MAX_HP", StatTypes.MAX_HP },
		{ "DAMAGE", StatTypes.DAMAGE },
		{ "PROJECTILE_COUNT", StatTypes.PROJECTILE_COUNT },
		{ "XP_GAIN", StatTypes.XP_GAIN },
		{ "ATTACK_SPEED", StatTypes.ATTACK_SPEED },
		{ "MOVEMENT_SPEED", StatTypes.MOVEMENT_SPEED }
	};
	private static readonly Dictionary<StatTypes, string> statToStringMap = new()
	{
		{ StatTypes.MAX_HP, "MAX_HP" },
		{ StatTypes.DAMAGE, "DAMAGE" },
		{ StatTypes.PROJECTILE_COUNT, "PROJECTILE_COUNT" },
		{ StatTypes.XP_GAIN, "XP_GAIN" },
		{ StatTypes.ATTACK_SPEED, "ATTACK_SPEED" },
		{ StatTypes.MOVEMENT_SPEED, "MOVEMENT_SPEED" }
	};
	public static Dictionary<StatTypes, float> ConvertToStatDictionary(Dictionary<string, float> input)
	{
		Dictionary<StatTypes, float> result = new();

		foreach (var pair in input)
		{
			if (stringToStatMap.TryGetValue(pair.Key, out StatTypes statType))
			{
				result[statType] = pair.Value;
			}
		}

		return result;
	}
	public static Dictionary<string, float> ConvertToStringDictionary(Dictionary<StatTypes, float> input)
	{
		Dictionary<string, float> result = new();

		foreach (var pair in input)
		{
			if (statToStringMap.TryGetValue(pair.Key, out string statName))
			{
				result[statName] = pair.Value;
			}
		}

		return result;
	}
}
public static class ItemDatabase
{
	private static readonly Dictionary<string, ItemContainerable> ItemsByName = new Dictionary<string, ItemContainerable>();

	static ItemDatabase()
	{
		var itemsList = new ItemContainerable[]
		{
			new ItemCalculator(),
			new ItemDoping(),
			new ItemMonster(),
			new ItemFancyShoes(),
			new ItemFlipFlop(),
			new ItemTrackShoes(),
			new ItemMap(),
			new ItemMathProblem(),
			new ItemTeeth(),
			new ItemAdBlock(),
			new ItemExplosive(),
			new ItemDagger(),
			new ItemPencil(),
			new ItemBookBio(),
			new ItemBookCzech(),
			new ItemBookMath(),
			new ItemBookZsv(),
			new ItemBookPhysics(),
			new ItemCross(),
			new ItemBryle(),
			new ItemBook(),
			new ItemBookPE(),
			new ItemPen(),
			new ItemScissors(),
			new ItemRubberBoots(),
			new ItemFixa(),
			new ItemLabcoat(),
			new ItemPencil(),
			new ItemRuler(),
			new ItemSesitCj(),
			new ItemSesitFyzika(),
			new ItemSesitMatika(),
			new ItemSesitModry(),
			new ItemSesitZeleny(),
			new ItemSesitZsv(),
			new ItemKruzitko(),
			new ItemOrezavatko(),
		};

		foreach (var item in itemsList)
		{
			ItemsByName[item.Name] = item;
		}
	}

	public static ItemContainerable GetItemByName(string name)
	{
		return ItemsByName.TryGetValue(name, out var item) ? (ItemContainerable)Activator.CreateInstance(item.GetType()) : null;
	}
	public static List<ItemContainerable> GetAllItems()
	{
		return ItemsByName.Values.ToList();
	}

}
