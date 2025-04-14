using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace TBoGV;

class ItemMagnet : ItemContainerable
{
	static Texture2D Sprite;
	public ItemMagnet(Vector2 position)
	{
		Rarity = 1;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Magnet z fyzikální laboratoře";
		Description = "Magnet fishing? Přitahuje mince.";
		Stats = new Dictionary<StatTypes, float>() { };
		Effects = new List<EffectTypes>() { EffectTypes.MAGNET };
		Sprite = TextureManager.GetTexture("magnet");
		ItemType = ItemTypes.EFFECT;
	}
	public ItemMagnet() : this(Vector2.Zero) { }
	public override Texture2D GetSprite()
	{
		return Sprite;
	}
	public override ItemContainerable Clone()
	{
		return new ItemMagnet();
	}
}

