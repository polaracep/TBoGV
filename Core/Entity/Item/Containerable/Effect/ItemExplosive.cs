﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
namespace TBoGV;
internal class ItemExplosive : ItemContainerable
{
	static Texture2D Sprite;
	public ItemExplosive(Vector2 position)
	{
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Lorentzova transformace";
		Description = "O tomhle nam Schovanek nerekl";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.DAMAGE, -6 }};
		Effects = new List<EffectTypes>() { EffectTypes.EXPLOSIVE };
		Sprite = TextureManager.GetTexture("lorentzovaTransformace");
		ItemType = ItemTypes.EFFECT;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
	}
}


