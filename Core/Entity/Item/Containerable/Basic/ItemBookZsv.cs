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
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Filozoficky text";
		Description = ",,Pokud věříš v Boha a on existuje, získáváš nekonečnou odměnu. \nPokud věříš a on neexistuje, nic neztrácíš. Pokud nevěříš a on existuje, \nriskuješ nekonečné utrpení. Proto je racionální v Boha věřit.\"";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.XP_GAIN, 6 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("bookZsv");
		ItemType = ItemTypes.BASIC;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}

}



