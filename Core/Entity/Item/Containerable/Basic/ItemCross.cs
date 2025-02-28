using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemCross : ItemContainerable
{
	static Texture2D Sprite;
	public ItemCross(Vector2 position)
	{
		Rarity = 2;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Křesťanský kříž";
		Description = "Třeba tě ochrání";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.XP_GAIN, 4 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("kriz");
		ItemType = ItemTypes.BASIC;
	}
	public ItemCross() : this(Vector2.Zero) { }
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}

}



