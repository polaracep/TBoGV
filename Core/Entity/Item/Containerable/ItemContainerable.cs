﻿using System;
using System.Collections.Generic;
namespace TBoGV;

public abstract class ItemContainerable : Item
{
	public string Name { get; set; }
	public string Description { get; set; }
	public ItemTypes ItemType { get; set; }
	public Dictionary<StatTypes, int> Stats { get; set; }
	public List<EffectTypes> Effects { get; set; }
	public bool IsKnown = true;
	public override void Interact(Entity e, Room r)
	{
        if(!r.player.Inventory.PickUpItem(this))
			r.drops.Add(r.player.Inventory.SwapItem(this));
	}
}

public enum StatTypes : int
{
    MAX_HP = 0,				// Biologie
    DAMAGE = 1,				// Matematika
    PROJECTILE_COUNT = 2,	// Fyzika
    XP_GAIN = 3,			// Zsv
    ATTACK_SPEED = 5,		// Cestina
    MOVEMENT_SPEED = 6,		// Telocvik
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
}
public enum ItemTypes : int
{
	EFFECT = 0,
	WEAPON = 1,
	ARMOR = 2,
	BASIC = 3,
}
