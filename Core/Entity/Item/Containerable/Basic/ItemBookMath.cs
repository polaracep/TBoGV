using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemBookMath : ItemContainerable
{
	static Texture2D Sprite;
	public ItemBookMath(Vector2 position)
	{
		Rarity = 4;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Petáková";
		Description = "Měl by jsi se pomodlit";
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.DAMAGE, 12 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("bookPetakova");
		ItemType = ItemTypes.BASIC;
	}
	public ItemBookMath() : this(Vector2.Zero) { }
	public override Texture2D GetSprite()
	{
		return Sprite;
	}
    public override ItemContainerable Clone()
    {
        return new ItemBookMath();
    }
}



