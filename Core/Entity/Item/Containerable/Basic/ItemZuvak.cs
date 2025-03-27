using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemZuvak : ItemContainerable
{
    static Texture2D Sprite;
    public ItemZuvak(Vector2 position)
    {
        Rarity = 2;
        Position = position;
        Size = new Vector2(50, 50);
        Name = "Žuvak";
        Description = "Kouřit cigaretu uprostřed hodiny je\nfakt debilní";
        Stats = new Dictionary<StatTypes, float>() { { StatTypes.XP_GAIN, 1 }, { StatTypes.ATTACK_SPEED, 2 }, { StatTypes.PROJECTILE_COUNT, -2 } };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("zuvak");
        ItemType = ItemTypes.BASIC;
    }
    public ItemZuvak() : this(Vector2.Zero) { }
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
        return new ItemZuvak();
    }
}



