﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
namespace TBoGV;
internal class ItemMathProblem : ItemContainerable
{
	static Texture2D Sprite;
	public ItemMathProblem(Vector2 position)
	{
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Slozity matematicky problem";
		Description = "Vyzaduje prilisne soustredeni, sance ignorace brainrotu.";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.DAMAGE, 4 }};
		Effects = new List<EffectTypes>() { EffectTypes.DODGE };
		Sprite = TextureManager.GetTexture("mathProblem");
		ItemType = ItemTypes.EFFECT;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
	}
}

