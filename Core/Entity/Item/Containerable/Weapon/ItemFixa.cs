using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

class ItemFixa : ItemContainerable
{
	static Texture2D Sprite;
	public ItemFixa(Vector2 position)
	{
		Rarity = 4;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Ukradená fialová fixa";
		Description = "Hlavně ať to nezjistí Schovánek. \nNepřeháněj to, ať ti chvíli vydrží.";
		Stats = new Dictionary<StatTypes, int>() { { StatTypes.DAMAGE, 5 }, { StatTypes.ATTACK_SPEED, 1150 }, { StatTypes.MAX_HP, -2 }};
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("fialovaFixa");
		ItemType = ItemTypes.WEAPON;
	}
	public ItemFixa() : this(Vector2.Zero) { }
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
        return new ItemFixa();
    }
}



