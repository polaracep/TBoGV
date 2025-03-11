using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

class ItemPen : ItemContainerable
{
	static Texture2D Sprite;
	public ItemPen(Vector2 position)
	{
		Rarity = 2;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Propiska";
		Description = "Nerýsuj s tím proboha";
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.DAMAGE, 2 }, { StatTypes.ATTACK_SPEED, 1212 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("pen");
		ItemType = ItemTypes.WEAPON;
	}
	public ItemPen() : this(Vector2.Zero) { }
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
        return new ItemPen();
    }
}



