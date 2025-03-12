using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace TBoGV;

class ItemRubberBoots : ItemContainerable
{
	static Texture2D Sprite;
	public ItemRubberBoots(Vector2 position)
	{
		Rarity = 1;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Gumáky";
		Description = "Snad to nějak okecáš, ale radši nemluv se školníkem.";
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.MOVEMENT_SPEED, 4 }, { StatTypes.XP_GAIN, -3 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("gumaky");
		ItemType = ItemTypes.ARMOR;
	}
	public ItemRubberBoots() : this(Vector2.Zero) { }
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
		return new ItemRubberBoots();
	}

}

