﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace TBoGV;

class ItemFancyShoes : ItemContainerable
{
	static Texture2D Sprite;
	public ItemFancyShoes(Vector2 position)
	{
		Rarity = 1;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Společenské boty";
		Description = "Florian style. Dej si pozor, ať to nevidí Fišerová";
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.MOVEMENT_SPEED, 1 }, { StatTypes.XP_GAIN, 3 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("fancyShoes");
		ItemType = ItemTypes.ARMOR;
	}
	public ItemFancyShoes() : this(Vector2.Zero) { }
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
        return new ItemFancyShoes();
    }
}

