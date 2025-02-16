using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;


namespace TBoGV;

internal class ItemMap : ItemContainerable
{
	static Texture2D Sprite;
	public ItemMap(Vector2 position)
	{
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Gymvod planek";
		Description = "S nim se neztratis. (minimapu otevres M)";
		Stats = new Dictionary<StatTypes, int>() { };
		Effects = new List<EffectTypes>() { EffectTypes.MAP_REVEAL };
		Sprite = TextureManager.GetTexture("gymvodMap");
		ItemType = ItemTypes.EFFECT;
	}
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
	}
}

