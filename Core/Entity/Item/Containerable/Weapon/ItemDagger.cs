using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace TBoGV;

class ItemDagger : ItemContainerable
{
	static Texture2D Sprite;
	public ItemDagger(Vector2 position)
	{
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Nuz";
		Description = "skolni pomucka Londynskych studentu\nTenhle ale asi tolik efektivni nebude...\n(Je ze skolni jidelny)";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.DAMAGE, 1 }, { StatTypes.ATTACK_SPEED, 100 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("dagger");
		ItemType = ItemTypes.WEAPON;
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
