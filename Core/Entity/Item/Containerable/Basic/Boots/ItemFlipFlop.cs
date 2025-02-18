using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace TBoGV;

internal class ItemFlipFlop: ItemContainerable
{
	static Texture2D Sprite;
	public ItemFlipFlop(Vector2 position)
	{
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Crocs";
		Description = "Legendary tier prezuvky";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.MOVEMENT_SPEED, 3 }};
		Effects = new List<EffectTypes>() { EffectTypes.BOOTS };
		Sprite = TextureManager.GetTexture("crocs");
		ItemType = ItemTypes.ARMOR;
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

