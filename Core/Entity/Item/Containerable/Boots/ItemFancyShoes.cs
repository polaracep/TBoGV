using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace TBoGV;

internal class ItemFancyShoes : ItemContainerable
{
	static Texture2D Sprite;
	public ItemFancyShoes(Vector2 position)
	{
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Spolecenske boty";
		Description = "Florian style. Dej si pozor, at to nevidi Fiserova";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.MOVEMENT_SPEED, 1 }, { StatTypes.XP_GAIN, 3} };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("tile");
		ItemType = ItemTypes.ARMOR;
	}
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
	}
}

