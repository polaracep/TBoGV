using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
namespace TBoGV;
internal class ItemTeeth : ItemContainerable
{
	static Texture2D Sprite;
	public ItemTeeth(Vector2 position) 
	{
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Teeth of vampirism";
		Description = "Heal using your foes, at a cost..";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.DAMAGE, 1 }, { StatTypes.MAX_HP, -6 } };
		Effects = new List<EffectTypes>() { EffectTypes.LIFE_STEAL};
		Sprite = TextureManager.GetTexture("teeth");
		ItemType = ItemTypes.EFFECT;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), Color.White);
	}
}

