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
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Petakova";
		Description = "Mel by jsi se pomodlit";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.DAMAGE, 8 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("bookPetakova");
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



