using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemBookZsv : ItemContainerable
{
	static Texture2D Sprite;
	public ItemBookZsv(Vector2 position)
	{
		Rarity = 3;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Filozoficky text";
		Description = ",,Pokud věříš v Boha a on existuje, získáváš nekonečnou odměnu. \nPokud věříš a on neexistuje, nic neztrácíš. Pokud nevěříš a on existuje, \nriskuješ nekonečné utrpení. Proto je racionální v Boha věřit.\"";
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.XP_GAIN, 8 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("bookZsv");
		ItemType = ItemTypes.BASIC;
	}
	public ItemBookZsv() : this(Vector2.Zero) { }
	public override Texture2D GetSprite()
	{
		return Sprite;
	}
    public override ItemContainerable Clone()
    {
        return new ItemBookZsv();
    }
}



