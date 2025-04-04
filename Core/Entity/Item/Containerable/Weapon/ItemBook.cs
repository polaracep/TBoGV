﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemBook : ItemContainerable
{
	static Texture2D Sprite;
	public ItemBook(Vector2 position)
	{
		Rarity = 3;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Neumělecká literatura";
		Description = "Moc se nedovíš, ale jako zbraň dobrá";
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.DAMAGE, 3 }, { StatTypes.ATTACK_SPEED, 1400 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("bookWeapon");
		ItemType = ItemTypes.WEAPON;
	}
	public ItemBook() : this(Vector2.Zero) { }
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
        return new ItemBook();
    }
}



