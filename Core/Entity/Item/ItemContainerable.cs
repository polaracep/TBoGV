using System;
using System.Collections.Generic;
namespace TBoGV;

public abstract class ItemContainerable : Item, IInteractable
{
	public string Name { get; set; }
	public string Description { get; set; }
	public ItemTypes ItemType { get; set; }
	public Dictionary<StatTypes, int> Stats { get; set; }
	public List<EffectTypes> Effects { get; set; }
	public virtual void Interact(Entity e, Room r)
	{
        throw new NotImplementedException();
	}
}

public enum StatTypes : int
{
    MAX_HP = 0,
    DAMAGE = 1,
    PROJECTILE_COUNT = 2,
    XP_GAIN = 3,
    ATTACK_SPEED = 5,
    MOVEMENT_SPEED = 6,
}
public enum EffectTypes : int
{ 
    MAP_REVEAL = 0,
    ARMOR = 1,
	LIFE_STEAL = 2,
}
public enum ItemTypes : int
{
	EFFECT = 0,
	WEAPON = 1,
	ARMOR = 2,
	BASIC = 3,
}
