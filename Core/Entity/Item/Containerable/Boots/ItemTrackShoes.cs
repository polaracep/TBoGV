using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace TBoGV;

internal class ItemTrackShoes : ItemContainerable
{
	static Texture2D Sprite;
	public ItemTrackShoes(Vector2 position)
	{
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Vojenske trekovky";
		Description = "Tohle asi nebudou prezuvky...";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.MOVEMENT_SPEED, 5 }};
		Effects = new List<EffectTypes>() { };
		Sprite = TextureManager.GetTexture("tile");
		ItemType = ItemTypes.ARMOR;
	}
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
	}
}

