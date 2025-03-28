using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace TBoGV;

class ItemMap : ItemContainerable
{
	static Texture2D Sprite;
	public ItemMap(Vector2 position)
	{
		Rarity = 1;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Gymvod plánek";
		Description = "S ním se neztratíš. \n(minimapu otevřeš M)\n";
		Stats = new Dictionary<StatTypes, float>() { };
		Effects = new List<EffectTypes>() { EffectTypes.MAP_REVEAL };
		Sprite = TextureManager.GetTexture("gymvodMap");
		ItemType = ItemTypes.EFFECT;
	}
	public ItemMap() : this(Vector2.Zero) { }
	public override Texture2D GetSprite()
	{
		return Sprite;
	}
    public override ItemContainerable Clone()
    {
        return new ItemMap();
    }
}

