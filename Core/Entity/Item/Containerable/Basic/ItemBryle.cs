using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemBryle : ItemContainerable
{
	static Texture2D Sprite;
	public ItemBryle(Vector2 position)
	{
		Rarity = 1;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Brýle";
		Description = "Cítíš se chytřeji";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.XP_GAIN, 2 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("bryle");
		ItemType = ItemTypes.BASIC;
	}
	public ItemBryle() : this(Vector2.Zero) { }
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}
    public override ItemContainerable Clone()
    {
        return new ItemBryle();
    }
}



