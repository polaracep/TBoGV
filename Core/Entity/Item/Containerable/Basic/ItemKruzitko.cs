using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemKruzitko : ItemContainerable
{
    static Texture2D Sprite;
    public ItemKruzitko(Vector2 position)
    {
        Rarity = 1;
        Position = position;
        Size = new Vector2(50, 50);
        Name = "Komerční kružítko";
        Description = "Overrated pomůcka. Praví inženýři si kružítko vyrobí sami.";
        Stats = new Dictionary<StatTypes, float>() { { StatTypes.DAMAGE, 1 } };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("kruzitko");
        ItemType = ItemTypes.BASIC;
    }
    public ItemKruzitko() : this(Vector2.Zero) { }
    public override Texture2D GetSprite()
    {
        return Sprite;
    }
    public override ItemContainerable Clone()
    {
        return new ItemKruzitko();
    }
}



