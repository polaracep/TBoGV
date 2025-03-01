﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemBookPE : ItemContainerable
{
	static Texture2D Sprite;
	public ItemBookPE(Vector2 position)
	{
		Rarity = 2;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Učebnice tělocviku";
		Description = "Co sis to proboha koupil..";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.MOVEMENT_SPEED, 4 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("bookPE");
		ItemType = ItemTypes.BASIC;
	}
	public ItemBookPE() : this(Vector2.Zero) { }
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
        return new ItemBookPE();
    }
}



