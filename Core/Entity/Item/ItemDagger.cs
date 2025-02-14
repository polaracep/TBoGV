using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace TBoGV;

internal class ItemDagger : ItemContainerable
{
	static Texture2D Sprite;
	public ItemDagger(Vector2 position)
	{
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Dagger";
		Description = "stabs";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.DAMAGE, 1 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("dagger");
		ItemType = ItemTypes.WEAPON;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), Color.White);
	}

}
