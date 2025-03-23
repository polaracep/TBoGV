using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TBoGV;
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
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
		switch (Rarity)
		{
			case 1: return 12;
			case 2: return 25;
			case 3: return 45;
			case 4: return 70;
			default:
				return 0;
		}
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
		ItemsByName[new ItemVysvedceni().Name] = new ItemVysvedceni();
    }
	public static ItemContainerable GetItemByName(string name)
	{
		return ItemsByName.TryGetValue(name, out var item) ? item.Clone() : null;
	}
	public static List<ItemContainerable> GetAllItems()
	{
		return ItemsByName.Values.ToList();
	}
}
