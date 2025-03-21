﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace TBoGV;

class ItemFlipFlop : ItemContainerable
{
	static Texture2D Sprite;
	public ItemFlipFlop(Vector2 position)
	{
		Rarity = 3;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Crocs";
		Description = "Legendary tier přezůvky";
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.MOVEMENT_SPEED, 6 }, { StatTypes.ATTACK_SPEED, 1 }, { StatTypes.PROJECTILE_COUNT, 1 }, { StatTypes.XP_GAIN, 1 }, { StatTypes.DAMAGE, 1 }, { StatTypes.MAX_HP, 1 } };
		Effects = new List<EffectTypes>() { EffectTypes.BOOTS };
		Sprite = TextureManager.GetTexture("crocs");
		ItemType = ItemTypes.ARMOR;
	}
	public ItemFlipFlop() : this(Vector2.Zero) { }
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}
    public override ItemContainerable Clone()
    {
        return new ItemFlipFlop();
    }

}

