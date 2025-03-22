using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemBookPhysics : ItemContainerable
{
	static Texture2D Sprite;
	public ItemBookPhysics(Vector2 position)
	{
		Rarity = 3;
		Position = position;
		Size = new Vector2(50, 50);
		Name = "Matematicko fyzikální tabulky";
		Description = "Goated, vyřeší všechny problémy";
		Stats = new Dictionary<StatTypes, float>() { { StatTypes.DAMAGE, 6 }, { StatTypes.PROJECTILE_COUNT, 4 } };
		Effects = new List<EffectTypes>();
		Sprite = TextureManager.GetTexture("bookTabulky");
		ItemType = ItemTypes.BASIC;
	}
	public ItemBookPhysics() : this(Vector2.Zero) { }
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
        return new ItemBookPhysics();
    }
}



