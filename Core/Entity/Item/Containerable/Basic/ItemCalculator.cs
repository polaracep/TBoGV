using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

class ItemCalculator : ItemContainerable
{
    static Texture2D Sprite;
    public ItemCalculator(Vector2 position)
    {
		Rarity = 3;
		Position = position;
        Size = new Vector2(50, 50);
        Name = "Kalkulačka";
        Description = "Rychlá jako schovánkovo opravování testů";
        Stats = new Dictionary<StatTypes, float>() { { StatTypes.DAMAGE, 5 } };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("calculator");
        ItemType = ItemTypes.BASIC;
    }
	public ItemCalculator() : this(Vector2.Zero) { }
    public override Texture2D GetSprite()
    {
        return Sprite;
    }
    public override ItemContainerable Clone()
    {
        return new ItemCalculator();
    }
}

