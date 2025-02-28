﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace TBoGV;

class ItemScissors : ItemContainerable
{
	static Texture2D Sprite;
	public ItemScissors(Vector2 position)
	{
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Nuzky";
		Description = "Jestli si je zapomeneš, Jirušová tě nechá propadnout";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.DAMAGE, 1 }, { StatTypes.ATTACK_SPEED, 100 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("scissors");
		ItemType = ItemTypes.WEAPON;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}
}
