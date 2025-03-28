using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemBookBio : ItemContainerable
{
	static Texture2D Sprite;
	public ItemBookBio(Vector2 position)
	{
		Rarity = 3;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Učebnice biologie";
		Description = "Když počet stránek nahradí hmotnost knihy." +
			"Enjoy";
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.MAX_HP, 6 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("bookBio");
		ItemType = ItemTypes.BASIC;
	}
	public ItemBookBio() : this(Vector2.Zero) { }
	public override Texture2D GetSprite()
	{
		return Sprite;
	}
    public override ItemContainerable Clone()
    {
        return new ItemBookBio();
    }
}



