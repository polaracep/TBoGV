﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
namespace TBoGV;
class ItemMonster : ItemContainerable
{
	static Texture2D Sprite;
	public ItemMonster(Vector2 position)
	{
		Rarity = 3;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Monster";
		Description = "Asi bys to neměl přehánět";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.MOVEMENT_SPEED, -10 }, { StatTypes.DAMAGE, 3 }, { StatTypes.ATTACK_SPEED, 3 }, { StatTypes.XP_GAIN, 3 }, { StatTypes.PROJECTILE_COUNT, 3 }, { StatTypes.MAX_HP, 3 } };
		Effects = new List<EffectTypes>() { };
		Sprite = TextureManager.GetTexture("monster");
		ItemType = ItemTypes.BASIC;
	}
	public ItemMonster() : this(Vector2.Zero) { }
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}

}

