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
		Rarity = 2;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Učebnice biologie";
		Description = "Když počet stránek nahradí hmotnost knihy." +
			"Enjoy";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.MAX_HP, 6 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("bookBio");
		ItemType = ItemTypes.BASIC;
	}
	public ItemBookBio() : this(Vector2.Zero) { }

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}

}



