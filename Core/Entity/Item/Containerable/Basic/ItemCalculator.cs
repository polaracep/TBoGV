using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

class ItemCalculator : ItemContainerable
{
    static Texture2D Sprite;
    public ItemCalculator(Vector2 position)
    {
		Rarity = 2;
		Position = position;
        Size = new Vector2(50, 50);
        Name = "Kalkulačka";
        Description = "Rychlá jako schovánkovo opravování testů";
        Stats = new Dictionary<StatTypes, int>() { { StatTypes.DAMAGE, 4 } };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("calculator");
        ItemType = ItemTypes.BASIC;
    }
	public ItemCalculator() : this(Vector2.Zero) { }
	public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
    }
    public override Texture2D GetSprite()
    {
        return Sprite;
    }

}

