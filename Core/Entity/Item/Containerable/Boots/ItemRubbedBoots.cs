using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace TBoGV;

class ItemRubbedBoots : ItemContainerable
{
	static Texture2D Sprite;
	public ItemRubbedBoots(Vector2 position)
	{
		Rarity = 1;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Gumáky";
		Description = "Snad to nějak okecáš, ale radši nemluv se školníkem.";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.MOVEMENT_SPEED, 4 }, { StatTypes.XP_GAIN, -3 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("gumaky");
		ItemType = ItemTypes.ARMOR;
	}
	public ItemRubbedBoots() : this(Vector2.Zero) { }
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
        return new ItemRubbedBoots();
    }

}

