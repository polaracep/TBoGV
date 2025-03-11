using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TBoGV;

class ItemDoping : ItemContainerable
{
    static Texture2D Sprite;
    public ItemDoping(Vector2 position)
    {
		Rarity = 3;
		Position = position;
        Size = new Vector2(50, 50);
        Name = "Řešitel problémů";
        Description = "Nedělejte drogy";
        Stats = new Dictionary<StatTypes, float>() { { StatTypes.DAMAGE, 12 }, { StatTypes.MAX_HP, -9 }, { StatTypes.MOVEMENT_SPEED, 12 } };
        Effects = new List<EffectTypes>() { };
        Sprite = TextureManager.GetTexture("heal");
        ItemType = ItemTypes.BASIC;
    }
	public ItemDoping() : this(Vector2.Zero) { }
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
        return new ItemDoping();
    }
}
