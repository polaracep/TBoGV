﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace TBoGV;

class ItemDagger : ItemContainerable
{
	static Texture2D Sprite;
	public ItemDagger(Vector2 position)
	{
		Rarity = 3;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Nůž";
		Description = "Školní pomůcka londýnských studentů\nTenhle ale asi tolik efektivní nebude...\n(Je ze školní jídelny)";
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.DAMAGE, 1 }, { StatTypes.ATTACK_SPEED, 500 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("dagger");
		ItemType = ItemTypes.WEAPON;
	}
	public ItemDagger() : this(Vector2.Zero) { }
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
        return new ItemDagger();
    }
}
