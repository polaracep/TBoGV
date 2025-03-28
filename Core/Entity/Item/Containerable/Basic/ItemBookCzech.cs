using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemBookCzech : ItemContainerable
{
	static Texture2D Sprite;
	public ItemBookCzech(Vector2 position)
	{
		Rarity = 2;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Učebnice češtiny";
		Description = "S tím ten didakťák musíš dát.";
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.ATTACK_SPEED, 4 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("bookCj");
		ItemType = ItemTypes.BASIC;
	}
	public ItemBookCzech() : this(Vector2.Zero) { }
	public override Texture2D GetSprite()
	{
		return Sprite;
	}
    public override ItemContainerable Clone()
    {
        return new ItemBookCzech();
    }
}



